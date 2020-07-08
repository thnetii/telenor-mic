using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MicUserCreateRequest : MicUserFullDetails
    {
    }
}
