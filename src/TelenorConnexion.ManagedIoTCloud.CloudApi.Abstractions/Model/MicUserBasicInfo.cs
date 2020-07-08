using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    /// <summary>
    /// Represents the most basic information required to identify a MIC User.
    /// </summary>
    public class MicUserBasicInfo : MicModel
    {
        /// <summary>
        /// The user name of the user.
        /// </summary>
        [JsonProperty("userName")]
        public string? Username { get; set; }
    }
}
