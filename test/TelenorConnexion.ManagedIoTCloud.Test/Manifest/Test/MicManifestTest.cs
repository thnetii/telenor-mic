﻿using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.Manifest.Data;
using Xunit;

namespace TelenorConnexion.ManagedIoTCloud.Manifest.Test
{
    public class MicManifestTest
    {
        public static IEnumerable<object[]> GetSampleFiles() =>
            EmbeddedData.GetFiles().Select(fi => new[] { fi });

        private static void AssertMicManifest(MicManifest manifest)
        {
            Assert.NotNull(manifest);
            foreach (var pi in manifest.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object pv = pi.GetValue(manifest);
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
        public static async Task FetchMicManifestForHostname(string hostname)
        {
            var manifest = await MicManifest.GetMicManifest(hostname);
            AssertMicManifest(manifest);
        }
    }
}
