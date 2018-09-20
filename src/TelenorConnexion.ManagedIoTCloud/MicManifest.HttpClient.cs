using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TelenorConnexion.ManagedIoTCloud
{
    public partial class MicManifest
    {
        private static readonly JsonSerializer serializer = JsonSerializer.CreateDefault();

        public const string ManifestServiceUrl = "https://1u31fuekv5.execute-api.eu-west-1.amazonaws.com/prod/manifest/";

        public static Uri ManifestServiceUri { get; } = new Uri(ManifestServiceUrl);

        public static Task<MicManifest> GetMicManifest(string hostname, CancellationToken cancellationToken = default) =>
            GetMicManifest(hostname, httpClient: null, cancellationToken);

        public static Task<MicManifest> GetMicManifest(string hostname, HttpClient httpClient, CancellationToken cancellationToken = default) =>
            GetMicManifest(ManifestServiceUri, hostname, httpClient, cancellationToken);

        [SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings")]
        public static Task<MicManifest> GetMicManifest(string manifestServiceUrl, string hostname, CancellationToken cancellationToken = default) =>
            GetMicManifest(manifestServiceUrl, hostname, httpClient: null, cancellationToken);

        public static Task<MicManifest> GetMicManifest(
            Uri manifestServiceUri, string hostname,
            CancellationToken cancellationToken = default)
        {
            return GetMicManifest(
                manifestServiceUri, hostname,
                httpClient: null, cancellationToken
                );
        }

        public static Task<MicManifest> GetMicManifest(
            string manifestServiceUrl, string hostname, HttpClient httpClient,
            CancellationToken cancellationToken = default)
        {
            return GetMicManifest(
                new Uri(manifestServiceUrl), hostname,
                httpClient, cancellationToken
                );
        }

        public static Task<MicManifest> GetMicManifest(
            Uri manifestServiceUri, string hostname, HttpClient httpClient,
            CancellationToken cancellationToken = default)
        {
            if (manifestServiceUri == null)
                throw new ArgumentNullException(nameof(manifestServiceUri));
            if (httpClient == null)
            {
                return GetMicManifestInternal(manifestServiceUri, hostname, cancellationToken);
            }

            return GetMicManifestInternal(manifestServiceUri, hostname, httpClient, cancellationToken);
        }

        private static async Task<MicManifest> GetMicManifestInternal(
            Uri manifestServiceUri, string hostname,
            CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                return await GetMicManifestInternal(
                    manifestServiceUri, hostname, httpClient, cancellationToken
                    ).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        private static async Task<MicManifest> GetMicManifestInternal(
            Uri manifestServiceUri, string hostname, HttpClient httpClient,
            CancellationToken cancellationToken)
        {
            var requestUri = new Uri(manifestServiceUri, "?hostname=" + Uri.EscapeDataString(hostname));
            using (var response = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
            using (var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false))
            using (var contentTextReader = new StreamReader(contentStream, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(contentTextReader))
            {
                return serializer.Deserialize<MicManifest>(jsonReader);
            }
        }
    }
}
