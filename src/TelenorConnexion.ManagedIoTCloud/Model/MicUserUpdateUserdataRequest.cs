using Newtonsoft.Json;

using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    public class MicUserUpdateUserdataRequest : MicUserBasicInfo
    {
        [JsonIgnore]
        public IDictionary<string, object?> Userdata =>
            ((IMicModel)this).AdditionalData;
    }
}
