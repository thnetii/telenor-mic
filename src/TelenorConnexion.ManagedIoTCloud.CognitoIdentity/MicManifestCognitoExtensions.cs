using Amazon.CognitoIdentity;
using System;

namespace TelenorConnexion.ManagedIoTCloud.CognitoIdentity
{
    /// <summary>
    /// Provides extensions methods for <see cref="MicManifest"/> to create
    /// AWS Cognito Identity credentials that can be used to perform requests
    /// authenticated by MIC on AWS Services.
    /// </summary>
    public static class MicManifestCognitoExtensions
    {
        /// <summary>
        /// Creates a new empty AWS Cognito Identity credentials object which
        /// uses the Identity Pool and AWS Region specified by the MIC Manifest.
        /// </summary>
        /// <param name="manifest">The MIC Manifest that specifies how to connect to MIC Cloud API. Must not be <c>null</c>.</param>
        /// <returns>An initialized <see cref="CognitoAWSCredentials"/> instance that can subsequently be filled using <see cref="MicClientCognitoExtensions.AddLoginToCognitoCredentials(IMicClient, CognitoAWSCredentials)"/>.</returns>
        public static CognitoAWSCredentials CreateEmptyAWSCredentials(
            this MicManifest manifest)
        {
            if (manifest is null)
                throw new ArgumentNullException(nameof(manifest));
            return new CognitoAWSCredentials(manifest.IdentityPool,
                manifest.AwsRegion);            
        }
    }
}
