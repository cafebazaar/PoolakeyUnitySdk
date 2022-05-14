using UnityEngine;

namespace Bazaar.Poolakey.Data
{
    public class TrialDetails
    {
        public bool isAvailable;
        public int trialPeriodDays;
        public TrialDetails() { }
        public TrialDetails(AndroidJavaObject entity)
        {
            this.isAvailable = entity.Get<bool>("isAvailable");
            this.trialPeriodDays = entity.Get<int>("trialPeriodDays");
        }

        override public string ToString() => $"isAvilable: {isAvailable}, trialPeriodDays: {trialPeriodDays}";
    }
}