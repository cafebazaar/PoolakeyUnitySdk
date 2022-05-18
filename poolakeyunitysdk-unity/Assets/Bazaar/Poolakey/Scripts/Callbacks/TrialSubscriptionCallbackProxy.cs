using Bazaar.Data;
using Bazaar.Callbacks;
using Bazaar.Poolakey.Data;
using System.Threading.Tasks;
using System;

namespace Bazaar.Poolakey.Callbacks
{

    public class TrialSubscriptionCallbackProxy : CallbackProxy<SKUDetails>
    {
        private SKUDetails trialSubscription;

        public TrialSubscriptionCallbackProxy(SKUDetails trialSubscription) : base("com.farsitel.bazaar.callback.TrialSubscriptionCallback")
        {
            this.trialSubscription = trialSubscription;
            taskCompletionSource = new TaskCompletionSource<Result<SKUDetails>>();
        }

        void onSuccess(bool isAvailable, int trialPeriodDays)
        {
            DateTime date = DateTime.Today;
            trialSubscription.subscriptionExpireDate = date.AddDays(trialPeriodDays);
            trialSubscription.isAvailable = isAvailable;
            if (isAvailable)
            {
                trialSubscription.description = $"For {trialPeriodDays} days.";
            }
            taskCompletionSource.SetResult(new Result<SKUDetails>(Status.Success, "Get TrialState completed.") { data = trialSubscription });
        }

        void onFailure(string message, string stackTrace)
        {
            taskCompletionSource.SetResult(new Result<SKUDetails>(Status.Failure, message, stackTrace));
        }
    }
}