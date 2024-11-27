using System;

namespace Asv.Mavlink;

public class HierarchicalStoreException : Exception
{
    public HierarchicalStoreException()
    {
    }

    public HierarchicalStoreException(string message) : base(message)
    {
    }

    public HierarchicalStoreException(string message, Exception inner) : base(message, inner)
    {
    }
}

public class HierarchicalStoreFolderAlreadyExistException : HierarchicalStoreException
{
    public HierarchicalStoreFolderAlreadyExistException(string newFolderName) : base($"Folder '{newFolderName}' already exist")
    {
        
    }
}

public class StoreException
{
    
}