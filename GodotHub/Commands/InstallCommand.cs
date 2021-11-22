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

            Handler = CommandHandler.Create<string, bool, bool, bool>(async (version, force, mono, unstable) =>
            {
                IOnlineRepository onlineRepository = new GithubVersionOnlineRepository();
                Console.WriteLine($"Checking availability of {version} (mono = {mono})");

                var installManager = new InstallationManager(Constants.InstallationDirectory);

                // TODO: force

                OnlineGodotVersion? versionToDownload = null;

                if (version == "latest")
                {
                    versionToDownload = await onlineRepository.GetLatestVersionAsync(unstable).ConfigureAwait(false);

                    Console.WriteLine(string.Format("Latest {0} version is {1}", unstable ? "unstable" : "stable", versionToDownload));
                }
                else
                {
                    versionToDownload = await onlineRepository.GetVersionAsync(version).ConfigureAwait(false);
                }

                if (versionToDownload == null)
                {
                    Console.WriteLine($"Version {version} was not found on the repository");
                    return;
                }

                if (mono && !versionToDownload.HasMono)
                {
                    Console.WriteLine($"Version {version} does not have a package with mono");
                    return;
                }

                (var osPlatform, var architecture) = CurrentOS.GetOsInfo();
                var packageToDownload = versionToDownload.GetPackageForSystem(osPlatform, architecture, mono);

                if (packageToDownload == null)
                {
                    Console.WriteLine($"Cannot find a package for {osPlatform}-{architecture}");
                    return;
                }

                await DownloadAndExtract(installManager, version, mono, packageToDownload).ConfigureAwait(false);

                Console.WriteLine($"Version {version} (mono = ({mono}) installed");
            });
        }

        private static async Task DownloadAndExtract(InstallationManager installManager, string version, bool mono, OnlineGodotPackage packageToDownload)
        {
            using var fileDonwloader = new FileDownloader(packageToDownload.DownloadUrl.ToString());

            using var progressbar = new ProgressBar(10000, "Downloading");
            var outFile = await fileDonwloader.DownloadFileAsync(Constants.DownloadsDirectory, progressbar.AsProgress<float>()).ConfigureAwait(false);

            await installManager.InstallPackageAsync(version, outFile, mono).ConfigureAwait(false);
        }
    }
}