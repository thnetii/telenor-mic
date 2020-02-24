using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    public class MicUserdataResponse : MicResponse
    {
        [JsonProperty("userdata")]
        public IDictionary<string, object?> Userdata { get; } =
            new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
    }
}
