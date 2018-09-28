using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    public interface IMicAuthLoginRequest : IMicUserBasicInfo
    {
        /// <summary>
        /// The password of the user.
        /// </summary>
        [JsonProperty("password")]
        string Password { get; set; }
    }

    /// <inheritdoc />
    public class MicAuthLoginRequest : MicUserBasicInfo, IMicAuthLoginRequest
    {
        /// <inheritdoc />
        public string Password { get; set; }
    }
}
