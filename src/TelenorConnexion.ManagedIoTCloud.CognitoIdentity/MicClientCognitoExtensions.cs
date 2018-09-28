using Amazon.CognitoIdentity;
using System;

namespace TelenorConnexion.ManagedIoTCloud.CognitoIdentity
{
    /// <summary>
    /// Provides extensions methods to use for AWS Cognito Credentials
    /// authentication.
    /// </summary>
    public static class MicClientCognitoExtensions
    {
        /// <summary>
        /// Gets the AWS Cognito Provider name to use for the MIC Client.S
        /// </summary>
        /// <param name="micClient">The MIC Client connected to the MIC Cloud API. Must not be <c>null</c>.</param>
        /// <returns>The MIC Cognito Provider name to use for login.</returns>
        /// <exception cref="ArgumentNullException">Either <paramref name="micClient"/> or the <see cref="IMicClient.Manifest"/> property of the client is <c>null</c>.</exception>
        public static string GetCognitoProviderName(this IMicClient micClient)
        {
            if (micClient is null)
                throw new ArgumentNullException(nameof(micClient));
            return micClient.Manifest.GetCognitoProviderName();
        }

        /// <summary>
        /// Gets the AWS Cognito Provider name to use for the MIC Client.S
        /// </summary>
        /// <param name="manifest">The MIC Manifest detailing how to connect to MIC Cloud API. Must not be <c>null</c>.</param>
        /// <returns>The MIC Cognito Provider name to use for login.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="manifest"/> is <c>null</c>.</exception>
        public static string GetCognitoProviderName(this MicManifest manifest)
        {
            if (manifest is null)
                throw new ArgumentNullException(nameof(manifest));
            return $"cognito-idp.{manifest.RegionSystemName}.amazonaws.com/{manifest.UserPool}"; ;
        }

        /// <summary>
        /// Adds the login credentials obtained from MIC Cloud API to be used for
        /// authenticated requests on AWS services.
        /// </summary>
        /// <param name="micClient">The MIC Client connected to the MIC Cloud API. Must not be <c>null</c>.</param>
        /// <param name="awsCredentials">The AWS Cognito Identity credentials that is used for authentication.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="micClient"/> or the <see cref="IMicClient.Manifest"/> property of the client is <c>null</c>.</exception>
        /// <remarks>If <paramref name="awsCredentials"/> is <c>null</c>, no action is performed.</remarks>
        public static void AddLoginToCognitoCredentials(this IMicClient micClient,
            CognitoAWSCredentials awsCredentials)
        {
            if (awsCredentials is null)
                return;

            awsCredentials.AddLogin(micClient.GetCognitoProviderName(),
                micClient.Credentials?.Token);
        }

        /// <summary>
        /// Creates a new AWS Cognito Idenity credentials object that can be used
        /// to perform autenticated requests on AWS Services.
        /// </summary>
        /// <param name="micClient">The MIC Client connected to the MIC Cloud API. Must not be <c>null</c>.</param>
        /// <returns>A fully initialized AWS Cognito Idenity credentials object with the token obtained from MIC Cloud API Login set as a login token.</returns>
        /// <exception cref="ArgumentNullException">Either <paramref name="micClient"/> or the <see cref="IMicClient.Manifest"/> property of the client is <c>null</c>.</exception>
        public static CognitoAWSCredentials GetCognitoAWSCredentials(this IMicClient micClient)
        {
            var creds = micClient.Manifest.CreateEmptyAWSCredentials();
            micClient.AddLoginToCognitoCredentials(creds);
            return creds;
        }
    }
}
