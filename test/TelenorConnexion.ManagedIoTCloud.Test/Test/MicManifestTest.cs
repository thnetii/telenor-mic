using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.Runtime;
using Microsoft.Extensions.FileProviders;

using Newtonsoft.Json;
using TelenorConnexion.ManagedIoTCloud.Data;

using Xunit;

namespace TelenorConnexion.ManagedIoTCloud.Test
{
    public static class MicManifestTest
    {
        public static IFileInfo GetExampleFile() =>
            EmbeddedData.GetFile("example.manifest.json");

        public static MicManifest GetExampleManifest()
        {
            var serializer = JsonSerializer.Create();
            using var stream = GetExampleFile().CreateReadStream();
            using var textReader = new StreamReader(stream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(textReader);
            return serializer.Deserialize<MicManifest>(jsonReader);
        }

        private static void AssertMicManifest(MicManifest manifest)
        {
            Assert.NotNull(manifest);
            foreach (var pi in manifest.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                _ = pi.GetValue(manifest);
            }
        }

        [Fact]
        public static void CanDeserializeSampleManifest()
        {
            MicManifest manifest = GetExampleManifest();
            AssertMicManifest(manifest);
        }

        [Fact]
        public static async Task GetOnlineMicManifest()
        {
            var parameters = OnlineParameters.GetEmbedded();
            var manifest = await MicManifest.GetMicManifest(parameters.Hostname);
            AssertMicManifest(manifest);
        }

        [Fact]
        public static void GetMicManifestWithCancelledTokenThrows()
        {
            var parameters = OnlineParameters.GetEmbedded();
            Assert.Throws<TaskCanceledException>(() =>
            {
                var cts = new CancellationTokenSource();
                cts.Cancel();
                MicManifest.GetMicManifest(parameters.Hostname, cts.Token)
                    .GetAwaiter().GetResult();
            });
        }

        public static IEnumerable<object[]> GetLoadedClientConfigTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => typeof(ClientConfig).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract)
                .Where(type => !(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, Type.DefaultBinder, Type.EmptyTypes, null) is null))
                .Select(type => new[] { type });
        }

        [Theory]
        [MemberData(nameof(GetLoadedClientConfigTypes))]
        public static void CreateClientConfigFromSampleManifest(Type configType)
        {
            var mi = typeof(MicManifestTest).GetMethod(
                nameof(CreateClientConfigFromSampleManifestImpl),
                BindingFlags.Static | BindingFlags.NonPublic);
            var action = (Action<IClientConfig>)mi
                .MakeGenericMethod(configType)
                .CreateDelegate(typeof(Action<IClientConfig>));

            var template = new MicClientConfig { Hostname = "example.com" };

            action.Invoke(template);
        }

        internal static void CreateClientConfigFromSampleManifestImpl<T>(IClientConfig template)
            where T : ClientConfig, new()
        {
            var manifest = GetExampleManifest();

            var config = manifest.CreateClientConfig<T>(template);

            Assert.NotNull(config);
            Assert.IsAssignableFrom<IClientConfig>(config);
            Assert.IsAssignableFrom<ClientConfig>(config);
            Assert.IsType<T>(config);

            Assert.Equal(manifest.AwsRegion, config.RegionEndpoint);

            config.Validate();
        }
    }
}
