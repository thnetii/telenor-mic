using Amazon.Lambda.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TelenorConnexion.ManagedIoTCloud
{
    /// <summary>
    /// Provides extension methods to use on response objects returned from clients
    /// connected to the MIC API.
    /// </summary>
    public static class MicResponseExtensions
    {
        /// <summary>
        /// Deserializes a response from an AWS Lambda invocation to a Cloud API
        /// function.
        /// </summary>
        /// <typeparam name="TResponse">The expected type to which the response payload should be deserialized.</typeparam>
        /// <param name="response">The AWS Lambda Invocation Response object received from the Cloud API Lambda client.</param>
        /// <param name="cancelToken">An optional cancellation token with which the deserialization can be interrupted.</param>
        /// <returns>A deserialized instance of type <typeparamref name="TResponse"/>.</returns>
        /// <exception cref="MicException">The payload included in the response indicated an error.</exception>
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
