#nullable enable
namespace Asv.Mavlink;

public class FileSystemHierarchicalStoreEntry<TKey> : IHierarchicalStoreEntry<TKey>
{
    public FileSystemHierarchicalStoreEntry(TKey id, string name, FolderStoreEntryType type, TKey parentId, string fullPath)
    {
        Id = id;
        Name = name;
        Type = type;
        ParentId = parentId;
        FullPath = fullPath;
    }

    public TKey Id { get; }
    public string Name { get; }
    public FolderStoreEntryType Type { get; }
    public TKey ParentId { get; }
    public string FullPath { get; }
}