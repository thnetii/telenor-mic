using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.AuthApi
{
    public class AuthLoginRequest : CloudApiRequest<AuthLoginRequestAttributes>
    {
    }

    public class AuthLoginRequestAttributes
    {
        public const string Action = "LOGIN";

        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
