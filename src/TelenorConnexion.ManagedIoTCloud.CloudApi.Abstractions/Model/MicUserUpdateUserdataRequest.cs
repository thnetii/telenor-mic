using Newtonsoft.Json;

using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    public class MicUserUpdateUserdataRequest : MicUserBasicInfo
    {
        [JsonIgnore]
        public IDictionary<string, object?> Userdata =>
            ((IMicModel)this).AdditionalData;
    }
}
