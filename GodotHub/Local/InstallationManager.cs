﻿using GodotHub.Core;
using System.Diagnostics;
using System.IO.Compression;

namespace GodotHub.Local
{
    public class InstallationManager
    {
        private readonly ILinkCreator _linkCreator;

        private readonly string _installationPath;

        public InstallationManager(GodotHubPaths constants, ILinkCreator linkCreator)
        {
            _installationPath = constants.InstallationDirectory;
            _linkCreator = linkCreator;
        }

        public IEnumerable<LocalGodotVersion> GetInstalledVersions()
        {
            foreach (var item in Directory.EnumerateDirectories(_installationPath))
            {
                yield return new LocalGodotVersion(item, _linkCreator.IsLink(item));
            }
        }

        public LocalGodotVersion? FindInstalledVersion(string version)
        {
            var installedVersions = GetInstalledVersions();

            return installedVersions.FirstOrDefault(x => x.ToString() == version);
        }

        public async Task InstallPackageAsync(string versionName, string packageFile, bool isMono)
        {
            await Task.Run(() =>
            {
                string destinationDirectory = _installationPath;

                using var stream = File.OpenRead(packageFile);
                using var archive = new ZipArchive(stream);

                if (!isMono)
                {
                    destinationDirectory = Path.Combine(destinationDirectory, versionName);
                }

                archive.ExtractToDirectory(destinationDirectory, true);

                if (isMono)
                {
                    var directoryToRename = archive.Entries[0].FullName;

                    Directory.Move(Path.Combine(destinationDirectory, directoryToRename), Path.Combine(destinationDirectory, $"{versionName}-mono"));
                }
            }).ConfigureAwait(false);
        }

        public async Task UninstallVersionAsync(string version)
        {
            await Task.Run(() =>
            {
                var installedVersion = FindInstalledVersion(version);

                if (installedVersion != null)
                {
                    Directory.Delete(installedVersion.InstallationPath, true);
                }
            }).ConfigureAwait(false);
        }

        public int Launch(string version, string[] commandLine)
        {
            var installedVersion = FindInstalledVersion(version);
            if (installedVersion != null)
            {
                (var osPlatform, var architecture) = CurrentOS.GetOsInfo();
                var editorPaths = installedVersion.GetSupportedEditorExecutables(osPlatform, architecture);

                if (editorPaths.Any())
                {
                    Process.Start(new ProcessStartInfo(editorPaths.First().EditorPath, string.Join(" ", commandLine))
                    {
                        //UseShellExecute = true
                    });

                    return 0;
                }

                Console.WriteLine($"Version {version} does not seem to have a viable editor");
                return 1;
            }
            else
            {
                Console.WriteLine($"Version {version} is not installed.");
                return 1;
            }
        }
    }
}
