using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;

namespace TelenorConnexion.ManagedIoTCloud
{
    public partial class MicClient : IAmazonService, IDisposable
    {
        private readonly bool externalHttpClient;
        private readonly HttpClient httpClient;
        private readonly Lazy<Task<MicManifest>> manifestLazy;

        private MicClient(MicClientConfig config, HttpClient? httpClient) : base()
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Config.Validate();
            externalHttpClient = !(httpClient is null);
            httpClient ??= Config.HttpClientFactory?.CreateHttpClient(config);
            this.httpClient = httpClient ?? new HttpClient();

            manifestLazy = new Lazy<Task<MicManifest>>(() => GetManifestNonLazy());
        }

        public MicClient(MicClientConfig config) : this(config, null) { }

        public MicClient(string hostname, HttpClient? httpClient = null)
            : this(new MicClientConfig
            {
                Hostname = hostname ?? throw new ArgumentNullException(nameof(hostname))
            }, httpClient)
        { }

        public MicClientConfig Config { get; }

        IClientConfig IAmazonService.Config => Config;

        private Task<MicManifest> GetManifestNonLazy(CancellationToken cancelToken = default) =>
            MicManifest.GetMicManifest(Config.Hostname, httpClient, cancelToken);

        public virtual async Task<MicManifest> GetManifest(
            CancellationToken cancelToken = default)
        {
            var manifest = await manifestLazy.Value.ConfigureAwait(false);
            cancelToken.ThrowIfCancellationRequested();
            _ = manifest.ApiGatewayRootUri ?? throw new HttpRequestException(
                $"A MIC manifest request returned an empty response, indicating a non-existant MIC Hostname '{Config.Hostname}'");
            return manifest;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (externalHttpClient)
                        httpClient.Dispose();
                }

                disposedValue = true;
            }
        }

        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        ~MicClient() => Dispose(false);

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
