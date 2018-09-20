using Newtonsoft.Json;
using System.Diagnostics;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    public class CloudApiRequest
    {
        /// <summary>
        /// Initializes a new Cloud API request object with the specified action
        /// and argument attributes.
        /// </summary>
        /// <param name="action">The action to perform. May be <c>null</c> if set after creation.</param>
        /// <param name="attributes">The argument data of the request. May be <c>null</c> if set after creation.</param>
        protected CloudApiRequest(string action, object attributes) : base() =>
            (Action, Attributes) = (action, attributes);

        [DebuggerStepThrough]
        internal CloudApiRequest() : this(default, default) { }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("attributes")]
        public object Attributes { get; set; }
    }
}
