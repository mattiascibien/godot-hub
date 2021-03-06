using ConfigurationSubstitution;
using GodotHub.Commands;
using GodotHub.Core;
using GodotHub.Local;
using GodotHub.Online;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Globalization;
using GodotHub.Resources;

namespace GodotHub
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            await BuildCommandLine()
            .UseHost(_ => Host.CreateDefaultBuilder(),
                host =>
                {
                    host.ConfigureAppConfiguration(builder =>
                    {
                        var defaults = new Dictionary<string, string>()
                        {
                            { "godothub_root", $"$({(OperatingSystem.IsWindows() ? "USERPROFILE" : "HOME")})/.godot-hub" },
                            { "installation_directory", "$(godothub_root)/installations" },
                            { "downloads_directory", "$(godothub_root)/downloads" }
                        };
                        builder.AddInMemoryCollection(defaults);

                        builder.AddEnvironmentVariables("GODOTHUB_");
                        builder.AddJsonFile(GodotHubPaths.LocalConfigFilename, optional: true, reloadOnChange: true); // this is relative to the current directory
                        builder.EnableSubstitutions("$(", ")");
                    });

                    host.ConfigureServices(services =>
                    {
                        services.AddLocalization();
                        services.AddSingleton<GodotHubPaths>();
                        services.AddTransient<InstallationManager>();
                        services.AddTransient<IOnlineRepository, GithubOnlineRepository>();

                        if (OperatingSystem.IsWindows())
                        {
                            services.AddSingleton<ILinkCreator, Core.Windows.LinkCreatorWindows>();
                        }
                        else
                        {
                            services.AddSingleton<ILinkCreator, Core.Unix.LinkCreatorUnix>();
                        }
                    });
                    host.UseCommandHandler<RunCommand, RunCommand.CommandHandler>();
                    host.UseCommandHandler<ListCommand, ListCommand.CommandHandler>();
                    host.UseCommandHandler<InstallCommand, InstallCommand.CommandHandler>();
                    host.UseCommandHandler<UninstallCommand, UninstallCommand.CommandHandler>();
                    host.UseCommandHandler<CreateLocalConfigurationCommand, CreateLocalConfigurationCommand.CommandHandler>();
                    host.UseCommandHandler<RegisterCommand, RegisterCommand.CommandHandler>();
                    host.UseCommandHandler<UnregisterCommand, UnregisterCommand.CommandHandler>();
                })
            .UseDefaults()
            .Build()
            .InvokeAsync(args).ConfigureAwait(false);
        }

        private static CommandLineBuilder BuildCommandLine()
        {
            // Create a root command with some options
            var rootCommand = new RootCommand
            {
                new RunCommand(),
                new ListCommand(),
                new InstallCommand(),
                new UninstallCommand(),
                new CreateLocalConfigurationCommand(),
                new RegisterCommand(),
                new UnregisterCommand(),
            };

            rootCommand.Description = Strings.RootCommandDescription;
            return new CommandLineBuilder(rootCommand);
        }
    }
}
