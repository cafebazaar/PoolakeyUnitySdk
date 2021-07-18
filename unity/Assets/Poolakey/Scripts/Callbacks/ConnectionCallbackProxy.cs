using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class ConnectionCallbackProxy : AndroidJavaProxy
    {
        public ConnectionCallbackProxy() : base("com.farsitel.bazaar.callback.ConnectionCallback") {}

        void onConnect()
        {
            Debug.Log("onConnectttttt");
        }

        void onDisconnect()
        {
            Debug.Log("onDisconnect");
        }

        void onFailure()
        {
            Debug.Log("onFailure");
        }
    }
}