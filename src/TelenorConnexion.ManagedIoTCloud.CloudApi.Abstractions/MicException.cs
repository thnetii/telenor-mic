using Newtonsoft.Json;

using System;
using System.Diagnostics;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    /// <summary>
    /// Throwable wrapper around a <see cref="ManagedIoTCloud.MicErrorMessage"/>
    /// instance.
    /// </summary>
    [DebuggerDisplay(nameof(DebuggerDisplay) + "()")]
    public class MicException : Exception
    {
        /// <summary>
        /// The JSON property name to look for in a returned response payload,
        /// to indicate whether an error occurred or not.
        /// </summary>
        public const string ErrorMessageKey = "errorMessage";

        /// <summary>
        /// Creats a new <see cref="MicException"/> instance without any associated
        /// error message.
        /// </summary>
        public MicException() : base() { }
        /// <summary>
        /// Creats a new <see cref="MicException"/> instance with the specified
        /// string containing the error message.
        /// </summary>
        /// <param name="message"></param>
        public MicException(string message) : base(message) { }
        /// <summary>
        /// Creates a new <see cref="MicException"/> instance with the specified
        /// string containing the error message and a reference to the inner
        /// exception that is the cause of the error.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MicException(string message, Exception innerException)
            : base(message, innerException) { }
        /// <summary>
        /// Creats a new <see cref="MicException"/> instance with the specified
        /// error message.
        /// </summary>
        /// <param name="errorMessage"></param>
        public MicException(MicErrorMessage errorMessage)
            : this(errorMessage?.Message!) =>
            MicErrorMessage = errorMessage;
        /// <summary>
        /// Creats a new <see cref="MicException"/> instance with the specified
        /// error message and a reference to the inner exception that is the
        /// cause of the error.
        /// </summary>
        public MicException(MicErrorMessage errorMessage, Exception innerException)
            : this(errorMessage?.Message!, innerException) =>
            MicErrorMessage = errorMessage;

        /// <summary>
        /// Gets a message that describes the error that occurred.
        /// </summary>
        /// <value>
        /// If a <see cref="ManagedIoTCloud.MicErrorMessage"/> is associated
        /// with this instance, the returned value is equal to <see cref="MicErrorMessage.Message"/>.
        /// Otherwise, it is equal to the message passed to the constructor when
        /// this instance was created.
        /// </value>
        public override string Message => MicErrorMessage?.Message ?? base.Message;

        /// <summary>
        /// The full error message returned from the MIC API that gives further
        /// detail on the error that ocurred.
        /// </summary>
        [JsonProperty(ErrorMessageKey)]
        public MicErrorMessage? MicErrorMessage { get; }

        private string DebuggerDisplay()
        {
            return JsonConvert.SerializeObject(MicErrorMessage
                ?? new MicErrorMessage { Message = Message }
                );
        }
    }
}
