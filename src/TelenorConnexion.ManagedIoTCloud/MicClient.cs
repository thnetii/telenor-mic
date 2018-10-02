using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.SecurityToken;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.Model;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud
{
    /// <summary>
    /// Defines the contract for MIC Cloud API clients.
    /// </summary>
    public partial interface IMicClient
    {
        /// <summary>
        /// Gets the Amazon Client Configuration used when accessing Amazon Web
        /// Services through the AWS SDK.
        /// </summary>
        MicClientConfig Config { get; }

        /// <summary>
        /// Gets the Manifest document describing the MIC deployment the client
        /// connects to.
        /// </summary>
        MicManifest Manifest { get; }

        /// <summary>
        /// Gets the last set of MIC Credentials received from a successful
        /// login or refresh operation.
        /// </summary>
        MicAuthCredentials Credentials { get; }

        /// <summary>
        /// Gets the AWS Cognito Identity Credentials object used to authenticate
        /// request to authenticated MIC API endpoints.
        /// </summary>
        CognitoAWSCredentials AwsCredentials { get; }
    }

    /// <summary>
    /// Provides basic functionality for MIC Client implementations.
    /// </summary>
    public abstract partial class MicClient : IMicClient, IAmazonService, IDisposable
    {
        private readonly AmazonCognitoIdentityClient cognitoClient;
        private readonly AmazonSecurityTokenServiceClient stsClient;
        private bool _disposed;

        /// <inheritdoc />
        public MicManifest Manifest { get; }

        /// <inheritdoc />
        public MicClientConfig Config { get; }

        IClientConfig IAmazonService.Config => Config;

        /// <inheritdoc />
        protected MicAuthCredentials Credentials { get; set; }

        MicAuthCredentials IMicClient.Credentials => Credentials;

        /// <inheritdoc />
        protected CognitoAWSCredentials AwsCredentials { get; }

        CognitoAWSCredentials IMicClient.AwsCredentials => AwsCredentials;

        #region Constructor

        /// <summary>
        /// Initializes a new MIC client instance using the specified
        /// MIC Manifest document.
        /// </summary>
        /// <param name="manifest"></param>
        protected MicClient(MicManifest manifest) : base()
        {
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));

            Config = new MicClientConfig() { RegionEndpoint = Manifest.AwsRegion };

            var anonymousCreds = new AnonymousAWSCredentials();
            cognitoClient = new AmazonCognitoIdentityClient(anonymousCreds, Config.Create<AmazonCognitoIdentityConfig>());
            stsClient = new AmazonSecurityTokenServiceClient(anonymousCreds, Config.Create<AmazonSecurityTokenServiceConfig>());

            AwsCredentials = new CognitoAWSCredentials(accountId: null, identityPoolId: Manifest.IdentityPool,
                unAuthRoleArn: null, authRoleArn: null,
                cognitoClient, stsClient
                );
        }

        #endregion // Constructor

        #region Helper methods

        /// <summary>
        /// Invokes the specified action on the configured MIC API endpoint.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object.</typeparam>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <param name="actionName">The identifier of the action that was invoked.</param>
        /// <param name="request">The request object, containing the operation parameters.</param>
        /// <param name="cancelToken">An optional cancellation token that can be used to prematurely cancel the operation.</param>
        /// <returns>A deserialized instance reqpresenting the response payload of the operation.</returns>
        /// <remarks>
        /// <para>Valid values for <paramref name="actionName"/> are the identifiers of the instance methods defined in the <see cref="MicClient"/> class that represent MIC API oprations.</para>
        /// <para>Classes derived from <see cref="MicClient"/> should implement this method using a switch statement with each case using the <c>nameof</c> operator.</para>
        /// <para>For MIC API endpoints that do not return any object <typeparamref name="TResponse"/> should be <see cref="MicModel"/> which represents an empty return value.</para>
        /// </remarks>
        protected abstract Task<TResponse> InvokeClientRequest<TRequest, TResponse>(string actionName,
            TRequest request, CancellationToken cancelToken = default)
            where TRequest : MicModel
            where TResponse : MicModel;

        /// <summary>
        /// Invokes the specified action on the configured MIC API endpoint and
        /// updates the Credentials if the response contains new AWS Credentials.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object.</typeparam>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <param name="actionName">The identifier of the action that was invoked.</param>
        /// <param name="request">The request object, containing the operation parameters.</param>
        /// <param name="cancelToken">An optional cancellation token that can be used to prematurely cancel the operation.</param>
        /// <returns>A deserialized instance reqpresenting the response payload of the operation.</returns>
        /// <remarks>
        /// <para>Valid values for <paramref name="actionName"/> are the identifiers of the instance methods defined in the <see cref="MicClient"/> class that represent MIC API oprations.</para>
        /// <para>Classes derived from <see cref="MicClient"/> should implement this method using a switch statement with each case using the <c>nameof</c> operator.</para>
        /// <para>For MIC API endpoints that do not return any object <typeparamref name="TResponse"/> should be <see cref="MicModel"/> which represents an empty return value.</para>
        /// </remarks>
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
            {
                Credentials = creds;
                AwsCredentials.AddLogin(Manifest.GetCognitoProviderName(), creds.Token);
            }
        }

        #endregion // Helper methods

        #region Dispose

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                cognitoClient.Dispose();
                stsClient.Dispose();

                _disposed = true;
            }
        }

        #endregion // Dispose
    }
}
