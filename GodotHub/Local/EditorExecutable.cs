using GodotHub.Core;
using Mono.Unix;
using System.Runtime.InteropServices;

namespace GodotHub.Local
{
    public class EditorExecutable : IOsSpecific
    {
        private static readonly Dictionary<string, int> Priorities = new Dictionary<string, int>()
        {
            { ".tools.", 1000 },
            { ".opt.", 900 },
        };

        public GodotOperatingSystem OperatingSystem { get; }

        public Architecture Architecture { get; }

        public bool IsMono { get; }

        public string EditorPath { get; }

        public int Priority { get; }

        public EditorExecutable(string path)
        {
            EditorPath = path;

            string fileName = Path.GetFileName(EditorPath);
            string extension = Path.GetExtension(EditorPath);

            IsMono = fileName.Contains("mono"); // this is needed for 

            if (extension == ".exe")
            {
                OperatingSystem = GodotOperatingSystem.Windows;
            }
            else
            {
                if (fileName.Contains("headless"))
                    OperatingSystem = GodotOperatingSystem.LinuxHeadless;
                else if (fileName.Contains("server"))
                    OperatingSystem = GodotOperatingSystem.LinuxServer;
                else if (fileName.Contains("x11") || fileName.Contains("linuxbsd"))
                    OperatingSystem = GodotOperatingSystem.X11;
                else if (fileName.Contains("osx"))
                    OperatingSystem = GodotOperatingSystem.OSX;
            }

            if (fileName.Contains("64") || fileName.Contains("universal"))
                Architecture = Architecture.X64;
            else
                Architecture = Architecture.X86;

            Priority = Priorities.Where(p => fileName.Contains(p.Key)).Sum(p => p.Value);
        }

        public static bool IsEditorExecutable(string path)
        {
            if (System.OperatingSystem.IsWindows())
                return IsEditorExecutableWindows(path);
            else
                return IsEditorExecutableUnix(path);
        }

        private static bool IsEditorExecutableUnix(string path)
        {
            var unixFileInfo = new UnixFileInfo(path);

            return (unixFileInfo.FileAccessPermissions & FileAccessPermissions.UserExecute) != 0
                || (unixFileInfo.FileAccessPermissions & FileAccessPermissions.GroupExecute) != 0
                || (unixFileInfo.FileAccessPermissions & FileAccessPermissions.GroupExecute) != 0;
        }

        private static bool IsEditorExecutableWindows(string path) => Path.GetExtension(path) == ".exe";
    }
}
