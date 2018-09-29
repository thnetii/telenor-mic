using Newtonsoft.Json;
using TelenorConnexion.ManagedIoTCloud.Model;

namespace TelenorConnexion.ManagedIoTCloud.LambdaClient.Model
{
    public class MicLambdaRequest<T> : MicModel
        where T : MicModel
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("attributes")]
        public T Attributes { get; set; }
    }
}
