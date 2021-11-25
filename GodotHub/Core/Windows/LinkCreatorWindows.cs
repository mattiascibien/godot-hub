using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotHub.Core.Windows
{
    internal class LinkCreatorWindows : ILinkCreator
    {
        public void CreateFolderLink(string currentDirectory, string linkName, string target)
        {
            UACHelper.UACHelper.StartElevated(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = string.Format("/c mklink /d {0} \"{1}\"", Path.Combine(currentDirectory, linkName), target),
                UseShellExecute = true
            });
        }

        public void DeleteFolderLink(string currentDirectory, string folderLink)
        {
            string path = Path.Combine(currentDirectory, folderLink);
            UACHelper.UACHelper.StartElevated(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = string.Format("/c rmdir \"{0}\"", path),
                UseShellExecute = true
            });
        }
    }
}
