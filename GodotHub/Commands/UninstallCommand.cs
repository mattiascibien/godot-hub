using GodotHub.Local;
using System.CommandLine;
using System.CommandLine.Invocation;
using GodotHub.Resources;
using GodotHub.Resources;

namespace GodotHub.Commands
{
    public class UninstallCommand : Command
    {
        public UninstallCommand() : base("uninstall", Strings.UninstallCommandDescription)
        {
            Add(new Argument<string>("version", Strings.UninstallCommandVersionArgumentDescription));
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
                Console.WriteLine(Strings.UninstallCommandUninstallingMessage, Version);
                await _installationManager.UninstallVersionAsync(Version).ConfigureAwait(false);

                Console.WriteLine(Strings.UninstallCommandUninstallCompleteMessage, Version);
                return 0;
            }
        }
    }
}
