using System;
using Poolakey.Scripts.Data;

namespace Poolakey.Scripts.Callbacks
{
    public class PaymentCallbackProxy : BaseCallbackProxy
    {
        private Action<PurchaseResult> onStartAction;

        public PaymentCallbackProxy(Action<PurchaseResult> onStartAction) : base("com.farsitel.bazaar.callback.PaymentCallback")
        {
            this.onStartAction = onStartAction;
        }

        void onStart()
        {
            onStartAction?.Invoke(new PurchaseResult(Status.Start, null, "Purchase flow started."));
        }

        void onCancel()
        {
            result = new PurchaseResult(Status.Cancel, null, "Purchase flow canceled.");
        }

        void onSuccess(string orderId, string purchaseToken, string payload, string packageName, int purchaseState, long purchaseTime, string productId, string originalJson, string dataSignature)
        {
            var data = new PurchaseInfo { orderId = orderId, purchaseToken = purchaseToken, payload = payload, packageName = packageName, purchaseState = (PurchaseInfo.State)purchaseState, purchaseTime = purchaseTime, productId = productId, originalJson = originalJson, dataSignature = dataSignature };
            result = new PurchaseResult(Status.Success, data, "Purchase Succeed.");
        }

        void onFailure(string message, string stackTrace)
        {
            result = new PurchaseResult(Status.Failure, null, message, stackTrace);
        }
    }
}