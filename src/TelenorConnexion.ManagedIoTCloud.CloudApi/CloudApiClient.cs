using Amazon.CognitoIdentity;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.CloudApi.AuthApi;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
    public class CloudApiClient : IDisposable
    {
        public static async Task<CloudApiClient> CreateFromHostname(
            string hostname, CancellationToken cancellationToken = default
            )
        {
            var manifest = await MicManifest.GetMicManifest(hostname, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return new CloudApiClient(manifest);
        }

        private readonly CognitoAWSCredentials credentials;
        private readonly AmazonLambdaClient lambdaClient;
        private readonly Lazy<AuthApiClient> authClient;

        public MicManifest Manifest { get; }

        public AuthApiClient AuthClient => authClient.Value;

        public async Task<TResponse> InvokeFunction<TResponse>(
            string functionName, ICloudApiRequestAttributes request,
            bool noAutoRefresh = false, CancellationToken cancellationToken = default)
        {
            var response = await lambdaClient.InvokeAsync(
                new InvokeRequest
                {
                    FunctionName = functionName,
                    Payload = JsonConvert.SerializeObject(request.CreateRequest())
                }, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return await response.ConvertOrThrowErrorMessage<TResponse>(
                cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        private CloudApiClient()
        {
            authClient = new Lazy<AuthApiClient>(CreateAuthApiClient);
        }

        public CloudApiClient(MicManifest manifest) : this()
        {
            Manifest = manifest;

            credentials = new CognitoAWSCredentials(manifest?.IdentityPool,
                manifest?.AwsRegion);
            lambdaClient = new AmazonLambdaClient(credentials, manifest?.AwsRegion);
        }

        private AuthApiClient CreateAuthApiClient() =>
            new AuthApiClient(this, credentials);

        /// <inheritdoc />
        [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
        public void Dispose()
        {
            lambdaClient?.Dispose();
        }
    }
}
