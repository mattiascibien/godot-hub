using GodotHub.Core;
using GodotHub.Local;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotHub.Commands
{
    public class RunCommand : Command
    {
        public RunCommand() : base("run", "Lanches Godot in the current directory")
        {
            Option<string?> versionOption = new Option<string?>("--use-version", () => null, "The version to launch");
            versionOption.AddAlias("-u");
            AddOption(versionOption);

            AddArgument(new Argument<string[]>("cmdline", "The command line to pass to godot"));
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly InstallationManager _installationManager;

            public string? UseVersion { get; set; }

            public string[] CmdLine { get; set; } = Array.Empty<string>();

            public CommandHandler(InstallationManager installationManager)
            {
                _installationManager = installationManager;
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                if (string.IsNullOrEmpty(UseVersion))
                {
                    var versionFile = Directory.EnumerateFiles(Directory.GetCurrentDirectory()).FirstOrDefault(f => Path.GetFileName(f) == Constants.VERSION_FILE_NAME);
                    if (versionFile != null)
                    {
                        UseVersion = (await File.ReadAllTextAsync(versionFile).ConfigureAwait(false)).Trim();
                        Console.WriteLine($"Using version {UseVersion} from {Constants.VERSION_FILE_NAME}");
                    }
                }
                if (UseVersion != null)
                    _installationManager.Launch(UseVersion, CmdLine);

                return 0;
            }
        }
    }
}
