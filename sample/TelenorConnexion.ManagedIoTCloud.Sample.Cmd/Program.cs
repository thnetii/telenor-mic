using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Amazon.IoTDeviceGateway;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TelenorConnexion.ManagedIoTCloud.Model;

using THNETII.CommandLine.Extensions;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.Sample.Cmd
{
    public static class Program
    {
        public static int Main()
        {
            try { ConsoleUtils.RunAsync(Run).GetAwaiter().GetResult(); }
            catch (OperationCanceledException) { return ProcessExitCode.ExitFailure; }
            return ProcessExitCode.ExitSuccess;
        }

        public static async Task<MicClient> CreateMicClient(string hostname, CancellationToken cancelToken)
        {
            HttpClientHandler httpHandler = new HttpClientHandler();
            var proxyAddress = Environment.GetEnvironmentVariable("TELENOR_MIC_PROXY");
            WebProxy webProxy = default;
            if (!string.IsNullOrWhiteSpace(proxyAddress))
            {
                webProxy = new WebProxy(proxyAddress);
                httpHandler.Proxy = webProxy;
                httpHandler.UseProxy = true;
            }

            MicClient micClient = await MicClient.CreateFromHostname(hostname, httpHandler, cancelToken);
            if (!(webProxy is null))
            {
                micClient.Config.ProxyHost = webProxy.Address.Host;
                micClient.Config.ProxyPort = webProxy.Address.Port;
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

                //var immutableCredentials = await awsCredentials.GetCredentialsAsync();
                //Console.WriteLine("AWS Immutable Credentials:");
                //Console.WriteLine($"  Access key: {immutableCredentials.AccessKey}");
                //Console.WriteLine($"  Secret key: {immutableCredentials.SecretKey}");
                //if (immutableCredentials.UseToken)
                //    Console.WriteLine($"  Token:      {immutableCredentials.Token}");
                //Console.WriteLine();

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
                        mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedLoggerHandler(mqttConsoleSync, cancelToken);

                        var mqttOptions = await mqttOptionsTask;
                        var connectInfo = await mqttClient.ConnectAsync(mqttOptions);
                        try
                        {
                            cancelToken.ThrowIfCancellationRequested();
                            Console.WriteLine("Successful!");
                            Console.WriteLine($"MQTT Client ID: {mqttClient.Options.ClientId}");
                            Console.Write($"Subscribing to events . . . ");
                            var subscriptionTasks = new Task<MqttClientSubscribeResult>[]
                            {
                                mqttClient.SubscribeAsync($"event{domainPath}"),
                                mqttClient.SubscribeAsync($"event{domainPath}#"),
                                mqttClient.SubscribeAsync($"thing-update{domainPath}#")
                            };
                            cancelToken.ThrowIfCancellationRequested();
                            Task.WaitAll(subscriptionTasks, cancelToken);
                            cancelToken.ThrowIfCancellationRequested();
                            int subCount = subscriptionTasks.Sum(t => t.Result.Items.Count);
                            Console.WriteLine($"{subCount} subscription{(subCount == 1 ? "" : "s")}.");
                            foreach (var sub in subscriptionTasks.SelectMany(t => t.Result.Items))
                            {
                                var tf = sub.TopicFilter;
                                Console.WriteLine($"{tf.Topic} (QoS: {tf.QualityOfServiceLevel}): {sub.ResultCode}");
                            }
                            Console.WriteLine();
                            cancelToken.ThrowIfCancellationRequested();

                            using (var resetEvent = new ManualResetEventSlim())
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

        private class MqttApplicationMessageReceivedLoggerHandler : IMqttApplicationMessageReceivedHandler
        {
            private readonly object mqttConsoleSync;
            private readonly CancellationToken cancelToken;

            public MqttApplicationMessageReceivedLoggerHandler(object mqttConsoleSync, CancellationToken cancelToken)
            {
                this.mqttConsoleSync = mqttConsoleSync;
                this.cancelToken = cancelToken;
            }

            public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
            {
                return Task.Run(() =>
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
#pragma warning disable CA1031 // Do not catch general exception types
                    catch { }
#pragma warning restore CA1031 // Do not catch general exception types
                    Console.WriteLine(payload);
                        Console.WriteLine();
                        Console.WriteLine(new string('-', count: 20));
                        Console.WriteLine();
                    }
                }, cancelToken);
            }
        }
    }
}
