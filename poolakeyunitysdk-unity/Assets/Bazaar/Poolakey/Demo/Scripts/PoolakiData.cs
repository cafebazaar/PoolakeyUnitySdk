using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoolakeyDemo
{
    public class PoolakiData : ScriptableObject
    {
        [field: SerializeField] public string RsaKey { get; private set; }
        [field: SerializeField] public List<ItemInfo> Items { get; private set; }

        [Serializable]
        public class ItemInfo
        {
            [field: SerializeField] public string ItemName { get; private set; }
            [field: SerializeField] public string ItemSKU { get; private set; }
            [field: SerializeField] public int ItemPrice { get; private set; }
            [field: SerializeField] public ItemType ItemType { get; private set; }
        }

        [Serializable]
        public enum ItemType
        {
            Purchasable,
            Subscription,
        }
    }
}