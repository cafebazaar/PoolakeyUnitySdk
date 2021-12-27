using Poolakey.Scripts.Data;

namespace Poolakey.Scripts.Callbacks
{
    public class ConnectionCallbackProxy : BaseCallbackProxy<bool>
    {
        public ConnectionCallbackProxy() : base("com.farsitel.bazaar.callback.ConnectionCallback") { }

        void onConnect()
        {
            result = new Result<bool>(Status.Success, "Connection Succeed.");
            result.data = true;
        }

        void onDisconnect()
        {
            result = new Result<bool>(Status.Disconnect, "Connection Disconnect.");
            result.data = false;
        }

        void onFailure(string message, string stackTrace)
        {
            result = new Result<bool>(Status.Failure, message, stackTrace);
            result.data = false;
        }
    }
}