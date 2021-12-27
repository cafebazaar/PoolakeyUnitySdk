using Poolakey.Scripts.Data;

namespace Poolakey.Scripts.Callbacks
{
    public class ConsumeCallbackProxy : BaseCallbackProxy<bool>
    {
        public ConsumeCallbackProxy() : base("com.farsitel.bazaar.callback.ConsumeCallback") { }

        void onSuccess()
        {
            result = new Result<bool>(Status.Success, "Consumption Succeed.");
            result.data = true;
        }

        void onFailure(string message, string stackTrace)
        {
            result = new Result<bool>(Status.Failure, message, stackTrace);
            result.data = false;
        }
    }
}