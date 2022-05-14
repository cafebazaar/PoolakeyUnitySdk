using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bazaar.Poolakey.Data
{
public class TRIALDetail
{
        public bool isAvailable;
        public int trialPeriodDays;
        public TRIALDetail() { }
        public TRIALDetail(AndroidJavaObject entity)
        {
            this.isAvailable = entity.Get<bool>("isAvailable");
            this.trialPeriodDays = entity.Get<int>("trialPeriodDays");
        }

        override public string ToString() => $"isAvilable: {isAvailable}, trialPeriodDays: {trialPeriodDays}";
    }
}