using UnityEngine;
using Poolakey;
using Poolakey.Scripts;
using UnityEngine.UI;

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
        payment.Connect();
    }

    public void GetPurchaseSkuDetails()
    {
        payment.GetPurchaseSkuDetails("test");
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

    public void Log(string message)
    {
        ConsoleText.text += message + "\n";
    }
}
