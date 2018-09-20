using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.AuthApi
{
    internal class AuthRefreshRequest : ICloudApiRequestAttributes
    {
        public const string Action = "REFRESH";

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        public CloudApiRequest CreateRequest() =>
            new CloudApiRequest
            {
                Action = Action,
                Attributes = this
            };
    }
}
