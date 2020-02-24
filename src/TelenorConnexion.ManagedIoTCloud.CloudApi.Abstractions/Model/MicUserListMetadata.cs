using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    /// <summary/>
    public class MicUserListMetadata
    {
        /// <summary/>
        [JsonProperty("count")]
        public MicUserListMetadataCount? Count { get; set; }
    }
}
