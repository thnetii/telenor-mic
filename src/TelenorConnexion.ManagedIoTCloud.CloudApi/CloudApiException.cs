using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    [DebuggerDisplay(nameof(DebuggerDisplay) + "()")]
    public class CloudApiException : Exception
    {
        public const string ErrorMessageKey = "errorMessage";

        public CloudApiException() : base() { }
        public CloudApiException(string message) : base(message) { }
        public CloudApiException(string message, Exception innerException)
            : base(message, innerException) { }
        public CloudApiException(CloudApiErrorMessage errorMessage)
            : this(errorMessage?.Message) =>
            ErrorJson = errorMessage;

        public override string Message => ErrorJson?.Message ?? base.Message;

        [JsonProperty(ErrorMessageKey)]
        public CloudApiErrorMessage ErrorJson { get; }

        private string DebuggerDisplay()
        {
            return JsonConvert.SerializeObject(ErrorJson
                ?? new CloudApiErrorMessage { Message = Message }
                );
        }
    }
}
