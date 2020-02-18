using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud
{
    /// <summary>
    /// Represents the details of an error that occurred during execution of
    /// a request to the MIC API.
    /// </summary>
    public class MicErrorMessage
    {
        /// <summary>
        /// A text representation of the error.
        /// </summary>
        /// <remarks>
        /// It is the only field that is always present and it is also used for
        /// unknown errors that may occur.S
        /// </remarks>
        [JsonProperty("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Used to localize the message and the possible values are described
        /// with each action.
        /// </summary>
        /// <remarks>
        /// The <see cref="MicErrorMessageKey"/> class contains possible
        /// contant values for this property.
        /// </remarks>
        [JsonProperty("messageKey")]
        public string? MessageKey { get; set; }

        /// <summary>
        /// Used to provide extra information about the error.
        /// </summary>
        [JsonProperty("messageParams")]
        public IDictionary<string, object?> Parameters { get; } =
            new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Contains the JSON name of the property if the error is related to a
        /// specific property.
        /// </summary>
        [JsonProperty("property")]
        public string? Property { get; set; }
    }
}
