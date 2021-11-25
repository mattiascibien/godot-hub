using GodotHub.Core;
using GodotHub.Local;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace GodotHub.Commands.Deprecated
{
    [Obsolete($"The command is not valid. Use the new {nameof(CreateLocalConfigurationCommand)}")]
    public class CreateGodotVersionFileCommand : Command
    {
        public CreateGodotVersionFileCommand() : base("create-godot-version", $"creates the {GodotHubPaths.VersionFileName} containing the version to use")
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
                if (File.Exists(GodotHubPaths.VersionFileName))
                {
                    Console.WriteLine($"A {GodotHubPaths.VersionFileName} already exists in the current directory");
                    return 1;
                }

                var installedVersion = _installationManager.FindInstalledVersion(Version);

                if (installedVersion != null)
                {
                    await File.WriteAllTextAsync(GodotHubPaths.VersionFileName, Version).ConfigureAwait(false);
                    Console.WriteLine($"{GodotHubPaths.VersionFileName} created.");
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
