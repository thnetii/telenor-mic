using Newtonsoft.Json;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud
{
    public class MicAuthLoginResponse
    {
        [JsonProperty("user")]
        public MicAuthLoginUser User { get; set; }

        [JsonProperty("credentials")]
        public MicAuthLoginCredentials Credentials { get; set; }

        [JsonProperty("permissions")]
        public MicAuthLoginPermissions Permissions { get; set; }

        [JsonExtensionData]
        internal IDictionary<string, object> AdditionalData { get; set; }
    }
}
