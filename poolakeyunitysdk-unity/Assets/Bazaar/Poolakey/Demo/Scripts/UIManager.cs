using System;
using System.Threading.Tasks;
using RTLTMPro;
using UnityEngine;

namespace PoolakeyDemo
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private IapManager _iapManager;
        [SerializeField] private PoolakiData _poolakiData;
        [SerializeField] private UiManagerData _uiManagerData;
        [SerializeField] MessageBoxPanel _messageBoxPanel;
        [SerializeField] ResourceManager _resourceManager;
        [SerializeField] private RTLTextMeshPro _text_starCount;
        [SerializeField] private RTLTextMeshPro _text_remainingJellyTime;

        private void Awake()
        {
            _resourceManager.OnStarCountChange += OnStarCountChane;
            _resourceManager.OnRemainingTimeChange += OnJellyTimeUpdate;
        }
        private void OnDestroy()
        {
            _resourceManager.OnStarCountChange -= OnStarCountChane;
            _resourceManager.OnRemainingTimeChange -= OnJellyTimeUpdate;
        }

        public void OnPurchaseAndConsumeClick()
        {
            PurchaseAndConsume(0);
        }

        public void OnPurchaseClick()
        {
            Purchase(0);
        }

        public void OnConsumeClick()
        {
            Consume(0);
        }

        public void OnPurchaseSubscriptionClick()
        {
            Purchase(1);
        }

        private async Task PurchaseAndConsume(int index)
        {
            await _iapManager.PurchaseWithCallBack(_poolakiData.Items[index].ItemSKU, ConsumeOnPurchaseSuccess,
                OnPurchaseFailure);
        }

        private void OnPurchaseFailure(bool userCancelled)
        {
            var message = userCancelled
                ? _uiManagerData.OnUserCancelledPurchase
                : _uiManagerData.OnPurchaseFailedMessage;
            _messageBoxPanel.Show(message);
        }

        private async Task ConsumeOnPurchaseSuccess(string itemSKU)
        {
            await _iapManager.ConsumePurchase(itemSKU, OnConsumeComplete);
        }

        private async Task Purchase(int itemIndex)
        {
            Action<bool> onComplete = itemIndex == 1 ? OnSubscriptionPurchaseComplete : OnPurchaseComplete;
            await _iapManager.Purchase(_poolakiData.Items[itemIndex].ItemSKU, onComplete);
        }

        private void OnSubscriptionPurchaseComplete(bool isSucceeded)
        {
#if UNITY_EDITOR
            _messageBoxPanel.Show(_uiManagerData.OnPurchaseSuccessMessage);
            _resourceManager.AddJellyEndTime(new TimeSpan(0, 5, 0));
            return;
#endif
            if (isSucceeded)
            {
                _messageBoxPanel.Show(_uiManagerData.OnPurchaseSuccessMessage);
                _resourceManager.AddJellyEndTime(new TimeSpan(0, 5, 0));
                return;
            }

            _messageBoxPanel.Show(_uiManagerData.OnPurchaseFailedMessage);
        }

        private void OnPurchaseComplete(bool isSucceeded)
        {
            if (isSucceeded)
            {
                _messageBoxPanel.Show(_uiManagerData.OnPurchaseSuccessMessage);
                return;
            }

            _messageBoxPanel.Show(_uiManagerData.OnPurchaseFailedMessage);
        }

        private async Task Consume(int itemIndex)
        {
            // if (_isConsuming)
            //     return;
            // _isConsuming = true;
            await _iapManager.ConsumePurchase(_poolakiData.Items[itemIndex].ItemSKU, OnConsumeComplete);
            // _isConsuming = false;
        }

        public async Task GetPurchaseData()
        {
            await _iapManager.GetPurchases();
        }

        private void OnConsumeComplete(bool isSucceeded)
        {
            if (isSucceeded)
            {
                _messageBoxPanel.Show(_uiManagerData.OnConsumptionSuccessMessage);
                _resourceManager.AddStar(100);
                return;
            }

            _messageBoxPanel.Show(_uiManagerData.OnConsumptionFailedMessage);
        }

        private void OnStarCountChane(int starCount)
        {
            _text_starCount.Farsi = true;
            _text_starCount.text = $"{starCount:N0}";
        }

        private void OnJellyTimeUpdate(string jellyTime)
        {
            _text_remainingJellyTime.Farsi = true;
            _text_remainingJellyTime.text = jellyTime;
        }
    }
}