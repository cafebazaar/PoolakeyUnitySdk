using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class ConsumeCallbackProxy : AndroidJavaProxy
    {
        private Payment owner;

        public ConsumeCallbackProxy(Payment owner) : base("com.farsitel.bazaar.callback.ConsumeCallback")
        {
            this.owner = owner;
        }

        void onSuccess()
        {
            Debug.Log("onSuccess");
        }

        void onFailure(AndroidJavaObject throwable)
        {
            Debug.Log("onFailure " + throwable);
        }
    }
}