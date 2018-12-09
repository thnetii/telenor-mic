using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.SecurityToken;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.Model;
using THNETII.Common;
using THNETII.Networking.Http;

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

        string ApiKey { get; set; }
    }

    /// <summary>
    /// Provides basic functionality for MIC Client implementations.
    /// </summary>
    public partial class MicClient : IMicClient, IAmazonService, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly Uri apiGatewayEndpoint;
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
        public CognitoAWSCredentials AwsCredentials { get; }

        CognitoAWSCredentials IMicClient.AwsCredentials => AwsCredentials;

        public string ApiKey { get; protected set; }

        string IMicClient.ApiKey
        {
            get => ApiKey;
            set => ApiKey = value;
        }

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

        public static async Task<MicClient> CreateFromHostname(
            string hostname, CancellationToken cancelToken = default)
        {
            var handler = new MicRestHttpHandler(new HttpClientHandler());
            var httpClient = new HttpClient(handler);
            MicClient micClient = await CreateFromHostname(hostname, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            handler.MicClient = micClient;
            return micClient;
        }

        public static async Task<MicClient> CreateFromHostname(
            string hostname, HttpMessageHandler httpHandler,
            CancellationToken cancelToken = default)
        {
            var handler = new MicRestHttpHandler(httpHandler);
            var httpClient = new HttpClient(handler);
            MicClient micClient = await CreateFromHostname(hostname, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            handler.MicClient = micClient;
            return micClient;
        }

        public static async Task<MicClient> CreateFromHostname(
            string hostname, string apiKey, CancellationToken cancelToken = default)
        {
            var handler = new MicRestHttpHandler(new HttpClientHandler());
            var httpClient = new HttpClient(handler);
            MicClient micClient = await CreateFromHostname(hostname, apiKey, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            handler.MicClient = micClient;
            return micClient;
        }

        public static async Task<MicClient> CreateFromHostname(
            string hostname, string apiKey, HttpMessageHandler httpHandler,
            CancellationToken cancelToken = default)
        {
            var handler = new MicRestHttpHandler(httpHandler);
            var httpClient = new HttpClient(handler);
            MicClient micClient = await CreateFromHostname(hostname, apiKey, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            handler.MicClient = micClient;
            return micClient;
        }

        private static async Task<MicClient> CreateFromHostname(string hostname, HttpClient httpClient, CancellationToken cancelToken)
        {
            var manifest = await MicManifest.GetMicManifest(hostname, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            var micClient = new MicClient(manifest, httpClient);
            var metadataManifest = await micClient.MetadataManifest(cancelToken).ConfigureAwait(continueOnCapturedContext: false);
            micClient.ApiKey = metadataManifest.ApiKey;
            return micClient;
        }

        private static async Task<MicClient> CreateFromHostname(string hostname, string apiKey, HttpClient httpClient, CancellationToken cancelToken)
        {
            var manifest = await MicManifest.GetMicManifest(hostname, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            //string apiKey = manifest.ApiKeyId;
            var micClient = new MicClient(manifest, apiKey, httpClient);
            return micClient;
        }

        private MicClient(MicManifest manifest, HttpClient httpClient) : this(manifest)
        {
            apiGatewayEndpoint = manifest.GetApiGatewayBaseEndpoint();
            this.httpClient = httpClient;
        }

        private MicClient(MicManifest manifest, string apiKey, HttpClient httpClient) : this(manifest, httpClient)
        {
            ApiKey = apiKey.ThrowIfNullOrWhiteSpace(nameof(apiKey));
        }

        #endregion // Constructor

        #region Helper methods

        /// <summary>
        /// Invokes the specified action on the configured MIC API endpoint.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object.</typeparam>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <param name="relativeUrl">The URL relative to the API Gateway root URI including all escaped path and query parameters.</param>
        /// <param name="request">The request object, containing the operation parameters.</param>
        /// <param name="cancelToken">An optional cancellation token that can be used to prematurely cancel the operation.</param>
        /// <returns>A deserialized instance reqpresenting the response payload of the operation.</returns>
        /// <remarks>
        /// <para>Valid values for <paramref name="relativeUrl"/> are the identifiers of the instance methods defined in the <see cref="MicClient"/> class that represent MIC API oprations.</para>
        /// <para>Classes derived from <see cref="MicClient"/> should implement this method using a switch statement with each case using the <c>nameof</c> operator.</para>
        /// <para>For MIC API endpoints that do not return any object <typeparamref name="TResponse"/> should be <see cref="MicModel"/> which represents an empty return value.</para>
        /// </remarks>
        [SuppressMessage("Design", "CA1054: Uri parameters should not be strings", Scope = "parameter")]
        protected virtual async Task<TResponse> InvokeClientRequest<TRequest, TResponse>(string relativeUrl, HttpMethod httpMethod,
            TRequest request, bool hasPayload = true, CancellationToken cancelToken = default)
            where TRequest : MicModel
            where TResponse : MicModel
        {
            Uri requestUri = new Uri(apiGatewayEndpoint, relativeUrl);
            using (var requestMessage = new HttpRequestMessage(httpMethod, requestUri))
            {
                if (hasPayload)
                {
                    string requestJson = JsonConvert.SerializeObject(request);
                    var requestContent = new StringContent(requestJson, Encoding.UTF8, HttpWellKnownMediaType.ApplicationJson);
                    requestMessage.Content = requestContent;
                }

                using (var responseMessage = await httpClient.SendAsync(requestMessage, cancelToken).ConfigureAwait(continueOnCapturedContext: false))
                    return await DeserializeOrThrow<TResponse>(responseMessage, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        /// <summary>
        /// Invokes the specified action on the configured MIC API endpoint and
        /// updates the Credentials if the response contains new AWS Credentials.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object.</typeparam>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <param name="relativeUrl">The URL relative to the API Gateway root URI including all escaped path and query parameters.</param>
        /// <param name="request">The request object, containing the operation parameters.</param>
        /// <param name="cancelToken">An optional cancellation token that can be used to prematurely cancel the operation.</param>
        /// <returns>A deserialized instance reqpresenting the response payload of the operation.</returns>
        /// <remarks>
        /// <para>For MIC API endpoints that do not return any object <typeparamref name="TResponse"/> should be <see cref="MicModel"/> which represents an empty return value.</para>
        /// </remarks>
        [SuppressMessage("Design", "CA1054: Uri parameters should not be strings", Scope = "parameter")]
        protected async Task<TResponse> HandleClientRequest<TRequest, TResponse>(
            string relativeUrl, HttpMethod httpMethod, TRequest request,
            bool hasPayload = true, CancellationToken cancelToken = default)
            where TRequest : MicModel
            where TResponse : MicModel
        {
            var response = await InvokeClientRequest<TRequest, TResponse>(
                relativeUrl, httpMethod, request, hasPayload, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            if (response is MicAuthLoginResponse loginResponse)
                UpdateCredentials(loginResponse);
            return response;
        }

        private async Task<TResponse> DeserializeOrThrow<TResponse>(HttpResponseMessage httpResponse, CancellationToken cancelToken)
        {
            httpResponse.EnsureSuccessStatusCode();
            if (!httpResponse.Content.IsJson())
            {
                throw new HttpRequestException("The Content-Type of the response is not acceptable.");
            }
            using (var textReader = await httpResponse.Content.ReadAsStreamReaderAsync().ConfigureAwait(continueOnCapturedContext: false))
            using (var jsonReader = new JsonTextReader(textReader))
                return await DeserializeOrThrow<TResponse>(jsonReader, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        protected static async Task<TResponse> DeserializeOrThrow<TResponse>(JsonReader jsonReader,
            CancellationToken cancelToken = default)
        {
            var jsonObject = await JObject.LoadAsync(jsonReader, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            if (jsonObject.TryGetValue(MicException.ErrorMessageKey, out var errorToken))
            {
                throw new MicException(errorToken.ToObject<MicErrorMessage>());
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
