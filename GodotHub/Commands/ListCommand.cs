using GodotHub.Local;
using GodotHub.Online;
using GodotHub.Resources;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace GodotHub.Commands
{
    public class ListCommand : Command
    {
        public ListCommand() : base("list", Strings.ListCommandDescription)
        {
            var installedOption = new Option<bool>("--online", () => false, Strings.ListCommandOnlineOptionDescription);
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

                if (!Online)
                    return 0;

                Console.WriteLine("");
                await ListOnlineVersions().ConfigureAwait(false);

                return 0;
            }

            private async Task ListInstalledVersions()
            {
                await Task.Run(() =>
                {
                    var installedVersions = _installationManager.GetInstalledVersions();

                    Console.WriteLine(Strings.ListCommandInstalledVersionsHeader);
                    foreach (var item in installedVersions)
                    {
                        Console.WriteLine(
                            item.IsExternal
                                ? Strings.ListCommandExternalVersionFormat
                                : Strings.ListCommandLocalVersionFormat, item);
                    }
                }).ConfigureAwait(false);
            }

            private async Task ListOnlineVersions()
            {
                Console.WriteLine(Strings.ListCommandOnlineVersionsHeader);
                await foreach (var item in _onlineRepository.GetVersionsAsync())
                {
                    Console.WriteLine(Strings.ListCommandOnlineVersionFormat, item, item.HasMono);
                }
            }
        }
    }
}