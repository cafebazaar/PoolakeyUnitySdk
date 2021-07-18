using UnityEngine;
using Poolakey;
using Poolakey.Scripts;
using UnityEngine.UI;

public class PoolakeyExample : MonoBehaviour
{
    public Text ConsoleText;

    public Button Btn_InitIAB, Btn_StartPurchase, Btn_ConsumePurchase, Btn_subscribe,Btn_purchaseList;

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
        payment.connect();
    }

    public void PurchaseProduct()
    {
        payment.purchaseProduct("test");
    }
    public void SubscribeProduct()
    {
        payment.subscribeProduct("test");
    }

    public void Log(string message)
    {
        ConsoleText.text += message + "\n";
    }
}
