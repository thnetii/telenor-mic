using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    public class MicAuthGetConsentDocumentResponse : MicModel
    {
        private readonly DuplexConversionTuple<string?, Uri?> url = new DuplexConversionTuple<string?, Uri?>(
            s => string.IsNullOrWhiteSpace(s) ? null : new Uri(s), u => u?.ToString()
            );

        /// <summary>
        /// The URL to use to get the file. This will give you temporary access to the file.
        /// </summary>
        [JsonProperty("url")]
        [SuppressMessage("Design", "CA1056: Uri properties should not be strings")]
        public string? Url
        {
            get => url.RawValue;
            set => url.RawValue = value;
        }

        /// <summary>
        /// The URL to use to get the file as a <see cref="System.Uri"/> instance. This will give you temporary access to the file.
        /// </summary>
        /// <value>
        /// A <see cref="System.Uri"/> instance constructed from the value of <see cref="Url"/> or <c>null</c>
        /// if <see cref="Url"/> is either <c>null</c>, empty or contains only whitespace characters.
        /// </value>
        [JsonIgnore]
        public Uri? Uri
        {
            get => url.ConvertedValue;
            set => url.ConvertedValue = value;
        }

        /// <summary>
        /// The custom version of the file.
        /// </summary>
        [JsonProperty("version")]
        public string? Version { get; set; }
    }
}
