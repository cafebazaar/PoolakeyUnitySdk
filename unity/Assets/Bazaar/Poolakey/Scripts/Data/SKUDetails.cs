using UnityEngine;

namespace Bazaar.Poolakey.Data
{
    public class SKUDetails
    {
        public enum Type { all, inApp, subscription }
        public Type type;
        public string sku, title, price, description;
        public SKUDetails(AndroidJavaObject entity)
        {
            this.type = entity.Get<string>("type") == "inapp" ? Type.inApp : Type.subscription;
            this.sku = entity.Get<string>("sku");
            this.title = entity.Get<string>("title");
            this.price = entity.Get<string>("price");
            this.description = entity.Get<string>("description");
        }

        override public string ToString() => $"sku: {sku}, type: {type}, title: {title}, price: {price}, description: {description}";
    }
}