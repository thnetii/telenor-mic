using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents the arguments used to confirm user signup.
    /// <para>Either a <see cref="MicAuthConfirmationCode.Token"/> or a combination of <see cref="MicUserBasicInfo.Username"/> and <see cref="MicAuthConfirmationCode.Code"/> is required for signup confirmation.</para>
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MicAuthConfirmSignupRequest : MicAuthConfirmationCode
    {
    }
}
