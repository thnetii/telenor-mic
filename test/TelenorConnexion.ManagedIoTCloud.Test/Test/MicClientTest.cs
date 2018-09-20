using Amazon.APIGateway;
using System.Threading.Tasks;
using Xunit;

namespace TelenorConnexion.ManagedIoTCloud
{
    public static class MicClientTest
    {
        [Theory]
        [InlineData("demo.mic.telenorconnexion.com")]
        [InlineData("startiot.mic.telenorconnexion.com")]
        public static Task GetApiKeyForHostnameWithoutLoginThrows(string hostname)
        {
            return Assert.ThrowsAsync<AmazonAPIGatewayException>(async () =>
            {
                using (var client = await MicClient.CreateFromHostname(hostname))
                {
                    var apiKey = await client.GetApiGatewayKey();
                    Assert.NotNull(apiKey);
                }
            });
        }
    }
}
