using System.IO;
using System.Text;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Data
{
    public class OnlineParameters
    {
        public static OnlineParameters GetFromUserSecrets()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<OnlineParameters>()
                .Build();
            var instance = new OnlineParameters();
            config.Bind(ConfigurationPath.Combine("TelenorMic", "Credentials"),
                instance);
            return instance;
        }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
