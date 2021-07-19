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
            result = new Result(Status.Success, "Connection Succeed.");
        }

        void onDisconnect()
        {
            result = new Result(Status.Disconnect, "Connection Disconnect.");
        }

        void onFailure(string message, string stackTrace)
        {
            result = new Result(Status.Failure, message, stackTrace);
        }
    
        public async Task<Result<bool>> WaitForResult()
        {
            while (result == null)
                await Task.Delay(100);
            return result;
        }
    }
}