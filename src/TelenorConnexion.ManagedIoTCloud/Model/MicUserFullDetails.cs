using Newtonsoft.Json;
using System.Globalization;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    public interface IMicUserFullDetails : IMicUserBasicDetails
    {
        /// <summary>
        /// The locale used by the user.
        /// </summary>
        [JsonProperty("locale")]
        string Locale { get; set; }

        /// <summary>
        /// The phone number of the user.
        /// </summary>
        [JsonProperty("phone")]
        string Phone { get; set; }

        /// <summary>
        /// The company that the user works at.
        /// </summary>
        [JsonProperty("company")]
        string Company { get; set; }

        /// <summary>
        /// The address of the user.
        /// </summary>
        [JsonProperty("address")]
        string Address { get; set; }

        /// <summary>
        /// The zip code of the user.
        /// </summary>
        [JsonProperty("zip")]
        string ZipCode { get; set; }

        /// <summary>
        /// The city of the user.
        /// </summary>
        [JsonProperty("city")]
        string City { get; set; }

        /// <summary>
        /// The country of the user.
        /// </summary>
        [JsonProperty("country")]
        string Country { get; set; }
    }

    /// <inheritdoc />
    public class MicUserFullDetails : MicUserBasicDetails, IMicUserFullDetails
    {
        private readonly DuplexConversionTuple<string, CultureInfo> locale =
            new DuplexConversionTuple<string, CultureInfo>(
                rawConvert: l => new CultureInfo(l),
                rawReverseConvert: ci => ci?.Name
                );

        string IMicUserFullDetails.Locale
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
        /// <inheritdoc />
        public string Phone { get; set; }
        /// <inheritdoc />
        public string Company { get; set; }
        /// <inheritdoc />
        public string Address { get; set; }
        /// <inheritdoc />
        public string ZipCode { get; set; }
        /// <inheritdoc />
        public string City { get; set; }
        /// <inheritdoc />
        public string Country { get; set; }
    }
}
