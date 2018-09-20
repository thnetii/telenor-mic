using System;

namespace TelenorConnexion.ManagedIoTCloud
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class MicRequestPayloadActionAttribute : Attribute
    {
        public MicRequestPayloadActionAttribute(string action) =>
            Action = action;

        public string Action { get; }
    }
}
