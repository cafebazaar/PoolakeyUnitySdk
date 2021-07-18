using UnityEngine;
using Poolakey.Scripts;
using UnityEngine.UI;
using Poolakey;
using Poolakey.Scripts.Data;

public class PoolakeyExample : MonoBehaviour
{
    public Text ConsoleText;

    private Payment payment;
    private PurchaseInfo purchase;

    void Start()
    {
        Log("Poolakey Plugin Version: " + PluginVersion.VersionString);
        SecurityCheck securityCheck = SecurityCheck.Disable();
        PaymentConfiguration paymentConfiguration = new PaymentConfiguration(securityCheck);
        payment = new Payment(paymentConfiguration);
    }

    public void Connect()
    {
        _ = payment.Connect();
    }

    public async void GetPurchaseSkuDetails()
    {
        var result = await payment.GetSkuDetails("test");
        if (result.status == Status.Success)
            foreach (var sku in result.data)
                print(sku.ToString());
    }

    public async void Purchase()
    {
        var result = await payment.Purchase("test");
        print(result.message + " .. " + result.stackTrace);
        if (result.status == Status.Success)
        {
            purchase = result.data;
            print(purchase.ToString());
        }
    }
    public void Subscribe()
    {
        _ = payment.Purchase("test", Payment.Type.subscription);
    }

    public async void Consume()
    {
        var result = await payment.Consume(purchase.purchaseToken);
        print(result.message + " .. " + result.stackTrace);
    }

    public void Log(string message)
    {
        ConsoleText.text += message + "\n";
    }

    void OnApplicationQuit()
    {
        payment.Disconnect();
    }
}
