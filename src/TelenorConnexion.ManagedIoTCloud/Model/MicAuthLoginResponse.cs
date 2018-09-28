using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    public interface IMicAuthLoginResponse : IMicModel
    {
        /// <summary>
        /// The deatils on the logged in user.
        /// </summary>
        [JsonProperty("user")]
        MicAuthUserDetails User { get; set; }

        /// <summary>
        /// The credentials for the logged in user.
        /// </summary>
        [JsonProperty("credentials")]
        MicAuthCredentials Credentials { get; set; }

        /// <summary>
        /// The permissions that have been granted to the logged in user.
        /// </summary>
        [JsonProperty("permissions")]
        MicAuthPermissions Permissions { get; set; }
    }

    /// <inheritdoc />
    public class MicAuthLoginResponse : MicModel, IMicAuthLoginResponse
    {
        /// <inheritdoc />
        public MicAuthUserDetails User { get; set; }

        /// <inheritdoc />
        public MicAuthCredentials Credentials { get; set; }

        /// <inheritdoc />
        public MicAuthPermissions Permissions { get; set; }
    }
}
