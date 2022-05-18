using UnityEngine;
using Bazaar.Data;
using Bazaar.Callbacks;
using Bazaar.Poolakey.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazaar.Poolakey.Callbacks{

    public class TrialSubscriptionCallbackProxy : CallbackProxy<TRIALDetail>
    {
        public TrialSubscriptionCallbackProxy() : base("com.farsitel.bazaar.callback.TrialSubscriptionCallback")
        {
             taskCompletionSource = new TaskCompletionSource<Result<TRIALDetail>>();
        }

        void onSuccess(AndroidJavaObject result)
        {
        var tRIALDetail = new TRIALDetail(result);
            taskCompletionSource.SetResult(new Result<TRIALDetail>(Status.Success, "Get TRialState completed.") { data = tRIALDetail });
        }

        void onFailure(string message, string stackTrace)
        {
           
            taskCompletionSource.SetResult(new Result<TRIALDetail>(Status.Failure, message, stackTrace));
        }
    }
}