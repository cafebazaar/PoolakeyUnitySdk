using UnityEngine;
using Poolakey.Scripts;
using UnityEngine.UI;
using Poolakey;
using Poolakey.Scripts.Data;

public class PoolakeyExample : MonoBehaviour
{
    public Text ConsoleText;

    private Payment payment;
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
        var result = await payment.GetSkuDetails("coin_6");
        if (result.status == Status.Success)
            foreach (var sku in result.data)
                print(sku.ToString());
    }
    public void GetSubscribesSkuDetails()
    {
        payment.GetSubscriptionSkuDetails("test");
    }

    public void Purchase()
    {
        payment.Purchase("test");
    }
    public void Subscribe()
    {
        payment.Subscribe("test");
    }

    public void Consume()
    {
        payment.Consume("token");
    }
    
    public void Log(string message)
    {
        ConsoleText.text += message + "\n";
    }

    void OnApplicationQuit(){
       payment.Disconnect(); 
    }
}
