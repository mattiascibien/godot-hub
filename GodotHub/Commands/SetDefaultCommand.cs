using System.CommandLine;
using System.CommandLine.Invocation;

namespace GodotHub.Commands
{
    public class SetDefaultCommand : Command
    {
        public SetDefaultCommand() : base("set-default", "Set the default version to use when opening projects")
        {
            Add(new Argument<string>("version", "The version to set as default"));

            Handler = CommandHandler.Create<string>((version) => Console.WriteLine($"Setting version {version} as default"));
        }
    }
}