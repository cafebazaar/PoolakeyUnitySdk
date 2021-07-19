using System.Threading.Tasks;
using Poolakey.Scripts.Data;
using UnityEngine;

namespace Poolakey.Scripts.Callbacks
{
    public class BaseCallbackProxy : AndroidJavaProxy
    {
        protected Result result;
        public BaseCallbackProxy(string address) : base(address) { }

    }
}