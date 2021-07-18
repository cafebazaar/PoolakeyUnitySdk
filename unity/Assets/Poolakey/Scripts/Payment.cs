using UnityEngine;
using Poolakey.Scripts.Callbacks;

namespace Poolakey.Scripts
{
    public class Payment
    {
        public enum Type { inApp, subscription }
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

        public void GetSkuDetails(string productId, Type type = Type.inApp)
        {
            poolakeyBridge.Call("getSkuDetails", type.ToString(), productId, new SKUDetailsCallbackProxy());
        }

        public void Purchase(string productId, Type type = Type.inApp, string payload = "")
        {
            poolakeyBridge.Call("purchase", type.ToString(), productId, payload, new PaymentCallbackProxy());
        }

        public void Consume(string token)
        {
            poolakeyBridge.Call("consume", token, new ConsumeCallbackProxy());
        }
    }
}