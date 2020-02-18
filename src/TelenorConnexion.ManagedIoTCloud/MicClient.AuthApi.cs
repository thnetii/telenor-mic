using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using TelenorConnexion.ManagedIoTCloud.Model;

using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud
{
    using static MicErrorMessageKey;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
    public partial class MicClient
    {
        #region Auth API

        #region Auth API : CONFIRM_SIGN_UP

        private const string authConfirmSignupUrl = "auth/confirm-sign-up";

        /// <summary>
        /// Confirms the account of a signed up user. This action should be called after a user have received their
        /// confirmation e-mail or SMS from the SIGN_UP action to confirm the account. An email will receive a token and
        /// an SMS will receive the username and a confirmation code.
        /// <para>This endpoint supports confirming the account with either token or username/code.</para>
        /// </summary>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>token</c></description></item>
        /// </list></term>
        /// <description>Returned if no token was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="INVALID_LOGIN"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>token</c></description></item>
        /// </list></term>
        /// <description>Returned if the token is incorrect or has expired.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="USER_NOT_FOUND"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>token</c></description></item>
        /// </list></term>
        /// <description>Returned if the user cannot be found.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/auth/#confirm-sign-up"/>
        public Task<MicResponse> AuthConfirmSignup(MicAuthConfirmSignupRequest request, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(authConfirmSignupUrl, HttpMethod.Post,
                request, hasPayload: true, cancelToken);

        /// <inheritdoc cref="AuthConfirmSignup(MicAuthConfirmSignupRequest, CancellationToken)"/>
        /// <param name="token">The token that the user received in the confirmation e-mail. The token is sent as a parameter in the url and the client must extract this parameter and send it as-is to this action. Required.</param>
        public Task<MicResponse> AuthConfirmSignup(string token, CancellationToken cancelToken = default) =>
            AuthConfirmSignup(new MicAuthConfirmSignupRequest { Token = token }, cancelToken);

        /// <inheritdoc cref="AuthConfirmSignup(MicAuthConfirmSignupRequest, CancellationToken)"/>
        /// <param name="username">The username that the user chose at signup. Required.</param>
        /// <param name="code">The confirmation code that was received in the SMS. Required.</param>
        public Task<MicResponse> AuthConfirmSignup(string username, string code, CancellationToken cancelToken = default) =>
            AuthConfirmSignup(new MicAuthConfirmSignupRequest { Username = username, Code = code }, cancelToken);

        /// <inheritdoc cref="AuthConfirmSignup(MicAuthConfirmSignupRequest, CancellationToken)"/>
        public Task<MicResponse> AuthConfirmSignup(MicAuthConfirmationCode confirmationCode, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(authConfirmSignupUrl, HttpMethod.Post,
                confirmationCode, hasPayload: true, cancelToken);

        #endregion // Auth API : CONFIRM_SIGN_UP

        #region Auth API : FORGOT_PASSWORD

        private const string authForgotPasswordUrl = "auth/forgot-password";

        /// <summary>
        /// If a user has forgotten the account password, this action can be used to let them reset their password.
        /// The user will get an email message containing a link to set a new password. The link is usable only once.
        /// </summary>
        /// <remarks>
        /// Note: After having invoked this action, it is still possible to login using the correct credentials;
        /// the password is not changed until the user has responded to the email sent by the <c>FORGOT_PASSWORD</c> action.
        /// </remarks>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>userName</c></description></item>
        /// </list></term>
        /// <description>Returned if no user name was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="USER_NOT_FOUND"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>userName</c></description></item>
        /// </list></term>
        /// <description>Returned if the user cannot be found.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/auth/#forgot-password"/>
        public Task<MicResponse> AuthForgotPassword(MicAuthForgotPasswordRequest request, CancellationToken cancelToken) =>
            HandleClientRequest<MicResponse>(authForgotPasswordUrl, HttpMethod.Post,
                request, hasPayload: true, cancelToken);

        /// <inheritdoc cref="AuthForgotPassword(MicAuthForgotPasswordRequest, CancellationToken)"/>
        /// <param name="username">The user name of the user to request a password reset link for.</param>
        public Task<MicResponse> AuthForgotPassword(string username, CancellationToken cancelToken = default) =>
            AuthForgotPassword(new MicAuthForgotPasswordRequest { Username = username }, cancelToken);

        /// <inheritdoc cref="AuthForgotPassword(MicAuthForgotPasswordRequest, CancellationToken)"/>
        public Task<MicResponse> AuthForgotPassword(MicUserBasicInfo userInfo, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(authForgotPasswordUrl, HttpMethod.Post,
                userInfo, hasPayload: true, cancelToken);

        #endregion // Auth API : FORGOT_PASSWORD

        #region Auth AP : GET_CONSENT_DOCUMENT

        private const string authGetConsentDocumentUrl = "auth/get-consent-document";

        /// <summary>
        /// Get a signed url to the latest version of a consent document (terms of service).
        /// </summary>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="FILE_NOT_FOUND"/></description></item>
        /// </list></term>
        /// <description>Returned if the consent document cannot be found.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/auth/#get-consent-document"/>
        public Task<MicAuthGetConsentDocumentResponse> AuthGetConsentDocument(CancellationToken cancelToken = default) =>
            HandleClientRequest<MicAuthGetConsentDocumentResponse>(
                authGetConsentDocumentUrl, HttpMethod.Get, request: null,
                hasPayload: false, cancelToken);

        #endregion // Auth AP : GET_CONSENT_DOCUMENT

        #region Auth API : LOGIN

        private const string authLoginUrl = "auth/login";

        /// <summary>
        /// Checks if a user is authorized to login and returns credentials that should be used when communicating with
        /// AWS. The access token is valid for 60 minutes, whereafter it needs to be refreshed.
        /// </summary>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>userName</c></description></item>
        /// </list></term>
        /// <description>Returned if no user name was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>password</c></description></item>
        /// </list></term>
        /// <description>Returned if no password was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="INVALID_LOGIN"/></description></item>
        /// </list></term>
        /// <description>Returned if the user name or password was incorrect.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/auth/#login"/>
        public Task<MicAuthLoginResponse> AuthLogin(MicAuthLoginRequest request, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicAuthLoginResponse>(authLoginUrl, HttpMethod.Post,
                request, hasPayload: true, cancelToken);

        /// <inheritdoc cref="AuthLogin(MicAuthLoginRequest, CancellationToken)"/>
        /// <param name="username">The user name of the user.</param>
        /// <param name="password">The password of the user.</param>
        public Task<MicAuthLoginResponse> AuthLogin(string username, string password, CancellationToken cancelToken = default) =>
            AuthLogin(new MicAuthLoginRequest
            {
                Username = username,
                Password = password
            }, cancelToken);

        /// <inheritdoc cref="AuthLogin(MicAuthLoginRequest, CancellationToken)"/>
        public Task<MicAuthLoginResponse> AuthLogin(MicAuthUserPassword userPassword, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicAuthLoginResponse>(authLoginUrl, HttpMethod.Post,
                userPassword, hasPayload: true, cancelToken);

        #endregion // Auth API : LOGIN

        #region Auth API : GIVE_CONSENT

        private const string authGiveConsentUrl = "auth/consent";

        /// <summary>
        /// You may require users to consent to terms and conditions before they can use the service.
        /// Use this endpoint to register that a user has given their consent.
        /// </summary>
        /// <returns>An empty object.</returns>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>userName</c></description></item>
        /// </list></term>
        /// <description>Returned if no user name was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>password</c></description></item>
        /// </list></term>
        /// <description>Returned if no password was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="INVALID_LOGIN"/></description></item>
        /// </list></term>
        /// <description>Returned if the user name or password was incorrect.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/auth/#give-consent"/>
        public Task<MicResponse> AuthGiveConsent(MicAuthGiveConsentRequest request, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(authGiveConsentUrl, HttpMethod.Post,
                request, hasPayload: true, cancelToken);

        /// <inheritdoc cref="AuthGiveConsent(MicAuthGiveConsentRequest, CancellationToken)"/>
        /// <param name="username">The user name of the user.</param>
        /// <param name="password">The password of the user.</param>
        public Task<MicResponse> AuthGiveConsent(string username, string password, CancellationToken cancelToken = default) =>
            AuthGiveConsent(new MicAuthGiveConsentRequest { Username = username, Password = password }, cancelToken);

        /// <inheritdoc cref="AuthGiveConsent(MicAuthGiveConsentRequest, CancellationToken)"/>
        public Task<MicResponse> AuthGiveConsent(MicAuthUserPassword userPassword, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(authGiveConsentUrl, HttpMethod.Post,
                userPassword, hasPayload: true, cancelToken);

        #endregion // Auth API : GIVE_CONSENT

        #region Auth API : REFRESH

        private const string authRefreshUrl = "auth/refresh";

        /// <summary>
        /// Lets a user refresh the token. Use the <see cref="MicAuthCredentials.RefreshToken"/> acquired from a <see cref="AuthLogin(MicAuthLoginRequest, CancellationToken)"/> call.
        /// By default, the refresh token is set to expire after 30 days but this can be changed to an arbitrary period.
        /// </summary>
        /// <returns>The response from refresh is exactly the same as from the login request, and should be handled the same way.</returns>
        /// <remarks>
        /// <para>
        /// To use the refresh token to get a new set of tokens, do the same call as you did when you logged in but use
        /// the refresh action instead and pass the refresh token as the argument. Please note that you need to reset
        /// your logins before you do this, otherwise you will try to do an authenticated call and since the
        /// access token is no longer valid, this will fail.
        /// </para>
        /// <para>
        /// In Managed IoT Cloud Appboard the refresh token is implemented by checking for errors and if the token has
        /// expired, it is refreshed and the operation is retried, and if it still fails, the user is redirected to the
        /// login page.
        /// </para>
        /// </remarks>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>refreshToken</c></description></item>
        /// </list></term>
        /// <description>Returned if no refresh token was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="INVALID_LOGIN"/></description></item>
        /// </list></term>
        /// <description>Returned if the refresh token was incorrect.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/auth/#refresh"/>
        public Task<MicAuthLoginResponse> AuthRefresh(MicAuthRefreshRequest request, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicAuthLoginResponse>(authRefreshUrl, HttpMethod.Post,
                request, hasPayload: true, cancelToken);

        /// <inheritdoc cref="AuthRefresh(MicAuthRefreshRequest, CancellationToken)"/>
        /// <exception cref="InvalidOperationException">No refresh token from previous login operations available.</exception>
        [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
        public Task<MicAuthLoginResponse> AuthRefresh(CancellationToken cancelToken = default)
        {
            if ((Credentials?.RefreshToken).TryNotNull(out string refreshToken))
                return AuthRefresh(refreshToken, cancelToken);
            throw new InvalidOperationException("No refresh token from previous login operations available.");
        }

        /// <inheritdoc cref="AuthRefresh(MicAuthRefreshRequest, CancellationToken)"/>
        /// <param name="refreshToken">The refresh token acquired when logging in.</param>
        public Task<MicAuthLoginResponse> AuthRefresh(string refreshToken, CancellationToken cancelToken = default) =>
            AuthRefresh(new MicAuthRefreshRequest { RefreshToken = refreshToken }, cancelToken);

        #endregion // Auth API : REFRESH

        #region Auth API : RESEND_CONFIRMATION_CODE

        private const string authResendConfirmationCodeUrl = "auth/resend-confirmation-code";

        /// <summary>
        /// Sends a new confirmation code to the user if the user didn't receive an e-mail or if the link has expired.
        /// </summary>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/auth/#resend-confirmation-code"/>
        public Task<MicResponse> AuthResendConfirmationCode(MicAuthResendConfirmationCodeRequest request, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(authResendConfirmationCodeUrl, HttpMethod.Post,
                request, hasPayload: true, cancelToken);

        /// <inheritdoc cref="AuthResendConfirmationCode(MicAuthResendConfirmationCodeRequest, CancellationToken)"/>
        /// <param name="userName">The user name of the user to resend the confirmation code for.</param>
        public Task<MicResponse> AuthResendConfirmationCode(string userName, CancellationToken cancelToken = default) =>
            AuthResendConfirmationCode(new MicAuthResendConfirmationCodeRequest { Username = userName }, cancelToken);

        /// <inheritdoc cref="AuthResendConfirmationCode(MicAuthResendConfirmationCodeRequest, CancellationToken)"/>
        public Task<MicResponse> AuthResendConfirmationCode(MicUserBasicInfo userInfo, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(authResendConfirmationCodeUrl, HttpMethod.Post,
                userInfo, hasPayload: true, cancelToken);

        #endregion // Auth API : RESEND_CONFIRMATION_CODE

        #region Auth API : SET_PASSWORD

        private const string authSetPasswordUrl = "auth/set-password";

        /// <summary>
        /// Sets the passsword of the user based on a token sent by the system when the user either forgets their password or is just created by an administrator.
        /// </summary>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>token</c></description></item>
        /// </list></term>
        /// <description>Returned if no token was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>password</c></description></item>
        /// </list></term>
        /// <description>Returned if no password was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="INVALID_LOGIN"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>token</c></description></item>
        /// </list></term>
        /// <description>Returned if the token is incorrect or has expired.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="USER_NOT_FOUND"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>token</c></description></item>
        /// </list></term>
        /// <description>Returned if the user contained in the token doesn't exist.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/auth/#set-password"/>
        public Task<MicResponse> AuthSetPassword(MicAuthSetPasswordRequest request, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(authSetPasswordUrl, HttpMethod.Post,
                request, hasPayload: true, cancelToken);

        /// <inheritdoc cref="AuthSetPassword(MicAuthSetPasswordRequest, CancellationToken)"/>
        /// <param name="token">The token that the user received in the confirmation e-mail. The token is sent as a parameter in the url and the application must extract this parameter and send it as-is to this action.</param>
        /// <param name="password">The new password for the user.</param>
        public Task<MicResponse> AuthSetPassword(string token, string password, CancellationToken cancelToken = default) =>
            AuthSetPassword(new MicAuthSetPasswordRequest
            {
                Token = token,
                Password = password
            }, cancelToken);

        /// <inheritdoc cref="AuthSetPassword(MicAuthSetPasswordRequest, CancellationToken)"/>
        /// <param name="userName">The username that the user chose at signup.</param>
        /// <param name="code">The confirmation code that was received in the SMS.</param>
        /// <param name="password">The new password for the user.</param>
        public Task<MicResponse> AuthSetPassword(string userName, string code, string password, CancellationToken cancelToken = default) =>
            AuthSetPassword(new MicAuthSetPasswordRequest
            {
                Username = userName,
                Code = code,
                Password = password
            }, cancelToken);

        /// <inheritdoc cref="AuthSetPassword(MicAuthSetPasswordRequest, CancellationToken)"/>
        /// <param name="confirmationCode">The confirmation code that was received either by email or SMS.</param>
        /// <param name="password">The new password for the user.</param>
        public Task<MicResponse> AuthSetPassword(MicAuthConfirmationCode confirmationCode, string password, CancellationToken cancelToken = default) =>
            AuthSetPassword(new MicAuthSetPasswordRequest
            {
                Username = confirmationCode?.Username,
                Code = confirmationCode?.Code,
                Token = confirmationCode?.Token,
                Password = password
            }, cancelToken);

        #endregion // Auth API : SET_PASSWORD

        #region Auth API : SIGN_UP

        private const string authSignUpUrl = "auth/sign-up";

        /// <summary>
        /// Lets a user initiate the process to create a Managed IoT Cloud user account.
        /// </summary>
        /// <remarks>
        /// A successful signup will result in an email containing a link is sent to their provided email address.
        /// Following that link verifies their email address. If a user didn’t get that email or if the link has expired,
        /// there is a possibility to resend the email via the endpoint <see cref="AuthResendConfirmationCode(MicAuthResendConfirmationCodeRequest, CancellationToken)"/>.
        /// </remarks>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>userName</c></description></item>
        /// </list></term>
        /// <description>Returned if no user name was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>password</c></description></item>
        /// </list></term>
        /// <description>Returned if no password was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>email</c></description></item>
        /// </list></term>
        /// <description>Returned if no email address was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="USER_USERNAME_EXISTS"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>userName</c></description></item>
        /// </list></term>
        /// <description>Returned if the user name that was provided is used by another user in the system.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="USER_CONSENT_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>consent</c></description></item>
        /// </list></term>
        /// <description>Returned if consent is required, and the user did not give it.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/auth/#sign-up"/>
        public Task<MicUserFullDetails> AuthSignup(MicAuthSignupRequest request, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserFullDetails>(authSignUpUrl, HttpMethod.Post,
                request, hasPayload: true, cancelToken);

        #endregion // Auth API : SIGN_UP

        #endregion // Auth API

    }
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
}
