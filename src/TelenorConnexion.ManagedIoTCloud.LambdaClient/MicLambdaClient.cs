using Amazon.CognitoIdentity;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.CognitoIdentity;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.LambdaClient
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
    public class MicLambdaClient : IAmazonService, IMicClient, IDisposable
    {
        private readonly CognitoAWSCredentials awsCredentials;
        private readonly AmazonLambdaClient lambdaClient;

        #region Constructors

        public static async Task<MicLambdaClient> CreateFromHostname(
            string hostname, CancellationToken cancelToken = default)
        {
            var manifest = await MicManifest.GetMicManifest(hostname, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return new MicLambdaClient(manifest);
        }

        public MicLambdaClient(MicManifest manifest)
        {
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));

            awsCredentials = manifest.CreateEmptyAWSCredentials();
            lambdaClient = new AmazonLambdaClient(awsCredentials,
                manifest.AwsRegion);
        }

        #endregion

        #region IMicClient

        #region Public members

        public MicManifest Manifest { get; }

        public MicAuthLoginCredentials Credentials { get; private set; }

        #endregion

        #region Auth

        #region Login

        public Task<MicAuthLoginResponse> AuthLogin(string username, string password,
            CancellationToken cancelToken = default) =>
            AuthLogin(new MicAuthLoginRequest
            {
                Username = username,
                Password = password
            }, cancelToken);

        public async Task<MicAuthLoginResponse> AuthLogin(MicAuthLoginRequest request,
            CancellationToken cancelToken = default)
        {
            var response = await InvokeLambdaFunction<MicAuthLoginResponse>(
                Manifest.AuthLambda, request, noAutoRefreshToken: true,
                cancelToken).ConfigureAwait(continueOnCapturedContext: false);
            UpdateCredentials(response);
            return response;
        }

        #endregion

        #region Refresh

        public Task<MicAuthLoginResponse> AuthRefresh(
            CancellationToken cancelToken = default)
        {
            if (!(Credentials?.RefreshToken).TryNotNullOrWhiteSpace(out string refreshToken))
                throw new InvalidOperationException("No refresh token available");
            return AuthRefresh(refreshToken, cancelToken);
        }

        public Task<MicAuthLoginResponse> AuthRefresh(string refreshToken,
            CancellationToken cancelToken = default) =>
            AuthRefresh(new MicAuthRefreshRequest
            {
                RefreshToken = refreshToken
            }, cancelToken);

        public async Task<MicAuthLoginResponse> AuthRefresh(MicAuthRefreshRequest request,
            CancellationToken cancelToken = default)
        {
            var response = await InvokeLambdaFunction<MicAuthLoginResponse>(
                Manifest.AuthLambda, request, noAutoRefreshToken: true,
                cancelToken).ConfigureAwait(continueOnCapturedContext: false);
            UpdateCredentials(response);
            return response;
        }

        #endregion

        #endregion

        #endregion

        #region Private Helper methods

        private async Task<TResponse> InvokeLambdaFunction<TResponse>(
            string functionName, IMicRequestAttributes request,
            bool noAutoRefreshToken = false, CancellationToken cancelToken = default)
        {
            var response = await lambdaClient.InvokeAsync(
                new InvokeRequest
                {
                    FunctionName = functionName,
                    Payload = JsonConvert.SerializeObject(request.CreateRequest())
                }, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
            return await response.DeserializeOrThrow<TResponse>(cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        private void UpdateCredentials(MicAuthLoginResponse loginResponse)
        {
            Credentials = loginResponse.Credentials;
            this.AddLoginToCognitoCredentials(awsCredentials);
        }

        #endregion

        #region IAmazonService

        public AmazonLambdaConfig Config => lambdaClient.Config as AmazonLambdaConfig;

        IClientConfig IAmazonService.Config => lambdaClient.Config;

        #endregion

        #region Dispose

        /// <inheritdoc />
        [DebuggerStepThrough]
        [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
        public void Dispose()
        {
            lambdaClient.Dispose();
        }

        #endregion
    }
}
