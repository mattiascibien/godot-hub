using GodotHub.Local;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotHub.Commands
{
    public class CreateGodotVersionFileCommand : Command
    {
        public CreateGodotVersionFileCommand() : base("create-godot-version", $"creates the {Constants.VERSION_FILE_NAME} containing the version to use")
        {
            Add(new Argument<string>("version", "the version to use"));

            Handler = CommandHandler.Create<string>(async (version) =>
            {
                if(File.Exists(Constants.VERSION_FILE_NAME))
                {
                    Console.WriteLine($"A {Constants.VERSION_FILE_NAME} already exists in the current directory");
                    return;
                }
                else
                {
                    var installedVersion = new InstallationManager(Constants.InstallationDirectory).FindInstalledVersion(version);

                    if(installedVersion != null)
                    {
                        await File.WriteAllTextAsync(Constants.VERSION_FILE_NAME, version).ConfigureAwait(false);
                        Console.WriteLine($"{Constants.VERSION_FILE_NAME} created.");
                    }
                    else
                    {
                        Console.WriteLine($"Version {version} is not installed. Install it with 'install {version}'");
                    }

                }
            });
        }
    }
}
