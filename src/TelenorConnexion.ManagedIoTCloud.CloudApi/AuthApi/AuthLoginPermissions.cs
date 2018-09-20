using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.AuthApi
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix")]
    public class AuthLoginPermissions
    {
        /// <summary>
        /// A list of permission objects with allowed operations for the user’s
        /// role, i.e. <c>CREATE</c>, <c>READ</c>, <c>UPDATE</c> or <c>DELETE</c>.
        /// </summary>
        [JsonProperty("objects")]
        public IDictionary<string, IList<string>> Objects { get; set; }
    }
}
