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

            var fromGithubOption = new Option<bool>("--github", () => true, "Search from github");
            Add(fromGithubOption);

            var fromTuxfamilyOption = new Option<bool>("--tuxfamily", () => false, "Search from tuxfamily");
            Add(fromTuxfamilyOption);

            Handler = CommandHandler.Create<bool, bool, bool>((online, tuxfamily, github) => ListVersions(online, tuxfamily, github));
        }

        private static async Task ListVersions(bool includeOnline, bool tuxfamily, bool github)
        {
            await ListInstalledVersions().ConfigureAwait(false);

            if (includeOnline)
            {
                Console.WriteLine("");

                if(tuxfamily)
                    await ListOnlineVersions(new TuxFamilyOnlineRepository()).ConfigureAwait(false);

                Console.WriteLine("");

                if (github)
                    await ListOnlineVersions(new GithubVersionOnlineRepository()).ConfigureAwait(false);
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

        private static async Task ListOnlineVersions(IOnlineRepository onlineRepository)
        {
            // TODO: do not use reflection
            Console.WriteLine($"Available Godot Versions from {onlineRepository.GetType().Name}\n");
            await foreach (var item in onlineRepository.GetVersionsAsync())
            {
                Console.WriteLine($" - {item} (mono available = {item.HasMono})");
            }
        }
    }
}