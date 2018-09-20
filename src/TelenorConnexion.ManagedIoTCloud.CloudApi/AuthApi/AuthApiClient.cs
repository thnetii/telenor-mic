using Amazon.CognitoIdentity;
using System;
using System.Threading;
using System.Threading.Tasks;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.AuthApi
{
    /// <summary>
    /// The Auth API is used to authenticate users and let new users sign up.
    /// </summary>
    public class AuthApiClient
    {
        private readonly string functionName;
        private readonly CognitoAWSCredentials credentials;
        private readonly CloudApiClient client;

        private string refreshToken;

        public AuthApiClient(CloudApiClient client, CognitoAWSCredentials credentials)
            : base()
        {
            this.client = client
                ?? throw new ArgumentNullException(nameof(client));
            this.credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));

            functionName = (client.Manifest?.AuthLambda)
                .ThrowIfNullOrWhiteSpace($"{nameof(client)}.{nameof(client.Manifest)}.{nameof(client.Manifest.AuthLambda)}");
        }

        private void HandleAuthLoginResponse(AuthLoginResponse response)
        {
            credentials.AddLogin(GetCognitoProvideName(), response.Credentials.Token);
            refreshToken = response.Credentials.RefreshToken;
        }

        protected virtual string GetCognitoProvideName()
        {
            return $"cognito-idp.{client.Manifest.RegionSystemName}.amazonaws.com/{client.Manifest.UserPool}";
        }

        /// <summary>
        /// Checks if a user is authorized to login and returns credentials that
        /// should be used when communicating with AWS. The access token is
        /// valid for 60 minutes, whereafter it needs to be refreshed.
        /// </summary>
        /// <param name="userName">The user name of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="cancellationToken" />
        public async Task<AuthLoginResponse> Login(string userName, string password,
            CancellationToken cancellationToken = default)
        {
            var request = new AuthLoginRequest
            {
                Username = userName,
                Password = password
            };
            var response = await client.InvokeFunction<AuthLoginResponse>(
                functionName, request, noAutoRefresh: true, cancellationToken
                ).ConfigureAwait(continueOnCapturedContext: false);
            HandleAuthLoginResponse(response);
            return response;
        }

        public Task<AuthLoginResponse> Refresh(CancellationToken cancellationToken = default) =>
            Refresh(refreshToken, cancellationToken);

        public async Task<AuthLoginResponse> Refresh(string refreshToken,
            CancellationToken cancellationToken = default)
        {
            var request = new AuthRefreshRequest
            {
                RefreshToken = refreshToken
            };
            var response = await client.InvokeFunction<AuthLoginResponse>(
                functionName, request, noAutoRefresh: true, cancellationToken
                ).ConfigureAwait(continueOnCapturedContext: false);
            HandleAuthLoginResponse(response);
            return response;
        }
    }
}
