using Poolakey.Scripts.Data;

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
    }
}