using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Data
{
    public class OnlineParameters
    {
        public static OnlineParameters GetEmbedded()
        {
            var fileInfo = EmbeddedData.GetFile("online.parameters.json");
            using var stream = fileInfo.CreateReadStream();
            using var textReader = new StreamReader(stream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(textReader) { CloseInput = false };
            return JsonSerializer.CreateDefault().Deserialize<OnlineParameters>(jsonReader);
        }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
