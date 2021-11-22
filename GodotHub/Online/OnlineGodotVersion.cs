using GodotHub.Core;
using System.Runtime.InteropServices;

namespace GodotHub.Online
{
    public sealed class OnlineGodotVersion : GodotVersion
    {
        public override Version Version { get; }

        public override string? PostFix { get; }

        public override bool HasMono => Packages.Any(pkg => pkg.IsMono);

        public bool IsStable { get; }

        public IList<OnlineGodotPackage> Packages { get; } = new List<OnlineGodotPackage>();

        public OnlineGodotVersion(string versionString)
        {
            var versionSplit = versionString.Split("-", StringSplitOptions.RemoveEmptyEntries);
            Version = Version.Parse(versionSplit[0]);
            PostFix = versionSplit.Length <= 1 || versionSplit[1] == "stable" ? null : versionSplit[1];
            IsStable = string.IsNullOrEmpty(PostFix);
        }

        public OnlineGodotPackage? GetPackageForSystem(OSPlatform osPlatform, Architecture architecture, bool mono)
        {
            return Packages.FirstOrDefault(pkg => pkg.IsSupported(osPlatform, architecture) && pkg.IsMono == mono);
        }

        public override string ToString()
        {
            string version = Version.ToString();
            if (!IsStable)
                version += $"-{PostFix}";

            return version;
        }
    }
}