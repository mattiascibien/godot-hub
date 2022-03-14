using GodotHub.Core;
using System.CommandLine;
using System.CommandLine.Invocation;
using GodotHub.Resources;

namespace GodotHub.Commands
{
    internal class RegisterCommand : Command
    {
        public RegisterCommand() : base("register", Strings.RegisterCommandDescription)
        {
            Add(new Argument<string>("customversion", Strings.RegisterCommandCustomVersionArgumentDescription));
            Add(new Argument<string>("path", Strings.RegisterCommandPathArgumentDescription));
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly GodotHubPaths _constants;
            private readonly ILinkCreator _linkCreator;

            public string CustomVersion { get; set; } = "";

            public string Path { get; set; } = "";

            public CommandHandler(GodotHubPaths constants, ILinkCreator linkCreator)
            {
                _constants = constants;
                _linkCreator = linkCreator;
            }

            public Task<int> InvokeAsync(InvocationContext context)
            {
                _linkCreator.CreateFolderLink(_constants.InstallationDirectory, CustomVersion, Path);
                Console.WriteLine(Strings.RegisterCommandCompletedMessage, Path, CustomVersion);
                return Task.FromResult(0);
            }
        }
    }
}
