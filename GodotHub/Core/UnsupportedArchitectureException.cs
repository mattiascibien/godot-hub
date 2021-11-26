using GodotHub.Resources;

namespace GodotHub.Core
{
    [Serializable]
    public class UnsupportedArchitectureException : Exception
    {
        public UnsupportedArchitectureException() : base(Strings.UnsupportedArchitectureExceptionDefaultMessage) { }

        public UnsupportedArchitectureException(string fileName) : base(string.Format(Strings.UnsupportedArchitectureExceptionMessageWithParameter, fileName)) { }

        public UnsupportedArchitectureException(string fileName, Exception inner) : base(string.Format(Strings.UnsupportedArchitectureExceptionMessageWithParameter, fileName), inner) { }

        protected UnsupportedArchitectureException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
