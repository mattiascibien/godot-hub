using System.Runtime.InteropServices;

namespace GodotHub.Online
{
    public enum PackageOperatingSystem
    {
        Windows,
        OSX,
        X11
    }

    public class OnlineGodotPackage
    {
        public bool IsMono { get; }

        public string FileName { get; }

        public Uri DownloadUrl { get; }

        public PackageOperatingSystem OperatingSystem;

        public Architecture Architecture;

        public OnlineGodotPackage(string url) : this(new Uri(url)) { }

        public bool IsSupported(OSPlatform osPlatform, Architecture architecture)
        {
            if (Architecture == architecture)
            {
                switch (OperatingSystem)
                {
                    case PackageOperatingSystem.Windows:
                        return osPlatform == OSPlatform.Windows;
                    case PackageOperatingSystem.OSX:
                        return osPlatform == OSPlatform.OSX;
                    case PackageOperatingSystem.X11:
                        return osPlatform == OSPlatform.Linux || osPlatform == OSPlatform.FreeBSD; //TODO: check FreeBSD
                }
            }

            return false;
        }

        public OnlineGodotPackage(Uri url)
        {
            DownloadUrl = url;
            IsMono = url.ToString().Contains("mono");
            FileName = Path.GetFileName(url.AbsolutePath);

            if (FileName.Contains("win"))
            {
                OperatingSystem = PackageOperatingSystem.Windows;
            }
            else if (FileName.Contains("x11"))
            {
                OperatingSystem = PackageOperatingSystem.X11;
            }
            else if (FileName.Contains("osx"))
            {
                OperatingSystem = PackageOperatingSystem.OSX;
            }
            else
            {
                // TODO: exception
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