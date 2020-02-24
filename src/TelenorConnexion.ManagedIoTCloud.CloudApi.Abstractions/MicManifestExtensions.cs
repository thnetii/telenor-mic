using System;
using System.Diagnostics.CodeAnalysis;

using Amazon;
using Amazon.CognitoIdentityProvider;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    public static class MicManifestExtensions
    {
        private static readonly AmazonCognitoIdentityProviderConfig cognitoIdpConfig =
            new AmazonCognitoIdentityProviderConfig();

        [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
        public static string GetCognitoProviderName(this MicManifest manifest)
        {
            _ = manifest ?? throw new ArgumentNullException(nameof(manifest));
            if (!(manifest.AwsRegion is RegionEndpoint region))
                throw new InvalidOperationException($"{nameof(manifest.AwsRegion)} is null.");
            return $"{region.GetEndpointForService(cognitoIdpConfig.RegionEndpointServiceName)}/{manifest.UserPool}";
        }
    }
}
