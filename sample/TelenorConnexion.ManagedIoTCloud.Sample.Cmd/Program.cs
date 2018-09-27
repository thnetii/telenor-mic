using Amazon.Util;
using McMaster.Extensions.CommandLineUtils;
using MQTTnet;
using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace TelenorConnexion.ManagedIoTCloud.Sample.Cmd
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var hostname = Prompt.GetString("MIC Hostname:");
            Console.WriteLine($"Getting MIC manifest from: {new Uri(MicManifest.ManifestServiceUri, $"?hostname={Uri.EscapeDataString(hostname)}")} . . .");
            using (var micClient = await MicClient.CreateFromHostname(hostname))
            {
                var username = Prompt.GetString("Username:");
                var password = Prompt.GetPassword("Password:");

                Console.Write("Logging in . . . ");
                var login = await micClient.AuthLogin(
                    username, password);
                Console.WriteLine("Successful!");
                Console.WriteLine();

                var mqttOptionsTask = micClient.CreateMqttWebSocketOptions();

                using (var mqttClient = new MqttFactory().CreateMqttClient())
                {
                    var connectResult = await mqttClient.ConnectAsync(await mqttOptionsTask);
                }
            }

            Console.ReadLine();
        }
    }
}
