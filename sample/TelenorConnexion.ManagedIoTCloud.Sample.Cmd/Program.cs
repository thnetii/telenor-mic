using Amazon.IoTDeviceGateway;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.LambdaClient;
using TelenorConnexion.ManagedIoTCloud.Model;
using TelenorConnexion.ManagedIoTCloud.RestClient;
using THNETII.Common;
using THNETII.Common.Cli;

namespace TelenorConnexion.ManagedIoTCloud.Sample.Cmd
{
    public static class Program
    {
        public const bool useProxy = true;
        public const bool useLambdaClient = false;

        public static int Main()
        {
            try { ConsoleUtils.RunAsync(Run).GetAwaiter().GetResult(); }
            catch (OperationCanceledException) { return ProcessExitCode.ExitFailure; }
            return ProcessExitCode.ExitSuccess;
        }

        public static async Task<MicClient> CreateMicClient(string hostname, CancellationToken cancelToken)
        {
            HttpClientHandler httpHandler = new HttpClientHandler();
            if (useProxy)
            {
                httpHandler.Proxy = new WebProxy("http://localhost:8888/");
                httpHandler.UseProxy = true;
            }

            MicClient micClient;
            if (useLambdaClient)
            {
                MicManifest manifest;
                using (var httpClient = new HttpClient(httpHandler))
                {
                    manifest = await MicManifest.GetMicManifest(hostname, httpClient, cancelToken);
                }
                micClient = new MicLambdaClient(manifest);
            }
            else
            {
                micClient = await MicRestClient.CreateFromHostname(hostname, httpHandler, cancelToken);
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
            var hostname = await ConsoleUtils.ReadLineAsync(cancelToken);
            Console.WriteLine("Getting MIC manifest . . .");
            using (var micClient = await CreateMicClient(hostname, cancelToken))
            {
                var micClientConfig = micClient.Config;
                micClientConfig.LogMetrics = true;

                Console.Write("Username: ");
                string username = await ConsoleUtils.ReadLineAsync(cancelToken);
                Console.Write("Password: ");
                string password = await ConsoleUtils.ReadLineMaskedAsync(cancelToken);

                Console.Write("Logging in . . . ");
                var login = await micClient.AuthLogin(
                    username, password, cancelToken);
                ((IMicModel)(login.User)).AdditionalData.TryGetValue("domainPath", out object domainPath);
                Console.WriteLine("Successful!");
                Console.WriteLine();

                var awsCredentials = ((IMicClient)micClient).AwsCredentials;
                Console.WriteLine($"Cognito Identity pool: {awsCredentials.IdentityPoolId}");
                Console.WriteLine($"  Identity Id: {login.Credentials.IdentityId}");
                Console.WriteLine($"  Login provider: {micClient.Manifest.GetCognitoProviderName()}");
                Console.WriteLine($"  Token: {login.Credentials.Token}");
                Console.WriteLine();

                var immutableCredentials = await awsCredentials.GetCredentialsAsync();
                Console.WriteLine("AWS Immutable Credentials:");
                Console.WriteLine($"  Access key: {immutableCredentials.AccessKey}");
                Console.WriteLine($"  Secret key: {immutableCredentials.SecretKey}");
                if (immutableCredentials.UseToken)
                    Console.WriteLine($"  Token:      {immutableCredentials.Token}");
                Console.WriteLine();

                var userInfo = await micClient.UserGet(login.User.Username, cancelToken);
                Console.WriteLine(JsonConvert.SerializeObject(userInfo, Formatting.Indented));
                Console.WriteLine();

                Console.WriteLine($"IoT Endpoint: {micClient.Manifest.IotEndpoint}");
                Console.Write("Connecting MQTT Client . . . ");
                var iotConfig = micClientConfig.Create<AmazonIoTDeviceGatewayConfig>();
                using (var iotClient = new AmazonIoTDeviceGatewayClient(awsCredentials, iotConfig))
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
                                    string payload = e.ApplicationMessage.ConvertPayloadToString();
                                    try
                                    {
                                        var jtoken = JToken.Parse(payload);
                                        payload = jtoken.ToString(Formatting.Indented);
                                    }
                                    catch { }
                                    Console.WriteLine(payload);
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
