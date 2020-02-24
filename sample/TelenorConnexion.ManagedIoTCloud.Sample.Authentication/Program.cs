using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TelenorConnexion.ManagedIoTCloud.Authentication;

using THNETII.CommandLine.Extensions;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.Sample.Authentication
{
    public static class Program
    {
        public static ICommandHandler Handler { get; } = CommandHandler.Create(
        async (ParseResult cmdlineParsed, ProgramDefinition definition, IHost host, CancellationToken cancelToken) =>
        {
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            var config = host.Services.GetRequiredService<IConfiguration>().GetSection(MicClientConfig.ServiceName);
            var username = cmdlineParsed.FindResultFor(definition.UsernameOption)
                ?.GetValueOrDefault<string>() ?? config["Username"];
            while (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Username must be specified.");
                Console.Write("Username: ");
                username = await ConsoleUtils.ReadLineAsync(cancelToken)
                    .ConfigureAwait(false);
            }
            var password = cmdlineParsed.FindResultFor(definition.PasswordOption)
                ?.GetValueOrDefault<string>() ?? config["Password"];
            while (string.IsNullOrEmpty(password))
            {
                Console.Write("Password: ");
                password = await ConsoleUtils.ReadLineMaskedAsync(cancelToken)
                    .ConfigureAwait(false);
            }

            ILogger logger;

            var micClient = host.Services.GetRequiredService<MicClient>();
            logger = loggerFactory.CreateLogger(micClient.GetType());
            logger.LogInformation("MIC Hostname: {hostname}", micClient.Config.Hostname);

            var manifest = await micClient.GetManifest(cancelToken).ConfigureAwait(false);
            logger.LogInformation("MIC Deployment: '{stack}', version {version}", manifest.StackName, manifest.Version);

            using var authClient = await micClient.GetAuthenticationClient(cancelToken)
                .ConfigureAwait(false);

            return ProcessExitCode.ExitSuccess;
        });

        public static Task<int> Main(string[] args)
        {
            var definition = new ProgramDefinition();
            var parser = new CommandLineBuilder(definition.RootCommand)
                .UseDefaults()
                .UseHost(Host.CreateDefaultBuilder, host =>
                {
                    host.ConfigureServices((context, services) =>
                    {
                        var hostConfig = new MicClientConfig();
                        context.Configuration.Bind(MicClientConfig.ServiceName, hostConfig);

                        services.AddSingleton(definition);
                        services.AddOptions<InvocationLifetimeOptions>()
                            .Configure<IConfiguration>((opts, config) =>
                                config.Bind("Lifetime", opts)
                                );
                        services.AddOptions<MicClientConfig>()
                            .Configure<IConfiguration>((opts, config) =>
                                config.Bind(MicClientConfig.ServiceName, opts))
                            .PostConfigure<ParseResult, ProgramDefinition>((config, cmdline, definition) =>
                            {
                                if (cmdline.FindResultFor(definition.HostOption) is OptionResult hostnameOption)
                                    config.Hostname = hostnameOption.GetValueOrDefault<string>();
                                while (string.IsNullOrEmpty(config.Hostname))
                                {
                                    Console.WriteLine("No MIC Hostname configured");
                                    Console.Write("MIC Hostname: ");
                                    config.Hostname = Console.ReadLine();
                                }
                            })
                            .PostConfigure<IHttpClientFactory>((config, httpFactory) =>
                                config.HttpClientFactory = new InjectionHttpClientFactory(httpFactory)
                                )
                            .Validate(opts =>
                            {
                                opts.Validate();
                                return true;
                            });
                        services.AddHttpClient(Options.DefaultName)
                            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                            {
                                Proxy = hostConfig.GetWebProxy(),
                                DefaultProxyCredentials = hostConfig.ProxyCredentials,
                            });
                        services.AddSingleton(sp =>
                        {
                            var config = sp.GetRequiredService<IOptions<MicClientConfig>>().Value;
                            return new MicClient(config);
                        });
                    });
                })
                .Build();
            return parser.InvokeAsync(args ?? Array.Empty<string>());
        }
    }

    internal class ProgramDefinition
    {
        public ProgramDefinition()
        {
            RootCommand = new RootCommand { Handler = Program.Handler };

            HostOption = new Option<string>(new[] { "-s", "--host", "--service", "--stack" }, "MIC Hostname");
            HostOption.Argument.Arity = ArgumentArity.ZeroOrOne;
            RootCommand.AddOption(HostOption);

            UsernameOption = new Option<string>(new[] { "-u", "--user", "--username" }, "MIC Username");
            UsernameOption.Argument.Arity = ArgumentArity.ZeroOrOne;
            RootCommand.AddOption(UsernameOption);

            PasswordOption = new Option<string>(new[] { "-p", "--password" }, "MIC Password");
            PasswordOption.Argument.Arity = ArgumentArity.ZeroOrOne;
            RootCommand.AddOption(PasswordOption);
        }

        public Command RootCommand { get; }
        public Option<string> HostOption { get; }
        public Option<string> UsernameOption { get; }
        public Option<string> PasswordOption { get; }
    }

    internal class InjectionHttpClientFactory : HttpClientFactory
    {
        private readonly IHttpClientFactory httpClientFactory;

        public InjectionHttpClientFactory(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory
                ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public override HttpClient CreateHttpClient(IClientConfig clientConfig)
        {
            return httpClientFactory.CreateClient();
        }
    }
}
