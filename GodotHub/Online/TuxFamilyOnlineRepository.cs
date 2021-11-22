using HtmlAgilityPack;
using SemVersion;
using System.Xml.Linq;

namespace GodotHub.Online
{
    public class TuxFamilyOnlineRepository : IOnlineRepository
    {
        private const string REPO_URL = "https://downloads.tuxfamily.org/godotengine/";

        public async Task<OnlineGodotVersion?> GetLatestVersionAsync(bool includeUnstable = false)
        {
            List<OnlineGodotVersion> versions = new List<OnlineGodotVersion>();
            await foreach (var item in GetVersionsAsync())
            {
                if (includeUnstable || item.IsStable)
                    versions.Add(item);
            }

            versions = versions.OrderByDescending(v => v.Version).ToList();

            if (versions.Count > 0)
                return null;

            return versions[0];
        }

        public async Task<OnlineGodotVersion?> GetVersionAsync(string code)
        {
            var doc = await RequestAsync($"{code}").ConfigureAwait(false);
            var packages = doc.DocumentNode
                .SelectNodes("/html/body/div[1]/table/tbody/tr/td[1]/a")
                .Where(n => n.Attributes["href"].Value != "../")
                .Select(n => $"{REPO_URL}{code}/{n.Attributes["href"].Value}");

            var version = new OnlineGodotVersion(code);
            foreach (var package in packages)
            {
                version.Packages.Add(new OnlineGodotPackage(package));
            }

            return version;
        }

        public async IAsyncEnumerable<OnlineGodotVersion> GetVersionsAsync()
        {
            var doc = await RequestAsync().ConfigureAwait(false);
            IEnumerable<string> versions = doc.DocumentNode
                .SelectNodes("/html/body/div[1]/table/tbody/tr/td[1]/a")
                .Where(n => SemanticVersion.IsVersion(n.InnerText)).Select(n => n.InnerText);

            if(versions.Any())
            {
                foreach (string version in versions)
                {
                    yield return await GetVersionAsync(version).ConfigureAwait(false)!;
                }
            }
        }

        public static async Task<HtmlDocument> RequestAsync(string? path = null)
        {
            using var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(REPO_URL)
            };

            var responseStream = await httpClient.GetStreamAsync(path).ConfigureAwait(false);
            var responseDocument = new HtmlDocument();
            responseDocument.Load(responseStream);
            return responseDocument;
        }
    }
}