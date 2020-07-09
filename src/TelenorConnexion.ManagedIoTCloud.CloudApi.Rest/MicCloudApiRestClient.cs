using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.SecurityToken;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TelenorConnexion.ManagedIoTCloud.CloudApi.Model;

using THNETII.Common;
using THNETII.Networking.Http;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    /// <summary>
    /// Provides basic functionality for MIC Client implementations.
    /// </summary>
    public partial class MicCloudApiRestClient : IAmazonService, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly Uri apiGatewayEndpoint;
        private readonly AmazonCognitoIdentityClient cognitoClient;
        private readonly AmazonSecurityTokenServiceClient stsClient;
        private bool _disposed;

        /// <summary>
        /// Gets the Manifest document describing the MIC deployment the client
        /// connects to.
        /// </summary>
        public MicManifest Manifest { get; }

        /// <summary>
        /// Gets the Amazon Client Configuration used when accessing Amazon Web
        /// Services through the AWS SDK.
        /// </summary>
        public MicClientConfig Config { get; }

        IClientConfig IAmazonService.Config => Config;

        /// <summary>
        /// Gets the last set of MIC Credentials received from a successful
        /// login or refresh operation.
        /// </summary>
        protected MicAuthCredentials? Credentials { get; set; }

        /// <summary>
        /// Gets the AWS Cognito Identity Credentials object used to authenticate
        /// requests to authenticated MIC API endpoints.
        /// </summary>
        public CognitoAWSCredentials AwsCredentials { get; }

        public string? ApiKey { get; protected set; }

        #region Constructor

        /// <summary>
        /// Initializes a new MIC client instance using the specified
        /// MIC Manifest document.
        /// </summary>
        /// <param name="manifest"></param>
        protected MicCloudApiRestClient(HttpClient httpClient, MicManifest manifest) : base()
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            apiGatewayEndpoint = manifest.GetApiGatewayBaseEndpoint();

            Config = new MicClientConfig() { RegionEndpoint = Manifest.AwsRegion! };

            var anonymousCreds = new AnonymousAWSCredentials();
            cognitoClient = new AmazonCognitoIdentityClient(
                anonymousCreds,
                Manifest.CreateClientConfig<AmazonCognitoIdentityConfig>(Config)
                );
            stsClient = new AmazonSecurityTokenServiceClient(
                anonymousCreds,
                Manifest.CreateClientConfig<AmazonSecurityTokenServiceConfig>(Config)
                );

            AwsCredentials = new CognitoAWSCredentials(accountId: null,
                identityPoolId: Manifest.IdentityPool,
                unAuthRoleArn: null, authRoleArn: null,
                cognitoClient, stsClient
                );
        }

        public MicCloudApiRestClient(HttpClient httpClient, MicManifest manifest, string apiKey)
            : this(httpClient, manifest) =>
            ApiKey = apiKey.ThrowIfNullOrWhiteSpace(nameof(apiKey));

        public static async Task<MicCloudApiRestClient> CreateFromHostname(
            HttpClient httpClient, string hostname,
            CancellationToken cancelToken = default)
        {
            var manifest = await MicManifest
                .GetMicManifest(hostname, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            var micClient = new MicCloudApiRestClient(httpClient, manifest);
            var metadataManifest = await micClient
                .MetadataManifest(cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            micClient.ApiKey = metadataManifest.ApiKey;
            return micClient;
        }

        public static async Task<MicCloudApiRestClient> CreateFromHostname(
            HttpClient httpClient, string hostname, string apiKey,
            CancellationToken cancelToken = default)
        {
            var manifest = await MicManifest
                .GetMicManifest(hostname, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            var micClient = new MicCloudApiRestClient(httpClient, manifest, apiKey);
            return micClient;
        }

        #endregion // Constructor

        #region Helper methods

        protected virtual void ApplyRequestAuthentication(HttpRequestMessage request)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            if (Credentials is MicAuthCredentials creds)
            {
                request.Headers.Add("identityId", creds.IdentityId ?? string.Empty);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", creds.Token);
            }
            if (ApiKey.TryNotNullOrWhiteSpace(out string? apiKey))
                request.Headers.Add("x-api-key", apiKey);
        }

        /// <summary>
        /// Invokes the specified action on the configured MIC API endpoint.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <param name="relativeUrl">The URL relative to the API Gateway root URI including all escaped path and query parameters.</param>
        /// <param name="request">The request object, containing the operation parameters.</param>
        /// <param name="cancelToken">An optional cancellation token that can be used to prematurely cancel the operation.</param>
        /// <returns>A deserialized instance reqpresenting the response payload of the operation.</returns>
        /// <remarks>
        /// <para>Valid values for <paramref name="relativeUrl"/> are the identifiers of the instance methods defined in the <see cref="MicCloudApiRestClient"/> class that represent MIC API oprations.</para>
        /// <para>Classes derived from <see cref="MicCloudApiRestClient"/> should implement this method using a switch statement with each case using the <c>nameof</c> operator.</para>
        /// <para>For MIC API endpoints that do not return any object <typeparamref name="TResponse"/> should be <see cref="MicModel"/> which represents an empty return value.</para>
        /// </remarks>
        [SuppressMessage("Design", "CA1054: Uri parameters should not be strings", Scope = "parameter")]
        protected virtual async Task<TResponse> InvokeClientRequest<TResponse>(
            string relativeUrl, HttpMethod httpMethod, MicModel? request,
            bool hasPayload = true, CancellationToken cancelToken = default)
            where TResponse : MicModel
        {
            // Strip leading '/' in order to make proper relative Urls the API Gateway Root URL.
            relativeUrl = relativeUrl is null ? string.Empty : relativeUrl.TrimStart('/');
            Uri requestUri = new Uri(apiGatewayEndpoint, relativeUrl);
            using var requestMessage = new HttpRequestMessage(httpMethod, requestUri);
            if (hasPayload)
            {
                _ = request ?? throw new ArgumentNullException(nameof(request));

                string requestJson = JsonConvert.SerializeObject(request);
                var requestContent = new StringContent(requestJson, Encoding.UTF8, HttpWellKnownMediaType.ApplicationJson);
                requestMessage.Content = requestContent;
            }

            ApplyRequestAuthentication(requestMessage);
            using var responseMessage = await httpClient
                .SendAsync(requestMessage, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return await DeserializeOrThrow<TResponse>(
                responseMessage, cancelToken
                )
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        /// <summary>
        /// Invokes the specified action on the configured MIC API endpoint and
        /// updates the Credentials if the response contains new AWS Credentials.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <param name="relativeUrl">The URL relative to the API Gateway root URI including all escaped path and query parameters.</param>
        /// <param name="request">The request object, containing the operation parameters.</param>
        /// <param name="cancelToken">An optional cancellation token that can be used to prematurely cancel the operation.</param>
        /// <returns>A deserialized instance reqpresenting the response payload of the operation.</returns>
        /// <remarks>
        /// <para>For MIC API endpoints that do not return any object <typeparamref name="TResponse"/> should be <see cref="MicModel"/> which represents an empty return value.</para>
        /// </remarks>
        [SuppressMessage("Design", "CA1054: Uri parameters should not be strings", Scope = "parameter")]
        protected async Task<TResponse> HandleClientRequest<TResponse>(
            string relativeUrl, HttpMethod httpMethod, MicModel? request,
            bool hasPayload = true, CancellationToken cancelToken = default)
            where TResponse : MicModel
        {
            var response = await InvokeClientRequest<TResponse>(
                relativeUrl, httpMethod, request, hasPayload, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            if (response is MicAuthLoginResponse loginResponse)
                UpdateCredentials(loginResponse);
            return response;
        }

        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
        private async Task<TResponse> DeserializeOrThrow<TResponse>(
            HttpResponseMessage httpResponse, CancellationToken cancelToken)
        {
            httpResponse.EnsureSuccessStatusCode();
            if (!httpResponse.Content.IsJson())
                throw new HttpRequestException("The Content-Type of the response is not acceptable.");
            using var textReader = await httpResponse.Content
                .ReadAsStreamReaderAsync()
                .ConfigureAwait(continueOnCapturedContext: false);
            using var jsonReader = new JsonTextReader(textReader);
            return await DeserializeOrThrow<TResponse>(jsonReader, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        protected static async Task<TResponse> DeserializeOrThrow<TResponse>(
            JsonReader jsonReader, CancellationToken cancelToken = default)
        {
            var jsonObject = await JObject.LoadAsync(jsonReader, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            if (jsonObject.TryGetValue(MicException.ErrorMessageKey, out var errorToken))
                throw new MicException(errorToken.ToObject<MicErrorMessage>()!);
            return jsonObject.ToObject<TResponse>();
        }

        protected virtual void UpdateCredentials(MicAuthLoginResponse loginResponse)
        {
            if (loginResponse?.Credentials is MicAuthCredentials creds)
            {
                Credentials = creds;
                AwsCredentials.CacheIdentityId(creds.IdentityId);
                AwsCredentials.AddLogin(
                    Manifest.GetCognitoProviderName(), creds.Token);
            }
        }

        #endregion // Helper methods

        #region Dispose

        /// <inheritdoc />
        ~MicCloudApiRestClient() => Dispose(disposing: false);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(disposing: true);
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
