using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents the arguments used to confirm user signup.
    /// <para>Either a <see cref="Token"/> or a combination of <see cref="IMicUserBasicInfo.Username"/> and <see cref="Code"/> is required for signup confirmation.</para>
    /// </summary>
    public interface IMicAuthConfirmSignupRequest : IMicUserBasicInfo
    {
        /// <summary>
        /// The token that the user received in the confirmation e-mail. The token is sent as a parameter in the url and the client must extract this parameter and send it as-is to this action.
        /// </summary>
        [JsonProperty("token")]
        string Token { get; set; }

        /// <summary>
        /// The confirmation code that was received in the SMS.
        /// </summary>
        [JsonProperty("code")]
        string Code { get; set; }
    }

    /// <inheritdoc />
    public class MicAuthConfirmSignupRequest : MicUserBasicInfo, IMicAuthConfirmSignupRequest
    {
        /// <inheritdoc />
        public string Token { get; set; }
        /// <inheritdoc />
        public string Code { get; set; }
    }
}
