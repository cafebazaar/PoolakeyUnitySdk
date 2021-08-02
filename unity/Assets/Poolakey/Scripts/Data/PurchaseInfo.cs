using UnityEngine;
namespace Poolakey.Scripts.Data
{
    public class PurchaseInfo
    {
        public enum State { Purchased = 0, Refunded = 1 }
        public string orderId, purchaseToken, payload, packageName, productId, originalJson, dataSignature;
        public State purchaseState;
        public long purchaseTime;

        override public string ToString() => $"orderId: {orderId}, purchaseToken: {purchaseToken}, productId: {productId}, purchaseState: {purchaseState}, purchaseTime: {purchaseTime}";
    }
}