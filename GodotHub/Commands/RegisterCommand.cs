using GodotHub.Core;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace GodotHub.Commands
{
    internal class RegisterCommand : Command
    {
        public RegisterCommand() : base("register", "register an external godot installation")
        {
            Add(new Argument<string>("customversion", "the custom version to use (i.e. X.Y-dev"));
            Add(new Argument<string>("path", "the path to the godot installation"));
        }

        public class CommandHanlder : ICommandHandler
        {
            private readonly GodotHubPaths _constants;
            private readonly ILinkCreator _linkCreator;

            public string CustomVersion { get; set; } = "";

            public string Path { get; set; } = "";

            public CommandHanlder(GodotHubPaths constants, ILinkCreator linkCreator)
            {
                _constants = constants;
                _linkCreator = linkCreator;
            }

            public Task<int> InvokeAsync(InvocationContext context)
            {
                _linkCreator.CreateFolderLink(_constants.InstallationDirectory, CustomVersion, Path);
                Console.WriteLine($"Registered {Path} as {CustomVersion}");
                return Task.FromResult(0);
            }
        }
    }
}
