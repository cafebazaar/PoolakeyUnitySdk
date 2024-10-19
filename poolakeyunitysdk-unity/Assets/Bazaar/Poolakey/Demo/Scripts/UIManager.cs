using System;
using System.Threading.Tasks;
using TMPro;
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
        [SerializeField] private TMP_Text _text_starCount;
        [SerializeField] private TMP_Text _text_remainingJellyTime;
        private bool _isPurchasing = false;
        private bool _isConsuming = false;
        private bool _isGettingPurchaseData = false;

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
            if (_isPurchasing)
                return;
            _isPurchasing = true;
            await _iapManager.PurchaseWithCallBack(_poolakiData.Items[index].ItemSKU, ConsumeOnPurchaseSuccess,
                OnPurchaseFailure);
            _isPurchasing = false;
        }

        private void OnPurchaseFailure()
        {
            _messageBoxPanel.Show(_uiManagerData.OnPurchaseFailedMessage);
        }

        private async Task ConsumeOnPurchaseSuccess(string itemSKU)
        {
            await _iapManager.ConsumePurchase(itemSKU, OnConsumeComplete);
        }

        private async Task Purchase(int itemIndex)
        {
            if (_isPurchasing)
                return;
            _isPurchasing = true;
            Action<bool> onComplete = itemIndex == 1 ? OnSubscriptionPurchaseComplete : OnPurchaseComplete;
            await _iapManager.Purchase(_poolakiData.Items[itemIndex].ItemSKU,onComplete);
            _isPurchasing = false;
        }

        private void OnSubscriptionPurchaseComplete(bool isSucceeded)
        {
            if (isSucceeded)
            {
                _messageBoxPanel.Show(_uiManagerData.OnPurchaseSuccessMessage);
                _resourceManager.AddJellyEndTime(new TimeSpan(0,5,0));
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
            if (_isConsuming)
                return;
            _isConsuming = true;
            await _iapManager.ConsumePurchase(_poolakiData.Items[itemIndex].ItemSKU, OnConsumeComplete);
            _isConsuming = false;
        }

        public async Task GetPurchaseData()
        {
            if (_isGettingPurchaseData)
                return;
            _isGettingPurchaseData = true;
            await _iapManager.GetPurchases();
            _isGettingPurchaseData = false;
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
            _text_starCount.text = $"{starCount:N0}";
        }

        private void OnJellyTimeUpdate(string jellyTime)
        {
            _text_remainingJellyTime.text = jellyTime;
        }
    }
}