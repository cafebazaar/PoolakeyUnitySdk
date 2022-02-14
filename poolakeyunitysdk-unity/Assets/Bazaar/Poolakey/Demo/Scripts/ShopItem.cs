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
    [SerializeField] private InputField dynamicPriceInput;
    private SKUDetails skuDetails;
    private PurchaseInfo purchaseInfo;
    private Action<SKUDetails, String> onSelect;
    private Action<PurchaseInfo> onDelete;
    private Button button;

    public ShopItem Init(Product product, Action<SKUDetails, string> onSelect, Action<PurchaseInfo> onDelete)
    {
        this.onSelect = onSelect;
        this.onDelete = onDelete;
        iconImage.sprite = product.icon;
        button = GetComponent<Button>();
        button.interactable = false;
        dynamicPriceInput.gameObject.SetActive(product.id == "dynamic_price");
        return this;
    }

    public void CommitData(SKUDetails skuDetails, PurchaseInfo purchaseInfo)
    {
        button.interactable = false;
        this.purchaseInfo = purchaseInfo;
        this.skuDetails = skuDetails;
        titleText.text = skuDetails.title;
        priceText.text = skuDetails.price.Replace(",", "");
        descriptionText.text = skuDetails.description;

        button.interactable = purchaseInfo == null;
        deleteButton.gameObject.SetActive(purchaseInfo != null && purchaseInfo.productId == "premium");
    }

    public void OnClick()
    {
        onSelect?.Invoke(skuDetails, dynamicPriceInput.text);
    }
    public void OnDeleteClick()
    {
        onDelete?.Invoke(purchaseInfo);
    }
}
