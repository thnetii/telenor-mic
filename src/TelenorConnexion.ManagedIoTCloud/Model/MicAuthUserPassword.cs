using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    public class MicAuthUserPassword : MicUserBasicInfo
    {
        /// <summary>
        /// The password of the user.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
