using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    public class CloudApiErrorMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("messageKey")]
        public string MessageKey { get; set; }

        [JsonProperty("messageParams")]
        public IDictionary<string, JToken> Parameters { get; set; }

        [JsonProperty("property")]
        public string Property { get; set; }
    }
}
