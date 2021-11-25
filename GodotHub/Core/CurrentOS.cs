using System.Runtime.InteropServices;

namespace GodotHub.Core
{
    public static class CurrentOS
    {
        public static (OSPlatform osPlatform, Architecture architecture) GetOsInfo()
        {
            OSPlatform? osPlatform = null;
            var architecture = RuntimeInformation.OSArchitecture;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                osPlatform = OSPlatform.Windows;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                osPlatform = OSPlatform.OSX;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                osPlatform = OSPlatform.Linux;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                osPlatform = OSPlatform.FreeBSD;

            if (osPlatform == null)
                throw new InvalidOperationException("Unsupported OS");

            return (osPlatform.Value, architecture);
        }
    }
}
