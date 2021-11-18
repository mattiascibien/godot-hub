namespace GodotHub.Online
{
    public interface IOnlineRepository
    {
        IAsyncEnumerable<OnlineGodotVersion> GetVersionsAsync();

        Task<OnlineGodotVersion?> GetLatestVersionAsync(bool includeUnstable = false);

        Task<OnlineGodotVersion?> GetVersionAsync(string code);
    }
}