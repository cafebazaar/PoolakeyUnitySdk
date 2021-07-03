using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class PaymentCallbackProxy : AndroidJavaProxy
    {
        public PaymentCallbackProxy() : base("com.farsitel.bazaar.callback.PaymentCallback") {}

        void onSuccess(AndroidJavaObject purchaseEntity)
        {
            Debug.Log("onSuccess" + purchaseEntity);
        }

        void onFailure(AndroidJavaObject throwable)
        {
            Debug.Log("onFailure"+throwable);
        }

        void onCancel()
        {
            Debug.Log("onCancel");
        }
    }
}