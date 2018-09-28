using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    public interface IMicUserBasicInfo : IMicModel
    {
        /// <summary>
        /// The user name of the user.
        /// </summary>
        [JsonProperty("userName")]
        string Username { get; set; }
    }

    /// <inheritdoc />
    public class MicUserBasicInfo : MicModel, IMicUserBasicInfo
    {
        /// <inheritdoc />
        public string Username { get; set; }
    }
}
