using Amazon.IoTDeviceGateway;
using McMaster.Extensions.CommandLineUtils;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.LambdaClient;
using TelenorConnexion.ManagedIoTCloud.Model;
using TelenorConnexion.ManagedIoTCloud.RestClient;

namespace TelenorConnexion.ManagedIoTCloud.Sample.Cmd
{
    public static class Program
    {
        public const bool useProxy = true;
        public const bool useLambdaClient = false;

        public static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) =>
            {
                cts.Cancel(throwOnFirstException: true);
                e.Cancel = true;
            };

            try { await Run(cts.Token); }
            catch (OperationCanceledException) { }
            catch (AggregateException aggregateExcept) when (aggregateExcept.InnerException is OperationCanceledException) { }
        }

        public static async Task<MicClient> CreateMicClient(string hostname, string apiKey, CancellationToken cancelToken)
        {
            HttpClientHandler httpHandler = new HttpClientHandler();
            if (useProxy)
            {
                httpHandler.Proxy = new WebProxy("http://localhost:8888/");
                httpHandler.UseProxy = true;
            }

            MicManifest manifest;
            MicClient micClient;
            if (useLambdaClient)
            {
                using (var httpClient = new HttpClient(httpHandler))
                {
                    manifest = await MicManifest.GetMicManifest(hostname, httpClient, cancelToken);
                }
                micClient = new MicLambdaClient(manifest);
            }
            else
            {
                micClient = await MicRestClient.CreateFromHostname(hostname, apiKey, httpHandler, cancelToken);
            }

            if (useProxy)
            {
                micClient.Config.ProxyHost = "localhost";
                micClient.Config.ProxyPort = 8888;
            }

            return micClient;
        }

        public static async Task Run(CancellationToken cancelToken)
        {
            Console.Write("MIC Hostname: ");
            var hostname = Console.ReadLine();
            string apiKey = null;
            if (!useLambdaClient)
            {
                Console.Write("API Key: ");
                apiKey = Console.ReadLine();
            }
            Console.WriteLine("Getting MIC manifest . . .");
            using (var proxyHandler = new HttpClientHandler() { Proxy = new WebProxy("http://localhost:8888/"), UseProxy = true })
            using (var micClient = await CreateMicClient(hostname, apiKey, cancelToken))
            {
                var micClientConfig = micClient.Config;
                micClientConfig.LogMetrics = true;

                Console.Write("Username: ");
                var username = Console.ReadLine();
                var password = Prompt.GetPassword("Password:");

                Console.Write("Logging in . . . ");
                var login = await micClient.AuthLogin(
                    username, password, cancelToken);
                ((IMicModel)(login.User)).AdditionalData.TryGetValue("domainPath", out object domainPath);
                Console.WriteLine("Successful!");
                Console.WriteLine();

                var userInfo = await micClient.UserGet(login.User.Username, cancelToken);
                Console.WriteLine(JsonConvert.SerializeObject(userInfo, Formatting.Indented));
                Console.WriteLine();

                Console.Write("Connecting MQTT Client . . . ");
                var iotConfig = micClientConfig.Create<AmazonIoTDeviceGatewayConfig>();
                using (var iotClient = new AmazonIoTDeviceGatewayClient(((IMicClient)micClient).AwsCredentials, iotConfig))
                {
                    var mqttOptionsTask = iotClient.CreateMqttWebSocketClientOptionsAsync(micClient.Manifest.IotEndpoint, cancelToken);

                    using (var mqttClient = new MqttFactory().CreateMqttClient())
                    {
                        var mqttConsoleSync = new object();
                        mqttClient.ApplicationMessageReceived += (sender, e) =>
                        {
                            Task.Run(() =>
                            {
                                lock (mqttConsoleSync)
                                {
                                    Console.WriteLine($"Application Message received by client {e.ClientId}");
                                    Console.WriteLine($"Topic: {e.ApplicationMessage.Topic}, QoS: {e.ApplicationMessage.QualityOfServiceLevel}");
                                    if (e.ApplicationMessage.Retain)
                                        Console.WriteLine("  Message should be retained");
                                    int payloadLength = e.ApplicationMessage.Payload?.Length ?? 0;
                                    Console.WriteLine($"  Message Payload: ({payloadLength} byte{(payloadLength == 1 ? "" : "s")})");
                                    Console.WriteLine();
                                    Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
                                    Console.WriteLine();
                                    Console.WriteLine(new string('-', count: 20));
                                    Console.WriteLine();
                                }
                            }, cancelToken);
                        };

                        var mqttOptions = await mqttOptionsTask;
                        var connectInfo = await mqttClient.ConnectAsync(mqttOptions);
                        try
                        {
                            cancelToken.ThrowIfCancellationRequested();
                            Console.WriteLine("Successful!");
                            Console.Write($"Subscribing to events . . . ");
                            var subscriptions = await mqttClient.SubscribeAsync($"event{domainPath}");
                            cancelToken.ThrowIfCancellationRequested();
                            var subscriptionsWild = await mqttClient.SubscribeAsync($"event{domainPath}#");
                            cancelToken.ThrowIfCancellationRequested();
                            var thingUpdateSubs = await mqttClient.SubscribeAsync($"thing-update{domainPath}#");
                            cancelToken.ThrowIfCancellationRequested();
                            int subCount = subscriptions.Count + subscriptionsWild.Count + thingUpdateSubs.Count;
                            Console.WriteLine($"{subCount} subscription{(subCount == 1 ? "" : "s")}.");
                            foreach (var sub in subscriptions.Concat(subscriptionsWild).Concat(thingUpdateSubs))
                            {
                                var tf = sub.TopicFilter;
                                Console.WriteLine($"{tf.Topic} (QoS: {tf.QualityOfServiceLevel}): {sub.ReturnCode}");
                            }
                            Console.WriteLine();
                            cancelToken.ThrowIfCancellationRequested();

                            var resetEvent = new ManualResetEventSlim();
                            resetEvent.Wait(cancelToken);
                        }
                        finally
                        {
                            await mqttClient.DisconnectAsync();
                        }
                    }
                }
            }
        }
    }
}
