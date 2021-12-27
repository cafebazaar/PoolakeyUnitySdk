using System.Collections.Generic;
using Poolakey.Scripts.Data;
using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class SKUDetailsCallbackProxy : BaseCallbackProxy<List<SKUDetails>>
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
            result = new Result<List<SKUDetails>>(Status.Success, "Fetch SKU details completed.");
            result.data = list;
        }

        void onFailure(string message, string stackTrace)
        {
            result = new Result<List<SKUDetails>>(Status.Failure, message, stackTrace);
            result.data = new List<SKUDetails>();
        }
    }
}