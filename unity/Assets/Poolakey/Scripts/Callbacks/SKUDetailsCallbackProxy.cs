using System.Collections.Generic;
using System.Threading.Tasks;
using Poolakey.Scripts.Data;
using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class SKUDetailsCallbackProxy : BaseCallbackProxy
    {
        public SKUDetailsCallbackProxy() : base("com.farsitel.bazaar.callback.SKUDetailsCallback"){}

        void onSuccess(AndroidJavaObject purchaseEntity)
        {
            var list = new List<SKUDetails>();
            var length = purchaseEntity.Call<int>("size");
            for (int i = 0; i < length; i++)
            {
                list.Add(new SKUDetails(purchaseEntity.Call<AndroidJavaObject>("get", i)));
            }
            result = new SKUDetailsResult(Status.Success, list, "Fetch SKU details completed.");
        }

        void onFailure(string message, string stackTrace)
        {
            result = new SKUDetailsResult(Status.Failure, null, message, stackTrace);
        }

        public async Task<Result<List<SKUDetails>>> WaitForResult()
        {
            while (result == null)
                await Task.Delay(100);
            return result;
        }
    }
}