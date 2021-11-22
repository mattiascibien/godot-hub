using GodotHub.Local;
using GodotHub.Online;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace GodotHub.Commands
{
    public class ListCommand : Command
    {
        public ListCommand() : base("list", "Lists installed (and available) Godot versions")
        {
            var installedOption = new Option<bool>("--online", () => false, "List also the available Godot versions");
            installedOption.AddAlias("-o");
            Add(installedOption);
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly IOnlineRepository _onlineRepository;
            private readonly InstallationManager _installationManager;

            public CommandHandler(InstallationManager installationManager, IOnlineRepository onlineRepository)
            {
                _onlineRepository = onlineRepository;
                _installationManager = installationManager;
            }

            public bool Online { get; set; }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                await ListInstalledVersions().ConfigureAwait(false);

                if (Online)
                {
                    Console.WriteLine("");
                    await ListOnlineVersions().ConfigureAwait(false);
                }

                return 0;
            }

            private async Task ListInstalledVersions()
            {
                await Task.Run(() =>
                {
                    var installedVersions = _installationManager.GetInstalledVersions();

                    Console.WriteLine("Installed Godot Versions\n");
                    foreach (var item in installedVersions)
                    {
                        if (item.IsExternal)
                            Console.WriteLine($" - {item} (external)");
                        else
                            Console.WriteLine($" - {item}");
                    }
                }).ConfigureAwait(false);
            }

            private async Task ListOnlineVersions()
            {
                Console.WriteLine("Available Godot Versions\n");
                await foreach (var item in _onlineRepository.GetVersionsAsync())
                {
                    Console.WriteLine($" - {item} (mono available = {item.HasMono})");
                }
            }
        }
    }
}