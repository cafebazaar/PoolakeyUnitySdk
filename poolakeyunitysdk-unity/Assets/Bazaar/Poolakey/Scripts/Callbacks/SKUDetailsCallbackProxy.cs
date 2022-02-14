using UnityEngine;
using Bazaar.Data;
using Bazaar.Callbacks;
using Bazaar.Poolakey.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazaar.Poolakey.Callbacks
{
    public class SKUDetailsCallbackProxy : CallbackProxy<List<SKUDetails>>
    {
        public SKUDetailsCallbackProxy() : base("com.farsitel.bazaar.callback.SKUDetailsCallback")
        {
            taskCompletionSource = new TaskCompletionSource<Result<List<SKUDetails>>>();
        }

        void onSuccess(AndroidJavaObject purchaseEntity)
        {
            var list = new List<SKUDetails>();
            var size = purchaseEntity.Call<int>("size");
            for (int index = 0; index < size; index++)
            {
                list.Add(new SKUDetails(purchaseEntity.Call<AndroidJavaObject>("get", index)));
            }
            taskCompletionSource.SetResult(new Result<List<SKUDetails>>(Status.Success, "Fetch SKU details completed.") { data = list });
        }

        void onFailure(string message, string stackTrace)
        {
            taskCompletionSource.SetResult(new Result<List<SKUDetails>>(Status.Failure, message, stackTrace));
        }
    }
}