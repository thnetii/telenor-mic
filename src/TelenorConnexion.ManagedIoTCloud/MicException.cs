using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace TelenorConnexion.ManagedIoTCloud
{
    [DebuggerDisplay(nameof(DebuggerDisplay) + "()")]
    public class MicException : Exception
    {
        public const string ErrorMessageKey = "errorMessage";

        public MicException() : base() { }
        public MicException(string message) : base(message) { }
        public MicException(string message, Exception innerException)
            : base(message, innerException) { }
        public MicException(MicErrorMessage errorMessage)
            : this(errorMessage?.Message) =>
            MicErrorMessage = errorMessage;

        public override string Message => MicErrorMessage?.Message ?? base.Message;

        [JsonProperty(ErrorMessageKey)]
        public MicErrorMessage MicErrorMessage { get; }

        private string DebuggerDisplay()
        {
            return JsonConvert.SerializeObject(MicErrorMessage
                ?? new MicErrorMessage { Message = Message }
                );
        }
    }
}
