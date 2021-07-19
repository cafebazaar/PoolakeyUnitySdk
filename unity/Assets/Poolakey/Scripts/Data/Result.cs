using System.Collections.Generic;

namespace Poolakey.Scripts.Data
{
    public enum Status { Success, Cancel, Disconnect, Failure, Unknown }
    public class Result
    {
        public Status status;
        public string message;
        public string stackTrace;

        public Result(Status status, string message, string stackTrace = null)
        {
            this.status = status;
            this.message = message;
            this.stackTrace = stackTrace;
        }
    }

    public class SKUDetailsResult : Result
    {
        public List<SKUDetails> data;
        public SKUDetailsResult(Status status, List<SKUDetails> data, string message, string stackTrace = null) : base(status, message, stackTrace) 
        {
            this.data = data;
        }
    }
    public class PurchaseResult : Result
    {
        public PurchaseInfo data;
        public PurchaseResult(Status status, PurchaseInfo data, string message, string stackTrace = null) : base(status, message, stackTrace) 
        {
            this.data = data;
        }
    }
}