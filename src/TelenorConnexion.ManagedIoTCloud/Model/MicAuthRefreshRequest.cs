using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents the arguments to perform an authentication refresh operation.
    /// </summary>
    public interface IMicAuthRefreshRequest : IMicModel
    {
        /// <summary>
        /// The refresh token acquired when logging in.
        /// </summary>
        [JsonProperty("refreshToken")]
        string RefreshToken { get; set; }
    }

    /// <inheritdoc />
    public class MicAuthRefreshRequest : MicModel, IMicAuthRefreshRequest
    {
        /// <inheritdoc />
        public string RefreshToken { get; set; }
    }
}
