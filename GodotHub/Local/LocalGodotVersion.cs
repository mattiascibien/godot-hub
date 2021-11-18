using GodotHub.Core;
using GodotHub.Online;

namespace GodotHub.Local
{
    public class LocalGodotVersion : GodotVersion
    {
        public override Version Version { get; }

        public override string? PostFix { get; }

        public override bool HasMono { get; }

        public bool IsStable { get; }

        public string InstallationPath { get; }

        public bool IsExternal { get;}

        public LocalGodotVersion(string path)
        {
            InstallationPath = path;
            HasMono = Directory.Exists(Path.Combine(InstallationPath, "GodotSharp"));
            var versionString = Path.GetFileName(path).Split("-");
            Version = Version.Parse(versionString[0]);
            PostFix = versionString.Length <= 1 || versionString[1] == "mono" ? null : versionString[1];
            IsStable = string.IsNullOrEmpty(PostFix);
            IsExternal = LinkCreator.IsLink(InstallationPath);
        }

        public string? GetEditorPath()
        {
            // Get the first file in the installation path directory
            return Directory.EnumerateFiles(InstallationPath).FirstOrDefault();
        }


        public override string ToString()
        {
            string version = Version.ToString();
            if (!IsStable)
                version += $"-{PostFix}";

            if (HasMono)
            {
                return $"{version}-mono";
            }

            return version;
        }

        public bool Equals(OnlineGodotVersion? other)
        {
            if (other == null)
                return false;

            if (Version != other.Version)
                return false;

            if (PostFix != other.PostFix)
                return false;

            if (HasMono != other.HasMono)
                return false;

            return true;
        }
    }
}
