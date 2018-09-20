using Amazon.Lambda.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi
{
    public static class CloudApiResponseConverter
    {
        public static async Task<TResponse> ConvertOrThrowErrorMessage<TResponse>(
            this InvokeResponse response, CancellationToken cancellationToken = default)
        {
            using (var textReader = new StreamReader(response.Payload, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                var jsonObject = await JObject.LoadAsync(jsonReader, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
                if (jsonObject.ContainsKey(CloudApiException.ErrorMessageKey))
                {
                    throw new CloudApiException(jsonObject.ToObject<CloudApiErrorMessage>());
                }
                return jsonObject.ToObject<TResponse>();
            }
        }
    }
}
