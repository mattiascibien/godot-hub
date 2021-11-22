using GodotHub.Local;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotHub.Commands
{
    public class RunCommand : Command
    {
        public RunCommand() : base("run", "Lanches Godot in the current directory")
        {
            Option<string> versionOption = new Option<string>("--use-version", () => "", "The version to launch");
            versionOption.AddAlias("-u");
            Add(versionOption);

            Add(new Argument<string[]>("cmdline", "The command line to pass to godot"));

            Handler = CommandHandler.Create<string?, string[]>(async (useVersion, cmdline) =>
            {
                if(string.IsNullOrEmpty(useVersion))
                {
                    var versionFile = Directory.EnumerateFiles(Directory.GetCurrentDirectory()).FirstOrDefault(f => Path.GetFileName(f) == Constants.VERSION_FILE_NAME);
                    if(versionFile != null)
                    {
                        useVersion = (await File.ReadAllTextAsync(versionFile).ConfigureAwait(false)).Trim();
                        Console.WriteLine($"Using version {useVersion} from {Constants.VERSION_FILE_NAME}");
                    }
                }
                if(useVersion != null)
                    new InstallationManager(Constants.InstallationDirectory).Launch(useVersion, cmdline);
            });
        }
    }
}
