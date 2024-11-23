using System;
using System.Collections;
using System.Threading.Tasks;
using Bazaar.Data;
using Bazaar.Poolakey;
using Bazaar.Poolakey.Data;
using UnityEngine;

namespace PoolakeyDemo
{
    public class IapManager : MonoBehaviour
    {
        [field: SerializeField] public PoolakiData Data { get; set; }

        private Payment _payment;

        private Func<string, Task> _onPurchaseSuccess = null;
        private Action _onPurchaseFailure = null;

        private void Awake()
        {
            var securityCheck = SecurityCheck.Enable(Data.RsaKey);
            var paymentConfiguration = new PaymentConfiguration(securityCheck);
            _payment = new Payment(paymentConfiguration);
        }

        private IEnumerator Start()
        {
            yield return _payment.Connect(OnPaymentConnectSuccess);
        }

        private void OnDestroy()
        {
            _payment.Disconnect();
        }

        private void OnPaymentConnectSuccess(Result<bool> result)
        {
            Debug.Log($"Payment connected: {result.ToString()}");
        }

        public async Task Purchase(string productId, Action<bool> onComplete)
        {
            Debug.Log($"Purchasing product: {productId}");
            var result = await _payment.Purchase(productId);
            Debug.Log($"purchase result: {result.message}, status: {result.status},{result.data.purchaseState}");
            if (result.status == Status.Success)
            {
                Debug.Log(result.data.ToString());
                var token = result.data.purchaseToken;
                PlayerPrefs.SetString(productId, token);
                onComplete.Invoke(true);
                return;
            }

            onComplete?.Invoke(false);
        }

        public async Task PurchaseWithCallBack(string productId, Func<string, Task> onSuccess = null,
            Action onFailure = null)
        {
            Debug.Log($"Purchasing product: {productId}");
            _onPurchaseFailure = onFailure;
            _onPurchaseSuccess = onSuccess;
            var result = await _payment.Purchase(productId, onComplete: OnComplete);
            Debug.Log($"purchase result: {result.message}, status: {result.status},{result.data.purchaseState}");
        }

        private void OnComplete(Result<PurchaseInfo> result)
        {
            if (result.status != Status.Success)
            {
                _onPurchaseFailure?.Invoke();
                _onPurchaseFailure = null;
                _onPurchaseSuccess = null;
                return;
            }

            Debug.Log(result.data.ToString());
            var token = result.data.purchaseToken;
            PlayerPrefs.SetString(result.data.productId, token);
            _onPurchaseSuccess?.Invoke(result.data.productId);
            _onPurchaseFailure = null;
            _onPurchaseSuccess = null;
        }

        public async Task GetPurchases()
        {
            Debug.Log($"Getting purchases");
            var result = await _payment.GetPurchases();
            Debug.Log($"{result.message}, {result.stackTrace}");
            result.data.ForEach(p =>
            {
                Debug.Log(p.ToString());
                if (PlayerPrefs.HasKey(p.productId))
                    return;
                PlayerPrefs.SetString(p.productId, p.purchaseToken);
            });
        }

        public async Task ConsumePurchase(string productId, Action<bool> onComplete = null)
        {
            Debug.Log($"consuming purchase: {productId}");
            if (PlayerPrefs.HasKey(productId))
                Debug.Log("Not purchased");
            var purchaseToken = PlayerPrefs.GetString(productId);
            var result = await _payment.Consume(purchaseToken);
            if (result.status == Status.Success)
            {
                Debug.Log(result.data.ToString());
                PlayerPrefs.DeleteKey(productId);
                onComplete?.Invoke(true);
                return;
            }

            onComplete?.Invoke(false);
        }
    }
}