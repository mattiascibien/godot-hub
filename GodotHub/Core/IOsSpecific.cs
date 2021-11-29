using System.Runtime.InteropServices;

namespace GodotHub.Core
{
    public interface IOsSpecific
    {
        GodotOperatingSystem OperatingSystem { get; }

        Architecture Architecture { get; }
    }

    public static class OsSpecificExtensions
    {
        public static bool IsSupported(this IOsSpecific osSpecific, OSPlatform osPlatform, Architecture architecture)
        {
            if (osSpecific.Architecture != architecture)
                return false;

            return osSpecific.OperatingSystem switch
            {
                GodotOperatingSystem.Windows => osPlatform == OSPlatform.Windows,
                GodotOperatingSystem.OSX => osPlatform == OSPlatform.OSX,
                GodotOperatingSystem.X11 => osPlatform == OSPlatform.Linux || osPlatform == OSPlatform.FreeBSD,
                GodotOperatingSystem.LinuxHeadless or GodotOperatingSystem.LinuxServer => osPlatform == OSPlatform.Linux,
                _ => false,
            };
        }
    }
}
