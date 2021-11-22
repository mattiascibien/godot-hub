namespace GodotHub.Core
{
    public class Constants
    {
        public readonly string BaseDirectory;

        public readonly string InstallationDirectory;

        public readonly string DownloadsDirectory;

        public const string VERSION_FILE_NAME = ".godot-version";

        public Constants()
        {
            BaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".godot-hub");
            InstallationDirectory = Path.Combine(BaseDirectory, "installations");
            DownloadsDirectory = Path.Combine(BaseDirectory, "downloads");

            if (!Directory.Exists(InstallationDirectory))
                Directory.CreateDirectory(InstallationDirectory);
        }
    }
}
