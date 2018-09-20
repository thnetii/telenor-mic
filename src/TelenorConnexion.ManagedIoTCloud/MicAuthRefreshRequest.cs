using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud
{
    [MicRequestPayloadAction("REFRESH")]
    public class MicAuthRefreshRequest : IMicRequestAttributes
    {
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
