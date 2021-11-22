namespace GodotHub.Core
{
    public interface ILinkCreator
    {
        void CreateFolderLink(string currentDirectory, string linkName, string target);
        void DeleteFolderLink(string currentDirectory, string folderLink); 
    }
}