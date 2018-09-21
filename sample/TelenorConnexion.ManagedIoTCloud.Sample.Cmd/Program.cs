﻿using McMaster.Extensions.CommandLineUtils;
using System;
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

                Console.WriteLine("Retrieving AWS Credentials . . .");
                var creds = await micClient.Credentials.GetCredentialsAsync();
                Console.WriteLine($"{nameof(creds.AccessKey)}: {creds.AccessKey}");
                Console.WriteLine($"{nameof(creds.SecretKey)}: {creds.SecretKey}");
                if (creds.UseToken)
                    Console.WriteLine($"{nameof(creds.Token)}: {creds.Token}");
            }
        }
    }
}
