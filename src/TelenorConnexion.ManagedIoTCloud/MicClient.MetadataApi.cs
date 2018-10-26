using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.Model;

namespace TelenorConnexion.ManagedIoTCloud
{
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
    public partial interface IMicClient
    {
        Task<MicMetadataManifest> MetadataManifest(CancellationToken cancelToken = default);
    }

    public partial class MicClient
    {
        #region Metadata API

        #region Metadata API: MANIFEST

        public Task<MicMetadataManifest> MetadataManifest(CancellationToken cancelToken = default) =>
            HandleClientRequest<MicModel, MicMetadataManifest>(nameof(MetadataManifest), default, cancelToken);

        #endregion

        #endregion
    }
#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
}
