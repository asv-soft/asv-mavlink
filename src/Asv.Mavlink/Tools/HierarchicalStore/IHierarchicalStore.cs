#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using Asv.Common;

namespace Asv.Mavlink;

public interface IHierarchicalStore<TKey, out TFile>: IDisposable 
    where TFile : IDisposable
{
    IRxValue<ushort> Count { get; }
    IRxValue<ulong> Size { get; }

    #region Entries

    IReadOnlyList<IHierarchicalStoreEntry<TKey>> GetEntries();
    bool TryGetEntry(TKey id, out IHierarchicalStoreEntry<TKey>? entry);
    bool ExistEntry(TKey id);

    #endregion
    
    #region Folders

    TKey RootFolderId { get; }
    IReadOnlyList<IHierarchicalStoreEntry<TKey>> GetFolders();
    TKey CreateFolder(TKey id, string name, TKey parentId);
    void DeleteFolder(TKey id);
    bool ExistFolder(TKey id);
    void RenameFolder(TKey id, string newName);
    void MoveFolder(TKey id, TKey newParentId);
    
    #endregion
    
    #region Files
    
    IReadOnlyList<IHierarchicalStoreEntry<TKey>> GetFiles();
    bool TryGetFile(TKey id, out IHierarchicalStoreEntry<TKey> entry);
    void DeleteFile(TKey id);
    bool ExistFile(TKey id);
    TKey RenameFile(TKey id, string newName);
    void MoveFile(TKey id, TKey newParentId);
    
    #endregion
    
    #region Open/Create

    ICachedFile<TKey, TFile> Open(TKey id);
    ICachedFile<TKey, TFile> Create(TKey id, string name, TKey parentId);

    #endregion
}

public interface ICachedFile<out TKey, out TObject>:IDisposable
    where TObject:IDisposable
{
    TKey ParentId { get; }
    TKey Id { get; }
    string Name { get; }
    TObject File { get; }
    
}

public static class HierarchicalStoreHelper
{
    public static void DeleteEntry<TKey,TFile>(this IHierarchicalStore<TKey,TFile> self, TKey id) 
        where TFile : IDisposable
    {
        if (!self.TryGetEntry(id, out var entry))
        {
            throw new InvalidOperationException($"Entry with id {id} not found");
        }

        if (entry == null)
            throw new InvalidOperationException($"Entry with id {id} not found");
        
        switch (entry.Type)
        {
            case FolderStoreEntryType.File:
                self.DeleteFile(id);
                break;
            case FolderStoreEntryType.Folder:
                self.DeleteFolder(id);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void RenameEntry<TKey,TFile>(this IHierarchicalStore<TKey,TFile> self,TKey id, string newName) 
        where TFile : IDisposable
    {
        if (!self.TryGetEntry(id, out var entry))
        {
            throw new InvalidOperationException($"Entry with id {id} not found");
        }

        if (entry == null)
            throw new InvalidOperationException($"Entry with id {id} not found");
        
        switch (entry.Type)
        {
            case FolderStoreEntryType.File:
                self.RenameFile(id,newName);
                break;
            case FolderStoreEntryType.Folder:
                self.RenameFolder(id,newName);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

   

    public static void MoveEntry<TKey,TFile>(this IHierarchicalStore<TKey,TFile> self,TKey id, TKey newParentId) 
        where TFile : IDisposable
    {
        if (!self.TryGetEntry(id, out var entry))
        {
            throw new InvalidOperationException($"Entry with id {id} not found");
        }

        if (entry == null)
            throw new InvalidOperationException($"Entry with id {id} not found");
        
        switch (entry.Type)
        {
            case FolderStoreEntryType.File:
                self.MoveFile(id,newParentId);
                break;
            case FolderStoreEntryType.Folder:
                self.MoveFolder(id,newParentId);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}




public interface IHierarchicalStoreEntry<out TKey>
{
    public TKey Id { get; }
    public string Name { get; }
    public FolderStoreEntryType Type { get; }
    public TKey ParentId { get; }
}



public enum FolderStoreEntryType
{
    File,
    Folder
}