using Bazaar.Data;
using Bazaar.Callbacks;

namespace Bazaar.Poolakey.Callbacks
{
    public class ConnectionCallbackProxy : CallbackProxy<bool>
    {
        public ConnectionCallbackProxy() : base("com.farsitel.bazaar.callback.ConnectionCallback") { }

        void onConnect()
        {
            result = new Result<bool>(Status.Success, "Connection Succeed.") { data = true };
        }

        void onDisconnect()
        {
            result = new Result<bool>(Status.Disconnected, "Connection Disconnect.") { data = false };
        }

        void onFailure(string message, string stackTrace)
        {
            result = new Result<bool>(Status.Failure, message, stackTrace) { data = false };
        }
    }
}