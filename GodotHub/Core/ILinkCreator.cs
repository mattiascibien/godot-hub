namespace GodotHub.Core
{
    public interface ILinkCreator
    {
        void CreateFolderLink(string currentDirectory, string linkName, string target);
        void DeleteFolderLink(string currentDirectory, string folderLink);

        bool IsLink(string path)
        {
            var pathInfo = new DirectoryInfo(path);
            return (pathInfo.Attributes & FileAttributes.ReparsePoint) != 0;
        }
    }
}