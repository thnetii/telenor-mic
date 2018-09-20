using Amazon.Lambda.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TelenorConnexion.ManagedIoTCloud
{
    public static class MicResponseExtensions
    {
        public static async Task<TResponse> DeserializeOrThrow<TResponse>(
            this InvokeResponse response, CancellationToken cancelToken = default)
        {
            using (var textReader = new StreamReader(response.Payload, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                var jsonObject = await JObject.LoadAsync(jsonReader, cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
                if (jsonObject.ContainsKey(MicException.ErrorMessageKey))
                {
                    throw new MicException(jsonObject.ToObject<MicErrorMessage>());
                }
                return jsonObject.ToObject<TResponse>();
            }
        }
    }
}
