using System.Threading.Tasks;
using Poolakey.Scripts.Data;
using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class ConsumeCallbackProxy : BaseCallbackProxy
    {
        public ConsumeCallbackProxy() : base("com.farsitel.bazaar.callback.ConsumeCallback") { }

        void onSuccess()
        {
            result = new Result(Status.Success, "Consumption Succeed.");
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