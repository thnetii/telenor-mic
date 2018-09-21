using Newtonsoft.Json;
using System.Diagnostics;

namespace TelenorConnexion.ManagedIoTCloud
{
    /// <summary>
    /// Represents a request to the MIC API.
    /// </summary>
    public class MicRequest
    {
        /// <summary>
        /// Initializes a new Cloud API request object with the specified action
        /// and argument attributes.
        /// </summary>
        /// <param name="action">The action to perform. May be <c>null</c> if set after creation.</param>
        /// <param name="attributes">The argument data of the request. May be <c>null</c> if set after creation.</param>
        [DebuggerStepThrough]
        protected MicRequest(string action, IMicRequestAttributes attributes)
            : base() => (Action, Attributes) = (action, attributes);

        [DebuggerStepThrough]
        internal MicRequest() : this(default, default) { }

        /// <summary>
        /// The action to perform on the endpoint the request is sent to.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>
        /// The attributes or arguments for the action to perform.
        /// </summary>
        [JsonProperty("attributes")]
        public IMicRequestAttributes Attributes { get; set; }
    }
}
