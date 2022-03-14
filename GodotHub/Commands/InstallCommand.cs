using GodotHub.Core;
using GodotHub.Local;
using GodotHub.Online;
using GodotHub.Resources;
using ShellProgressBar;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace GodotHub.Commands
{
    public class InstallCommand : Command
    {
        public InstallCommand() : base("install", Strings.InstallCommandDescription)
        {
            Add(new Argument<string>("version", () => "latest", Strings.InstallCommandVersionArgumentDescription));

            var includeUnstableOption = new Option<bool>("--unstable", () => false, Strings.InstallCommandUnstableOptionDescription);
            includeUnstableOption.AddAlias("-u");
            Add(includeUnstableOption);

            var monoOption = new Option<bool>("--mono", () => false, Strings.InstallCommandMonoOptionDescription);
            monoOption.AddAlias("-m");
            Add(monoOption);

            var headlessOption = new Option<bool>("--headless", () => false, Strings.InstallCommandHeadlessOptionDescription);
            Add(headlessOption);
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly GodotHubPaths _constants;
            private readonly InstallationManager _installationManager;
            private readonly IOnlineRepository _onlineRepository;

            public string Version { get; set; } = "";

            public bool Unstable { get; set; }

            public bool Mono { get; set; }

            public bool Headless { get; set; }

            public CommandHandler(GodotHubPaths constants, InstallationManager installationManager, IOnlineRepository onlineRepository)
            {
                _constants = constants;
                _installationManager = installationManager;
                _onlineRepository = onlineRepository;
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                Console.WriteLine(Strings.InstallCommandCheckAvailabilityMessage, Version, Mono, Headless);

                OnlineGodotVersion? versionToDownload;
                if (Version == "latest")
                {
                    versionToDownload = await _onlineRepository.GetLatestVersionAsync(Unstable).ConfigureAwait(false);

                    Console.WriteLine(Strings.InstallCommandLatestVersionMessage, Unstable ? "unstable" : "stable", versionToDownload);
                }
                else
                {
                    versionToDownload = await _onlineRepository.GetVersionAsync(Version).ConfigureAwait(false);
                }

                if (versionToDownload == null)
                {
                    Console.WriteLine(Strings.InstallCommandVersionNotFoundMessage, Version);
                    return 1;
                }

                if (Mono && !versionToDownload.HasMono)
                {
                    Console.WriteLine(Strings.InstallCommandMonoNotFoundMessage, Version);
                    return 1;
                }

                var (osPlatform, architecture) = CurrentOS.GetOsInfo();
                var packageToDownload = versionToDownload.GetPackageForSystem(osPlatform, architecture, Mono, Headless);

                if (packageToDownload == null)
                {
                    Console.WriteLine(Strings.InstallCommandPackageNotFoundMessage, osPlatform, architecture);
                    return 1;
                }

                await DownloadAndExtract(packageToDownload).ConfigureAwait(false);

                Console.WriteLine(Strings.InstallCommandInstallationCompleteMessage, Version, Mono, Headless);
                return 0;
            }

            private async Task DownloadAndExtract(OnlineGodotPackage packageToDownload)
            {
                using var fileDownloader = new FileDownloader(packageToDownload.DownloadUrl.ToString());

                using var progressbar = new ProgressBar(10000, Strings.InstallCommandDownloadingMessage);
                var outFile = await fileDownloader.DownloadFileAsync(_constants.DownloadsDirectory, progressbar.AsProgress<float>()).ConfigureAwait(false);

                await _installationManager.InstallPackageAsync(Version, outFile, Mono, Headless).ConfigureAwait(false);
            }
        }
    }
}