using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud
{
    /// <summary>
    /// Represents the arguments to perform an authentication refresh operation.
    /// </summary>
    [MicRequestPayloadAction("REFRESH")]
    public class MicAuthRefreshRequest : IMicRequestAttributes
    {
        /// <summary>
        /// The refresh token acquired when logging in.
        /// </summary>
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
