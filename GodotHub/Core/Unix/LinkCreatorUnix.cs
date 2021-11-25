using System.Diagnostics;

namespace GodotHub.Core.Unix
{
    internal class LinkCreatorUnix : ILinkCreator
    {
        public void CreateFolderLink(string currentDirectory, string linkName, string target)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "ln",
                Arguments = $"-s \"{ target }\" \"{ Path.Combine(currentDirectory, linkName) }\"",
                UseShellExecute = true
            });
        }

        public void DeleteFolderLink(string currentDirectory, string folderLink)
        {
            string path = Path.Combine(currentDirectory, folderLink);
            File.Delete(path);
        }
    }
}
