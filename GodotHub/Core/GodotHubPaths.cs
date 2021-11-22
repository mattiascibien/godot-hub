using Microsoft.Extensions.Configuration;

namespace GodotHub.Core
{
    public class GodotHubPaths
    {
        public readonly string InstallationDirectory;

        public readonly string DownloadsDirectory;

        public const string VERSION_FILE_NAME = ".godot-version";

        public GodotHubPaths(IConfiguration config)
        {
            InstallationDirectory = config["installation_directory"];
            DownloadsDirectory = config["donwloads_directory"];

            if (!Directory.Exists(InstallationDirectory))
                Directory.CreateDirectory(InstallationDirectory);

            if (!Directory.Exists(DownloadsDirectory))
                Directory.CreateDirectory(DownloadsDirectory);
        }
    }
}
