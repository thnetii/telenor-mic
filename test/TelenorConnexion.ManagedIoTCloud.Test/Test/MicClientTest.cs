using System;
using System.Net.Http;

using Amazon.Runtime;

using TelenorConnexion.ManagedIoTCloud.Data;

using Xunit;

namespace TelenorConnexion.ManagedIoTCloud.Test
{
    public class MicClientTest
    {
        [Fact]
        public void ConstructorWithInvalidConfigThrows()
        {
            var config = new MicClientConfig();

            Assert.Null(config.Hostname);
            Assert.Throws<ArgumentNullException>(() =>
            {
                using var client = new MicClient(config);
                _ = client;
            });
        }

        [Fact]
        public void ConstructorWithNullHostnameThrows()
        {
            string hostname = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                using var client = new MicClient(hostname);
                _ = client;
            });
        }

        [Fact]
        public void ConstructorWithNullHttpClientDoesNotThrow()
        {
            string hostname = "example.com";
            HttpClient httpClient = null;

            using var client = new MicClient(hostname, httpClient);
        }

        [Fact]
        public void ConstructsConfigWithHostname()
        {
            string hostname = "example.com";

            using var micClient = new MicClient(hostname);
            var config = micClient.Config;

            Assert.NotNull(config);
            Assert.Equal(hostname, config.Hostname);
        }

        [Fact]
        public void IsAmazonService()
        {
            using var client = new MicClient(new MicClientConfig
            {
                Hostname = "example.com"
            });

            Assert.IsAssignableFrom<IAmazonService>(client);
        }

        [Fact]
        public void GetsOnlineMicManifestUsingConfig()
        {
            var parameters = OnlineParameters.GetFromUserSecrets();
            using var client = new MicClient(new MicClientConfig
            {
                Hostname = parameters.Hostname
            });

            var manifest = client.GetManifest().GetAwaiter().GetResult();

            Assert.NotNull(manifest);
        }

        [Fact]
        public void GetsOnlineMicManifestUsingHostname()
        {
            var parameters = OnlineParameters.GetFromUserSecrets();
            using var client = new MicClient(parameters.Hostname);

            var manifest = client.GetManifest().GetAwaiter().GetResult();

            Assert.NotNull(manifest);
        }

        [Fact]
        public void GetManifestWithBogusHostnameThrows()
        {
            var hostname = "bogus";
            using var client = new MicClient(hostname);

            Assert.ThrowsAny<HttpRequestException>(() =>
            {
                _ = client.GetManifest().GetAwaiter().GetResult();
            });
        }
    }
}
