using Microsoft.Extensions.FileProviders;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TelenorConnexion.ManagedIoTCloud.Data;

using Xunit;

namespace TelenorConnexion.ManagedIoTCloud.Manifest.Test
{
    public static class MicManifestTest
    {
        public static IEnumerable<object[]> GetSampleFiles() => EmbeddedData
            .GetFiles().Where(fi => fi.Name.EndsWith("manifest.json", StringComparison.OrdinalIgnoreCase))
            .Select(fi => new[] { fi });

        private static void AssertMicManifest(MicManifest manifest)
        {
            Assert.NotNull(manifest);
            foreach (var pi in manifest.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                _ = pi.GetValue(manifest);
            }
        }

        [Theory, MemberData(nameof(GetSampleFiles))]
        public static void CanDeserializeSampleData(IFileInfo sampleFile)
        {
            var serializer = JsonSerializer.Create();

            using (var stream = sampleFile.CreateReadStream())
            using (var textReader = new StreamReader(stream, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(textReader))
                AssertMicManifest(serializer.Deserialize<MicManifest>(jsonReader));
        }

        [SkippableTheory]
        [InlineData("demo.mic.telenorconnexion.com")]
        [InlineData("startiot.mic.telenorconnexion.com")]
        public static async Task GetMicManifestForHostname(string hostname)
        {
            var manifest = await MicManifest.GetMicManifest(hostname);
            AssertMicManifest(manifest);
        }

        [Theory]
        [InlineData("demo.mic.telenorconnexion.com")]
        [InlineData("startiot.mic.telenorconnexion.com")]
        public static void GetMicManifestWithCancelledTokenThrows(string hostname)
        {
            Assert.Throws<TaskCanceledException>(() =>
            {
                var cts = new CancellationTokenSource();
                cts.Cancel();
                MicManifest.GetMicManifest(hostname, cts.Token)
                    .GetAwaiter().GetResult();
            });
        }
    }
}
