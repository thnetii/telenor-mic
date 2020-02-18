using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents the permissions for the logged in user returned by a
    /// successful login operation.
    /// </summary>
    public class MicAuthPermissions : MicModel
    {
        /// <summary>
        /// A list of permission objects with allowed operations for the user’s
        /// role, i.e. <c>CREATE</c>, <c>READ</c>, <c>UPDATE</c> or <c>DELETE</c>.
        /// </summary>
        [JsonProperty("objects")]
        public IDictionary<string, IList<string>> Objects { get; } =
            new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
    }
}
