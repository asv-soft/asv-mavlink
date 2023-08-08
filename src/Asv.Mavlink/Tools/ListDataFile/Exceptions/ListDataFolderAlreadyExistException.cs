namespace Asv.Mavlink;

public class ListDataFolderAlreadyExistException : ListDataException
{
    public ListDataFolderAlreadyExistException(string newFolderName) : base($"Folder '{newFolderName}' already exist")
    {
        
    }
}