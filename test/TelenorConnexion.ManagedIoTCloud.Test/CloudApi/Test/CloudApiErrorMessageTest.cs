using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TelenorConnexion.ManagedIoTCloud.CloudApi.Data;
using Xunit;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    public class CloudApiErrorMessageTest
    {
        public static IEnumerable<object[]> GetSampleFiles() =>
            EmbeddedData.GetFiles().Select(fi => new[] { fi });

        [Theory, MemberData(nameof(GetSampleFiles))]
        public static void CanDeserializeSampleData(IFileInfo sampleFile)
        {
            var serializer = JsonSerializer.Create();

            using (var stream = sampleFile.CreateReadStream())
            using (var textReader = new StreamReader(stream, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                var errorMessage = serializer.Deserialize<CloudApiErrorMessage>(
                    jsonReader);
            }
        }
    }
}
