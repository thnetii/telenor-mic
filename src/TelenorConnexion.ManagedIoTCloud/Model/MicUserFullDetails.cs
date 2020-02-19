using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents fully detailed information on a MIC User.
    /// </summary>
    public class MicUserFullDetails : MicUserBasicDetails
    {
        private readonly DuplexConversionTuple<string?, CultureInfo?> locale =
            new DuplexConversionTuple<string?, CultureInfo?>(
                rawConvert: l => l is null ? null : new CultureInfo(l),
                rawReverseConvert: ci => ci?.Name
                );

        /// <summary>
        /// The identifier of the locale used by the user.
        /// <para>
        /// This is an ISO639‑1 language code in lowercase and an ISO3166‑1 country code in uppercase, separated by a dash. For example, <c>en-US</c> or <c>sv-SE</c>. When not specified, it will default to <c>en-US</c>.
        /// </para>
        /// </summary>
        [JsonProperty("locale")]
        public string? LocaleIdentifier
        {
            get => locale.RawValue;
            set => locale.RawValue = value;
        }

        /// <summary>
        /// The Culture used by the user as inferred from <see cref="LocaleIdentifier"/>.
        /// <para>When not specified, it will default to the <c>English (United States)</c> culture.</para>
        /// </summary>
        [JsonIgnore]
        public CultureInfo? Locale
        {
            get => locale.ConvertedValue;
            set => locale.ConvertedValue = value;
        }

        /// <summary>
        /// The phone number of the user.
        /// </summary>
        [JsonProperty("phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// The company that the user works at.
        /// </summary>
        [JsonProperty("company")]
        public string? Company { get; set; }

        /// <summary>
        /// The address of the user.
        /// </summary>
        [JsonProperty("address")]
        public string? Address { get; set; }

        /// <summary>
        /// The zip code of the user.
        /// </summary>
        [JsonProperty("zip")]
        public string? ZipCode { get; set; }

        /// <summary>
        /// The city of the user.
        /// </summary>
        [JsonProperty("city")]
        public string? City { get; set; }

        /// <summary>
        /// The country of the user.
        /// </summary>
        [JsonProperty("country")]
        public string? Country { get; set; }

        /// <summary/>
        [JsonProperty("roles")]
        public string? Roles { get; set; }

        /// <summary/>
        [JsonProperty("termsAgreed")]
        public bool? TermsAgreed { get; set; }

        /// <summary/>
        [JsonProperty("dateTermsAgreed", ItemConverterType = typeof(JavaScriptDateTimeConverter))]
        public DateTimeOffset? DateTermsAgreed { get; set; }

        /// <summary/>
        [JsonProperty("termsVersion")]
        public string? TermsVersion { get; set; }

        /// <summary/>
        [JsonProperty("createdAt", ItemConverterType = typeof(JavaScriptDateTimeConverter))]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary/>
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }

        /// <summary>
        /// Additional notes field.
        /// </summary>
        [JsonProperty("notes1")]
        public string? Notes1 { get; set; }

        /// <summary>
        /// Additional notes field.
        /// </summary>
        [JsonProperty("notes2")]
        public string? Notes2 { get; set; }

        /// <summary>
        /// Additional notes field.
        /// </summary>
        [JsonProperty("notes3")]
        public string? Notes3 { get; set; }
    }
}
