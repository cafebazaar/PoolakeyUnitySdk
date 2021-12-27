using UnityEngine;
namespace Bazaar.Poolakey.Data
{
    public class PurchaseInfo
    {
        public enum State { Purchased = 0, Refunded = 1, Consumed = 2 }
        public string orderId, purchaseToken, payload, packageName, productId, originalJson, dataSignature;
        public State purchaseState;
        public long purchaseTime;
        private AndroidJavaObject androidJavaObject;

        public PurchaseInfo() { }
        public PurchaseInfo(AndroidJavaObject entity)
        {
            this.orderId = entity.Get<string>("orderId");
            this.purchaseToken = entity.Get<string>("purchaseToken");
            this.payload = entity.Get<string>("payload");
            this.packageName = entity.Get<string>("packageName");
            this.productId = entity.Get<string>("productId");
            this.originalJson = entity.Get<string>("originalJson");
            this.dataSignature = entity.Get<string>("dataSignature");
            this.purchaseTime = entity.Get<long>("purchaseTime");
        }

        override public string ToString() => $"orderId: {orderId}, purchaseToken: {purchaseToken}, productId: {productId}, purchaseState: {purchaseState}, purchaseTime: {purchaseTime}";
    }
}