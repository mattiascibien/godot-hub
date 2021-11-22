﻿using GodotHub.Core;
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
        }

        public class CommandHanlder : ICommandHandler
        {
            private readonly Constants _constants;

            public string CustomVersion { get; set; }

            public string Path { get; set; }

            public CommandHanlder(Constants constants)
            {
                _constants = constants;
            }

            public Task<int> InvokeAsync(InvocationContext context)
            {
                LinkCreator.CreateFolderLink(_constants.InstallationDirectory, CustomVersion, Path);
                Console.WriteLine($"Registered {Path} as {CustomVersion}");
                return Task.FromResult(0);
            }
        }
    }
}
