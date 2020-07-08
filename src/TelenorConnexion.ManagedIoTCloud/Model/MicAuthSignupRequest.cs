using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents the attributes of the requrest to sign up a new MIC User.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MicAuthSignupRequest : MicUserFullDetails
    {
        /// <summary>
        /// The password of the user.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// If the user has to give consent to terms and conditions before signing up, they need to set this boolean to true.
        /// Required if if the account requires consent, otherwise optional.
        /// <para>Refer to the <see cref="IMicClient.AuthGiveConsent"/> endpoint for more details.</para>
        /// </summary>
        [JsonProperty("consent")]
        public bool? RequireConsent { get; set; }
    }
}
