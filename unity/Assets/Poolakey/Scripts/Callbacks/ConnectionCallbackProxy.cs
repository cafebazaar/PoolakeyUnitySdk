using System.Threading.Tasks;
using Poolakey.Scripts.Data;
using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class ConnectionCallbackProxy : BaseCallbackProxy
    {
        public ConnectionCallbackProxy() : base("com.farsitel.bazaar.callback.ConnectionCallback") { }

        void onConnect()
        {
            result = new Result<bool>(Status.Success, true, "Connection Succeed.");
        }

        void onDisconnect()
        {
            result = new Result<bool>(Status.Disconnect, true, "Connection Disconnect.");
        }

        void onFailure(string message, string stackTrace)
        {
            result = new Result<bool>(Status.Failure, false, message, stackTrace);
        }
    
        public async Task<Result<bool>> WaitForResult()
        {
            while (result == null)
                await Task.Delay(100);
            return result;
        }
    }
}