using System.Diagnostics;

namespace GodotHub.Core
{
    public class LinkCreator : ILinkCreator
    {
        public void CreateFolderLink(string currentDirectory, string linkName, string target)
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

        public void DeleteFolderLink(string currentDirectory, string folderLink)
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
    }
}