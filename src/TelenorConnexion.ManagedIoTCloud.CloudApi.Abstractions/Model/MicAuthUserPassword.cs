using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    /// <summary>
    /// Represents a combination of username and password.
    /// </summary>
    public class MicAuthUserPassword : MicUserBasicInfo
    {
        /// <summary>
        /// The password of the user.
        /// </summary>
        [JsonProperty("password")]
        public string? Password { get; set; }
    }
}
