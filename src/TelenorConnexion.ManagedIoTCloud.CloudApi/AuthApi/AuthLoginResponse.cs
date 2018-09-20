using Newtonsoft.Json;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.AuthApi
{
    public class AuthLoginResponse
    {
        [JsonProperty("user")]
        public AuthLoginUser User { get; set; }

        [JsonProperty("credentials")]
        public AuthLoginCredentials Credentials { get; set; }

        [JsonProperty("permissions")]
        public AuthLoginPermissions Permissions { get; set; }

        [JsonExtensionData]
        internal IDictionary<string, object> AdditionalData { get; set; }
    }
}
