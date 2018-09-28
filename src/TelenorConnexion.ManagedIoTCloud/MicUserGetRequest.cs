using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud
{
    [MicRequestPayloadAction("GET")]
    public class MicUserGetRequest : MicUserBasicInfo, IMicRequestAttributes
    {
        private static readonly Dictionary<string, string> JsonKeyForProperty = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [nameof(FirstName)] = "firstName",
            [nameof(LastName)] = "lastName",
            [nameof(Email)] = "email",
            [nameof(Phone)] = "phone",
            [nameof(Locale)] = "locale",
            [nameof(Company)] = "company",
            [nameof(Address)] = "address",
            [nameof(ZipCode)] = "zip",
            [nameof(Country)] = "country",
            [nameof(RoleName)] = "roleName",
            [nameof(DomainName)] = "domainName",
            [nameof(Notes1)] = "notes1",
            [nameof(Notes2)] = "notes2",
            [nameof(Notes3)] = "notes3",
            [nameof(Data)] = "data",
        };

        [JsonExtensionData]
        internal IDictionary<string, object> Attributes { get; } =
            new Dictionary<string, object>(StringComparer.Ordinal);

        /// <summary>
        /// Whether to retrieve the first name of the user.
        /// </summary>
        [JsonIgnore]
        public bool FirstName
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(FirstName)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(FirstName)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(FirstName)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the last name of the user.
        /// </summary>
        [JsonIgnore]
        public bool LastName
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(LastName)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(LastName)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(LastName)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the email address of the user.
        /// </summary>
        [JsonIgnore]
        public bool Email
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(Email)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(Email)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(Email)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the phone number of the user.
        /// </summary>
        [JsonIgnore]
        public bool Phone
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(Phone)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(Phone)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(Phone)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the locale used by the user.
        /// </summary>
        [JsonIgnore]
        public bool Locale
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(Locale)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(Locale)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(Locale)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the company the user is associated with.
        /// </summary>
        [JsonIgnore]
        public bool Company
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(Company)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(Company)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(Company)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the postal address of the user.
        /// </summary>
        [JsonIgnore]
        public bool Address
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(Address)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(Address)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(Address)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the ZIP code of the user's address.
        /// </summary>
        [JsonIgnore]
        public bool ZipCode
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(ZipCode)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(ZipCode)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(ZipCode)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the country of the user's address.
        /// </summary>
        [JsonIgnore]
        public bool Country
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(Country)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(Country)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(Country)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the role name given to the user.
        /// </summary>
        [JsonIgnore]
        public bool RoleName
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(RoleName)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(RoleName)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(RoleName)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the name of the domain the user is in.
        /// </summary>
        [JsonIgnore]
        public bool DomainName
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(DomainName)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(DomainName)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(DomainName)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the first name of the user.
        /// </summary>
        [JsonIgnore]
        public bool Notes1
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(Notes1)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(Notes1)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(Notes1)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the first name of the user.
        /// </summary>
        [JsonIgnore]
        public bool Notes2
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(Notes2)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(Notes2)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(Notes2)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the first name of the user.
        /// </summary>
        [JsonIgnore]
        public bool Notes3
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(Notes3)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(Notes3)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(Notes3)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the first name of the user.
        /// </summary>
        [JsonIgnore]
        public bool Data
        {
            get => Attributes.ContainsKey(JsonKeyForProperty[nameof(Data)]);
            set
            {
                if (value)
                    Attributes[JsonKeyForProperty[nameof(Data)]] = null;
                else
                    Attributes.Remove(JsonKeyForProperty[nameof(Data)]);
            }
        }

        /// <summary>
        /// Sets the request to retrieve all known attributes of the specified user.
        /// </summary>
        public void SetAll()
        {
            foreach (var key in JsonKeyForProperty.Values)
                Attributes[key] = null;
        }

        /// <summary>
        /// Clears all requested attributes and sets the request to retrieve no
        /// attributes of the specified user.
        /// </summary>
        public void ClearAll() => Attributes.Clear();
    }
}
