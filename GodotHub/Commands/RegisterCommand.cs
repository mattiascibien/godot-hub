using GodotHub.Core;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotHub.Commands
{
    internal class RegisterCommand : Command
    {
        public RegisterCommand() : base("register", "register an external godot installation")
        {
            Add(new Argument<string>("customversion", "the custom version to use (i.e. X.Y-dev"));
            Add(new Argument<string>("path", "the path to the godot installation"));

            Handler = CommandHandler.Create<string, string>((customversion, path) =>
            {
                LinkCreator.CreateFolderLink(Constants.InstallationDirectory, customversion, path);
                Console.WriteLine($"Registered {path} as {customversion}");
            });
        }
    }
}
