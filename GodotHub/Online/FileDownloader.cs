using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotHub.Online
{
    public class FileDownloader : IDisposable
    {
        private HttpClient _httpClient;

        private string _uri;

        public FileDownloader(string uri)
        {
            _uri = uri;
            _httpClient = new HttpClient();
        }

        public async Task<string> DownloadFileAsync(string outDirectory)
        {
            if (!Directory.Exists(outDirectory))
                Directory.CreateDirectory(outDirectory);

            string outFileName = Path.GetFileName(_uri);

            var fileInfo = new FileInfo(Path.Combine(outDirectory, outFileName));

            var response = await _httpClient.GetAsync(_uri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            await using var ms = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var fs = File.Create(fileInfo.FullName);
            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(fs);

            return fileInfo.FullName;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  // Violates rule
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_httpClient != null)
                {
                    _httpClient.Dispose();
                    _httpClient = null;
                }
            }
        }
    }
}
