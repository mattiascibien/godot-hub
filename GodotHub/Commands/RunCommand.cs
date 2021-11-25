using GodotHub.Core;
using GodotHub.Local;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotHub.Commands
{
    public class RunCommand : Command
    {
        public RunCommand() : base("run", "Lanches Godot in the current directory")
        {
            Option<string?> versionOption = new Option<string?>("--use-version", () => null, "The version to launch");
            versionOption.AddAlias("-u");
            AddOption(versionOption);

            AddArgument(new Argument<string[]>("cmdline", "The command line to pass to godot"));
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly IConfiguration _configuration;
            private readonly InstallationManager _installationManager;

            public string? UseVersion { get; set; }

            public string[] CmdLine { get; set; } = Array.Empty<string>();

            public CommandHandler(InstallationManager installationManager, IConfiguration configuration)
            {
                _configuration = configuration;
                _installationManager = installationManager;
            }

            public Task<int> InvokeAsync(InvocationContext context)
            {
                if (string.IsNullOrEmpty(UseVersion))
                {
                    UseVersion = _configuration["version"];
                }

                int returnValue;
                if (UseVersion != null)
                {
                    returnValue = _installationManager.Launch(UseVersion, CmdLine);
                }
                else
                {
                    Console.WriteLine("Cannot find a version to run. Specify it with -u <version> parameter");
                    returnValue = 1;
                }

                return Task.FromResult(returnValue);
            }
        }
    }
}
