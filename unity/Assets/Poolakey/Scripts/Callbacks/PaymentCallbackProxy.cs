using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class PaymentCallbackProxy : AndroidJavaProxy
    {
        private Payment owner;

        public PaymentCallbackProxy(Payment owner) : base("com.farsitel.bazaar.callback.PaymentCallback")
        {
            this.owner = owner;
        }

        void onSuccess(AndroidJavaObject purchaseEntity)
        {
            Debug.Log("onSuccess " + purchaseEntity.Get<string>("originalJson"));
        }

        void onFailure(AndroidJavaObject throwable)
        {
            Debug.Log("onFailure " + throwable);
        }

        void onCancel()
        {
            Debug.Log("onCancel");
        }
    }
}