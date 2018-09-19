using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    public class CloudApiRequest<TAttributes>
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("attributes")]
        public TAttributes Attributes { get; set; }
    }
}
