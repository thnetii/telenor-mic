using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud
{
    /// <summary>
    /// Represents the arguments to perform a login operation.
    /// </summary>
    [MicRequestPayloadAction("LOGIN")]
    public class MicAuthLoginRequest : IMicRequestAttributes
    {
        /// <summary>
        /// The user name of the user.
        /// </summary>
        [JsonProperty("userName")]
        public string Username { get; set; }

        /// <summary>
        /// The password of the user.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
