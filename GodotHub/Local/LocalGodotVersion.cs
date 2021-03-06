using GodotHub.Core;
using GodotHub.Online;
using System.Runtime.InteropServices;

namespace GodotHub.Local
{
    public sealed class LocalGodotVersion : GodotVersion
    {
        public override Version Version { get; }

        public override string? PostFix { get; }

        public override bool HasMono { get; }

        public bool IsStable { get; }

        public string InstallationPath { get; }

        public bool IsExternal { get; }

        public LocalGodotVersion(string path, bool isExternal)
        {
            InstallationPath = path;
            HasMono = Directory.Exists(Path.Combine(InstallationPath, "GodotSharp"));
            var versionString = Path.GetFileName(path).Split("-");
            Version = Version.Parse(versionString[0]);
            PostFix = versionString.Length <= 1 || versionString[1] == "mono" ? null : versionString[1];
            IsStable = string.IsNullOrEmpty(PostFix);
            IsExternal = isExternal;
        }

        public IEnumerable<EditorExecutable> GetSupportedEditorExecutables(OSPlatform osPlatform, Architecture architecture)
        {
            // find all the executables for the platforms
            // and return the one that has the highest priority
            return Directory.EnumerateFiles(InstallationPath)
                .Where(EditorExecutable.IsEditorExecutable)
                .Select(x => new EditorExecutable(x))
                .Where(x => x.IsSupported(osPlatform, architecture))
                .OrderByDescending(x => x.Priority);
        }

        public override string ToString()
        {
            string version = Version.ToString();
            if (!IsStable)
                version += $"-{PostFix}";

            return HasMono ? $"{version}-mono" : version;
        }

        public bool Equals(OnlineGodotVersion? other)
        {
            if (other == null)
                return false;

            if (Version != other.Version)
                return false;

            if (PostFix != other.PostFix)
                return false;

            return HasMono == other.HasMono;
        }
    }
}
