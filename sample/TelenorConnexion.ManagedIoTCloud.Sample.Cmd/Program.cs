using Amazon.IoTDeviceGateway;
using Amazon.Runtime;
using McMaster.Extensions.CommandLineUtils;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.CognitoIdentity;
using TelenorConnexion.ManagedIoTCloud.LambdaClient;

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
                //var micClientConfig = new MicClientConfig() { RegionEndpoint = micClient.Manifest.AwsRegion };
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
                Console.WriteLine("Successful!");
                Console.WriteLine();

                var userInfo = await micClient.UserGet(login.User.Username);
                Console.WriteLine(JsonConvert.SerializeObject(userInfo, Formatting.Indented));

                Console.WriteLine();
                Console.WriteLine("Connecting MQTT Client . . .");
                var iotConfig = micClientConfig.Create<AmazonIoTDeviceGatewayConfig>();
                using (var iotClient = new AmazonIoTDeviceGatewayClient(micClient.GetCognitoAWSCredentials(), iotConfig))
                {
                    var mqttOptionsTask = iotClient.CreateMqttWebSocketClientOptionsAsync(micClient.Manifest.IotEndpoint);

                    using (var mqttClient = new MqttFactory().CreateMqttClient())
                    {
                        var mqttOptions = await mqttOptionsTask;
                        if (mqttOptions.ChannelOptions is MqttClientWebSocketOptions webSocketOptions)
                        {
                            if (webSocketOptions.ProxyOptions is null)
                                webSocketOptions.ProxyOptions = new MqttClientWebSocketProxyOptions();
                            webSocketOptions.ProxyOptions.Address = "http://localhost:8888/";
                        }

                        var connectInfo = await mqttClient.ConnectAsync(mqttOptions);
                        Console.WriteLine($"{nameof(connectInfo.IsSessionPresent)}: {connectInfo.IsSessionPresent}");
                        await mqttClient.DisconnectAsync();
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
