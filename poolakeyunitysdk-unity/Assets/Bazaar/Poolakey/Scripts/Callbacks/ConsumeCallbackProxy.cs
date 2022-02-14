using Bazaar.Data;
using Bazaar.Callbacks;
using System.Threading.Tasks;

namespace Bazaar.Poolakey.Callbacks
{
    public class ConsumeCallbackProxy : CallbackProxy<bool>
    {
        public ConsumeCallbackProxy() : base("com.farsitel.bazaar.callback.ConsumeCallback")
        {
            taskCompletionSource = new TaskCompletionSource<Result<bool>>();
        }

        void onSuccess()
        {
            taskCompletionSource.SetResult(new Result<bool>(Status.Success, "Consumption Succeed.") { data = true });
        }

        void onFailure(string message, string stackTrace)
        {
            taskCompletionSource.SetResult(new Result<bool>(Status.Failure, message, stackTrace) { data = false });
        }
    }
}