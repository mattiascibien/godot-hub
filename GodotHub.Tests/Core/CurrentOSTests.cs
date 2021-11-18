using GodotHub.Core;
using System.Runtime.InteropServices;
using Xunit;

namespace GodotHub.Tests.Core
{
    public class CurrentOSTests
    { 
        [SkippableFact]
        public void ReturnsCorrectOSWindows()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            (var os, _) = CurrentOS.GetOsInfo();

            Assert.Equal(OSPlatform.Windows, os);
        }

        [SkippableFact]
        public void ReturnsCorrectOSLinux()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Linux));

            (var os, _) = CurrentOS.GetOsInfo();

            Assert.Equal(OSPlatform.Linux, os);
        }

        [SkippableFact]
        public void ReturnsCorrectOSOSX()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.OSX));

            (var os, _) = CurrentOS.GetOsInfo();

            Assert.Equal(OSPlatform.OSX, os);
        }
    }
}
