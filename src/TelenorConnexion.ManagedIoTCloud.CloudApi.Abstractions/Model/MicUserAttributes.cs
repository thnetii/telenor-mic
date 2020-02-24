using System;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    public class MicUserAttributes : MicUserDataAttributes
    {
        private static readonly Dictionary<string, string> JsonKeyForProperty = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [nameof(Username)] = "userName",
        };

        public MicUserAttributes() => SetAll();

        /// <summary>
        /// Whether to retrieve the user name of the user.
        /// </summary>
        public bool Username
        {
            get => ((IMicModel)this).AdditionalData.ContainsKey(JsonKeyForProperty[nameof(Username)]);
            set
            {
                if (value)
                    ((IMicModel)this).AdditionalData[JsonKeyForProperty[nameof(Username)]] = null;
                else
                    ((IMicModel)this).AdditionalData.Remove(JsonKeyForProperty[nameof(Username)]);
            }
        }

        /// <inheritdoc />
        public override void SetAll()
        {
            foreach (var key in JsonKeyForProperty.Values)
                ((IMicModel)this).AdditionalData[key] = null;
            base.SetAll();
        }

        /// <inheritdoc />
        protected override IDictionary<string, string> GetJsonKeyForPropertyDictionary()
        {
            var dict = base.GetJsonKeyForPropertyDictionary();
            foreach (var pair in JsonKeyForProperty)
                dict[pair.Key] = pair.Value;
            return dict;
        }
    }
}
