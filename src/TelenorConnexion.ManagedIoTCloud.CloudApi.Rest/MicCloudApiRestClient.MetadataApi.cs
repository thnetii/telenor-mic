using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using TelenorConnexion.ManagedIoTCloud.CloudApi.Model;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
    public partial class MicCloudApiRestClient
    {
        #region Metadata API

        #region Metadata API: MANIFEST
        private const string metadataManifestUrl = "metadata/manifest";

        public Task<MicMetadataManifest> MetadataManifest(CancellationToken cancelToken = default) =>
            HandleClientRequest<MicMetadataManifest>(metadataManifestUrl, HttpMethod.Get,
                request: null, hasPayload: false, cancelToken);
        #endregion

        #endregion
    }
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
}
