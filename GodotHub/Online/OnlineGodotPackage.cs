using GodotHub.Core;
using System.Runtime.InteropServices;

namespace GodotHub.Online
{

    public class OnlineGodotPackage : IOsSpecific
    {
        public bool IsMono { get; }

        public string FileName { get; }

        public Uri DownloadUrl { get; }

        public GodotOperatingSystem OperatingSystem {  get; }

        public Architecture Architecture { get; }

        public OnlineGodotPackage(string url) : this(new Uri(url)) { }

        public OnlineGodotPackage(Uri url)
        {
            DownloadUrl = url;
            IsMono = url.ToString().Contains("mono");
            FileName = Path.GetFileName(url.AbsolutePath);

            if (FileName.Contains("win"))
            {
                OperatingSystem = GodotOperatingSystem.Windows;
            }
            else if (FileName.Contains("x11"))
            {
                OperatingSystem = GodotOperatingSystem.X11;
            }
            else if (FileName.Contains("osx"))
            {
                OperatingSystem = GodotOperatingSystem.OSX;
            }
            else if (FileName.Contains("server"))
            {
                OperatingSystem = GodotOperatingSystem.LinuxServer;
            }
            else if (FileName.Contains("headless"))
            {
                OperatingSystem = GodotOperatingSystem.LinuxHeadless;
            }

            if (FileName.Contains("32"))
            {
                Architecture = Architecture.X86;
            }
            else if (FileName.Contains("64"))
            {
                Architecture = Architecture.X64;
            }
            else
            {
                // TODO: exception
            }
        }
    }
}