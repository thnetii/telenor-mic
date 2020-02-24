using System;

using Amazon.Extensions.CognitoAuthentication;

using TelenorConnexion.ManagedIoTCloud.Data;
using TelenorConnexion.ManagedIoTCloud.Test;

using Xunit;

namespace TelenorConnexion.ManagedIoTCloud.Authentication.Test
{
    public static class MicAuthenticationClientTest
    {
        [Fact]
        public static void ConstructFromSampleManifest()
        {
            var manifest = MicManifestTest.GetExampleManifest();

            using var authClient = new MicAuthenticationClient(manifest);
        }

        [Fact]
        public static void ConstructWithNullManifestThrows()
        {
            MicManifest manifest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                using var authClient = new MicAuthenticationClient(manifest);
                _ = authClient;
            });
        }

        [Fact]
        public static void GetAuthClientFromOnlineManifest()
        {
            var parameters = OnlineParameters.GetEmbedded();
            using var client = new MicClient(parameters.Hostname);

            using var authClient = client.GetAuthenticationClient()
                .GetAwaiter().GetResult();

            Assert.NotNull(authClient);
            Assert.NotNull(authClient.CognitoUserPool);
            Assert.NotNull(authClient.CognitoUserPool.ClientID);
            Assert.NotNull(authClient.CognitoUserPool.PoolID);
        }

        [Fact]
        public static void CanSrpAuthenticateOnlineCredentials()
        {
            var parameters = OnlineParameters.GetEmbedded();
            using var client = new MicClient(parameters.Hostname);
            using var authClient = client.GetAuthenticationClient()
                .GetAwaiter().GetResult();

            var user = authClient.GetCognitoUser(parameters.Username);
            var authResponse = user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest
            {
                Password = parameters.Password
            }).GetAwaiter().GetResult();

            Assert.NotNull(authResponse);
            Assert.NotNull(authResponse.AuthenticationResult);
            Assert.NotNull(user.SessionTokens);
            Assert.True(user.SessionTokens.IsValid());
        }

        [Fact]
        public static void GetCognitoCredentialsFromOnlineCredentials()
        {
            var parameters = OnlineParameters.GetEmbedded();
            using var client = new MicClient(parameters.Hostname);
            using var authClient = client.GetAuthenticationClient()
                .GetAwaiter().GetResult();

            var user = authClient.GetCognitoUser(parameters.Username);
            _ = user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest
            {
                Password = parameters.Password
            }).GetAwaiter().GetResult();
            var credentials = user.GetCognitoAWSCredentials(authClient.Manifest);
            var access = credentials.GetCredentials();

            Assert.NotNull(access);
        }

        [Fact]
        public static void GetUserDetailsFromOnlineCredentials()
        {
            var parameters = OnlineParameters.GetEmbedded();
            using var client = new MicClient(parameters.Hostname);
            using var authClient = client.GetAuthenticationClient()
                .GetAwaiter().GetResult();

            var user = authClient.GetCognitoUser(parameters.Username);
            _ = user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest
            {
                Password = parameters.Password
            }).GetAwaiter().GetResult();
            var details = user.GetUserDetailsAsync()
                .GetAwaiter().GetResult();

            Assert.NotNull(details);
        }
    }
}
