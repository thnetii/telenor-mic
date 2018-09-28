using Newtonsoft.Json;
using TelenorConnexion.ManagedIoTCloud.Model;

namespace TelenorConnexion.ManagedIoTCloud.LambdaClient.Model
{
    public interface IMicLambdaRequest<T> : IMicModel
        where T : IMicModel
    {
        [JsonProperty("action")]
        string Action { get; set; }

        [JsonProperty("attributes")]
        T Attributes { get; set; }
    }

    public class MicLambdaRequest<T> : MicModel, IMicLambdaRequest<T>
        where T : IMicModel
    {
        public string Action { get; set; }
        public T Attributes { get; set; }
    }
}
