using Newtonsoft.Json;

using System.Globalization;

using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    public class MicUserUpdateRequest : MicUserBasicInfo
    {
        internal bool ShouldSerializeUsername() => false;

        /// <inheritdoc cref="MicUserBasicDetails.RoleName"/>
        [JsonProperty("roleName")]
        public Maybe<string?> RoleName { get; set; }
        internal bool SouldSerializeRoleName() => RoleName.HasValue;

        /// <inheritdoc cref="MicUserBasicDetails.FirstName"/>
        [JsonProperty("firstName")]
        public Maybe<string?> FirstName { get; set; }
        internal bool SouldSerializeFirstName() => FirstName.HasValue;

        /// <inheritdoc cref="MicUserBasicDetails.LastName"/>
        [JsonProperty("lastName")]
        public Maybe<string?> LastName { get; set; }
        internal bool SouldSerializeLastName() => LastName.HasValue;

        /// <inheritdoc cref="MicUserBasicDetails.Email"/>
        [JsonProperty("email")]
        public Maybe<string?> Email { get; set; }
        internal bool SouldSerializeEmail() => Email.HasValue;

        /// <inheritdoc cref="MicUserBasicDetails.DomainName"/>
        [JsonProperty("domainName")]
        public Maybe<string?> DomainName { get; set; }
        internal bool SouldSerializeDomainName() => DomainName.HasValue;

        private readonly DuplexConversionTuple<Maybe<string?>, Maybe<CultureInfo?>> locale =
            new DuplexConversionTuple<Maybe<string?>, Maybe<CultureInfo?>>(
                rawConvert: lm => lm.HasValue
                    ? lm.Value is string l ? new CultureInfo(l) : null
                    : default,
                rawReverseConvert: cim => cim.HasValue ? cim.Value?.Name : default
                );
        /// <inheritdoc cref="MicUserFullDetails.LocaleIdentifier"/>
        [JsonProperty("locale")]
        public Maybe<string?> LocaleIdentifier

        {
            get => locale.RawValue;
            set => locale.RawValue = value;
        }
        internal bool SouldSerializeLocaleIdentifier() => LocaleIdentifier.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.Locale"/>
        [JsonIgnore]
        public Maybe<CultureInfo?> Locale
        {
            get => locale.ConvertedValue;
            set => locale.ConvertedValue = value;
        }

        /// <inheritdoc cref="MicUserFullDetails.Phone"/>
        [JsonProperty("phone")]
        public Maybe<string?> Phone { get; set; }
        internal bool SouldSerializePhone() => Phone.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.Company"/>
        [JsonProperty("company")]
        public Maybe<string?> Company { get; set; }
        internal bool SouldSerializeCompany() => Company.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.Address"/>
        [JsonProperty("address")]
        public Maybe<string?> Address { get; set; }
        internal bool SouldSerializeAddress() => Address.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.ZipCode"/>
        [JsonProperty("zip")]
        public Maybe<string?> ZipCode { get; set; }
        internal bool SouldSerializeZipCode() => ZipCode.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.City"/>
        [JsonProperty("city")]
        public Maybe<string?> City { get; set; }
        internal bool SouldSerializeCity() => City.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.Country"/>
        [JsonProperty("country")]
        public Maybe<string?> Country { get; set; }
        internal bool SouldSerializeCountry() => Country.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.Roles"/>
        [JsonProperty("roles")]
        public Maybe<string?> Roles { get; set; }
        internal bool SouldSerializeRoles() => Roles.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.Enabled"/>
        [JsonProperty("enabled")]
        public Maybe<bool> Enabled { get; set; }
        internal bool SouldSerializeEnabled() => Enabled.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.Notes1"/>
        [JsonProperty("notes1")]
        public Maybe<string?> Notes1 { get; set; }
        internal bool SouldSerializeNotes1() => Notes1.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.Notes2"/>
        [JsonProperty("notes2")]
        public Maybe<string?> Notes2 { get; set; }
        internal bool SouldSerializeNotes2() => Notes2.HasValue;

        /// <inheritdoc cref="MicUserFullDetails.Notes3"/>
        [JsonProperty("notes3")]
        public Maybe<string?> Notes3 { get; set; }
        internal bool SouldSerializeNotes3() => Notes3.HasValue;
    }
}
