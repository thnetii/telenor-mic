using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TelenorConnexion.ManagedIoTCloud.RestClient
{
    public class MicRestHttpHandler : DelegatingHandler
    {
        public MicRestHttpHandler() : base() { }
        public MicRestHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }
        public MicRestHttpHandler(MicRestClient micClient) : base() =>
            MicClient = micClient;
        public MicRestHttpHandler(MicRestClient micClient, HttpMessageHandler innerHandler) : base(innerHandler) =>
            MicClient = micClient;

        public MicRestClient MicClient { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var creds = (MicClient as IMicClient)?.Credentials;
            if (!(creds is null))
            {
                request.Headers.Add("identityId", creds.IdentityId ?? string.Empty);
                var authAdded = request.Headers.TryAddWithoutValidation(nameof(request.Headers.Authorization), creds.Token);
                Debug.Assert(authAdded, "Authorization header could not be added");
            }
            string apiKey = MicClient?.ApiKey;
            if (!string.IsNullOrEmpty(apiKey))
                request.Headers.Add("x-api-key", apiKey);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
