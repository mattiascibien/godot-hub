using System.Runtime.InteropServices;

namespace GodotHub.Core
{
    public interface IOsSpecific
    {
        GodotOperatingSystem OperatingSystem { get; }

        Architecture Architecture { get; }
    }

    public static class IOsSpecificExtensions
    {
        public static bool IsSupported(this IOsSpecific osSpecific, OSPlatform osPlatform, Architecture architecture)
        {
            if (osSpecific.Architecture == architecture)
            {
                switch (osSpecific.OperatingSystem)
                {
                    case GodotOperatingSystem.Windows:
                        return osPlatform == OSPlatform.Windows;
                    case GodotOperatingSystem.OSX:
                        return osPlatform == OSPlatform.OSX;
                    case GodotOperatingSystem.X11:
                        return osPlatform == OSPlatform.Linux || osPlatform == OSPlatform.FreeBSD;
                    case GodotOperatingSystem.LinuxHeadless:
                    case GodotOperatingSystem.LinuxServer:
                        return osPlatform == OSPlatform.Linux;
                }
            }

            return false;
        }
    }
}
