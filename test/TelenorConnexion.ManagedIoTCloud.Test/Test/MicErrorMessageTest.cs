using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TelenorConnexion.ManagedIoTCloud.Data;
using Xunit;

namespace TelenorConnexion.ManagedIoTCloud.Test
{
    public static class MicErrorMessageTest
    {
        public static IEnumerable<object[]> GetSampleFiles() => EmbeddedData
            .GetFiles().Where(fi => fi.Name.EndsWith("errorMessage.json", StringComparison.OrdinalIgnoreCase))
            .Select(fi => new[] { fi });

        [Theory, MemberData(nameof(GetSampleFiles))]
        public static void CanDeserializeSampleData(IFileInfo sampleFile)
        {
            var serializer = JsonSerializer.Create();

            using (var stream = sampleFile.CreateReadStream())
            using (var textReader = new StreamReader(stream, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                var errorMessage = serializer.Deserialize<MicErrorMessage>(
                    jsonReader);
            }
        }
    }
}
