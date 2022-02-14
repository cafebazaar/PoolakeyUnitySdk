using UnityEngine;
using Bazaar.Data;
using Bazaar.Callbacks;
using Bazaar.Poolakey.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazaar.Poolakey.Callbacks
{
    public class PurchasesCallbackProxy : CallbackProxy<List<PurchaseInfo>>
    {
        public PurchasesCallbackProxy() : base("com.farsitel.bazaar.callback.PurchasesCallback") { 
            taskCompletionSource = new TaskCompletionSource<Result<List<PurchaseInfo>>>();
        }

        void onSuccess(AndroidJavaObject purchaseEntity)
        {
            var list = new List<PurchaseInfo>();
            var size = purchaseEntity.Call<int>("size");
            for (int index = 0; index < size; index++)
            {
                list.Add(new PurchaseInfo(purchaseEntity.Call<AndroidJavaObject>("get", index)));
            }
            taskCompletionSource.SetResult(new Result<List<PurchaseInfo>>(Status.Success, "Get purchases completed.") { data = list });
        }

        void onFailure(string message, string stackTrace)
        {
            taskCompletionSource.SetResult(new Result<List<PurchaseInfo>>(Status.Failure, message, stackTrace));
        }
    }
}