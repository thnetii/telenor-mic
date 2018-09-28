using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.Model;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud
{
    public interface IMicClient
    {
        MicManifest Manifest { get; }

        MicAuthCredentials Credentials { get; }
    }

    public abstract class MicClient : IMicClient
    {
        public MicManifest Manifest { get; }

        protected MicAuthCredentials Credentials { get; set; }

        MicAuthCredentials IMicClient.Credentials => Credentials;

        #region Constructor

        protected MicClient(MicManifest manifest) : base() =>
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));

        #endregion // Constructor

        #region Public Cloud API methods

        #region Auth API

        #region Auth API : CONFIRM_SIGN_UP

        public Task<MicModel> AuthConfirmSignup(string token, CancellationToken cancelToken = default) =>
            AuthConfirmSignup(new MicAuthConfirmSignupRequest { Token = token }, cancelToken);

        public Task<MicModel> AuthConfirmSignup(string username, string code, CancellationToken cancelToken = default) =>
            AuthConfirmSignup(new MicAuthConfirmSignupRequest { Username = username, Code = code }, cancelToken);

        public Task<MicModel> AuthConfirmSignup(IMicAuthConfirmSignupRequest request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<IMicAuthConfirmSignupRequest, MicModel>(
                nameof(AuthConfirmSignup), request, cancelToken);

        #endregion // Auth API : CONFIRM_SIGN_UP

        #region Auth API : Login

        public Task<MicAuthLoginResponse> AuthLogin(string username, string password,
            CancellationToken cancelToken = default) =>
            AuthLogin(new MicAuthLoginRequest
            {
                Username = username,
                Password = password
            }, cancelToken);

        public Task<MicAuthLoginResponse> AuthLogin(IMicAuthLoginRequest request,
            CancellationToken cancelToken = default) =>
            HandleClientRequest<IMicAuthLoginRequest, MicAuthLoginResponse>(
                nameof(AuthLogin), request, cancelToken);

        #endregion // Auth API : Login

        #endregion // Auth API

        #endregion // Public Cloud API methods

        #region Helper methods

        protected abstract Task<TResponse> InvokeClientRequest<TRequest, TResponse>(string actionName,
            TRequest request, CancellationToken cancelToken = default)
            where TRequest : IMicModel
            where TResponse : IMicModel;

        protected async Task<TResponse> HandleClientRequest<TRequest, TResponse>(
            string actionName, TRequest request,
            CancellationToken cancelToken = default)
            where TRequest : IMicModel
            where TResponse : IMicModel
        {
            var response = await InvokeClientRequest<TRequest, TResponse>(
                actionName, request, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            if (response is IMicAuthLoginResponse loginResponse)
                UpdateCredentials(loginResponse);
            return response;
        }

        protected async Task<TResponse> DeserializeOrThrow<TResponse>(JsonReader jsonReader,
            CancellationToken cancelToken = default)
        {
            var jsonObject = await JObject.LoadAsync(jsonReader, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            if (jsonObject.ContainsKey(MicException.ErrorMessageKey))
            {
                throw new MicException(jsonObject.ToObject<MicErrorMessage>());
            }
            return jsonObject.ToObject<TResponse>();
        }

        protected virtual void UpdateCredentials(IMicAuthLoginResponse loginResponse)
        {
            if ((loginResponse?.Credentials).TryNotNull(out var creds))
                Credentials = creds;
        }

        #endregion // Helper methods
    }
}
