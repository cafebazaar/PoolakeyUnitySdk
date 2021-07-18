using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class SKUDetailsCallbackProxy : AndroidJavaProxy
    {
        private Payment owner;

        public SKUDetailsCallbackProxy(Payment owner) : base("com.farsitel.bazaar.callback.SKUDetailsCallback")
        {
            this.owner = owner;
        }

        void onSuccess(AndroidJavaObject purchaseEntity)
        {
            Debug.Log("onSuccess ");
            var length = purchaseEntity.Call<int>("size");
            for (int i = 0; i < length; i++)
            {
                var sku = purchaseEntity.Call<AndroidJavaObject>("get", i);
                Debug.Log(sku.Call<string>("toString"));
            }
        }

        void onFailure(AndroidJavaObject throwable)
        {
            Debug.Log("onFailure " + throwable);
        }
    }
}