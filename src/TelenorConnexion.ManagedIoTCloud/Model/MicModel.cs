using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    public interface IMicModel
    {
        [JsonExtensionData]
        IDictionary<string, object> AdditionalData { get; }
    }

    public abstract class MicModel : IMicModel
    {
        [JsonExtensionData]
        IDictionary<string, object> IMicModel.AdditionalData { get; } =
            new Dictionary<string, object>(StringComparer.Ordinal);
    }
}
