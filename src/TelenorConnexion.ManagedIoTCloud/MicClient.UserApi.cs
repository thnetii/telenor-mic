using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.Model;

namespace TelenorConnexion.ManagedIoTCloud
{
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
    public partial interface IMicClient
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        Task<MicUserCreateResponse> UserCreate(MicUserCreateRequest request, CancellationToken cancelToken = default);

        /// <summary>
        /// Resets the password for the specified user which results in a link being sent by email to the user,
        /// that can visit the link to set a new password. The link is usable only once. If the link has expired,
        /// this endpoint can be invoked again.
        /// </summary>
        /// <remarks>
        /// Note: Following a password reset, the user account is disabled until the password has been successfully changed.
        /// </remarks>
        Task<MicResponse> UserResetPassword(MicUserResetPasswordRequest request, CancellationToken cancelToken = default);

        /// <summary>
        /// Gets information about a user.
        /// </summary>
        Task<MicUserGetResponse> UserGet(MicUserGetRequest request, CancellationToken cancelToken = default);
    }

    public partial class MicClient
    {
        #region User API : CREATE
        private const string userCreateUrl = "users";

        /// <summary>
        /// Creates a new user.
        /// </summary>
        public Task<MicUserCreateResponse> UserCreate(MicUserFullDetails userDetails, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserFullDetails, MicUserCreateResponse>(userCreateUrl, HttpMethod.Post,
                userDetails, hasPayload: true, cancelToken);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        public Task<MicUserCreateResponse> UserCreate(MicUserCreateRequest request, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserCreateRequest, MicUserCreateResponse>(userCreateUrl, HttpMethod.Post,
                request, hasPayload: true, cancelToken);
        #endregion // User API : CREATE

        #region User API : RESET_PASSWORD
        private static string UserResetPasswordUrl(string userName) =>
            FormattableString.Invariant($"/users/{userName}/reset-password");

        /// <summary>
        /// Resets the password for the specified user which results in a link being sent by email to the user,
        /// that can visit the link to set a new password. The link is usable only once. If the link has expired,
        /// this endpoint can be invoked again.
        /// </summary>
        /// <remarks>
        /// Note: Following a password reset, the user account is disabled until the password has been successfully changed.
        /// </remarks>
        public Task UserResetPassword(string userName, CancellationToken cancelToken = default) =>
            UserResetPassword(new MicUserResetPasswordRequest { Username = userName }, cancelToken);

        /// <summary>
        /// Resets the password for the specified user which results in a link being sent by email to the user,
        /// that can visit the link to set a new password. The link is usable only once. If the link has expired,
        /// this endpoint can be invoked again.
        /// </summary>
        /// <remarks>
        /// Note: Following a password reset, the user account is disabled until the password has been successfully changed.
        /// </remarks>
        public Task UserResetPassword(MicUserBasicInfo userInfo, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserBasicInfo, MicResponse>(UserResetPasswordUrl(userInfo?.Username), HttpMethod.Post,
                userInfo, hasPayload: true, cancelToken);

        /// <summary>
        /// Resets the password for the specified user which results in a link being sent by email to the user,
        /// that can visit the link to set a new password. The link is usable only once. If the link has expired,
        /// this endpoint can be invoked again.
        /// </summary>
        /// <remarks>
        /// Note: Following a password reset, the user account is disabled until the password has been successfully changed.
        /// </remarks>
        public Task UserResetPassword(MicUserResetPasswordRequest request, CancellationToken cancelToken = default) =>
            ((IMicClient)this).UserResetPassword(request, cancelToken);

        Task<MicResponse> IMicClient.UserResetPassword(MicUserResetPasswordRequest request, CancellationToken cancelToken) =>
            HandleClientRequest<MicUserResetPasswordRequest, MicResponse>(UserResetPasswordUrl(request?.Username), HttpMethod.Post,
                request, hasPayload: true, cancelToken);
        #endregion // User API : RESET_PASSWORD

        #region User API : GET
        private static string UserGetUrl(MicUserGetRequest request)
        {
            var attributes = ((IMicModel)request)?.AdditionalData;
            var attributesValue = string.Join(",", attributes?.Keys.Select(k => Uri.EscapeDataString(k)) ?? Enumerable.Empty<string>());
            return FormattableString.Invariant($"users/{request?.Username}?attributes={attributesValue}");
        }

        /// <summary>
        /// Gets information about a user.
        /// </summary>
        /// <param name="userName">The user name of the user to get.</param>
        public Task<MicUserGetResponse> UserGet(string userName, CancellationToken cancelToken = default) =>
            UserGet(new MicUserGetRequest { Username = userName }, cancelToken);

        /// <summary>
        /// Gets information about a user.
        /// </summary>
        public Task<MicUserGetResponse> UserGet(MicUserGetRequest request, CancellationToken cancelToken = default) =>
            HandleClientRequest<MicUserGetRequest, MicUserGetResponse>(UserGetUrl(request), HttpMethod.Get,
                request, hasPayload: false, cancelToken);
        #endregion // User API : GET
    }
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
}
