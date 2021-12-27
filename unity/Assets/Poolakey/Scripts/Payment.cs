using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Poolakey.Scripts.Data;
using Poolakey.Scripts.Callbacks;

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

        public async Task<Result<bool>> Connect(Action<Result<bool>> onComplete = null)
        {
            Result<bool> result = Result<bool>.GetDefault();
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

        public async Task<Result<List<SKUDetails>>> GetSkuDetails(string productIds, SKUDetails.Type type = SKUDetails.Type.inApp, Action<Result<List<SKUDetails>>> onComplete = null)
        {
            var result = Result<List<SKUDetails>>.GetDefault();
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

        public async Task<Result<List<PurchaseInfo>>> GetPurchases(SKUDetails.Type type = SKUDetails.Type.inApp, Action<Result<List<PurchaseInfo>>> onComplete = null)
        {
            var result = Result<List<PurchaseInfo>>.GetDefault();
            if (isAndroid)
            {
            var callback = new PurchasesCallbackProxy();
                poolakeyBridge.Call("getPurchases", type.ToString(), callback);
                result = await callback.WaitForResult();
            }
            else
            {
                await Task.Delay(1);
            }
            onComplete?.Invoke(result);
            return result;
        }

        public async Task<Result<PurchaseInfo>> Purchase(string productId, SKUDetails.Type type = SKUDetails.Type.inApp, Action<Result<PurchaseInfo>> onStart = null, Action<Result<PurchaseInfo>> onComplete = null, string payload = "", string dynamicPriceToken = null)
        {
            var result = Result<PurchaseInfo>.GetDefault();
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

        public async Task<Result<bool>> Consume(string token, Action<Result<bool>> onComplete = null)
        {

            Result<bool> result = Result<bool>.GetDefault();
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