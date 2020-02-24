using System;
using Amazon.Runtime;

using Xunit;

namespace TelenorConnexion.ManagedIoTCloud.Test
{
    public static class MicClientConfigTest
    {
        [Fact]
        public static void IsAmazonClientConfig()
        {
            var config = new MicClientConfig();

            Assert.IsAssignableFrom<ClientConfig>(config);
            Assert.IsAssignableFrom<IClientConfig>(config);
        }

        [Fact]
        public static void ValidateThrowsIfNoHostname()
        {
            var config = new MicClientConfig();

            Assert.Null(config.Hostname);
            Assert.Throws<ArgumentNullException>(() =>
            {
                config.Validate();
            });
        }
    }
}
