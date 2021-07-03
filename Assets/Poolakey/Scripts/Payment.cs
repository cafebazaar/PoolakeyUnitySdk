using UnityEngine;
using Poolakey.Scripts.Callbacks;
namespace Poolakey.Scripts
{
    public class Payment
    {
        PaymentConfiguration paymentConfiguration;
        private AndroidJavaObject poolakeyBridge;
        public Payment(PaymentConfiguration paymentConfiguration)
        {
            this.paymentConfiguration = paymentConfiguration;
            poolakeyBridge = new AndroidJavaObject("com.farsitel.bazaar.PoolakeyBridge");
        }
        public void connect()
        {
            poolakeyBridge.CallStatic<AndroidJavaObject>(
                "connect",
                getActivity(), 
                paymentConfiguration.securityCheck.rsaPublicKey,
                new ConnectionCallbackProxy()
            );
        }

        public void purchaseProduct(string productId, string payload)
        {
            poolakeyBridge.CallStatic(
                "purchaseProduct",
                getActivity(), 
                paymentConfiguration.securityCheck.rsaPublicKey,
                productId,
                payload,
                new PaymentCallbackProxy()
            );
        }

        private AndroidJavaObject getActivity()
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            return currentActivity;
        }
    }
}