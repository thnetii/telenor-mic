using Microsoft.Extensions.FileProviders;
using System;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Data
{
    public static class EmbeddedData
    {
        private static readonly Type typeRef = typeof(EmbeddedData);
        private static readonly IFileProvider fileProvider =
            new EmbeddedFileProvider(typeRef.Assembly, typeRef.Namespace);

        public static IDirectoryContents GetFiles() =>
            fileProvider.GetDirectoryContents("");
    }
}
