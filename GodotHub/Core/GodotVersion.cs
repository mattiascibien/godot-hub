namespace GodotHub.Core
{
    public abstract class GodotVersion : IEquatable<GodotVersion>
    {
        public abstract Version Version { get; }

        public abstract string? PostFix { get; }

        public abstract bool HasMono { get; }

        public bool Equals(GodotVersion? other)
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

        public override bool Equals(object? obj) => Equals(obj as GodotVersion);

        public override int GetHashCode() => HashCode.Combine(Version, PostFix, HasMono);
    }
}
