using UnityEngine;
using Poolakey.Scripts.Callbacks;
using System.Threading.Tasks;
using Poolakey.Scripts.Data;
using System;

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

        public async Task<Result> Connect(Action<Result> onComplete = null)
        {
            var callback = new ConnectionCallbackProxy();
            poolakeyBridge.Call("connect", paymentConfiguration.securityCheck.rsaPublicKey, callback);
            var result = await callback.WaitForResult();
            onComplete?.Invoke(result);
            return result;
        }
        public void Disconnect()
        {
            poolakeyBridge.Call("disconnect");
        }

        public async Task<SKUDetailsResult> GetSkuDetails(string productId, Action<SKUDetailsResult> onComplete = null, Type type = Type.inApp)
        {
            var callback = new SKUDetailsCallbackProxy();
            poolakeyBridge.Call("getSkuDetails", type.ToString(), productId, callback);
            var result = (SKUDetailsResult)await callback.WaitForResult();
            onComplete?.Invoke(result);
            return result;
        }

        public async Task<PurchaseResult> Purchase(string productId, Type type = Type.inApp, Action<PurchaseResult> onStart = null, Action<PurchaseResult> onComplete = null, string payload = "")
        {
            var callback = new PaymentCallbackProxy(onStart);
            poolakeyBridge.Call("purchase", type.ToString(), productId, payload, callback);
            var result = (PurchaseResult)await callback.WaitForResult();
            onComplete?.Invoke(result);
            return result;
        }

        public async Task<Result> Consume(string token, Action<Result> onComplete = null)
        {
            var callback = new ConsumeCallbackProxy();
            poolakeyBridge.Call("consume", token, callback);
            var result = await callback.WaitForResult();
            onComplete?.Invoke(result);
            return result;
        }
    }
}