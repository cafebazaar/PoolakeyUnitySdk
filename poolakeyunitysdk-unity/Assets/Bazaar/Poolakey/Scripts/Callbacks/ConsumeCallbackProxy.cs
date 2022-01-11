using Bazaar.Data;
using Bazaar.Callbacks;

namespace Bazaar.Poolakey.Callbacks
{
    public class ConsumeCallbackProxy : CallbackProxy<bool>
    {
        public ConsumeCallbackProxy() : base("com.farsitel.bazaar.callback.ConsumeCallback") { }

        void onSuccess()
        {
            result = new Result<bool>(Status.Success, "Consumption Succeed.") { data = true };
        }

        void onFailure(string message, string stackTrace)
        {
            result = new Result<bool>(Status.Failure, message, stackTrace) { data = false };
        }
    }
}