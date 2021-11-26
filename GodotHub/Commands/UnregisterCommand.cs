using GodotHub.Core;
using System.CommandLine;
using System.CommandLine.Invocation;
using GodotHub.Resources;

namespace GodotHub.Commands
{
    public class UnregisterCommand : Command
    {
        public UnregisterCommand() : base("unregister", Strings.UnregisterCommandDescription)
        {
            Add(new Argument<string>("customversion", Strings.UnregisterCommandCustomVersionArgumentDescription));
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly GodotHubPaths _constants;
            private readonly ILinkCreator _linkCreator;

            public string CustomVersion { get; set; } = "";

            public CommandHandler(GodotHubPaths constants, ILinkCreator linkCreator)
            {
                _constants = constants;
                _linkCreator = linkCreator;
            }

            public Task<int> InvokeAsync(InvocationContext context)
            {
                if (_linkCreator.IsLink(Path.Combine(_constants.InstallationDirectory, CustomVersion)))
                {
                    _linkCreator.DeleteFolderLink(_constants.InstallationDirectory, CustomVersion);
                    Console.WriteLine(Strings.UnregisterCommandUnregisterCompleteMessage, CustomVersion);
                }
                else
                {
                    Console.WriteLine(Strings.UnregisterCommandVersionNotExternalMessage, CustomVersion);
                }
                return Task.FromResult(0);
            }
        }
    }
}
