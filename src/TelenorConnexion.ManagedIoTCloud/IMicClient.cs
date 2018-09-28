using System.Threading;
using System.Threading.Tasks;

namespace TelenorConnexion.ManagedIoTCloud
{
    public interface IMicClient
    {
        MicManifest Manifest { get; }

        MicAuthLoginCredentials Credentials { get; }

        Task<TResponse> CloudApiRequest<TRequest, TResponse>(string actionName,
            TRequest request, CancellationToken cancelToken = default);

        #region Auth

        Task<MicAuthLoginResponse> AuthLogin(MicAuthLoginRequest request,
            CancellationToken cancelToken = default);

        Task<MicAuthLoginResponse> AuthRefresh(MicAuthRefreshRequest request,
            CancellationToken cancelToken = default);

        #endregion // Auth

        #region User

        Task<MicUserCreateResponse> UserCreate(MicUserCreateRequest request,
            CancellationToken cancelToken = default);

        Task UserResetPassword(MicUserResetPasswordRequest request,
            CancellationToken cancelToken = default);

        Task<MicUserDetails> UserGet(MicUserGetRequest request,
            CancellationToken cancelToken = default);

        #endregion // User
    }
}
