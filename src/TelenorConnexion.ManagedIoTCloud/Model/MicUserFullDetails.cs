using Newtonsoft.Json;
using System.Globalization;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents fully detailed information on a MIC User.
    /// </summary>
    public class MicUserFullDetails : MicUserBasicDetails
    {
        private readonly DuplexConversionTuple<string, CultureInfo> locale =
            new DuplexConversionTuple<string, CultureInfo>(
                rawConvert: l => new CultureInfo(l),
                rawReverseConvert: ci => ci?.Name
                );

        /// <summary>
        /// The identifier of the locale used by the user.
        /// </summary>
        [JsonProperty("locale")]
        public string LocaleIdentifier
        {
            get => locale.RawValue;
            set => locale.RawValue = value;
        }

        [JsonIgnore]
        public CultureInfo Locale
        {
            get => locale.ConvertedValue;
            set => locale.ConvertedValue = value;
        }

        /// <summary>
        /// The phone number of the user.
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// The company that the user works at.
        /// </summary>
        [JsonProperty("company")]
        public string Company { get; set; }

        /// <summary>
        /// The address of the user.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// The zip code of the user.
        /// </summary>
        [JsonProperty("zip")]
        public string ZipCode { get; set; }

        /// <summary>
        /// The city of the user.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// The country of the user.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
