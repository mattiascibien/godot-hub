using GodotHub.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GodotHub.Commands
{
    public class CreateLocalGodotHubConfigurationCommand : Command
    {
        public CreateLocalGodotHubConfigurationCommand() : base("create-local-config", "Creates a local config for the folder")
        {
            AddOption(new Option<bool>("--migrate", "Migrates the old .godot-version file"));
            var useVersionOption = new Option<string?>("--use-version", "Specifies the version to use in our local config");
            useVersionOption.AddAlias("-u");
            AddOption(useVersionOption);
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly IConfiguration _configuration;

            private static readonly JsonWriterOptions options = new()
            {
                Indented = true
            };

            public string? UseVersion {  get; set; }

            public bool Migrate { get; set; }

            public CommandHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                string? currentVersion = UseVersion;
                if(currentVersion == null && Migrate && File.Exists(GodotHubPaths.VersionFileName))
                {
                    currentVersion = await File.ReadAllTextAsync(GodotHubPaths.VersionFileName).ConfigureAwait(false);
                }

                using var stream = File.OpenWrite(GodotHubPaths.LocalConfigFilename);
                using var writer = new Utf8JsonWriter(stream, options);

                writer.WriteStartObject();

                if(currentVersion != null)
                {
                    writer.WriteString("version", currentVersion);
                }

                foreach (var (key, value) in _configuration.AsEnumerable())
                {
                    // SKIP environment variables
                    if(Environment.GetEnvironmentVariable(key) != null || key == "contentRoot")
                        continue;

                    writer.WriteString(key, value);
                }

                writer.WriteEndObject();

                if(Migrate && currentVersion != null)
                {
                    File.Delete(GodotHubPaths.VersionFileName);
                }

                return 0;
            }
        }
    }
}
