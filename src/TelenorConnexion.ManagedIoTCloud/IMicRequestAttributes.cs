using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TelenorConnexion.ManagedIoTCloud
{
    /// <summary>
    /// The <see cref="MicRequest.Attributes"/> of a <see cref="MicRequest"/>
    /// instance.
    /// <para>
    /// Implementations of this interface SHOULD make use of the
    /// <see cref="MicRequestPayloadActionAttribute"/>.
    /// </para>
    /// </summary>
    [SuppressMessage("Design", "CA1040:Avoid empty interfaces")]
    public interface IMicRequestAttributes { }

    /// <summary>
    /// Provides extension methods for MIC Request Attributes.
    /// </summary>
    public static class MicRequestAttributesExtensions
    {
        private static readonly ConcurrentDictionary<Type, string> action =
            new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// Creates a new <see cref="MicRequest" /> instance for the specified
        /// attributes instance.
        /// </summary>
        /// <param name="attributes">The attributes to wrap into the new request instance.</param>
        /// <returns>
        /// A new <see cref="MicRequest"/> instance using the specified
        /// instance as the value for the <see cref="MicRequest.Attributes"/>
        /// property.
        /// <para>
        /// The value for the <see cref="MicRequest.Action"/> property is, if
        /// possible, determined by the <see cref="MicRequestPayloadActionAttribute"/>
        /// applied to the type implementing the specified <see cref="IMicRequestAttributes"/>
        /// interface.
        /// </para>
        /// </returns>
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
