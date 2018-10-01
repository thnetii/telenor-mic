using Amazon.CognitoIdentity;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Amazon.SecurityToken;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.CognitoIdentity;
using TelenorConnexion.ManagedIoTCloud.LambdaClient.Model;
using TelenorConnexion.ManagedIoTCloud.Model;

namespace TelenorConnexion.ManagedIoTCloud.LambdaClient
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
    public class MicLambdaClient : MicClient, IMicClient, IAmazonService, IDisposable
    {
        private readonly CognitoAWSCredentials awsCredentials;
        private readonly AmazonCognitoIdentityClient cognitoClient;
        private readonly AmazonSecurityTokenServiceClient stsClient;
        private readonly AmazonLambdaClient lambdaClient;

        #region Constructors

        public static async Task<MicLambdaClient> CreateFromHostname(
            string hostname, CancellationToken cancelToken = default)
        {
            var manifest = await MicManifest.GetMicManifest(hostname, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return new MicLambdaClient(manifest);
        }

        public MicLambdaClient(MicManifest manifest) : base(manifest)
        {
            Config = new MicClientConfig()
            {
                RegionEndpoint = manifest.AwsRegion
            };

            AnonymousAWSCredentials anonymousAwsCreds = new AnonymousAWSCredentials();
            cognitoClient = new AmazonCognitoIdentityClient(anonymousAwsCreds, Config.Create<AmazonCognitoIdentityConfig>());
            stsClient = new AmazonSecurityTokenServiceClient(anonymousAwsCreds, Config.Create<AmazonSecurityTokenServiceConfig>());

            awsCredentials = CreateEmptyAWSCredentials();

            lambdaClient = new AmazonLambdaClient(awsCredentials, Config.Create<AmazonLambdaConfig>());
        }

        #endregion // Constructors

        #region Overrides

        protected override async Task<TResponse> InvokeClientRequest<TRequest, TResponse>(string actionName, TRequest request, CancellationToken cancelToken = default)
        {
            var lambdaRequest = new InvokeRequest();
            var micRequest = new MicLambdaRequest<TRequest> { Attributes = request };

            switch (actionName)
            {
                #region Auth API
                case nameof(AuthConfirmSignup):
                    micRequest.Action = "CONFIRM_SIGN_UP";
                    goto case nameof(Manifest.AuthLambda);
                case nameof(AuthForgotPassword):
                    micRequest.Action = "FORGOT_PASSWORD";
                    goto case nameof(Manifest.AuthLambda);
                case nameof(AuthLogin):
                    micRequest.Action = "LOGIN";
                    awsCredentials.RemoveLogin(Manifest.GetCognitoProviderName());
                    goto case nameof(Manifest.AuthLambda);
                case nameof(AuthGiveConsent):
                    micRequest.Action = "GIVE_CONSENT";
                    goto case nameof(Manifest.AuthLambda);
                case nameof(AuthRefresh):
                    micRequest.Action = "REFRESH";
                    awsCredentials.RemoveLogin(Manifest.GetCognitoProviderName());
                    goto case nameof(Manifest.AuthLambda);
                case nameof(AuthResendConfirmationCode):
                    micRequest.Action = "RESEND_CONFIRMATION_CODE";
                    goto case nameof(Manifest.AuthLambda);
                case nameof(AuthSetPassword):
                    micRequest.Action = "SET_PASSWORD";
                    goto case nameof(Manifest.AuthLambda);
                case nameof(AuthSignup):
                    micRequest.Action = "SIGN_UP";
                    goto case nameof(Manifest.AuthLambda);
                case nameof(Manifest.AuthLambda):
                    lambdaRequest.FunctionName = Manifest.AuthLambda;
                    break;
                #endregion // Auth API
                #region User API
                case nameof(UserCreate):
                    micRequest.Action = "CREATE";
                    goto case nameof(Manifest.UserLambda);
                case nameof(UserResetPassword):
                    micRequest.Action = "RESET_PASSWORD";
                    goto case nameof(Manifest.UserLambda);
                case nameof(UserGet):
                    micRequest.Action = "GET";
                    goto case nameof(Manifest.UserLambda);
                case nameof(Manifest.UserLambda):
                    lambdaRequest.FunctionName = Manifest.UserLambda;
                    break;
                #endregion // User API
                default:
                    throw new InvalidOperationException("Unknown action name: " + actionName);
            }

            lambdaRequest.Payload = JsonConvert.SerializeObject(micRequest);
            var lambdaResponse = await lambdaClient.InvokeAsync(lambdaRequest, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return await DeserializeOrThrow<TResponse>(lambdaResponse,
                cancelToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        protected override void UpdateCredentials(MicAuthLoginResponse loginResponse)
        {
            base.UpdateCredentials(loginResponse);
            this.AddLoginToCognitoCredentials(awsCredentials);
        }

        #endregion // Overrides

        #region Public helper methods

        public CognitoAWSCredentials CreateEmptyAWSCredentials()
        {
            return new CognitoAWSCredentials(
                accountId: null, Manifest.IdentityPool, unAuthRoleArn: null,
                authRoleArn: null, cognitoClient, stsClient
                );
        }

        public CognitoAWSCredentials GetCognitoAWSCredentials()
        {
            var creds = CreateEmptyAWSCredentials();
            this.AddLoginToCognitoCredentials(creds);
            return creds;
        }

        #endregion // Public helper methods

        #region Private Helper methods

        private Task<TResponse> DeserializeOrThrow<TResponse>(InvokeResponse lambdaResponse,
            CancellationToken cancelToken)
        {
            using (var textReader = new StreamReader(lambdaResponse.Payload, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(textReader))
                return DeserializeOrThrow<TResponse>(jsonReader, cancelToken);
        }

        #endregion // Private Helper methods

        #region IAmazonService

        public MicClientConfig Config { get; }

        IClientConfig IAmazonService.Config => Config;

        #endregion // IAmazonService

        #region Dispose

        /// <inheritdoc />
        [DebuggerStepThrough]
        [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
        public void Dispose()
        {
            cognitoClient.Dispose();
            stsClient.Dispose();
            lambdaClient.Dispose();
        }

        #endregion // Dispose
    }
}
