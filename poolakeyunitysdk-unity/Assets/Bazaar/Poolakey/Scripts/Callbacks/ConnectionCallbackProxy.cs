using Bazaar.Data;
using Bazaar.Callbacks;
using System.Threading.Tasks;

namespace Bazaar.Poolakey.Callbacks
{
    public class ConnectionCallbackProxy : CallbackProxy<bool>
    {
        public ConnectionCallbackProxy() : base("com.farsitel.bazaar.callback.ConnectionCallback")
        {
            taskCompletionSource = new TaskCompletionSource<Result<bool>>();
        }

        void onConnect()
        {
            taskCompletionSource.SetResult(new Result<bool>(Status.Success, "Connection Succeed.") { data = true });
        }

        void onDisconnect()
        {
            taskCompletionSource.SetResult(new Result<bool>(Status.Disconnected, "Connection Disconnect.") { data = false });
        }

        void onFailure(string message, string stackTrace)
        {
            taskCompletionSource.SetResult(new Result<bool>(Status.Failure, message, stackTrace) { data = false });
        }
    }
}