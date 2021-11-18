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

            Handler = CommandHandler.Create<bool>((online) => ListVersions(online));
        }

        private static async Task ListVersions(bool includeOnline)
        {
            await ListInstalledVersions().ConfigureAwait(false);

            if (includeOnline)
            {
                Console.WriteLine("");
                await ListOnlineVersions().ConfigureAwait(false);
            }
        }

        private static async Task ListInstalledVersions()
        {
            await Task.Run(() =>
            {
                var installationsManager = new InstallationManager(Constants.InstallationDirectory);

                var installedVersions = installationsManager.GetInstalledVersions();

                Console.WriteLine("Installed Godot Versions\n");
                foreach (var item in installedVersions)
                {
                    if(item.IsExternal)
                        Console.WriteLine($" - {item} (external)");
                    else
                        Console.WriteLine($" - {item}");
                }
            }).ConfigureAwait(false);
        }

        private static async Task ListOnlineVersions()
        {
            IOnlineRepository onlineRepository = new GithubVersionOnlineRepository();

            Console.WriteLine("Available Godot Versions\n");
            await foreach (var item in onlineRepository.GetVersionsAsync())
            {
                Console.WriteLine($" - {item} (mono available = {item.HasMono})");
            }
        }
    }
}