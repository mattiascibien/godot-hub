using GodotHub;
using GodotHub.Commands;
using System.CommandLine;

if (!Directory.Exists(Constants.InstallationDirectory))
    Directory.CreateDirectory(Constants.InstallationDirectory);

// Create a root command with some options
var rootCommand = new RootCommand
{
    new RunCommand(),
    new ListCommand(),
    new InstallCommand(),
    new UninstallCommand(),
    new CreateGodotVersionFileCommand(),
    new RegisterCommand(),
    new UnregisterCommand(),
    //new SetDefaultCommand(),
};

rootCommand.Description = "Godot installer and version manager";

// Parse the incoming args and invoke the handler
return await rootCommand.InvokeAsync(args).ConfigureAwait(false);