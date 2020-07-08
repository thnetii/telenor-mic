using Newtonsoft.Json;

using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    /// <summary/>
    public class MicUserListResponse : MicModel
    {
        /// <summary>
        /// The list of users in the chosen category (active by default)
        /// </summary>
        [JsonProperty("users")]
        public IList<MicUserFullDetails> Users { get; } =
            new List<MicUserFullDetails>();

        /// <summary>
        /// Number of pages available, support for pagination. If size was not set this will return <c>1</c>.
        /// </summary>
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// The actual page returned. Will be set to <c>1</c> if neither the paging size nor a specific page number was requested.
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary/>
        [JsonProperty("metadata")]
        public MicUserListMetadata? Metadata { get; set; }
    }
}
