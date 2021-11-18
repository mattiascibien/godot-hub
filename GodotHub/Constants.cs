namespace GodotHub
{
    internal static class Constants
    {
        public static readonly string BaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".godot-hub");

        public static readonly string InstallationDirectory = Path.Combine(BaseDirectory, "installations");

        public static readonly string DownloadsDirectory = Path.Combine(BaseDirectory, "downloads");

        public const string VERSION_FILE_NAME = ".godot-version";
    }
}
