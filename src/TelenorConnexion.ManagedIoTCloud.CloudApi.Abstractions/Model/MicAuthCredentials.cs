using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    /// <summary>
    /// Represents the credentials returned from a successfull login
    /// operation.
    /// </summary>
    public class MicAuthCredentials : MicModel
    {
        /// <summary>
        /// A Cognito IdentityId to use when communicating with AWS.
        /// </summary>
        [JsonProperty("identityId")]
        public string? IdentityId { get; set; }

        /// <summary>
        /// A OpenID Connect token to use when communicating with AWS.
        /// </summary>
        [JsonProperty("token")]
        public string? Token { get; set; }

        /// <summary>
        /// A refresh token to use for getting a new access token.
        /// </summary>
        [JsonProperty("refreshToken")]
        public string? RefreshToken { get; set; }
    }
}
