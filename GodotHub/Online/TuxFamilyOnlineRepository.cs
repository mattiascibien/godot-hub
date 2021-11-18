namespace GodotHub.Online
{
    public class TuxFamilyOnlineRepository : IOnlineRepository
    {
        public Task<OnlineGodotVersion?> GetLatestVersionAsync(bool includeUnstable = false)
        {
            throw new NotImplementedException();
        }

        public Task<OnlineGodotVersion?> GetVersionAsync(string code)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<OnlineGodotVersion> GetVersionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}