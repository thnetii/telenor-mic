using System.Diagnostics.CodeAnalysis;

namespace TelenorConnexion.ManagedIoTCloud
{
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
    public static class MicErrorMessageKey
    {
        public const string INVALID_ACTION = nameof(INVALID_ACTION);
        public const string NOT_AUTHORIZED = nameof(NOT_AUTHORIZED);
        public const string PROPERTY_REQUIRED = nameof(PROPERTY_REQUIRED);
        public const string INVALID_LOGIN = nameof(INVALID_LOGIN);
        public const string USER_CONSENT_REQUIRED = nameof(USER_CONSENT_REQUIRED);
    }
}
