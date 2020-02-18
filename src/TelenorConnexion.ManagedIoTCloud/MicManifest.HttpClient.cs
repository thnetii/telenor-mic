using Newtonsoft.Json;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using THNETII.Networking.Http;

namespace TelenorConnexion.ManagedIoTCloud
{
    public partial class MicManifest
    {
        private static readonly JsonSerializer serializer = JsonSerializer.CreateDefault();

        /// <summary>
        /// The default URL-string specifying from where MIC manifest documents
        /// should be retrieved.
        /// </summary>
        public const string ManifestServiceUrl = "https://1u31fuekv5.execute-api.eu-west-1.amazonaws.com/prod/manifest/";

        /// <summary>
        /// The default URI from where MIC manifest documents should be retrieved.
        /// </summary>
        public static Uri ManifestServiceUri { get; } = new Uri(ManifestServiceUrl);

        /// <summary>
        /// Retrieves a MIC manifest document for the specified hostname.
        /// </summary>
        /// <param name="hostname">The MIC hostname for which to retrieve the manifest.</param>
        /// <param name="cancellationToken">An optional cancellation token with which the request can be interrupted.</param>
        /// <returns>
        /// A <see cref="MicManifest"/> instance representing the returned manifest document.
        /// <para>
        /// If an invalid value was specified for <paramref name="hostname"/>,
        /// a manifest document with all properties set to their default value
        /// is returned.
        /// </para>
        /// </returns>
        public static Task<MicManifest> GetMicManifest(string hostname, CancellationToken cancellationToken = default) =>
            GetMicManifest(hostname, httpClient: null, cancellationToken);

        /// <summary>
        /// Retrieves a MIC manifest document for the specified hostname using
        /// the specified HTTP Client.
        /// </summary>
        /// <param name="hostname">The MIC hostname for which to retrieve the manifest.</param>
        /// <param name="httpClient">The HTTP Client instance to use for the request.</param>
        /// <param name="cancellationToken">An optional cancellation token with which the request can be interrupted.</param>
        /// <returns>
        /// A <see cref="MicManifest"/> instance representing the returned manifest document.
        /// <para>
        /// If an invalid value was specified for <paramref name="hostname"/>,
        /// a manifest document with all properties set to their default value
        /// is returned.
        /// </para>
        /// </returns>
        public static Task<MicManifest> GetMicManifest(string hostname, HttpClient httpClient, CancellationToken cancellationToken = default) =>
            GetMicManifest(ManifestServiceUri, hostname, httpClient, cancellationToken);

        /// <summary>
        /// Retrieves a MIC manifest document for the specified hostname from a
        /// custom location.
        /// </summary>
        /// <param name="manifestServiceUrl">The custom URL from which the manifest document will be retrived.</param>
        /// <param name="hostname">The MIC hostname for which to retrieve the manifest.</param>
        /// <param name="cancellationToken">An optional cancellation token with which the request can be interrupted.</param>
        /// <returns>
        /// A <see cref="MicManifest"/> instance representing the returned manifest document.
        /// <para>
        /// If an invalid value was specified for <paramref name="hostname"/>,
        /// a manifest document with all properties set to their default value
        /// is returned.
        /// </para>
        /// </returns>
        [SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings")]
        public static Task<MicManifest> GetMicManifest(string manifestServiceUrl, string hostname, CancellationToken cancellationToken = default) =>
            GetMicManifest(manifestServiceUrl, hostname, httpClient: null, cancellationToken);

        /// <summary>
        /// Retrieves a MIC manifest document for the specified hostname from a
        /// custom location.
        /// </summary>
        /// <param name="manifestServiceUri">The custom URI from which the manifest document will be retrived.</param>
        /// <param name="hostname">The MIC hostname for which to retrieve the manifest.</param>
        /// <param name="cancellationToken">An optional cancellation token with which the request can be interrupted.</param>
        /// <returns>
        /// A <see cref="MicManifest"/> instance representing the returned manifest document.
        /// <para>
        /// If an invalid value was specified for <paramref name="hostname"/>,
        /// a manifest document with all properties set to their default value
        /// is returned.
        /// </para>
        /// </returns>
        public static Task<MicManifest> GetMicManifest(Uri manifestServiceUri, string hostname, CancellationToken cancellationToken = default) =>
            GetMicManifest(manifestServiceUri, hostname, httpClient: null, cancellationToken);

        /// <summary>
        /// Retrieves a MIC manifest document for the specified hostname from a
        /// custom location using the specified HTTP Client.
        /// </summary>
        /// <param name="manifestServiceUrl">The custom URL from which the manifest document will be retrived.</param>
        /// <param name="hostname">The MIC hostname for which to retrieve the manifest.</param>
        /// <param name="httpClient">The HTTP Client instance to use for the request.</param>
        /// <param name="cancellationToken">An optional cancellation token with which the request can be interrupted.</param>
        /// <returns>
        /// A <see cref="MicManifest"/> instance representing the returned manifest document.
        /// <para>
        /// If an invalid value was specified for <paramref name="hostname"/>,
        /// a manifest document with all properties set to their default value
        /// is returned.
        /// </para>
        /// </returns>
        public static Task<MicManifest> GetMicManifest(string manifestServiceUrl, string hostname, HttpClient httpClient, CancellationToken cancellationToken = default) =>
            GetMicManifest(new Uri(manifestServiceUrl), hostname, httpClient, cancellationToken);

        /// <summary>
        /// Retrieves a MIC manifest document for the specified hostname from a
        /// custom location using the specified HTTP Client.
        /// </summary>
        /// <param name="manifestServiceUri">The custom URI from which the manifest document will be retrived.</param>
        /// <param name="hostname">The MIC hostname for which to retrieve the manifest.</param>
        /// <param name="httpClient">The HTTP Client instance to use for the request.</param>
        /// <param name="cancellationToken">An optional cancellation token with which the request can be interrupted.</param>
        /// <returns>
        /// A <see cref="MicManifest"/> instance representing the returned manifest document.
        /// <para>
        /// If an invalid value was specified for <paramref name="hostname"/>,
        /// a manifest document with all properties set to their default value
        /// is returned.
        /// </para>
        /// </returns>
        public static Task<MicManifest> GetMicManifest(
            Uri manifestServiceUri, string hostname, HttpClient httpClient,
            CancellationToken cancellationToken = default)
        {
            if (manifestServiceUri is null)
                throw new ArgumentNullException(nameof(manifestServiceUri));
            if (httpClient is null)
                return GetMicManifestInternal(manifestServiceUri, hostname, cancellationToken);

            return GetMicManifestInternal(manifestServiceUri, hostname, httpClient, cancellationToken);
        }

        private static async Task<MicManifest> GetMicManifestInternal(
            Uri manifestServiceUri, string hostname,
            CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            return await GetMicManifestInternal(manifestServiceUri, hostname,
                httpClient, cancellationToken
                )
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        private static async Task<MicManifest> GetMicManifestInternal(
            Uri manifestServiceUri, string hostname, HttpClient httpClient,
            CancellationToken cancellationToken)
        {
            var requestUri = new Uri(manifestServiceUri, "?hostname=" + Uri.EscapeDataString(hostname ?? string.Empty));
            using var response = await httpClient
                .GetAsync(requestUri, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            if (!response.Content.IsJson())
                throw new HttpRequestException($"Content-Type '{response.Content.Headers.ContentType}' does not indicate a JSON response");
            using var contentTextReader = await response.Content
                .ReadAsStreamReaderAsync(Encoding.UTF8)
                .ConfigureAwait(continueOnCapturedContext: false);
            using var jsonReader = new JsonTextReader(contentTextReader);
            return serializer.Deserialize<MicManifest>(jsonReader)!;
        }
    }
}
