namespace Poolakey.Scripts.Data
{
    public enum Status { Success, Cancel, Disconnect, Failure, Unknown }
    public class Result<T>
    {
        public Status status;
        public string message;
        public T data;
        public string stackTrace;

        public Result(Status status, T data, string message, string stackTrace = null)
        {
            this.status = status;
            this.message = message;
            this.data = data;
            this.stackTrace = stackTrace;
        }
    }
}