using Amazon.APIGateway;
using Amazon.APIGateway.Model;
using Amazon.CognitoIdentity;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
    public class MicClient : IDisposable
    {
        public static async Task<MicClient> CreateFromHostname(
            string hostname, CancellationToken cancelToken = default)
        {
            var manifest = await MicManifest.GetMicManifest(hostname, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return new MicClient(manifest);
        }

        private MicAuthLoginCredentials loginCredentials;
        private readonly CognitoAWSCredentials credentials;
        private readonly string cognitoProviderName;

        private readonly Lazy<AmazonAPIGatewayClient> apiGatewayClient;
        private readonly Lazy<AmazonLambdaClient> lambdaClient;

        public MicManifest Manifest { get; }

        public async Task<MicAuthLoginResponse> AuthLogin(string username, string password,
            CancellationToken cancelToken = default)
        {
            var request = new MicAuthLoginRequest
            {
                Username = username,
                Password = password
            };
            var response = await InvokeLambdaFunction<MicAuthLoginResponse>(
                Manifest.AuthLambda, request, noAutoRefreshToken: true,
                cancelToken).ConfigureAwait(continueOnCapturedContext: false);
            UpdateCredentials(response);
            return response;
        }

        public Task<MicAuthLoginResponse> AuthRefresh(
            CancellationToken cancelToken = default)
        {
            if (!(loginCredentials?.RefreshToken).TryNotNullOrWhiteSpace(out string refreshToken))
                throw new InvalidOperationException("No refresh token available");
            return AuthRefresh(refreshToken, cancelToken);
        }

        public async Task<MicAuthLoginResponse> AuthRefresh(string refreshToken,
            CancellationToken cancelToken = default)
        {
            var request = new MicAuthRefreshRequest { RefreshToken = refreshToken };
            var response = await InvokeLambdaFunction<MicAuthLoginResponse>(
                Manifest.AuthLambda, request, noAutoRefreshToken: true,
                cancelToken).ConfigureAwait(continueOnCapturedContext: false);
            UpdateCredentials(response);
            return response;
        }

        public async Task<string> GetApiGatewayKey(
            CancellationToken cancelToken = default)
        {
            var client = apiGatewayClient.Value;
            var response = await client.GetApiKeyAsync(new GetApiKeyRequest
            {
                ApiKey = Manifest.ApiKeyId,
                IncludeValue = true
            }, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
            return response.Value;
        }

        private MicClient()
        {
            apiGatewayClient = new Lazy<AmazonAPIGatewayClient>(GetApiGatewayClient);
            lambdaClient = new Lazy<AmazonLambdaClient>(GetLambdaClient);
        }

        public MicClient(MicManifest manifest) : this()
        {
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            credentials = new CognitoAWSCredentials(manifest.IdentityPool,
                manifest.AwsRegion);
            cognitoProviderName = $"cognito-idp.{manifest.RegionSystemName}.amazonaws.com/{manifest.UserPool}";
        }

        private AmazonAPIGatewayClient GetApiGatewayClient() =>
            new AmazonAPIGatewayClient(credentials, Manifest.AwsRegion);

        private AmazonLambdaClient GetLambdaClient() =>
            new AmazonLambdaClient(credentials, Manifest.AwsRegion);

        protected async Task<TResponse> InvokeLambdaFunction<TResponse>(
            string functionName, IMicRequestAttributes request,
            bool noAutoRefreshToken = false, CancellationToken cancelToken = default)
        {
            var response = await lambdaClient.Value.InvokeAsync(
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
            credentials.ClearCredentials();
            credentials.AddLogin(cognitoProviderName, loginResponse.Credentials.Token);
            loginCredentials = loginResponse.Credentials;
        }

        /// <inheritdoc />
        [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
        public void Dispose()
        {
            if (apiGatewayClient.IsValueCreated)
                apiGatewayClient.Value.Dispose();
            if (lambdaClient.IsValueCreated)
                lambdaClient.Value.Dispose();
        }
    }
}
