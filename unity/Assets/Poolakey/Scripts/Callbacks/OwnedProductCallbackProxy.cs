using System.Collections.Generic;
using Poolakey.Scripts.Data;
using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class OwnedProductsCallbackProxy : BaseCallbackProxy
    {
        public OwnedProductsCallbackProxy() : base("com.farsitel.bazaar.callback.OwnedProductsCallback"){}

        void onSuccess(AndroidJavaObject purchaseEntity)
        {
            var list = new List<PurchaseInfo>();
            var size = purchaseEntity.Call<int>("size");
            for (int index = 0; index < size; index++)
            {
                list.Add(new PurchaseInfo(purchaseEntity.Call<AndroidJavaObject>("get", index)));
            }
            result = new OwnedProductsResult(Status.Success, list, "Fetch Owned products completed.");
        }

        void onFailure(string message, string stackTrace)
        {
            result = new OwnedProductsResult(Status.Failure, null, message, stackTrace);
        }
    }
}