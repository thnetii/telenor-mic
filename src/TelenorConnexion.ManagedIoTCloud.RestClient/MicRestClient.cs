﻿using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.Model;
using THNETII.Common;
using THNETII.Networking.Http;

namespace TelenorConnexion.ManagedIoTCloud.RestClient
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
    public class MicRestClient : MicClient, IMicClient, IDisposable
    {
        private readonly HttpClient httpClient;

        public string ApiKey { get; }

        #region Constructors

        public static async Task<MicRestClient> CreateFromHostname(
            string hostname, string apiKey, CancellationToken cancelToken = default)
        {
            var handler = new MicRestHttpHandler(new HttpClientHandler());
            var httpClient = new HttpClient(handler);
            MicRestClient micClient = await CreateFromHostname(hostname, apiKey, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            handler.MicClient = micClient;
            return micClient;
        }

        public static async Task<MicRestClient> CreateFromHostname(
            string hostname, string apiKey, HttpMessageHandler httpHandler,
            CancellationToken cancelToken = default)
        {
            var handler = new MicRestHttpHandler(httpHandler);
            var httpClient = new HttpClient(handler);
            MicRestClient micClient = await CreateFromHostname(hostname, apiKey, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            handler.MicClient = micClient;
            return micClient;
        }

        private static async Task<MicRestClient> CreateFromHostname(string hostname, string apiKey, HttpClient httpClient, CancellationToken cancelToken)
        {
            var manifest = await MicManifest.GetMicManifest(hostname, httpClient, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            //string apiKey = manifest.ApiKeyId;
            var micClient = new MicRestClient(manifest, apiKey, httpClient);
            return micClient;
        }

        private MicRestClient(MicManifest manifest, string apiKey, HttpClient httpClient) : base(manifest)
        {
            ApiKey = apiKey.ThrowIfNullOrWhiteSpace(nameof(apiKey));
            this.httpClient = httpClient;
        }

        public MicRestClient(MicManifest manifest, string apiKey) : this(manifest, apiKey, new HttpClient(new MicRestHttpHandler(new HttpClientHandler())))
        {
        }

        #endregion

        #region Overrides

        protected override async Task<TResponse> InvokeClientRequest<TRequest, TResponse>(string actionName, TRequest request, CancellationToken cancelToken = default)
        {
            HttpMethod httpMethod = request is null ? HttpMethod.Get : HttpMethod.Post;
            string relativeUri;

            switch (actionName)
            {
                #region Auth API
                case nameof(AuthLogin):
                    relativeUri = "/auth/login";
                    break;
                case nameof(AuthRefresh):
                    relativeUri = "/auth/refresh";
                    break;
                #endregion // Auth API
                #region User API
                case nameof(UserGet):
                    httpMethod = HttpMethod.Get;
                    var attributes = ((IMicModel)request).AdditionalData;
                    var userBasicInfo = request as MicUserBasicInfo;
                    var attributesValue = string.Join(",", attributes?.Keys.Select(k => Uri.EscapeDataString(k)) ?? Enumerable.Empty<string>());
                    relativeUri = $"/users/{userBasicInfo?.Username ?? string.Empty}?attributes={attributesValue}";
                    break;
                #endregion // User API
                default:
                    throw new InvalidOperationException("Unknown action name: " + actionName);
            }

            Uri requestUri = new Uri(Manifest.ApiGatewayRootUri, relativeUri);
            string requestJson = JsonConvert.SerializeObject(request);
            using (var requestContent = new StringContent(requestJson, Encoding.UTF8, HttpWellKnownMediaType.ApplicationJson))
            using (var requestMessage = new HttpRequestMessage(httpMethod, requestUri) { Content = requestContent })
            using (var responseMessage = await httpClient.SendAsync(requestMessage, cancelToken).ConfigureAwait(continueOnCapturedContext: false))
                return await DeserializeOrThrow<TResponse>(responseMessage, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        #endregion // Overrides

        #region Private Helper methods

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

        #endregion // Private Helper methods

        #region Dispose

        /// <inheritdoc />
        [DebuggerStepThrough]
        [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
        public void Dispose()
        {
            httpClient.Dispose();
        }

        #endregion // Dispose
    }
}
