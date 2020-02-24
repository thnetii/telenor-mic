using System;
using System.Threading;
using System.Threading.Tasks;

using Amazon.CognitoIdentity;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;

using TelenorConnexion.ManagedIoTCloud.Authentication;

namespace TelenorConnexion.ManagedIoTCloud
{
    public static class MicClientAuthenticationExtensions
    {
        public static async Task<MicAuthenticationClient> GetAuthenticationClient(
            this MicClient client, CancellationToken cancelToken = default)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));
            var manifest = await client.GetManifest(cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            cancelToken.ThrowIfCancellationRequested();

            return new MicAuthenticationClient(manifest, client.Config);
        }

        public static CognitoAWSCredentials GetCognitoAWSCredentials(
            this CognitoUser user, MicManifest manifest)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));
            _ = manifest ?? throw new ArgumentNullException(nameof(manifest));

            return user.GetCognitoAWSCredentials(manifest.IdentityPool, manifest.AwsRegion);
        }
    }
}
