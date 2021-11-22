using GodotHub.Core;
using GodotHub.Online;
using System.Runtime.InteropServices;
using Xunit;

namespace GodotHub.Tests.Online
{
    public class GodotPackageTests
    {
        [Fact]
        public void CorrectlyParsesWindowsMonoBeta()
        {
            var mono34beta = "https://downloads.tuxfamily.org/godotengine/3.4/beta6/mono/Godot_v3.4-beta6_mono_win64.zip";

            var godotPackage = new OnlineGodotPackage(mono34beta);

            Assert.True(godotPackage.IsMono);
            Assert.Equal("Godot_v3.4-beta6_mono_win64.zip", godotPackage.FileName);
            Assert.Equal(Architecture.X64, godotPackage.Architecture);
            Assert.Equal(GodotOperatingSystem.Windows, godotPackage.OperatingSystem);
        }

        [Fact]
        public void CorrectlyParsesWindowsNonMonoBeta()
        {
            var nonMono34beta = "https://downloads.tuxfamily.org/godotengine/3.4/beta6/Godot_v3.4-beta6_win64.exe.zip";

            var godotPackage = new OnlineGodotPackage(nonMono34beta);

            Assert.False(godotPackage.IsMono);
            Assert.Equal("Godot_v3.4-beta6_win64.exe.zip", godotPackage.FileName);
            Assert.Equal(Architecture.X64, godotPackage.Architecture);
            Assert.Equal(GodotOperatingSystem.Windows, godotPackage.OperatingSystem);
        }

        [Fact]
        public void CorrectlyParsesLinuxMonoBeta()
        {
            var mono34beta = "https://downloads.tuxfamily.org/godotengine/3.4/beta6/mono/Godot_v3.4-beta6_mono_x11_64.zip";

            var godotPackage = new OnlineGodotPackage(mono34beta);

            Assert.True(godotPackage.IsMono);
            Assert.Equal("Godot_v3.4-beta6_mono_x11_64.zip", godotPackage.FileName);
            Assert.Equal(Architecture.X64, godotPackage.Architecture);
            Assert.Equal(GodotOperatingSystem.X11, godotPackage.OperatingSystem);
        }

        [Fact]
        public void CorrectlyParsesLinuxNonMonoBeta()
        {
            var nonMono34beta = "https://downloads.tuxfamily.org/godotengine/3.4/beta6/Godot_v3.4-beta6_x11.64.zip";

            var godotPackage = new OnlineGodotPackage(nonMono34beta);

            Assert.False(godotPackage.IsMono);
            Assert.Equal("Godot_v3.4-beta6_x11.64.zip", godotPackage.FileName);
            Assert.Equal(Architecture.X64, godotPackage.Architecture);
            Assert.Equal(GodotOperatingSystem.X11, godotPackage.OperatingSystem);
        }

        [Fact]
        public void IsCorrectlyAvailableForLinux64()
        {
            var nonMono34beta = "https://downloads.tuxfamily.org/godotengine/3.4/beta6/Godot_v3.4-beta6_x11.64.zip";

            var godotPackage = new OnlineGodotPackage(nonMono34beta);

            Assert.True(godotPackage.IsSupported(OSPlatform.Linux, Architecture.X64));
            Assert.False(godotPackage.IsSupported(OSPlatform.Linux, Architecture.X86));
            Assert.False(godotPackage.IsSupported(OSPlatform.Windows, Architecture.X64));
            Assert.False(godotPackage.IsSupported(OSPlatform.OSX, Architecture.X64));
        }

        [Fact]
        public void IsCorrectlyAvailableForLinux32()
        {
            var nonMono34beta = "https://downloads.tuxfamily.org/godotengine/3.4/beta6/Godot_v3.4-beta6_x11.32.zip";

            var godotPackage = new OnlineGodotPackage(nonMono34beta);

            Assert.True(godotPackage.IsSupported(OSPlatform.Linux, Architecture.X86));
            Assert.False(godotPackage.IsSupported(OSPlatform.Linux, Architecture.X64));
            Assert.False(godotPackage.IsSupported(OSPlatform.Windows, Architecture.X86));
            Assert.False(godotPackage.IsSupported(OSPlatform.OSX, Architecture.X86));
        }

        [Fact]
        public void IsCorrectlyAvailableForWindows64()
        {
            var nonMono34beta = "https://downloads.tuxfamily.org/godotengine/3.4/beta6/Godot_v3.4-beta6_win64.exe.zip";

            var godotPackage = new OnlineGodotPackage(nonMono34beta);

            Assert.True(godotPackage.IsSupported(OSPlatform.Windows, Architecture.X64));
            Assert.False(godotPackage.IsSupported(OSPlatform.Windows, Architecture.X86));
            Assert.False(godotPackage.IsSupported(OSPlatform.Linux, Architecture.X64));
            Assert.False(godotPackage.IsSupported(OSPlatform.OSX, Architecture.X64));
        }

        [Fact]
        public void IsCorrectlyAvailableForWindows32()
        {
            var nonMono34beta = "https://downloads.tuxfamily.org/godotengine/3.4/beta6/Godot_v3.4-beta6_win32.exe.zip";

            var godotPackage = new OnlineGodotPackage(nonMono34beta);

            Assert.True(godotPackage.IsSupported(OSPlatform.Windows, Architecture.X86));
            Assert.False(godotPackage.IsSupported(OSPlatform.Windows, Architecture.X64));
            Assert.False(godotPackage.IsSupported(OSPlatform.Linux, Architecture.X86));
            Assert.False(godotPackage.IsSupported(OSPlatform.OSX, Architecture.X86));
        }
    }
}