namespace GodotHub.Core
{
    [Serializable]
    public class UnsupportedArchitectureException : Exception
    {
        public UnsupportedArchitectureException() : base("Cannot determine architecture") { }

        public UnsupportedArchitectureException(string fileName) : base($"Cannot determine architecture for package {fileName}") { }

        public UnsupportedArchitectureException(string fileName, Exception inner) : base($"Cannot determine architecture for package {fileName}", inner) { }

        protected UnsupportedArchitectureException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
