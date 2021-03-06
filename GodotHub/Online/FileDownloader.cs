namespace GodotHub.Online
{
    public class FileDownloader : IDisposable
    {
        private readonly HttpClient _httpClient;

        private readonly string _uri;

        public FileDownloader(string uri)
        {
            _uri = uri;
            _httpClient = new HttpClient();
        }

        public async Task<string> DownloadFileAsync(string outDirectory, IProgress<float> progress)
        {
            if (!Directory.Exists(outDirectory))
                Directory.CreateDirectory(outDirectory);

            var outFileName = Path.GetFileName(_uri);

            var response = await _httpClient.GetAsync(_uri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var fileName = Path.Combine(outDirectory, outFileName);
            var totalBytes = response.Content.Headers.ContentLength;
            await using var ms = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            await ProcessContentStreamAsync(fileName, totalBytes, ms, progress).ConfigureAwait(false);

            return fileName;
        }

        private static async Task ProcessContentStreamAsync(string fileName, long? totalDownloadSize, Stream contentStream, IProgress<float> progress)
        {
            var totalBytesRead = 0L;
            var readCount = 0L;
            var buffer = new byte[8192];
            var isMoreToRead = true;

            await using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

            do
            {
                var bytesRead = await contentStream.ReadAsync(buffer).ConfigureAwait(false);
                if (bytesRead == 0)
                {
                    isMoreToRead = false;
                    TriggerProgressChanged(progress, totalDownloadSize, totalBytesRead);
                    continue;
                }

                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead)).ConfigureAwait(false);

                totalBytesRead += bytesRead;
                readCount++;

                if (readCount % 100 == 0)
                    TriggerProgressChanged(progress, totalDownloadSize, totalBytesRead);
            }
            while (isMoreToRead);
        }

        private static void TriggerProgressChanged(IProgress<float> progress, long? totalDownloadSize, long totalBytesRead)
        {
            if (!totalDownloadSize.HasValue) return;

            var progressPercentage = (float)totalBytesRead / totalDownloadSize.Value;
            progress.Report(progressPercentage);
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
                _httpClient.Dispose();
            }
        }
    }
}
