using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.Model;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud
{
    public partial interface IMicClient
    {
        MicManifest Manifest { get; }

        MicAuthCredentials Credentials { get; }
    }

    /// <summary>
    /// Provides basic functionality for MIC Client implementations.
    /// </summary>
    public abstract partial class MicClient : IMicClient
    {
        public MicManifest Manifest { get; }

        protected MicAuthCredentials Credentials { get; set; }

        MicAuthCredentials IMicClient.Credentials => Credentials;

        #region Constructor

        protected MicClient(MicManifest manifest) : base() =>
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));

        #endregion // Constructor

        #region Helper methods

        protected abstract Task<TResponse> InvokeClientRequest<TRequest, TResponse>(string actionName,
            TRequest request, CancellationToken cancelToken = default)
            where TRequest : MicModel
            where TResponse : MicModel;

        protected async Task<TResponse> HandleClientRequest<TRequest, TResponse>(
            string actionName, TRequest request,
            CancellationToken cancelToken = default)
            where TRequest : MicModel
            where TResponse : MicModel
        {
            var response = await InvokeClientRequest<TRequest, TResponse>(
                actionName, request, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            if (response is MicAuthLoginResponse loginResponse)
                UpdateCredentials(loginResponse);
            return response;
        }

        protected static async Task<TResponse> DeserializeOrThrow<TResponse>(JsonReader jsonReader,
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

        protected virtual void UpdateCredentials(MicAuthLoginResponse loginResponse)
        {
            if ((loginResponse?.Credentials).TryNotNull(out var creds))
                Credentials = creds;
        }

        #endregion // Helper methods
    }
}
