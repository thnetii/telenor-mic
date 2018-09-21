using System;

namespace TelenorConnexion.ManagedIoTCloud
{
    /// <summary>
    /// An attribute to use on implementors of the <see cref="IMicRequestAttributes"/>
    /// interface to mark which value should be used for the <see cref="MicRequest.Action"/>
    /// property of <see cref="MicRequest"/> instances created from these attributes.S
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class MicRequestPayloadActionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new MIC Request Payload Action attribute with the
        /// specified action.
        /// </summary>
        /// <param name="action">The action to use for the <see cref="MicRequest.Action"/> property of created <see cref="MicRequest"/> instances.</param>
        public MicRequestPayloadActionAttribute(string action) =>
            Action = action;

        /// <summary>
        /// The action to set as the value for the <see cref="MicRequest.Action"/> property of created <see cref="MicRequest"/> instances.
        /// </summary>
        public string Action { get; }
    }
}
