using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    /// <summary>
    /// The base contratc definition for types used by MIC as Data-Transfer-Objects.
    /// </summary>
    public interface IMicModel
    {
        /// <summary>
        /// Represents additional data that is not recognized by the original
        /// model definition.
        /// </summary>
        [JsonExtensionData]
        IDictionary<string, object?> AdditionalData { get; }
    }

    /// <summary>
    /// The base definition for types used by MIC as Data-Transfer-Objects.
    /// </summary>
    public abstract class MicModel : IMicModel
    {
        [JsonExtensionData]
        IDictionary<string, object?> IMicModel.AdditionalData { get; } =
            new Dictionary<string, object?>(StringComparer.Ordinal);
    }
}
