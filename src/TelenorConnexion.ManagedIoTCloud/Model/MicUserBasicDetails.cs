using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents basic details of the information about a MIC User.
    /// </summary>
    public class MicUserBasicDetails : MicUserBasicInfo
    {
        /// <summary>
        /// The first name of the user.
        /// </summary>
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        /// <summary>
        /// The e-mail address of the user.
        /// </summary>
        [JsonProperty("email")]
        public string? Email { get; set; }

        /// <summary>
        /// The name of the role that the user has (<c>Read</c> | <c>ReadWrite</c>).
        /// </summary>
        [JsonProperty("roleName")]
        public string? RoleName { get; set; }

        /// <summary>
        /// The name of the domain that the user is assigned to.
        /// </summary>
        [JsonProperty("domainName")]
        public string? DomainName { get; set; }
    }
}
