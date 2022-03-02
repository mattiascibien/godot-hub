using System.Diagnostics;

namespace GodotHub.Core.Windows
{
    internal class LinkCreatorWindows : ILinkCreator
    {
        public void CreateFolderLink(string currentDirectory, string linkName, string target)
        {
            UACHelper.UACHelper.StartElevated(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c mklink /d {Path.Combine(currentDirectory, linkName)} \"{target}\"",
                UseShellExecute = true
            });
        }

        public void DeleteFolderLink(string currentDirectory, string folderLink)
        {
            var path = Path.Combine(currentDirectory, folderLink);
            UACHelper.UACHelper.StartElevated(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c rmdir \"{path}\"",
                UseShellExecute = true
            });
        }
    }
}
