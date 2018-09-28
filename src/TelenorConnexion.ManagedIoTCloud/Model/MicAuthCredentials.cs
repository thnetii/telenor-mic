using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents the credentials returned from a successfull login
    /// operation.
    /// </summary>
    public interface IMicAuthCredentials : IMicModel
    {
        /// <summary>
        /// A Cognito IdentityId to use when communicating with AWS.
        /// </summary>
        [JsonProperty("identityId")]
        string IdentityId { get; set; }

        /// <summary>
        /// A OpenID Connect token to use when communicating with AWS.
        /// </summary>
        [JsonProperty("token")]
        string Token { get; set; }

        /// <summary>
        /// A refresh token to use for getting a new access token.
        /// </summary>
        [JsonProperty("refreshToken")]
        string RefreshToken { get; set; }
    }

    /// <inheritdoc />
    public class MicAuthCredentials : MicModel, IMicAuthCredentials
    {
        /// <inheritdoc />
        public string IdentityId { get; set; }

        /// <inheritdoc />
        public string Token { get; set; }

        /// <inheritdoc />
        public string RefreshToken { get; set; }
    }
}
