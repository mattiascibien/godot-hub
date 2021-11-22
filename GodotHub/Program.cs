using GodotHub.Commands;
using GodotHub.Core;
using GodotHub.Local;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    host.ConfigureServices(services =>
                    {
                        services.AddSingleton<Constants>();
                        services.AddTransient<InstallationManager>();
                    });
                    host.UseCommandHandler<RunCommand, RunCommand.CommandHandler>();
                    host.UseCommandHandler<ListCommand, ListCommand.CommandHandler>();
                    host.UseCommandHandler<InstallCommand, InstallCommand.CommandHandler>();
                    host.UseCommandHandler<UninstallCommand, UninstallCommand.CommandHandler>();

                    host.UseCommandHandler<CreateGodotVersionFileCommand, CreateGodotVersionFileCommand.CommandHandler>();
                    host.UseCommandHandler<RegisterCommand, RegisterCommand.CommandHanlder>();
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
                new CreateGodotVersionFileCommand(),
                new RegisterCommand(),
                new UnregisterCommand(),
            };

            rootCommand.Description = "Godot installer and version manager";
            return new CommandLineBuilder(rootCommand);
        }
    }
}
