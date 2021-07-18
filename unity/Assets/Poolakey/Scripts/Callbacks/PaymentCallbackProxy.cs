using System.Threading.Tasks;
using Poolakey.Scripts.Data;
using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class PaymentCallbackProxy : AndroidJavaProxy
    {
        private Result<PurchaseInfo> result;

        public PaymentCallbackProxy() : base("com.farsitel.bazaar.callback.PaymentCallback") { }
        void onSuccess(string orderId, string purchaseToken, string payload, string packageName, int purchaseState, long purchaseTime, string productId, string originalJson, string dataSignature)
        {
            var data = new PurchaseInfo { orderId = orderId, purchaseToken = purchaseToken, payload = payload, packageName = packageName, purchaseState = (PurchaseInfo.State)purchaseState, purchaseTime = purchaseTime, productId = productId, originalJson = originalJson, dataSignature = dataSignature };
            result = new Result<PurchaseInfo>(Status.Success, data, "Purchase Succeed.");
        }

        void onFailure(string message, string stackTrace)
        {
            result = new Result<PurchaseInfo>(Status.Failure, null, message, stackTrace);
        }

        void onCancel()
        {
            result = new Result<PurchaseInfo>(Status.Cancel, null, "Purchase Canceled.");
        }

        public async Task<Result<PurchaseInfo>> WaitForResult()
        {
            while (result == null)
                await Task.Delay(100);
            return result;
        }
    }
}