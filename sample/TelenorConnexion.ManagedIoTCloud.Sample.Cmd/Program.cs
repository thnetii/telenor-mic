using Amazon.IoTDeviceGateway;
using McMaster.Extensions.CommandLineUtils;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.LambdaClient;
using TelenorConnexion.ManagedIoTCloud.Model;

namespace TelenorConnexion.ManagedIoTCloud.Sample.Cmd
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Write("MIC Hostname: ");
            var hostname = Console.ReadLine();
            //Console.Write("API Key: ");
            //var apiKey = Console.ReadLine();
            Console.WriteLine($"Getting MIC manifest from: {new Uri(MicManifest.ManifestServiceUri, $"?hostname={Uri.EscapeDataString(hostname)}")} . . .");
            using (var proxyHandler = new HttpClientHandler() { Proxy = new WebProxy("http://localhost:8888/"), UseProxy = true })
            //using (var micClient = await MicRestClient.CreateFromHostname(hostname, apiKey, proxyHandler))
            using (var httpClient = new HttpClient(proxyHandler))
            using (var micClient = new MicLambdaClient(await MicManifest.GetMicManifest(hostname, httpClient)))
            {
                var micClientConfig = micClient.Config;
                micClientConfig.LogMetrics = true;
                micClientConfig.ProxyHost = "localhost";
                micClientConfig.ProxyPort = 8888;

                Console.Write("Username: ");
                var username = Console.ReadLine();
                var password = Prompt.GetPassword("Password:");

                Console.Write("Logging in . . . ");
                var login = await micClient.AuthLogin(
                    username, password);
                ((IMicModel)(login.User)).AdditionalData.TryGetValue("domainPath", out object domainPath);
                Console.WriteLine("Successful!");
                Console.WriteLine();

                var userInfo = await micClient.UserGet(login.User.Username);
                Console.WriteLine(JsonConvert.SerializeObject(userInfo, Formatting.Indented));

                Console.WriteLine();
                Console.Write("Connecting MQTT Client . . . ");
                var iotConfig = micClientConfig.Create<AmazonIoTDeviceGatewayConfig>();
                using (var iotClient = new AmazonIoTDeviceGatewayClient(((IMicClient)micClient).AwsCredentials, iotConfig))
                {
                    var mqttOptionsTask = iotClient.CreateMqttWebSocketClientOptionsAsync(micClient.Manifest.IotEndpoint);

                    using (var mqttClient = new MqttFactory().CreateMqttClient())
                    {
                        var mqttOptions = await mqttOptionsTask;
                        var connectInfo = await mqttClient.ConnectAsync(mqttOptions);
                        Console.WriteLine("Successful!");
                        Console.Write($"Subscribing to events on domain path {domainPath} . . . ");
                        var subscriptions = await mqttClient.SubscribeAsync($"event{domainPath}#");
                        Console.WriteLine($"{subscriptions.Count} subscriptions.");
                        foreach (var sub in subscriptions)
                        {
                            var tf = sub.TopicFilter;
                            Console.WriteLine($"{tf.Topic} (QoS: {tf.QualityOfServiceLevel}): {sub.ReturnCode}");
                        }
                        await mqttClient.DisconnectAsync();
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
