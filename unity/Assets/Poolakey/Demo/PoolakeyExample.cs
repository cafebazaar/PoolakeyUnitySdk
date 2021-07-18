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
        SecurityCheck securityCheck = SecurityCheck.Enable("MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwCr1DxbF5Ub4SnksjwVnEu5wmLyzoD7MHtM8rVzDFeZInWLsxefy4j9sm853u7haBEBk83na5wHAYzHHK6oq5nzCpdxzbQUuVfw5x4Ir4zb6cUPbHlHNHgRYMfcEaWWV4ek/kY+PebYsZNAdPpiWH0tx+kTYjRvKUsvrkVvsUfHOYyUJmhZFHwVJohSVL2X6uTqBdlZVPsD0aJtCrbXL2JuzsNvH3q91OcQ6yLV1NsCAwEAAQ==");
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
