using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    /// <summary>
    /// Includes the values that tell how many users are returned per category.
    /// </summary>
    public class MicUserListMetadataCount
    {
        /// <summary/>
        [JsonProperty("pending")]
        public int Pending { get; set; }
        /// <summary/>
        [JsonProperty("active")]
        public int Active { get; set; }
        /// <summary/>
        [JsonProperty("all")]
        public int All { get; set; }
        /// <summary/>
        [JsonProperty("unconfirmed")]
        public int Unconfirmed { get; set; }
    }
}
