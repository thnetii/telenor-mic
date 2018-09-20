using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud
{
    [MicRequestPayloadAction("LOGIN")]
    public class MicAuthLoginRequest : IMicRequestAttributes
    {
        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
