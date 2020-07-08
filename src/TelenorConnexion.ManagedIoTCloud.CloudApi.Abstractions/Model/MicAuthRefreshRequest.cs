using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    /// <summary>
    /// Represents the arguments to perform an authentication refresh operation.
    /// </summary>
    public class MicAuthRefreshRequest : MicModel
    {
        /// <summary>
        /// The refresh token acquired when logging in.
        /// </summary>
        [JsonProperty("refreshToken")]
        public string? RefreshToken { get; set; }
    }
}
