using System;
using Bazaar.Data;
using Bazaar.Callbacks;
using Bazaar.Poolakey.Data;
using System.Threading.Tasks;

namespace Bazaar.Poolakey.Callbacks
{
    public class PaymentCallbackProxy : CallbackProxy<PurchaseInfo>
    {
        private Action<Result<PurchaseInfo>> onStartAction;

        public PaymentCallbackProxy(Action<Result<PurchaseInfo>> onStartAction) : base("com.farsitel.bazaar.callback.PaymentCallback")
        {
            this.onStartAction = onStartAction;
            taskCompletionSource = new TaskCompletionSource<Result<PurchaseInfo>>();
        }

        void onStart()
        {
            onStartAction?.Invoke(new Result<PurchaseInfo>(Status.Started, "Purchase flow started."));
        }

        void onCancel()
        {
            taskCompletionSource.SetResult(new Result<PurchaseInfo>(Status.Canceled, null, "Purchase flow canceled."));
        }

        void onSuccess(string orderId, string purchaseToken, string payload, string packageName, int purchaseState, long purchaseTime, string productId, string originalJson, string dataSignature)
        {
            var purchase = new PurchaseInfo { orderId = orderId, purchaseToken = purchaseToken, payload = payload, packageName = packageName, purchaseState = (PurchaseInfo.State)purchaseState, purchaseTime = purchaseTime, productId = productId, originalJson = originalJson, dataSignature = dataSignature };
            taskCompletionSource.SetResult(new Result<PurchaseInfo>(Status.Success, "Purchase Succeed.") { data = purchase });
        }

        void onFailure(string message, string stackTrace)
        {
            taskCompletionSource.SetResult(new Result<PurchaseInfo>(Status.Failure, message, stackTrace));
        }
    }
}