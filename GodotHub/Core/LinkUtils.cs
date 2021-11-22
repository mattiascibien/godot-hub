namespace GodotHub.Core
{
    public static class LinkUtils
    {
        public static bool IsLink(string path)
        {
            var pathInfo = new DirectoryInfo(path);
            return (pathInfo.Attributes & FileAttributes.ReparsePoint) != 0;
        }
    }
}