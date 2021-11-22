using Octokit;

namespace GodotHub.Online
{
    public class GithubOnlineRepository : IOnlineRepository
    {
        private static readonly string[] Exclusions = new[]
        {
            ".aar", // exclude android
            ".tar.xz",
            ".tpz",
            ".txt"
        };

        private const string OWNER = "godotengine";
        private const string REPO = "godot";

        private readonly GitHubClient _githubClient;

        public GithubOnlineRepository()
        {
            _githubClient = new GitHubClient(new ProductHeaderValue("GodotHub"));
        }

        public async Task<OnlineGodotVersion?> GetLatestVersionAsync(bool includeUnstable = false)
        {
            var releases = await _githubClient.Repository.Release.GetAll(OWNER, REPO).ConfigureAwait(false);

            var latestRelease = releases.Where(x => !includeUnstable || x.Prerelease).OrderByDescending(x => x.TagName).FirstOrDefault();
            if (latestRelease != null)
            {
                return CreateVersion(latestRelease);
            }

            return null;
        }

        public async IAsyncEnumerable<OnlineGodotVersion> GetVersionsAsync()
        {
            var releases = await _githubClient.Repository.Release.GetAll(OWNER, REPO).ConfigureAwait(false);

            foreach (var release in releases.OrderByDescending(x => x.TagName))
            {
                yield return CreateVersion(release);
            }
        }

        public static OnlineGodotVersion CreateVersion(Release release)
        {
            var godotVersion = new OnlineGodotVersion(release.TagName);

            foreach (var asset in release.Assets)
            {
                // exclude packages which are not supported
                if (Exclusions.Any(asset.Name.Contains))
                    continue;

                var godotPackage = new OnlineGodotPackage(asset.BrowserDownloadUrl);
                godotVersion.Packages.Add(godotPackage);
            }

            return godotVersion;
        }

        public async Task<OnlineGodotVersion?> GetVersionAsync(string code)
        {
            var releases = await _githubClient.Repository.Release.GetAll(OWNER, REPO).ConfigureAwait(false);

            // if this is a stable version, append stable
            if(!code.Contains('-'))
            {
                code = $"{code}-stable";
            }

            var release = releases.FirstOrDefault(r => r.TagName == code);

            if(release == null)
                return null;

            return CreateVersion(release);
        }
    }
}