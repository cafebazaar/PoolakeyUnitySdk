using UnityEngine;
using Poolakey.Scripts;
using Poolakey;
using Poolakey.Scripts.Data;
using System.Collections.Generic;
using System;
using RTLTMPro;

public class PoolakeyExample : MonoBehaviour
{
    [SerializeField] private Vehicle vehicle;
    [SerializeField] private List<Product> products;
    [SerializeField] private ShopItem shopItemTemplate;
    [SerializeField] private RTLTextMeshPro ConsoleText;

    private Payment payment;
    private Dictionary<string, ShopItem> shopItems;
    void Start()
    {
        Log("Poolakey Plugin Version: " + PluginVersion.VersionString);

        // TODO: SecurityCheck.Enable("Your RSA key");
        SecurityCheck securityCheck = SecurityCheck.Disable();
        PaymentConfiguration paymentConfiguration = new PaymentConfiguration(securityCheck);
        payment = new Payment(paymentConfiguration);

        CreateShopItems();
        Connect();
    }

    private void CreateShopItems()
    {
        shopItems = new Dictionary<string, ShopItem>();
        foreach (var p in products)
        {
            shopItems.Add(p.id, Instantiate<ShopItem>(shopItemTemplate, transform).Init(p, Purchase, Consume));
        }
    }

    private async void Connect()
    {
        var result = await payment.Connect();
        Log(result.ToString());
        if (result.status == Status.Success)
        {
            GetSkuDetails();
        }
    }

    private async void GetSkuDetails()
    {
        var productIds = "";
        foreach (var p in products)
        {
            productIds += p.id + ",";
        }
        var result = await payment.GetSkuDetails(productIds);
        Log(result.ToString());
        if (result.status == Status.Success)
        {
            GetPurchases(result.data);
        }
    }

    private async void GetPurchases(List<SKUDetails> skuDetailsList)
    {
        var result = await payment.GetPurchases();
        Log(result.ToString());
        if (result.status == Status.Success)
        {
            var purchases = result.data;
            var gas = Math.Min(vehicle.gas, 4);
            foreach (var skuDetails in skuDetailsList)
            {
                var purchaseInfo = purchases.Find(pi => pi.productId == skuDetails.sku);

                if (purchaseInfo != null)
                {
                    if (IsConsumable(purchaseInfo.productId))
                    {
                        Consume(purchaseInfo);
                    }
                    else
                    {
                        UpdateStats(purchaseInfo);
                    }
                }
                shopItems[skuDetails.sku].CommitData(skuDetails, purchaseInfo);
            }

        }
    }

    private void UpdateStats(PurchaseInfo purchaseInfo)
    {
        switch (purchaseInfo.productId)
        {
            case "infinite_gas_monthly":
                vehicle.SetGas(5);
                break;
            case "gas":
                vehicle.Increase();
                break;
            case "premium":
                vehicle.SetSkin(purchaseInfo.purchaseState == PurchaseInfo.State.Purchased ? 1 : 0);
                break;
            default: return;
        }
    }

    private async void Purchase(SKUDetails skuDetails)
    {
        var result = await payment.Purchase(skuDetails.sku, skuDetails.type);
        Log(result.ToString());
        if (result.status == Status.Success)
        {

            if (IsConsumable(result.data.productId))
            {
                Consume(result.data);
            }
            else
            {
                UpdateStats(result.data);
                GetSkuDetails();
            }
        }
    }

    private async void Consume(PurchaseInfo purchaseInfo)
    {
        if (purchaseInfo == null)
        {
            Log("You must purchase an item pefore!");
            return;
        }

        var result = await payment.Consume(purchaseInfo.purchaseToken);
        Log(result.ToString());
        if (result.status == Status.Success)
        {
            Log($"=={purchaseInfo.productId}");
            purchaseInfo.purchaseState = PurchaseInfo.State.Consumed;
            UpdateStats(purchaseInfo);
            GetSkuDetails();
        }
    }

    // The 'gas' item is only consumable in this example project
    private bool IsConsumable(string productId) => productId == "gas";

    public void Log(string message)
    {
        ConsoleText.text = ConsoleText.OriginalText + message + "\n";
    }

    void OnApplicationQuit()
    {
        payment.Disconnect();
    }
}

[Serializable]
public class Product
{
    public string id;
    public Sprite icon;
}