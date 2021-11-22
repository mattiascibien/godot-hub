using GodotHub.Core;
using GodotHub.Local;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotHub.Commands
{
    public class CreateGodotVersionFileCommand : Command
    {
        public CreateGodotVersionFileCommand() : base("create-godot-version", $"creates the {GodotHubPaths.VERSION_FILE_NAME} containing the version to use")
        {
            Add(new Argument<string>("version", "the version to use"));
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly InstallationManager _installationManager;

            public string Version { get; set; } = "";

            public CommandHandler(InstallationManager installationManager)
            {
                _installationManager = installationManager;
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                if (File.Exists(GodotHubPaths.VERSION_FILE_NAME))
                {
                    Console.WriteLine($"A {GodotHubPaths.VERSION_FILE_NAME} already exists in the current directory");
                    return 1;
                }

                var installedVersion = _installationManager.FindInstalledVersion(Version);

                if (installedVersion != null)
                {
                    await File.WriteAllTextAsync(GodotHubPaths.VERSION_FILE_NAME, Version).ConfigureAwait(false);
                    Console.WriteLine($"{GodotHubPaths.VERSION_FILE_NAME} created.");
                    return 0;
                }
                else
                {
                    Console.WriteLine($"Version {Version} is not installed. Install it with 'install {Version}'");
                    return 1;
                }
            }
        }
    }
}
