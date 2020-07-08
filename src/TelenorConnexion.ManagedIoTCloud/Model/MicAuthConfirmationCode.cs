using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents User Confirmation Code consisting either of a token or a username and a code.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MicAuthConfirmationCode : MicUserBasicInfo
    {
        /// <summary>
        /// The token that the user received in the confirmation e-mail. The token is sent as a parameter in the url and the client must extract this parameter and send it as-is to this action.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
        /// <summary>
        /// The confirmation code that was received in the SMS.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
