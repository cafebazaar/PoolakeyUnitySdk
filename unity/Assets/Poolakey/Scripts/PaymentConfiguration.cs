namespace Poolakey.Scripts
{
    public class PaymentConfiguration
    { 
        public SecurityCheck securityCheck;
        public PaymentConfiguration(SecurityCheck securityCheck)
        {
            this.securityCheck = securityCheck;
        }
    }
}