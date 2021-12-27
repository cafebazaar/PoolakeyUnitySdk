namespace Bazaar.Poolakey
{
    public class SecurityCheck
    {
        public string rsaPublicKey;
        public static SecurityCheck Enable(string RsaPublicKey)
        {
            SecurityCheck securityCheck = new SecurityCheck();
            securityCheck.rsaPublicKey = RsaPublicKey;
            return securityCheck;
        }
        public static SecurityCheck Disable()
        {
            SecurityCheck securityCheck = new SecurityCheck();
            return securityCheck;
        }
    }
}