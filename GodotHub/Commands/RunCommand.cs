using GodotHub.Local;
using Microsoft.Extensions.Configuration;
using System.CommandLine;
using System.CommandLine.Invocation;
using GodotHub.Resources;

namespace GodotHub.Commands
{
    public class RunCommand : Command
    {
        public RunCommand() : base("run", Strings.RunCommandDescription)
        {
            Option<string?> versionOption = new Option<string?>("--use-version", () => null, Strings.RunCommandUseVersionOptionDescription);
            versionOption.AddAlias("-u");
            AddOption(versionOption);

            AddArgument(new Argument<string[]>("cmdline", Strings.RunCommandCmdLineArgumentDescription));
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly IConfiguration _configuration;
            private readonly InstallationManager _installationManager;

            public string? UseVersion { get; set; }

            public string[] CmdLine { get; set; } = Array.Empty<string>();

            public CommandHandler(InstallationManager installationManager, IConfiguration configuration)
            {
                _configuration = configuration;
                _installationManager = installationManager;
            }

            public Task<int> InvokeAsync(InvocationContext context)
            {
                if (string.IsNullOrEmpty(UseVersion))
                {
                    UseVersion = _configuration["version"];
                }

                int returnValue;
                if (UseVersion != null)
                {
                    returnValue = _installationManager.Launch(UseVersion, CmdLine);
                }
                else
                {
                    Console.WriteLine(Strings.RunCommandCannotFindVersionMessage);
                    returnValue = 1;
                }

                return Task.FromResult(returnValue);
            }
        }
    }
}
