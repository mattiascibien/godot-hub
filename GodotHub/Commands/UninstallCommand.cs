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

            Handler = CommandHandler.Create<string>(async (version) =>
            {
                Console.WriteLine($"Uninstalling {version}");
                var installationManager = new InstallationManager(Constants.InstallationDirectory);
                await installationManager.UninstallVersionAsync(version).ConfigureAwait(false);

                Console.WriteLine($"Godot {version} uninstalled.");
            });
        }
    }
}
