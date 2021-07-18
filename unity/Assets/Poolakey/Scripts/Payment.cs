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

        public void Connect()
        {
            poolakeyBridge.Call(
                "connect",
                paymentConfiguration.securityCheck.rsaPublicKey,
                new ConnectionCallbackProxy());
        }
        public void Disconnect()
        {
            poolakeyBridge.Call("disconnect");
        }

        public void GetPurchaseSkuDetails(string productId)
        {
            poolakeyBridge.Call(
                "getPurchaseSkuDetails",
                productId,
                new SKUDetailsCallbackProxy(this));
        }
        public void GetSubscriptionSkuDetails(string productId)
        {
            poolakeyBridge.Call(
                "getSubscriptionSkuDetails",
                productId,
                new SKUDetailsCallbackProxy(this));
        }

        public void Purchase(string productId, string payload = "")
        {
            poolakeyBridge.Call(
                "purchase",
                productId,
                payload,
                new PaymentCallbackProxy(this));
        }
        public void Subscribe(string productId, string payload = "")
        {
            poolakeyBridge.Call(
                "subscribe",
                productId,
                payload,
                new PaymentCallbackProxy(this));
        }

        public void Consume(string token)
        {
            poolakeyBridge.Call(
                "consume",
                token,
                new ConsumeCallbackProxy(this));
        }
    }
}