using System;
using Bazaar.Poolakey.Data;

namespace Bazaar.Poolakey.Callbacks
{
    public class PaymentCallbackProxy : BaseCallbackProxy<PurchaseInfo>
    {
        private Action<Result<PurchaseInfo>> onStartAction;

        public PaymentCallbackProxy(Action<Result<PurchaseInfo>> onStartAction) : base("com.farsitel.bazaar.callback.PaymentCallback")
        {
            this.onStartAction = onStartAction;
        }

        void onStart()
        {
            onStartAction?.Invoke(new Result<PurchaseInfo>(Status.Start, "Purchase flow started."));
            result.data = new PurchaseInfo();
        }

        void onCancel()
        {
            result = new Result<PurchaseInfo>(Status.Cancel, null, "Purchase flow canceled.");
            result.data = new PurchaseInfo();
        }

        void onSuccess(string orderId, string purchaseToken, string payload, string packageName, int purchaseState, long purchaseTime, string productId, string originalJson, string dataSignature)
        {
            result = new Result<PurchaseInfo>(Status.Success, "Purchase Succeed.");
            result.data = new PurchaseInfo { orderId = orderId, purchaseToken = purchaseToken, payload = payload, packageName = packageName, purchaseState = (PurchaseInfo.State)purchaseState, purchaseTime = purchaseTime, productId = productId, originalJson = originalJson, dataSignature = dataSignature };
        }

        void onFailure(string message, string stackTrace)
        {
            result = new Result<PurchaseInfo>(Status.Failure, message, stackTrace);
            result.data = new PurchaseInfo();
        }
    }
}