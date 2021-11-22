using GodotHub.Local;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace GodotHub.Commands
{
    public class UninstallCommand : Command
    {
        public UninstallCommand() : base("uninstall", "Uninstall a specific godot version")
        {
            Add(new Argument<string>("version", "The version to install"));
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
                Console.WriteLine($"Uninstalling {Version}");
                await _installationManager.UninstallVersionAsync(Version).ConfigureAwait(false);

                Console.WriteLine($"Godot {Version} uninstalled.");
                return 0;
            }
        }
    }
}
