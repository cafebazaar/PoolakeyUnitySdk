using Bazaar.Data;
using Bazaar.Callbacks;
using Bazaar.Poolakey.Data;
using System.Threading.Tasks;

namespace Bazaar.Poolakey.Callbacks
{

    public class TrialSubscriptionCallbackProxy : CallbackProxy<TrialDetails>
    {
        public TrialSubscriptionCallbackProxy() : base("com.farsitel.bazaar.callback.TrialSubscriptionCallback")
        {
            taskCompletionSource = new TaskCompletionSource<Result<TrialDetails>>();
        }

        void onSuccess(bool isAvailable, int trialPeriodDays)
        {
            var TrialDetails = new TrialDetails { isAvailable = isAvailable, trialPeriodDays = trialPeriodDays };
            taskCompletionSource.SetResult(new Result<TrialDetails>(Status.Success, "Get TrialState completed.") { data = TrialDetails });
        }

        void onFailure(string message, string stackTrace)
        {
            taskCompletionSource.SetResult(new Result<TrialDetails>(Status.Failure, message, stackTrace));
        }
    }
}