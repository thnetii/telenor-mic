using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.AuthApi
{
    /// <summary>
    /// The Auth API is used to authenticate users and let new users sign up.
    /// </summary>
    public class AuthApiClient
    {
        private readonly string functionName;
        private readonly AmazonLambdaClient lambdaClient;

        public async Task Login(string userName, string password,
            CancellationToken cancellationToken = default)
        {
            var request = new AuthLoginRequest
            {
                Action = "LOGIN",
                Attributes = new AuthLoginRequestAttributes
                {
                    Username = userName,
                    Password = password
                }
            };
            var response = await lambdaClient.InvokeAsync(
                new InvokeRequest
                {
                    FunctionName = functionName,
                    Payload = JsonConvert.SerializeObject(request)
                }, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
