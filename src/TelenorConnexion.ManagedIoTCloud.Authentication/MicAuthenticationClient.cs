using System;

using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;

namespace TelenorConnexion.ManagedIoTCloud.Authentication
{
    public class MicAuthenticationClient : IDisposable
    {
        private readonly AmazonCognitoIdentityProviderClient cognitoIdp;

        public MicAuthenticationClient(MicManifest manifest,
            IClientConfig? clientConfig = null)
        {
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));

            string? poolID = manifest.UserPool;
            string? clientID = manifest.UserPoolClient;
            var cognitoIdpCreds = new AnonymousAWSCredentials();
            var cognitoIdpConfig = manifest.CreateClientConfig<AmazonCognitoIdentityProviderConfig>(clientConfig);
            cognitoIdp = new AmazonCognitoIdentityProviderClient(
                cognitoIdpCreds, cognitoIdpConfig);
            CognitoUserPool = new CognitoUserPool(poolID, clientID, cognitoIdp);
        }

        public MicManifest Manifest { get; }
        public CognitoUserPool CognitoUserPool { get; }

        public CognitoUser GetCognitoUser(string username) =>
            CognitoUserPool.GetUser(username);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cognitoIdp.Dispose();
                }

                disposedValue = true;
            }
        }

        ~MicAuthenticationClient()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
