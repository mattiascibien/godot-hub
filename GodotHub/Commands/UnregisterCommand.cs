using GodotHub.Core;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace GodotHub.Commands
{
    public class UnregisterCommand : Command
    {
        public UnregisterCommand() : base("unregister", "unregisters an external godot installation")
        {
            Add(new Argument<string>("customversion", "the custom version to unregister (i.e. X.Y-dev"));
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly Constants _constants;
            private readonly ILinkCreator _linkCreator;

            public string CustomVersion { get; set; } = "";

            public CommandHandler(Constants constants, ILinkCreator linkCreator)
            {
                _constants = constants;
                _linkCreator = linkCreator;
            }

            public Task<int> InvokeAsync(InvocationContext context)
            {
                if (LinkUtils.IsLink(Path.Combine(_constants.InstallationDirectory, CustomVersion)))
                {
                    _linkCreator.DeleteFolderLink(_constants.InstallationDirectory, CustomVersion);
                    Console.WriteLine($"Unregistered {CustomVersion}");
                }
                else
                {
                    Console.WriteLine($"Version {CustomVersion} does not correspond to an external version");
                }
                return Task.FromResult(0);
            }
        }
    }
}
