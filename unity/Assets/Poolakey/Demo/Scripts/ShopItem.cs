using System;
using UnityEngine;
using Poolakey.Scripts.Data;
using UnityEngine.UI;
using RTLTMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private RTLTextMeshPro titleText;
    [SerializeField] private RTLTextMeshPro descriptionText;
    [SerializeField] private RTLTextMeshPro priceText;
    [SerializeField] private Button deleteButton;
    private SKUDetails skuDetails;
    private PurchaseInfo purchaseInfo;
    private Action<SKUDetails> onSelect;
    private Action<PurchaseInfo> onDelete;
    private UnityEngine.UI.Button button;

    public ShopItem Init(Product product, Action<SKUDetails> onSelect, Action<PurchaseInfo> onDelete)
    {
        this.onSelect = onSelect;
        this.onDelete = onDelete;
        iconImage.sprite = product.icon;
        button = GetComponent<UnityEngine.UI.Button>();
        button.interactable = false;
        return this;
    }

    public void CommitData(SKUDetails skuDetails, PurchaseInfo purchaseInfo)
    {
        button.interactable = false;
        this.purchaseInfo = purchaseInfo;
        this.skuDetails = skuDetails;
        titleText.text = skuDetails.title;
        priceText.text = skuDetails.price;
        descriptionText.text = skuDetails.description;

        button.interactable = purchaseInfo == null;
        deleteButton.gameObject.SetActive(purchaseInfo != null && purchaseInfo.productId == "premium");
    }

    public void OnClick()
    {
        onSelect?.Invoke(skuDetails);
    }
    public void OnDeleteClick()
    {
        onDelete?.Invoke(purchaseInfo);
    }
}
