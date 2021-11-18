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
    public class UnregisterCommand : Command
    {
        public UnregisterCommand() : base("unregister", "unregisters an external godot installation")
        {
            Add(new Argument<string>("customversion", "the custom version to unregister (i.e. X.Y-dev"));

            Handler = CommandHandler.Create<string>((customversion) =>
            {
                if(LinkCreator.IsLink(customversion))
                {
                    LinkCreator.DeleteFolderLink(Constants.InstallationDirectory, customversion);
                    Console.WriteLine($"Unregistered {customversion}");
                }
                else
                {
                    Console.WriteLine($"Version {customversion} does not correspond to an external version");
                }
            });
        }
    }
}
