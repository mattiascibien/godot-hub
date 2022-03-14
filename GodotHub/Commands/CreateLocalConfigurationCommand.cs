using GodotHub.Core;
using Microsoft.Extensions.Configuration;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using GodotHub.Resources;

namespace GodotHub.Commands
{
    public class CreateLocalConfigurationCommand : Command
    {
        public CreateLocalConfigurationCommand() : base("create-local-config", Strings.CreateLocalConfigurationCommandDescription)
        {
            AddOption(new Option<bool>("--migrate", Strings.LocalConfigurationCommandMigrateOptionDescription));
            var useVersionOption = new Option<string?>("--use-version", Strings.CreateLocalConfigurationCommandUseVersionOptionDescription);
            useVersionOption.AddAlias("-u");
            AddOption(useVersionOption);
        }

        public class CommandHandler : ICommandHandler
        {
            private readonly IConfiguration _configuration;

            private static readonly JsonWriterOptions Options = new()
            {
                Indented = true
            };

            public string? UseVersion { get; set; }

            public bool Migrate { get; set; }

            public CommandHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                var currentVersion = UseVersion;
                if (currentVersion == null && Migrate && File.Exists(GodotHubPaths.VersionFileName))
                {
                    currentVersion = await File.ReadAllTextAsync(GodotHubPaths.VersionFileName).ConfigureAwait(false);
                }

                await using var stream = File.OpenWrite(GodotHubPaths.LocalConfigFilename);
                await using var writer = new Utf8JsonWriter(stream, Options);

                writer.WriteStartObject();

                if (currentVersion != null)
                {
                    writer.WriteString("version", currentVersion);
                }

                foreach (var (key, value) in _configuration.AsEnumerable())
                {
                    // SKIP environment variables
                    if (Environment.GetEnvironmentVariable(key) != null || key == "contentRoot")
                        continue;

                    writer.WriteString(key, value);
                }

                writer.WriteEndObject();

                if (Migrate && currentVersion != null)
                {
                    File.Delete(GodotHubPaths.VersionFileName);
                }

                return 0;
            }
        }
    }
}
