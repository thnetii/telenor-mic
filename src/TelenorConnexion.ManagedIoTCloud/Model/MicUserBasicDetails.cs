using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    public interface IMicUserBasicDetails : IMicUserBasicInfo
    {
        /// <summary>
        /// The name of the role that the user has (<c>Read</c> | <c>ReadWrite</c>).
        /// </summary>
        [JsonProperty("roleName")]
        string RoleName { get; set; }

        /// <summary>
        /// The first name of the user.
        /// </summary>
        [JsonProperty("firstName")]
        string FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        [JsonProperty("lastName")]
        string LastName { get; set; }

        /// <summary>
        /// The e-mail address of the user.
        /// </summary>
        [JsonProperty("email")]
        string Email { get; set; }
    }

    /// <inheritdoc />
    public class MicUserBasicDetails : MicUserBasicInfo, IMicUserBasicDetails
    {
        /// <inheritdoc />
        public string RoleName { get; set; }

        /// <inheritdoc />
        public string FirstName { get; set; }

        /// <inheritdoc />
        public string LastName { get; set; }

        /// <inheritdoc />
        public string Email { get; set; }
    }
}
