using UnityEngine;

namespace PoolakeyDemo
{
    public class UiManagerData : ScriptableObject
    {
        [field: SerializeField] public string OnPurchaseSuccessMessage { get; private set; }
        [field: SerializeField] public string OnPurchaseFailedMessage { get; private set; }
        [field: SerializeField] public string OnConsumptionSuccessMessage { get; private set; }
        [field: SerializeField] public string OnConsumptionFailedMessage { get; private set; }
        [field: SerializeField] public string OnUserCancelledPurchase { get; private set; }
        
    }
}