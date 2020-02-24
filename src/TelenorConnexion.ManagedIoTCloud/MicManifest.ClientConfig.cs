using System;
using System.Linq;
using System.Reflection;

using Amazon.Runtime;

namespace TelenorConnexion.ManagedIoTCloud
{
    partial class MicManifest
    {
        public TConfig CreateClientConfig<TConfig>(IClientConfig? template = default)
            where TConfig : ClientConfig, new()
        {
            var config = new TConfig();
            if (!(template is null))
            {
                const BindingFlags bf = BindingFlags.Public | BindingFlags.Instance;
                var templateProperties = template.GetType()
                    .GetProperties(bf)
                    .Where<PropertyInfo>(pi => pi.CanRead)
                    .Where(pi => (pi.GetIndexParameters()?.Length ?? 0) == 0);
                foreach (var templatePi in templateProperties)
                {
                    if (!(GetMatchingPropertyInfo(templatePi) is PropertyInfo configPi))
                        continue;

                    if (!IsPropertyValueDefaultValue(templatePi, template, out var templateValue) &&
                        IsPropertyValueDefaultValue(configPi, config, out _))
                        configPi.SetValue(config, templateValue);
                }

                static PropertyInfo? GetMatchingPropertyInfo(PropertyInfo templPi)
                {
                    var pi = typeof(TConfig).GetProperty(templPi.Name, bf);
                    if (pi is null || !pi.CanWrite ||
                        (pi.GetIndexParameters()?.Length ?? 0) != 0 ||
                        !pi.PropertyType.IsAssignableFrom(templPi.PropertyType))
                        return null;
                    return pi;
                }

                static bool IsPropertyValueDefaultValue(PropertyInfo pi, IClientConfig config, out object? value)
                {
                    if (!pi.CanRead)
                    {
                        value = null;
                        return true;
                    }
                    value = pi.GetValue(config);
                    bool isValueType = pi.PropertyType
#if NETSTANDARD1_3
                        .GetTypeInfo()
#endif
                        .IsValueType;
                    if (isValueType && value != null)
                    {
                        var defaultValue = Activator.CreateInstance(pi.PropertyType);
                        return value.Equals(defaultValue);
                    }
                    else
                        return value is null;
                }
            }
            config.RegionEndpoint = AwsRegion;
            return config;
        }
    }
}
