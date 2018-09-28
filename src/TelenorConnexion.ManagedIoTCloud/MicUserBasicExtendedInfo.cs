using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud
{
    public class MicUserBasicExtendedInfo : MicUserBasicInfo
    {
        /// <summary>
        /// The name of the role that the user has (<c>Read</c> | <c>ReadWrite</c>).
        /// </summary>
        [JsonProperty("roleName")]
        public string RoleName { get; set; }

        /// <summary>
        /// The first name of the user.
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// The e-mail address of the user.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
