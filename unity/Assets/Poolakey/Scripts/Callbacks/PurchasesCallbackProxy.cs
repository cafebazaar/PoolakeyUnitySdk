using System.Collections.Generic;
using Poolakey.Scripts.Data;
using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class PurchasesCallbackProxy : BaseCallbackProxy
    {
        public PurchasesCallbackProxy() : base("com.farsitel.bazaar.callback.PurchasesCallback") { }

        void onSuccess(AndroidJavaObject purchaseEntity)
        {
            var list = new List<PurchaseInfo>();
            var size = purchaseEntity.Call<int>("size");
            for (int index = 0; index < size; index++)
            {
                list.Add(new PurchaseInfo(purchaseEntity.Call<AndroidJavaObject>("get", index)));
            }
            result = new PurchasesResult(Status.Success, "Get purchases completed.");
            result.data = list;
        }

        void onFailure(string message, string stackTrace)
        {
            result = new PurchasesResult(Status.Failure, message, stackTrace);
            result.data = new List<PurchaseInfo>();
        }
    }
}