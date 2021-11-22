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
                    Arguments = $"-s \"{ target }\" \"{ Path.Combine(currentDirectory, linkName) }\"",
                    UseShellExecute = true
                });
            }
        }

        public static void DeleteFolderLink(string currentDirectory, string folderLink)
        {
            string path = Path.Combine(currentDirectory, folderLink);
            if (OperatingSystem.IsWindows())
            {
                UACHelper.UACHelper.StartElevated(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = string.Format("/c rmdir \"{0}\"", path),
                    UseShellExecute = true
                });
            }
            else
            {
                File.Delete(path);
            }
        }

        public static bool IsLink(string path)
        {
            DirectoryInfo pathInfo = new DirectoryInfo(path);
            return (pathInfo.Attributes & FileAttributes.ReparsePoint) != 0;
        }
    }
}