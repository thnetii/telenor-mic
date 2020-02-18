using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary/>
    public class MicUserListMetadata
    {
        /// <summary/>
        [JsonProperty("count")]
        public MicUserListMetadataCount? Count { get; set; }
    }
}
