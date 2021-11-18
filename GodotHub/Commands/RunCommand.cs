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

            Handler = CommandHandler.Create<string?, string[]>(async (version, cmdline) =>
            {
                if(string.IsNullOrEmpty(version))
                {
                    var versionFile = Directory.EnumerateFiles(Directory.GetCurrentDirectory()).FirstOrDefault(f => Path.GetFileName(f) == Constants.VERSION_FILE_NAME);
                    if(versionFile != null)
                    {
                        version = (await File.ReadAllTextAsync(versionFile).ConfigureAwait(false)).Trim();
                        Console.WriteLine($"Using version {version} from {Constants.VERSION_FILE_NAME}");
                    }
                }
                if(version != null)
                    new InstallationManager(Constants.InstallationDirectory).Launch(version, cmdline);
            });
        }
    }
}
