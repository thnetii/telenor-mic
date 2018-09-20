using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TelenorConnexion.ManagedIoTCloud
{
    [SuppressMessage("Design", "CA1040:Avoid empty interfaces")]
    public interface IMicRequestAttributes { }

    public static class MicRequestAttributesExtensions
    {
        private static readonly ConcurrentDictionary<Type, string> action =
            new ConcurrentDictionary<Type, string>();

        public static MicRequest CreateRequest(this IMicRequestAttributes attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));
            return new MicRequest
            {
                Action = action.GetOrAdd(attributes.GetType(), GetActionString),
                Attributes = attributes
            };
        }

        private static string GetActionString(Type type)
        {
            return type
#if NETSTANDARD1_3
                .GetTypeInfo()
#endif
                .GetCustomAttribute<MicRequestPayloadActionAttribute>()?
                .Action;
        }
    }
}
