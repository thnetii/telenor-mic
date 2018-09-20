using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TelenorConnexion.ManagedIoTCloud.CloudApi;

namespace TelenorConnexion.ManagedIoTCloud.Sample.Cmd
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var hostname = Prompt.GetString("MIC Hostname:");
            Console.WriteLine($"Getting MIC manifest from: {new Uri(MicManifest.ManifestServiceUri, $"?hostname={Uri.EscapeDataString(hostname)}")} . . .");
            using (var cloudApiClient = await CloudApiClient.CreateFromHostname(hostname))
            {
                var username = Prompt.GetString("Username:");
                var password = Prompt.GetPassword("Password:");

                Console.Write("Logging in . . . ");
                var login = await cloudApiClient.AuthClient.Login(
                    username, password);
                Console.WriteLine("Successful!");
                Console.WriteLine();
                Console.WriteLine(JsonConvert.SerializeObject(login, Formatting.Indented));

                login = await cloudApiClient.AuthClient.Refresh();
            }
        }
    }
}
