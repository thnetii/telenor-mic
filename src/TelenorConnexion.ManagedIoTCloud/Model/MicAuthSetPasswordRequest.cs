using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents the arguments used for a SET_PASSWORD operation in the Auth API.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MicAuthSetPasswordRequest : MicAuthConfirmationCode
    {
        /// <summary>
        /// The new password for the user.
        /// </summary>
        [JsonProperty("password")]
        public string? Password { get; set; }
    }
}
