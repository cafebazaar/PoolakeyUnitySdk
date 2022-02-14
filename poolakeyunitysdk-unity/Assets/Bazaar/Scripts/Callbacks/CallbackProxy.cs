using UnityEngine;
using System.Threading.Tasks;
using Bazaar.Data;

namespace Bazaar.Callbacks
{
    public class CallbackProxy<T> : AndroidJavaProxy
    {
        public CallbackProxy(string address) : base(address) { }
        public TaskCompletionSource<Result<T>> taskCompletionSource;
    }
}