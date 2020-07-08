using Newtonsoft.Json;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MicUserCreateRequest : MicUserFullDetails
    {
    }
}
