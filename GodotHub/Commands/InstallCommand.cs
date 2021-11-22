using GodotHub.Core;
using GodotHub.Local;
using GodotHub.Online;
using ShellProgressBar;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Runtime.InteropServices;

namespace GodotHub.Commands
{
    public class InstallCommand : Command
    {
        public InstallCommand() : base("install", "Install manager for godot versions")
        {
            Add(new Argument<string>("version", () => "latest", "The version to install"));

            var includeUnstableOption = new Option<bool>("--unstable", () => false, "Also check for unstable versions");
            includeUnstableOption.AddAlias("-u");
            Add(includeUnstableOption);

            var forceOption = new Option<bool>("--force", () => false, "Force a reinstall even if the version is already installed");
            forceOption.AddAlias("-f");
            Add(forceOption);

            var monoOption = new Option<bool>("--mono", () => false, "Force a reinstall even if the version is already installed");
            monoOption.AddAlias("-m");
            Add(monoOption);
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly Constants _constants;
            private readonly InstallationManager _installationManager;
            private readonly IOnlineRepository _onlineRepository;

            public string Version { get; set; } = "";
            public bool Unstable { get; set; }
            public bool Mono { get; set; }

            public CommandHandler(Constants constants, InstallationManager installationManager, IOnlineRepository onlineRepository)
            {
                _constants = constants;
                _installationManager = installationManager;
                _onlineRepository = onlineRepository;
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                Console.WriteLine($"Checking availability of {Version} (mono = {Mono})");

                OnlineGodotVersion? versionToDownload;
                if (Version == "latest")
                {
                    versionToDownload = await _onlineRepository.GetLatestVersionAsync(Unstable).ConfigureAwait(false);

                    Console.WriteLine(string.Format("Latest {0} version is {1}", Unstable ? "unstable" : "stable", versionToDownload));
                }
                else
                {
                    versionToDownload = await _onlineRepository.GetVersionAsync(Version).ConfigureAwait(false);
                }

                if (versionToDownload == null)
                {
                    Console.WriteLine($"Version {Version} was not found on the repository");
                    return 1;
                }

                if (Mono && !versionToDownload.HasMono)
                {
                    Console.WriteLine($"Version {Version} does not have a package with mono");
                    return 1;
                }

                (var osPlatform, var architecture) = CurrentOS.GetOsInfo();
                var packageToDownload = versionToDownload.GetPackageForSystem(osPlatform, architecture, Mono);

                if (packageToDownload == null)
                {
                    Console.WriteLine($"Cannot find a package for {osPlatform}-{architecture}");
                    return 1;
                }

                await DownloadAndExtract(packageToDownload).ConfigureAwait(false);

                Console.WriteLine($"Version {Version} (mono = ({Mono}) installed");
                return 0;
            }

            private async Task DownloadAndExtract(OnlineGodotPackage packageToDownload)
            {
                using var fileDonwloader = new FileDownloader(packageToDownload.DownloadUrl.ToString());

                using var progressbar = new ProgressBar(10000, "Downloading");
                var outFile = await fileDonwloader.DownloadFileAsync(_constants.DownloadsDirectory, progressbar.AsProgress<float>()).ConfigureAwait(false);

                await _installationManager.InstallPackageAsync(Version, outFile, Mono).ConfigureAwait(false);
            }
        }
    }
}