using UnityEngine;
using Poolakey.Scripts.Callbacks;
using System.Threading.Tasks;
using Poolakey.Scripts.Data;
using System.Collections.Generic;
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

        public async Task<Result<bool>> Connect(Action<Result<bool>> onComplete = null)
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

        public async Task<Result<List<SKUDetails>>> GetSkuDetails(string productId, Action<Result<List<SKUDetails>>> onComplete = null, Type type = Type.inApp)
        {
            var callback = new SKUDetailsCallbackProxy();
            poolakeyBridge.Call("getSkuDetails", type.ToString(), productId, callback);
            var result = await callback.WaitForResult();
            onComplete?.Invoke(result);
            return result;
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