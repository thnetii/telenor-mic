using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using TelenorConnexion.ManagedIoTCloud.CloudApi.Model;
using THNETII.Common;
using THNETII.Common.Collections.Generic;
using THNETII.Networking.Http;
using THNETII.TypeConverter.Serialization;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    using static MicErrorMessageKey;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
    public partial class MicCloudApiRestClient
    {
        #region User API : LIST
        private static string UserListUrl(MicUserListRequest? request)
        {
            const string userListUrl = "users";
            var queryParams = request is null ? null
                : new[]
                {
                    request.Attributes is MicUserAttributes attributes
                        ? (nameof(attributes), SerializeAttributes(attributes)).AsKeyValuePair()
                        : default,
                    request.FreeTextFilter is string freeText
                        ? (nameof(freeText), freeText).AsKeyValuePair()
                        : default,
                    request.Category.HasValue
                        ? ("category", EnumMemberStringConverter.ToString(request.Category.Value)).AsKeyValuePair()
                        : default,
                    request.Page.HasValue
                        ? ("page", request.Page.Value.ToString(CultureInfo.InvariantCulture)).AsKeyValuePair()
                        : default,
                    request.Size.HasValue
                        ? ("size", request.Size.Value.ToString(CultureInfo.InvariantCulture)).AsKeyValuePair()
                        : default,
                    request.SortProp is string sortProp
                        ? (nameof(sortProp), sortProp).AsKeyValuePair()
                        : default,
                };
            string queryString = HttpUrlHelper.ToQueryString(queryParams.AsSpan());
            return userListUrl + queryString;

            static string SerializeAttributes(MicUserAttributes attributes)
            {
                var names = ((IMicModel)attributes).AdditionalData.Keys;
                return string.Join(",", names.Select(k => Uri.EscapeDataString(k)));
            }
        }

        /// <summary>
        /// Lists all users.
        /// </summary>
        /// <returns>A list of users where each item in the list have the requested properties.</returns>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/user/#list"/>
        public Task<MicUserListResponse> UserList(MicUserListRequest? request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserListResponse>(UserListUrl(request),
                HttpMethod.Get, request: request, hasPayload: false, cancelToken);

#if !(NETSTANDARD1_3 || NETSTANDARD2_0)
        /// <inheritdoc cref="UserList(MicUserListRequest, CancellationToken)"/>
        /// <param name="request">
        /// A user list request object specifying the initial state of the
        /// pagination.
        /// <para><strong>The <see cref="MicUserListRequest.Page"/> property will be mutated as the returned <see cref="IAsyncEnumerable{MicUserFullDetails}"/> is consumed.</strong></para>
        /// </param>
        /// <param name="metadata">
        /// Optional. A metadata object that will receive the response metadata
        /// as each page is requested. If <see langword="null"/> the caller will
        /// not have access to the metadata returned by the server.
        /// </param>
        public async IAsyncEnumerable<MicUserFullDetails> UserListEnumerate(
            MicUserListRequest? request, MicUserListMetadata? metadata = null,
            [EnumeratorCancellation] CancellationToken cancelToken = default)
        {
            request ??= new MicUserListRequest();
            MicUserListResponse response;
            do
            {
                response = await UserList(request, cancelToken)
                    .ConfigureAwait(false);
                if (response is null)
                    break;
                if (metadata is MicUserListMetadata dst && response.Metadata is MicUserListMetadata src)
                {
                    dst.Count = src.Count;
                }
                foreach (var user in response.Users ?? Enumerable.Empty<MicUserFullDetails>())
                {
                    if (cancelToken.IsCancellationRequested)
                        break;
                    yield return user;
                }
            } while (!cancelToken.IsCancellationRequested && MoveNext(request, response));

            static bool MoveNext(MicUserListRequest req, MicUserListResponse resp)
            {
                if (resp.Page >= resp.TotalPages)
                    return false;
                req.Page = resp.Page + 1;
                return true;
            }
        }
#endif // !(NETSTANDARD1_3 || NETSTANDARD2_0)

        #endregion

        #region User API : CREATE
        private const string userCreateUrl = "users";

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="NOT_AUTHORIZED_DOMAIN"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>domainName</c></description></item>
        /// </list></term>
        /// <description>Returned if the user tries to add a user to a domain that the user is not authorized to see.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>userName</c></description></item>
        /// </list></term>
        /// <description>Returned if no userName was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>roleName</c></description></item>
        /// </list></term>
        /// <description>Returned if no role name was provided.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_REQUIRED"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>domainName</c></description></item>
        /// </list></term>
        /// <description>Returned if no domain name was provided.</description>
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
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="INVALID_ARGUMENTS"/></description></item>
        /// </list></term>
        /// <description>Returned if a parameter does not fulfil requirements.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/user/#create"/>
        public Task<MicUserCreateResponse> UserCreate(MicUserCreateRequest request, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserCreateResponse>(userCreateUrl, HttpMethod.Post,
                request, hasPayload: true, cancelToken);

        /// <inheritdoc cref="UserCreate(MicUserCreateRequest, CancellationToken)"/>
        public Task<MicUserCreateResponse> UserCreate(MicUserFullDetails userDetails, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserCreateResponse>(userCreateUrl, HttpMethod.Post,
                userDetails, hasPayload: true, cancelToken);

        #endregion // User API : CREATE

        #region User API : GET
        private static string UserGetUrl(MicUserGetRequest? request)
        {
            FormattableString userGetUrl = $"users/{request?.Username}";
            var queryParams = request is null ? null
                : new[]
                {
                    ((IMicModel?)request.Attributes)?.AdditionalData?.Keys is IEnumerable<string> attributes
                        ? (nameof(attributes), string.Join(",", attributes.Select(k => Uri.EscapeDataString(k)))).AsKeyValuePair()
                        : default
                };
            string queryString = HttpUrlHelper.ToQueryString(queryParams.AsSpan());
            return FormattableString.Invariant(userGetUrl) + queryString;
        }

        /// <summary>
        /// Gets information about a user.
        /// </summary>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="NOT_AUTHORIZED_DOMAIN"/></description></item>
        /// </list></term>
        /// <description>Returned if the user tries to get a user that is assigned to a domain that the user is not authorized to see.</description>
        /// </item>
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
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/user/#get"/>
        public Task<MicUserGetResponse> UserGet(MicUserGetRequest request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserGetResponse>(UserGetUrl(request),
                HttpMethod.Get, request, hasPayload: false, cancelToken);

        /// <inheritdoc cref="UserGet(MicUserGetRequest, CancellationToken)"/>
        /// <param name="userName">The user name of the user to get.</param>
        public Task<MicUserGetResponse> UserGet(string userName,
            MicUserDataAttributes? attributes = null,
            CancellationToken cancelToken = default) =>
            UserGet(new MicUserGetRequest { Username = userName, Attributes = attributes }, cancelToken);
        #endregion // User API : GET

        #region User API : WHOAMI
        private static string UserWhoamiUrl(MicUserDataAttributes? request)
        {
            const string userWhoamiUrl = "user";
            var queryParams = request is null ? default
                : new[]
                {
                    ((IMicModel)request).AdditionalData?.Keys is IEnumerable<string> attributes
                        ? (nameof(attributes), string.Join(",", attributes.Select(k => Uri.EscapeDataString(k)))).AsKeyValuePair()
                        : default
                };
            var queryString = HttpUrlHelper.ToQueryString(queryParams.AsSpan());
            return userWhoamiUrl + queryString;
        }

        /// <summary>
        /// Gets information about logged in user.
        /// </summary>
        /// <param name="request">The attributes you want to get. Optional.</param>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="INVALID_ARGUMENTS"/></description></item>
        /// </list></term>
        /// <description>Returned if a parameter does not fulfil requirements.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/user/#whoami"/>
        public Task<MicUserGetResponse> UserWhoami(MicUserWhoamiRequest request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserGetResponse>(UserWhoamiUrl(request),
                HttpMethod.Get, request, hasPayload: false, cancelToken);

        /// <inheritdoc cref="UserWhoami(MicUserWhoamiRequest, CancellationToken)"/>
        public Task<MicUserGetResponse> UserWhoami(MicUserDataAttributes? attributes,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserGetResponse>(UserWhoamiUrl(attributes),
                HttpMethod.Get, attributes, hasPayload: false, cancelToken);

        #endregion

        #region User API : REVOKE_CONSENT
        private static string UserRevokeConsentUrl(string? userName) =>
            FormattableString.Invariant($"users/{userName}/revoke-consent");

        /// <summary>
        /// This endpoint can be used in order to revoke consent that a user has given to the terms and conditions. A user can revoke their own consent. An administrator can revoke the consent of a user.
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
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="USER_NOT_FOUND"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>userName</c></description></item>
        /// </list></term>
        /// <description>Returned if the user cannot be found.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/user/#revoke-consent"/>
        public Task<MicModel> UserRevokeConsent(MicUserRevokeConsentRequest request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicModel>(UserRevokeConsentUrl(request?.Username),
                HttpMethod.Post, request, hasPayload: true, cancelToken);

        /// <inheritdoc cref="UserRevokeConsent(MicUserRevokeConsentRequest, CancellationToken)"/>
        /// <param name="userName">The user name of the user to revoke consent for.</param>
        public Task<MicModel> UserRevokeConsent(string userName,
            CancellationToken cancelToken = default) =>
            UserRevokeConsent(new MicUserRevokeConsentRequest { Username = userName },
                cancelToken);

        /// <inheritdoc cref="UserRevokeConsent(MicUserRevokeConsentRequest, CancellationToken)"/>
        public Task<MicModel> UserRevokeConsent(MicUserBasicInfo userInfo,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicModel>(UserRevokeConsentUrl(userInfo?.Username),
                HttpMethod.Post, userInfo, hasPayload: true, cancelToken);
        #endregion

        #region User API : REMOVE
        private static string UserRemoveUrl(string? userName) =>
            FormattableString.Invariant($"/users/{userName}");

        /// <summary>
        /// Removes a user.
        /// </summary>
        /// <returns>No repsonse</returns>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="NOT_AUTHORIZED_DOMAIN"/></description></item>
        /// </list></term>
        /// <description>Returned if the user tries to remove a user that is assigned to a domain that the user is not authorized to see.</description>
        /// </item>
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
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/user/#remove"/>
        public Task<MicResponse> UserRemove(MicUserRemoveRequest request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(UserRemoveUrl(request?.Username),
                HttpMethod.Delete, request, hasPayload: false, cancelToken);

        /// <inheritdoc cref="UserRemove(MicUserRemoveRequest, CancellationToken)"/>
        public Task<MicResponse> UserRemove(MicUserBasicInfo userInfo,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(UserRemoveUrl(userInfo?.Username),
                HttpMethod.Delete, userInfo, hasPayload: false, cancelToken);

        /// <inheritdoc cref="UserRemove(MicUserRemoveRequest, CancellationToken)"/>
        /// <param name="userName">A single username</param>
        public Task<MicResponse> UserRemove(string userName,
            CancellationToken cancelToken = default) =>
            UserRemove(new MicUserRemoveRequest { Username = userName },
                cancelToken);
        #endregion

        #region User API : UPDATE
        private static string UserUpdateUrl(string? userName) =>
            FormattableString.Invariant($"/users/{userName}");

        /// <summary>
        /// Updates a user. There is a possibility to disable an active user, using the <see cref="MicUserUpdateRequest.Enabled"/> property.
        /// <para>User attributes can be deleted by setting properties in the specified <see cref="MicUserUpdateRequest"/> instance to <see langword="null"/>.</para>
        /// </summary>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="NOT_AUTHORIZED_DOMAIN"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>domainName</c></description></item>
        /// </list></term>
        /// <description>Returned if the user tries to move a user to a domain that the user is not authorized to see.</description>
        /// </item>
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
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_NOT_DELETABLE"/></description></item>
        /// </list></term>
        /// <description>Returned if a non-deletable property was sent to be deleted.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="USER_NOT_FOUND"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>userName</c></description></item>
        /// </list></term>
        /// <description>Returned if the user cannot be found.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="INVALID_ARGUMENTS"/></description></item>
        /// </list></term>
        /// <description>Returned if a parameter does not fulfil requirements.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/user/#update"/>
        public Task<MicUserFullDetails> UserUpdate(MicUserUpdateRequest request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserFullDetails>(UserUpdateUrl(request?.Username),
#if NETSTANDARD1_3 || NETSTANDARD2_0
                new HttpMethod("PATCH"),
#else
                HttpMethod.Patch,
#endif
                request, hasPayload: true, cancelToken
                );
        #endregion

        #region User API : UPDATE_PROFILE
        private static string UserUpdateProfileUrl(string? userName) =>
            FormattableString.Invariant($"/users/{userName}/profile");

        /// <summary>
        /// Updates the profile of the logged in user. Currently updating the password is not possible at this endpoint. To achieve that the user needs to invoke the <see cref="AuthForgotPassword(MicAuthForgotPasswordRequest, CancellationToken)"/> and follow the email link as described above.
        /// <para>User attributes can be deleted by setting properties in the specified <see cref="MicUserUpdateRequest"/> instance to <see langword="null"/>.</para>
        /// </summary>
        /// <param name="request">The user name of the user must be set to the currently logged in user. This endpoint can only update the currently logged in user.</param>
        /// <exception cref="MicException">
        /// <list type="table">
        /// <listheader><term><see cref="MicException.MicErrorMessage"/></term><description>Description</description></listheader>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="NOT_AUTHORIZED"/></description></item>
        /// </list></term>
        /// <description>Returned if the user tries to update the profile of another user.</description>
        /// </item>
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
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="PROPERTY_NOT_DELETABLE"/></description></item>
        /// </list></term>
        /// <description>Returned if a non-deletable property was sent to be deleted.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="USER_NOT_FOUND"/></description></item>
        /// <item><term><see cref="MicErrorMessage.Property"/></term><description><c>userName</c></description></item>
        /// </list></term>
        /// <description>Returned if the user cannot be found.</description>
        /// </item>
        /// <item>
        /// <term><list type="table">
        /// <listheader><term>Property</term><description>Value</description></listheader>
        /// <item><term><see cref="MicErrorMessage.MessageKey"/></term><description><see cref="INVALID_ARGUMENTS"/></description></item>
        /// </list></term>
        /// <description>Returned if a parameter does not fulfil requirements.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/user/#update-profile"/>
        public Task<MicUserFullDetails> UserUpdateProfile(MicUserUpdateRequest request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserFullDetails>(UserUpdateProfileUrl(request?.Username),
#if NETSTANDARD1_3 || NETSTANDARD2_0
                new HttpMethod("PATCH"),
#else
                HttpMethod.Patch,
#endif
                request, hasPayload: true, cancelToken
                );
        #endregion

        #region User API : UPDATE_USERDATA
        private static string UserUpdateUserdataUrl(string? userName) =>
            FormattableString.Invariant($"/users/{userName}/data");

        /// <summary>
        /// Updates the customer user data of a user
        /// </summary>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/user/#update-userdata"/>
        public Task<MicUserdataResponse> UserUpdateUserdata(MicUserUpdateUserdataRequest request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserdataResponse>(UserUpdateUserdataUrl(request?.Username),
#if NETSTANDARD1_3 || NETSTANDARD2_0
                new HttpMethod("PATCH"),
#else
                HttpMethod.Patch,
#endif
                request, hasPayload: true, cancelToken
                );
        #endregion

        #region User API : RESET_PASSWORD
        private static string UserResetPasswordUrl(string? userName) =>
            FormattableString.Invariant($"/users/{userName}/reset-password");

        /// <summary>
        /// Resets the password for the specified user which results in a link being sent by email to the user,
        /// that can visit the link to set a new password. The link is usable only once. If the link has expired,
        /// this endpoint can be invoked again.
        /// </summary>
        /// <remarks>
        /// Note: Following a password reset, the user account is disabled until the password has been successfully changed.
        /// </remarks>
        /// <seealso href="https://docs.telenorconnexion.com/mic/rest-api/user/#reset-password"/>
        public Task<MicResponse> UserResetPassword(MicUserResetPasswordRequest request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(UserResetPasswordUrl(request?.Username),
                HttpMethod.Post, request, hasPayload: true, cancelToken);

        /// <inheritdoc cref="UserResetPassword(MicUserResetPasswordRequest, CancellationToken)"/>
        /// <param name="userName">The username to reset set password for.</param>
        public Task<MicResponse> UserResetPassword(string userName, CancellationToken cancelToken = default) =>
            UserResetPassword(new MicUserResetPasswordRequest { Username = userName }, cancelToken);

        /// <inheritdoc cref="UserResetPassword(MicUserResetPasswordRequest, CancellationToken)"/>
        public Task<MicResponse> UserResetPassword(MicUserBasicInfo userInfo, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicResponse>(UserResetPasswordUrl(userInfo?.Username), HttpMethod.Post,
                userInfo, hasPayload: true, cancelToken);
        #endregion // User API : RESET_PASSWORD
    }
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
}