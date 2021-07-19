using System.Collections.Generic;
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
            var size = purchaseEntity.Call<int>("size");
            for (int index = 0; index < size; index++)
            {
                list.Add(new SKUDetails(purchaseEntity.Call<AndroidJavaObject>("get", index)));
            }
            result = new SKUDetailsResult(Status.Success, list, "Fetch SKU details completed.");
        }

        void onFailure(string message, string stackTrace)
        {
            result = new SKUDetailsResult(Status.Failure, null, message, stackTrace);
        }
    }
}