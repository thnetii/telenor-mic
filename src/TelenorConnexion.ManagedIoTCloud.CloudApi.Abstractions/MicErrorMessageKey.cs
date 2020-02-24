using System.Diagnostics.CodeAnalysis;
using System.Threading;

using TelenorConnexion.ManagedIoTCloud.CloudApi.Model;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    /// <summary>
    /// Defines the possible value for the <see cref="MicErrorMessage.MessageKey"/>
    /// property of a MIC Error Message.
    /// </summary>
    [SuppressMessage("Naming", "CA1707: Identifiers should not contain underscores")]
    public static class MicErrorMessageKey
    {
        /// <summary>
        /// An invalid action was requested.
        /// </summary>
        public const string INVALID_ACTION = nameof(INVALID_ACTION);
        /// <summary>
        /// The is not authorized to perform the specified action.
        /// </summary>
        public const string NOT_AUTHORIZED = nameof(NOT_AUTHORIZED);
        /// <summary>
        /// A required property was missing in the requrest or set to <c>null</c>.
        /// <para>
        /// The <see cref="MicErrorMessage.Property"/> property specifies which
        /// property was missing.
        /// </para>
        /// </summary>
        public const string PROPERTY_REQUIRED = nameof(PROPERTY_REQUIRED);
        /// <summary>
        /// The combination of username and password was incorrect.
        /// </summary>
        public const string INVALID_LOGIN = nameof(INVALID_LOGIN);
        /// <summary>
        /// The user has not given consent, refer to the <see cref="MicClient.AuthGiveConsent(MicAuthGiveConsentRequest, CancellationToken)"/>
        /// method.
        /// </summary>
        public const string USER_CONSENT_REQUIRED = nameof(USER_CONSENT_REQUIRED);
    }
}
