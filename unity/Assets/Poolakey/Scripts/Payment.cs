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
            using (var pluginClass = new AndroidJavaClass("com.farsitel.bazaar.PoolakeyBridge"))
            {
                if (pluginClass != null)
                {
                    poolakeyBridge = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
        }
            }
        }

        {
            poolakeyBridge.CallStatic<AndroidJavaObject>(
                "connect",
                getActivity(), 
                paymentConfiguration.securityCheck.rsaPublicKey,
                new ConnectionCallbackProxy()
            );
        }

        public void purchaseProduct(string productId, string payload = null)
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

        public void subscribeProduct(string productId, string payload = null)
        {
            poolakeyBridge.CallStatic(
                "subscribeProduct",
                getActivity(), 
                paymentConfiguration.securityCheck.rsaPublicKey,
                productId,
                payload,
                new PaymentCallbackProxy()
            );
        }

}