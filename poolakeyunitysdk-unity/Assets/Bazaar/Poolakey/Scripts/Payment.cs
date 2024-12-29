using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bazaar.Poolakey.Data;
using Bazaar.Poolakey.Callbacks;
using Bazaar.Data;

namespace Bazaar.Poolakey
{
    public class Payment : Bridge
    {
        PaymentConfiguration paymentConfiguration;
        public Payment(PaymentConfiguration paymentConfiguration) : base("com.farsitel.bazaar.PoolakeyBridge")
        {
            this.paymentConfiguration = paymentConfiguration;
        }

        public async Task<Result<bool>> Connect(Action<Result<bool>> onComplete = null)
        {
            Result<bool> result = Result<bool>.GetDefault();
            if (isAndroid)
            {
                var callback = new ConnectionCallbackProxy();
                bridge.Call("connect", paymentConfiguration.securityCheck.rsaPublicKey, callback);
                result = await callback.taskCompletionSource.Task;
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
                bridge.Call("disconnect");
            }
        }
        public async Task<Result<List<SKUDetails>>> GetSkuDetails(IEnumerable<string> productIds, SKUDetails.Type type = SKUDetails.Type.all, Action<Result<List<SKUDetails>>> onComplete = null)
        {
            var result = Result<List<SKUDetails>>.GetDefault();
            if (isAndroid)
            {
                var callback = new SKUDetailsCallbackProxy();
                bridge.Call("getSkuDetails", type.ToString(), productIds, callback);
                result = await callback.taskCompletionSource.Task;

                if (result.status == Status.Success)
                {
                    var trialSubscription = result.data.Find(x => x.sku == "trial_subscription");
                    if (trialSubscription != null)
                    {
                        var trialCallback = new TrialSubscriptionCallbackProxy(trialSubscription);
                        bridge.Call("checkTrialSubscriptionState", trialCallback);
                        var trialResult = await trialCallback.taskCompletionSource.Task;
                        trialSubscription = trialResult.data;
                    }
                }
            }
            else
            {
                await Task.Delay(1);
            }
            onComplete?.Invoke(result);
            return result;
        }
        public async Task<Result<List<SKUDetails>>> GetSkuDetails(string productIds, SKUDetails.Type type = SKUDetails.Type.all, Action<Result<List<SKUDetails>>> onComplete = null)
        {
            var result = Result<List<SKUDetails>>.GetDefault();
            if (isAndroid)
            {
                var callback = new SKUDetailsCallbackProxy();
                bridge.Call("getSkuDetails", type.ToString(), productIds, callback);
                result = await callback.taskCompletionSource.Task;

                if (result.status == Status.Success)
                {
                    var trialSubscription = result.data.Find(x => x.sku == "trial_subscription");
                    if (trialSubscription != null)
                    {
                        var trialCallback = new TrialSubscriptionCallbackProxy(trialSubscription);
                        bridge.Call("checkTrialSubscriptionState", trialCallback);
                        var trialResult = await trialCallback.taskCompletionSource.Task;
                        trialSubscription = trialResult.data;
                    }
                }
            }
            else
            {
                await Task.Delay(1);
            }
            onComplete?.Invoke(result);
            return result;
        }

        public async Task<Result<List<PurchaseInfo>>> GetPurchases(SKUDetails.Type type = SKUDetails.Type.all, Action<Result<List<PurchaseInfo>>> onComplete = null)
        {
            var result = Result<List<PurchaseInfo>>.GetDefault();
            if (isAndroid)
            {
                var callback = new PurchasesCallbackProxy();
                bridge.Call("getPurchases", type.ToString(), callback);
                result = await callback.taskCompletionSource.Task;
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
                bridge.Call("purchase", type.ToString(), productId, payload, dynamicPriceToken, callback);
                result = await callback.taskCompletionSource.Task;
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
                bridge.Call("consume", token, callback);
                result = await callback.taskCompletionSource.Task;
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