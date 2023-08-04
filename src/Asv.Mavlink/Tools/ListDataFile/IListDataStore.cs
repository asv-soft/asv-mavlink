#nullable enable
using System;
using System.Collections.Generic;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink;



public interface IListDataStore<out TMetadata, TKey>:IDisposable
    where TMetadata:ISizedSpanSerializable,new()
{
    IRxValue<ushort> Count { get; }
    IRxValue<ulong> Size { get; }

    IEnumerable<IListDataStoreEntry<TKey>> GetEntries();
    bool TryGetEntry(TKey id, out IListDataStoreEntry<TKey> entry);
    bool ExistEntry(TKey id);
    
    IReadOnlyList<TKey> GetFolders();
    TKey CreateFolder(TKey parentId, string name);
    bool DeleteFolder(TKey id);
    bool ExistFolder(TKey id);
    bool RenameFolder(TKey id, string newName);
    bool MoveFolder(TKey id, TKey newParentId);
    
    
    IReadOnlyList<TKey> GetFiles();
    IListDataFile<TMetadata> OpenOrCreateFile(TKey id, string name, TKey parentId);
    bool DeleteFile(TKey id);
    bool ExistFile(TKey id);
    bool RenameFile(TKey id, string newName);
    bool MoveFile(TKey id, TKey newParentId);
}

public static class ListDataStoreHelper
{
    public static bool DeleteEntry<TMetadata,TKey>(IListDataStore<TMetadata,TKey> self, TKey id) 
        where TMetadata : ISizedSpanSerializable, new()
    {
        if (self.TryGetEntry(id, out var entry))
        {
            return entry.Type == StoreEntryType.File ? self.DeleteFile(id) : self.DeleteFolder(id);
        }

        return false;
    }

    public static bool RenameEntry<TMetadata,TKey>(IListDataStore<TMetadata,TKey> self,TKey id, string newName) 
        where TMetadata : ISizedSpanSerializable, new()
    {
        if (self.TryGetEntry(id, out var entry))
        {
            return entry.Type == StoreEntryType.File ? self.RenameFile(id, newName) : self.RenameFolder(id, newName);
        }

        return false;
    }

   

    public static bool MoveEntry<TMetadata,TKey>(IListDataStore<TMetadata,TKey> self,TKey id, TKey newParentId) 
        where TMetadata : ISizedSpanSerializable, new()
    {
        if (self.TryGetEntry(id, out var entry))
        {
            return entry.Type == StoreEntryType.File ? self.MoveFile(id, newParentId) : self.MoveFolder(id, newParentId);
        }

        return false;
    }
}

public enum StoreEntryType
{
    File,
    Folder
}

public interface IListDataStoreEntry<out TKey>
{
    public TKey Id { get; }
    public string Name { get; }
    public StoreEntryType Type { get; }
    public TKey ParentId { get; }
}