using UnityEngine;
using Poolakey.Scripts.Callbacks;
using System.Threading.Tasks;
using Poolakey.Scripts.Data;
using System;

namespace Poolakey.Scripts
{
    public class Payment
    {
        PaymentConfiguration paymentConfiguration;
        private AndroidJavaObject poolakeyBridge;
        private bool isAndroid;

        public Payment(PaymentConfiguration paymentConfiguration)
        {
            isAndroid = Application.platform == RuntimePlatform.Android;
            if (!isAndroid) return;
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
            Result<bool> result = Result.GetDefault();
            if (isAndroid)
            {
            var callback = new ConnectionCallbackProxy();
            poolakeyBridge.Call("connect", paymentConfiguration.securityCheck.rsaPublicKey, callback);
                result = await callback.WaitForResult();
            }
            else
            {
                await Task.Delay(1);
            }
            onComplete?.Invoke(result);
            return result;
        }
        public void Disconnect()
        {
            if (isAndroid)
            {
            poolakeyBridge.Call("disconnect");
        }
        }

        public async Task<SKUDetailsResult> GetSkuDetails(string productIds, Action<SKUDetailsResult> onComplete = null, Type type = Type.inApp)
        {
            var result = Result.GetDefault();
            if (isAndroid)
            {
            var callback = new SKUDetailsCallbackProxy();
            poolakeyBridge.Call("getSkuDetails", type.ToString(), productIds, callback);
                result = await callback.WaitForResult();
            }
            else
            {
                await Task.Delay(1);
            }
            onComplete?.Invoke(result);
            return result;
        }

        public async Task<OwnedProductsResult> GetOwnedProducts(Type type = Type.inApp, Action<OwnedProductsResult> onComplete = null)
        {
            var result = Result.GetDefault();
            if (isAndroid)
            {
            var callback = new OwnedProductsCallbackProxy();
            poolakeyBridge.Call("getOwnedProducts", type.ToString(), callback);
                result = await callback.WaitForResult();
            }
            else
            {
                await Task.Delay(1);
            }
            onComplete?.Invoke(result);
            return result;
        }

        public async Task<PurchaseResult> Purchase(string productId, Type type = Type.inApp, Action<PurchaseResult> onStart = null, Action<PurchaseResult> onComplete = null, string payload = "", string dynamicPriceToken = null)
        {
            var result = Result.GetDefault();
            if (isAndroid)
            {
            var callback = new PaymentCallbackProxy(onStart);
            poolakeyBridge.Call("purchase", type.ToString(), productId, payload, dynamicPriceToken, callback);
                result = await callback.WaitForResult();
            }
            else
            {
                await Task.Delay(1);
            }
            onComplete?.Invoke(result);
            return result;
        }

        public async Task<Result> Consume(string token, Action<Result> onComplete = null)
        {

            Result<bool> result = Result.GetDefault();
            if (isAndroid)
            {
            var callback = new ConsumeCallbackProxy();
            poolakeyBridge.Call("consume", token, callback);
                result = await callback.WaitForResult();
            }
            else
            {
                await Task.Delay(1);
            }
            onComplete?.Invoke(result);
            return result;
        }
    }
}