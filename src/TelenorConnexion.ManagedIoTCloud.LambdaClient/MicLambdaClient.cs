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
using TelenorConnexion.ManagedIoTCloud.LambdaClient.Model;
using TelenorConnexion.ManagedIoTCloud.Model;

namespace TelenorConnexion.ManagedIoTCloud.LambdaClient
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
    public class MicLambdaClient : MicClient, IMicClient, IDisposable
    {
        private bool _disposed;

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
            lambdaClient = new AmazonLambdaClient(AwsCredentials, Config.Create<AmazonLambdaConfig>());
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
                    AwsCredentials.RemoveLogin(Manifest.GetCognitoProviderName());
                    goto case nameof(Manifest.AuthLambda);
                case nameof(AuthGiveConsent):
                    micRequest.Action = "GIVE_CONSENT";
                    goto case nameof(Manifest.AuthLambda);
                case nameof(AuthRefresh):
                    micRequest.Action = "REFRESH";
                    AwsCredentials.RemoveLogin(Manifest.GetCognitoProviderName());
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

        #endregion // Overrides

        #region Private Helper methods

        private Task<TResponse> DeserializeOrThrow<TResponse>(InvokeResponse lambdaResponse,
            CancellationToken cancelToken)
        {
            using (var textReader = new StreamReader(lambdaResponse.Payload, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(textReader))
                return DeserializeOrThrow<TResponse>(jsonReader, cancelToken);
        }

        #endregion // Private Helper methods

        #region Dispose

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                lambdaClient.Dispose();

                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion // Dispose
    }
}
