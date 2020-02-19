using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    public class MicUserDataAttributes : MicModel
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
            [nameof(Roles)] = "roles",
            [nameof(TermsAgreed)] = "termsAgreed",
            [nameof(DateTermsAgreed)] = "dateTermsAgreed",
            [nameof(TermsVersion)] = "termsVersion",
            [nameof(CreatedAt)] = "createdAt",
            [nameof(Enabled)] = "enabled",
        };

        [SuppressMessage("Usage", "CA2214: Do not call overridable methods in constructors")]
        public MicUserDataAttributes() : base() => SetAll();

        /// <summary>
        /// Whether to retrieve the first name of the user.
        /// </summary>
        [JsonIgnore]
        public bool FirstName
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(FirstName)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(FirstName)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(FirstName)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the last name of the user.
        /// </summary>
        [JsonIgnore]
        public bool LastName
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(LastName)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(LastName)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(LastName)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the email address of the user.
        /// </summary>
        [JsonIgnore]
        public bool Email
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Email)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Email)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Email)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the phone number of the user.
        /// </summary>
        [JsonIgnore]
        public bool Phone
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Phone)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Phone)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Phone)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the locale used by the user.
        /// </summary>
        [JsonIgnore]
        public bool Locale
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Locale)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Locale)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Locale)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the company the user is associated with.
        /// </summary>
        [JsonIgnore]
        public bool Company
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Company)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Company)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Company)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the postal address of the user.
        /// </summary>
        [JsonIgnore]
        public bool Address
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Address)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Address)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Address)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the ZIP code of the user's address.
        /// </summary>
        [JsonIgnore]
        public bool ZipCode
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(ZipCode)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(ZipCode)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(ZipCode)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the country of the user's address.
        /// </summary>
        [JsonIgnore]
        public bool Country
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Country)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Country)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Country)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the role name given to the user.
        /// </summary>
        [JsonIgnore]
        public bool RoleName
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(RoleName)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(RoleName)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(RoleName)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the name of the domain the user is in.
        /// </summary>
        [JsonIgnore]
        public bool DomainName
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(DomainName)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(DomainName)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(DomainName)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the first name of the user.
        /// </summary>
        [JsonIgnore]
        public bool Notes1
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Notes1)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Notes1)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Notes1)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the first name of the user.
        /// </summary>
        [JsonIgnore]
        public bool Notes2
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Notes2)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Notes2)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Notes2)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the first name of the user.
        /// </summary>
        [JsonIgnore]
        public bool Notes3
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Notes3)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Notes3)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Notes3)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the first name of the user.
        /// </summary>
        [JsonIgnore]
        public bool Data
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Data)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Data)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Data)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the roles that are assigned to the user.
        /// </summary>
        [JsonIgnore]
        public bool Roles
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Roles)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Roles)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Roles)]);
            }
        }

        /// <summary>
        /// Whether to retrieve a value that indicates whether the user has ever agreed to the terms of service.
        /// </summary>
        [JsonIgnore]
        public bool TermsAgreed
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(TermsAgreed)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(TermsAgreed)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(TermsAgreed)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the date when the user has agreed to the terms of service.
        /// </summary>
        [JsonIgnore]
        public bool DateTermsAgreed
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(DateTermsAgreed)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(DateTermsAgreed)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(DateTermsAgreed)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the version of the terms of service that the user has agreed to.
        /// </summary>
        [JsonIgnore]
        public bool TermsVersion
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(TermsVersion)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(TermsVersion)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(TermsVersion)]);
            }
        }

        /// <summary>
        /// Whether to retrieve the date on which the user was created.
        /// </summary>
        [JsonIgnore]
        public bool CreatedAt
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(CreatedAt)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(CreatedAt)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(CreatedAt)]);
            }
        }

        /// <summary>
        /// Whether to retrieve a value indicating whether the user account is enabled.
        /// </summary>
        [JsonIgnore]
        public bool Enabled
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Enabled)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Enabled)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Enabled)]);
            }
        }

        /// <summary>
        /// Sets the request to retrieve all known ((IMicModel)this).AdditionalData of the specified user.
        /// </summary>
        public virtual void SetAll()
        {
            foreach (var key in JsonKeyForProperty.Values)
                ((IMicModel)this).AdditionalData[key] = null;
        }

        /// <summary>
        /// Clears all requested ((IMicModel)this).AdditionalData and sets the request to retrieve no
        /// ((IMicModel)this).AdditionalData of the specified user.
        /// </summary>
        public void ClearAll() => ((IMicModel)this).AdditionalData.Clear();

        protected virtual IDictionary<string, string> GetJsonKeyForPropertyDictionary() =>
            new Dictionary<string, string>(JsonKeyForProperty, StringComparer.OrdinalIgnoreCase);
    }
}
