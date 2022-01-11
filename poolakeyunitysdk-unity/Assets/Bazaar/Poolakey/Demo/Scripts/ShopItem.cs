using System;
using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using Bazaar.Poolakey.Data;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private RTLTextMeshPro titleText;
    [SerializeField] private RTLTextMeshPro descriptionText;
    [SerializeField] private RTLTextMeshPro priceText;
    [SerializeField] private Button deleteButton;
    private SKUDetails skuDetails;
    private PurchaseInfo purchaseInfo;
    private Action<SKUDetails> onSelect;
    private Action<PurchaseInfo> onDelete;
    private Button button;

    public ShopItem Init(Product product, Action<SKUDetails> onSelect, Action<PurchaseInfo> onDelete)
    {
        this.onSelect = onSelect;
        this.onDelete = onDelete;
        iconImage.sprite = product.icon;
        button = GetComponent<Button>();
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
