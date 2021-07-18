using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class ConsumeCallbackProxy : AndroidJavaProxy
    {
        public ConsumeCallbackProxy() : base("com.farsitel.bazaar.callback.ConsumeCallback") { }

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