using UnityEngine;

namespace Poolakey.Scripts.Data
{
    public class SKUDetails
    {
        public string sku, type, title, price, description;
        public SKUDetails(AndroidJavaObject entity)
        {
            this.sku = entity.Get<string>("sku");
            this.type = entity.Get<string>("type");
            this.title = entity.Get<string>("title");
            this.price = entity.Get<string>("price");
            this.description = entity.Get<string>("description");
        }

        override public string ToString() => $"sku: {sku}, type: {type}, title: {title}, price: {price}, description: {description}";
    }
}