using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    /// <summary>
    /// Represents the response received from a successful login operation.
    /// </summary>
    public class MicAuthLoginResponse : MicModel
    {
        /// <summary>
        /// The deatils on the logged in user.
        /// </summary>
        [JsonProperty("user")]
        public MicAuthUserDetails? User { get; set; }

        /// <summary>
        /// The credentials for the logged in user.
        /// </summary>
        [JsonProperty("credentials")]
        public MicAuthCredentials? Credentials { get; set; }

        /// <summary>
        /// The permissions that have been granted to the logged in user.
        /// </summary>
        [JsonProperty("permissions")]
        public MicAuthPermissions? Permissions { get; set; }
    }
}
