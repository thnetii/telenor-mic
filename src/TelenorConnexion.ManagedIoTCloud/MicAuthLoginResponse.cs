using Newtonsoft.Json;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud
{
    /// <summary>
    /// Represents the full response returned from a successful login operation.
    /// </summary>
    public class MicAuthLoginResponse
    {
        /// <summary>
        /// The deatils on the logged in user.
        /// </summary>
        [JsonProperty("user")]
        public MicAuthLoginUser User { get; set; }

        /// <summary>
        /// The AWS credentials for the logged in user.
        /// </summary>
        [JsonProperty("credentials")]
        public MicAuthLoginCredentials Credentials { get; set; }

        /// <summary>
        /// The permissions that have been granted to the logged in user.
        /// </summary>
        [JsonProperty("permissions")]
        public MicAuthLoginPermissions Permissions { get; set; }

        [JsonExtensionData]
        internal IDictionary<string, object> AdditionalData { get; set; }
    }
}
