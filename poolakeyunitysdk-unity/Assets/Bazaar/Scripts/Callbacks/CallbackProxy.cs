using UnityEngine;
using System.Threading.Tasks;
using Bazaar.Data;

namespace Bazaar.Callbacks
{
    public class CallbackProxy<T> : AndroidJavaProxy
    {
        protected Result<T> result;
        public CallbackProxy(string address) : base(address) { }

        public async Task<Result<T>> WaitForResult()
        {
            while (result == null)
                await Task.Delay(100);
            return (Result<T>)result;
        }
    }
}