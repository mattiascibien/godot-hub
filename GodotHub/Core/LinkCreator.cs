using System.Diagnostics;

namespace GodotHub.Core
{
    public static class LinkCreator
    {
        public static void CreateFolderLink(string currentDirectory, string linkName, string target)
        {
            if(OperatingSystem.IsWindows())
            {
                UACHelper.UACHelper.StartElevated(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = string.Format("/c mklink /d {0} \"{1}\"", Path.Combine(currentDirectory, linkName), target),
                    UseShellExecute = true
                });
            }
            else
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "ln",
                    Arguments = $"\"{ Path.Combine(currentDirectory, linkName) }\"" +
                        $" \"{ target }\"" +
                        $" -s",
                    UseShellExecute = true
                });
            }
        }

        public static void DeleteFolderLink(string currentDirectory, string folderLink)
        {
            if (OperatingSystem.IsWindows())
            {
                UACHelper.UACHelper.StartElevated(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = string.Format("/c del \"{0}\"", Path.Combine(currentDirectory, folderLink)),
                    UseShellExecute = true
                });
            }
            else
            {
                File.Delete(Path.Combine(currentDirectory, folderLink));
            }
        }

        public static bool IsLink(string path)
        {
            DirectoryInfo pathInfo = new DirectoryInfo(path);
            return (pathInfo.Attributes & FileAttributes.ReparsePoint) != 0;
        }
    }
}