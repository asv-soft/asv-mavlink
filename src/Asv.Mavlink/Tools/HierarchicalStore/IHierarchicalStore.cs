#nullable enable
using System;
using System.Collections.Generic;
using Asv.Common;
using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents a hierarchical store for files and folders.
/// </summary>
/// <typeparam name="TKey">The type of the key for identifying entries.</typeparam>
/// <typeparam name="TFile">The type of the file.</typeparam>
public interface IHierarchicalStore<TKey, out TFile>: IDisposable 
    where TFile : IDisposable
{
    /// <summary>
    /// Gets the value representing the count.
    /// </summary>
    /// <value>
    /// The value representing the count.
    /// </value>
    ReadOnlyReactiveProperty<ushort> Count { get; }

    /// <summary>
    /// Gets the size of the object.
    /// </summary>
    /// <returns>An object implementing the IRxValue interface that represents the size of the object.</returns>
    ReadOnlyReactiveProperty<ulong> Size { get; }

    #region Entries

    /// <summary>
    /// Retrieves a list of entries in the hierarchical store.
    /// </summary>
    /// <typeparam name="TKey">The type of the entry key.</typeparam>
    /// <returns>A read-only list of <see cref="IHierarchicalStoreEntry{TKey}"/> objects.</returns>
    IReadOnlyList<IHierarchicalStoreEntry<TKey>> GetEntries();

    /// <summary>
    /// Tries to retrieve the entry with the specified ID from the hierarchical store.
    /// </summary>
    /// <param name="id">The ID of the entry to retrieve.</param>
    /// <param name="entry">When this method returns, contains the entry associated with the specified ID, if found; otherwise, null.</param>
    /// <returns>
    /// true if the entry with the specified ID is found in the hierarchical store; otherwise, false.
    /// </returns>
    bool TryGetEntry(TKey id, out IHierarchicalStoreEntry<TKey>? entry);

    /// <summary>
    /// Checks if an entry with the given ID exists.
    /// </summary>
    /// <param name="id">The ID of the entry to check.</param>
    /// <returns>True if an entry with the given ID exists; otherwise, false.</returns>
    bool EntryExists(TKey id);

    #endregion
    
    #region Folders

    /// <summary>
    /// Gets the ID of the root folder.
    /// </summary>
    /// <typeparam name="TKey">The data type of the root folder ID.</typeparam>
    /// <returns>The ID of the root folder.</returns>
    TKey RootFolderId { get; }

    /// <summary>
    /// Retrieves a list of folders from the hierarchical store.
    /// </summary>
    /// <typeparam name="TKey">The type of key used in the hierarchical store.</typeparam>
    /// <returns>A IReadOnlyList of IHierarchicalStoreEntry objects representing the folders.</returns>
    IReadOnlyList<IHierarchicalStoreEntry<TKey>> GetFolders();

    /// <summary>
    /// Creates a new folder with the specified id, name, and parentId.
    /// </summary>
    /// <param name="id">The id of the new folder.</param>
    /// <param name="name">The name of the new folder.</param>
    /// <param name="parentId">The id of the parent folder.</param>
    /// <returns>The key of the newly created folder.</returns>
    /// <typeparam name="TKey">The data type of the folder key.</typeparam>
    /// <remarks>
    /// This method is used to create a new folder in the system. The folder will have the specified id, name, and parentId.
    /// The parentId should correspond to the id of an existing folder in the system. If the parent folder does not exist,
    /// an exception will be thrown. The method returns the key of the newly created folder.
    /// </remarks>
    TKey CreateFolder(TKey id, string name, TKey parentId);

    /// <summary>
    /// Deletes the folder with the specified id.
    /// </summary>
    /// <param name="id">The identifier of the folder to delete.</param>
    /// <returns>None.</returns>
    /// <remarks>
    /// This method deletes the folder from the file system, database, or any other storage medium based on the provided id.
    /// </remarks>
    void DeleteFolder(TKey id);

    /// <summary>
    /// Checks if a folder with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the folder to check.</param>
    /// <returns>True if the folder exists, otherwise false.</returns>
    bool FolderExists(TKey id);

    /// <summary>
    /// Renames a folder with the specified ID to the new name.
    /// </summary>
    /// <param name="id">The ID of the folder to be renamed.</param>
    /// <param name="newName">The new name for the folder.</param>
    void RenameFolder(TKey id, string newName);

    /// <summary>
    /// Moves a folder with the specified <paramref name="id"/> to a new parent folder with the specified <paramref name="newParentId"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the folder identifier.</typeparam>
    /// <param name="id">The identifier of the folder to be moved.</param>
    /// <param name="newParentId">The identifier of the new parent folder.</param>
    void MoveFolder(TKey id, TKey newParentId);
    
    #endregion
    
    #region Files

    /// <summary>
    /// Retrieves a list of files from the hierarchical store.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used to identify the files.</typeparam>
    /// <returns>A read-only list of hierarchical store entries representing the files.</returns>
    IReadOnlyList<IHierarchicalStoreEntry<TKey>> GetFiles();
    bool TryGetFile(TKey id, out IHierarchicalStoreEntry<TKey> entry);

    /// <summary>
    /// Deletes a file with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the file to be deleted.</param>
    void DeleteFile(TKey id);

    /// <summary>
    /// Checks if a file with the specified <paramref name="id"/> exists.
    /// </summary>
    /// <typeparam name="TKey">The type of the file's identifier.</typeparam>
    /// <param name="id">The identifier of the file.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    bool FileExists(TKey id);

    /// <summary>
    /// Renames a file by its unique identifier.
    /// </summary>
    /// <typeparam name="TKey">The type of the file identifier.</typeparam>
    /// <param name="id">The unique identifier of the file.</param>
    /// <param name="newName">The new name for the file.</param>
    /// <returns>The updated file identifier.</returns>
    TKey RenameFile(TKey id, string newName);

    /// <summary>
    /// Moves a file to a new parent folder.
    /// </summary>
    /// <param name="id">The unique identifier of the file to be moved.</param>
    /// <param name="newParentId">The unique identifier of the new parent folder.</param>
    /// <remarks>
    /// This method moves the file specified by <paramref name="id"/> to the new parent folder specified by <paramref name="newParentId"/>.
    /// </remarks>
    void MoveFile(TKey id, TKey newParentId);
    
    #endregion
    
    #region Open/Create

    /// <summary>
    /// Opens a file with the specified ID.
    /// </summary>
    /// <typeparam name="TKey">The type of the file ID.</typeparam>
    /// <typeparam name="TFile">The type of the file.</typeparam>
    /// <param name="id">The ID of the file to open.</param>
    /// <returns>An instance of ICachedFile representing the opened file.</returns>
    ICachedFile<TKey, TFile> OpenFile(TKey id);

    /// <summary>
    /// Creates a new cached file with the specified ID, name, and parent ID.
    /// </summary>
    /// <typeparam name="TKey">The type of the ID.</typeparam>
    /// <typeparam name="TFile">The type of the file.</typeparam>
    /// <param name="id">The ID of the file.</param>
    /// <param name="name">The name of the file.</param>
    /// <param name="parentId">The parent ID of the file.</param>
    /// <returns>The created cached file.</returns>
    ICachedFile<TKey, TFile> CreateFile(TKey id, string name, TKey parentId);

    #endregion
}

/// <summary>
/// Represents a cached file object with a specified parent ID, ID, name, and file content.
/// </summary>
/// <typeparam name="TKey">The type of the parent ID and ID.</typeparam>
/// <typeparam name="TObject">The type of the file content.</typeparam>
public interface ICachedFile<out TKey, out TObject>:IDisposable
    where TObject:IDisposable
{
    /// <summary>
    /// Gets the parent identifier.
    /// </summary>
    /// <value>
    /// The parent identifier.
    /// </value>
    TKey ParentId { get; }

    /// <summary>
    /// Gets the unique identifier for the property.
    /// </summary>
    /// <typeparam name="TKey">The type of the identifier.</typeparam>
    /// <value>The identifier for the property.</value>
    TKey Id { get; }

    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <returns>
    /// A string representing the name of the property.
    /// </returns>
    string Name { get; }

    /// <summary>
    /// Represents a file object.
    /// </summary>
    /// <remarks>
    /// The File property is a reference to a TObject that represents a file.
    /// </remarks>
    /// <returns>
    /// A reference to a TObject representing a file.
    /// </returns>
    TObject File { get; }
    
}

/// <summary>
/// A helper class for interacting with an IHierarchicalStore.
/// </summary>
public static class HierarchicalStoreHelper
{
    /// <summary>
    /// Deletes an entry from the hierarchical store.
    /// </summary>
    /// <typeparam name="TKey">The type of the entry id.</typeparam>
    /// <typeparam name="TFile">The type of the file.</typeparam>
    /// <param name="self">The hierarchical store instance.</param>
    /// <param name="id">The id of the entry to delete.</param>
    /// <exception cref="InvalidOperationException">Thrown when the entry with the specified id is not found.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the entry type is invalid.</exception>
    /// <remarks>
    /// The method deletes a file or a folder from the hierarchical store based on the entry type.
    /// If the entry is not found, an <see cref="InvalidOperationException"/> is thrown.
    /// </remarks>
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

    /// Renames an entry in the hierarchical store.
    /// @typeparam TKey The type of the entry identifier.
    /// @typeparam TFile The type of the file in the store.
    /// @param self The hierarchical store instance.
    /// @param id The identifier of the entry to be renamed.
    /// @param newName The new name for the entry.
    /// @throws System.InvalidOperationException if the entry with the specified id is not found in the store.
    /// @throws System.ArgumentOutOfRangeException if the entry type is not supported.
    /// @remarks
    /// - This method should be called on an instance of the `IHierarchicalStore` interface.
    /// - The store entry with the specified id will be looked up in the hierarchical store using the `TryGetEntry` method.
    /// - If the entry is not found, an InvalidOperationException is thrown with a descriptive message.
    /// - If the entry is found, it will be checked for nullity.
    /// - Depending on the type of the entry (file or folder), the corresponding method (`RenameFile` or `RenameFolder`) is called on the hierarchical store.
    /// /
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


    /// <summary>
    /// Moves an entry identified by its ID to a new parent entry identified by its newParentId.
    /// </summary>
    /// <typeparam name="TKey">The type of the entry ID.</typeparam>
    /// <typeparam name="TFile">The type of the file associated with the entry.</typeparam>
    /// <param name="self">The instance of the IHierarchicalStore interface.</param>
    /// <param name="id">The ID of the entry to be moved.</param>
    /// <param name="newParentId">The ID of the new parent entry.</param>
    /// <exception cref="InvalidOperationException">Thrown if the entry with the provided id is not found.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the entry type is neither Folder nor File.</exception>
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

/// Represents an entry in a hierarchical store.
/// /
public interface IHierarchicalStoreEntry<out TKey>
{
    /// <summary>
    /// Gets the identifier of the property.
    /// </summary>
    /// <typeparam name="TKey">The type of the identifier.</typeparam>
    /// <value>The identifier of the property.</value>
    public TKey Id { get; }

    /// <summary>
    /// Gets the name property.
    /// </summary>
    /// <returns>The name as a string.</returns>
    public string Name { get; }

    /// <summary>
    /// Gets the type of the folder store entry.
    /// </summary>
    /// <returns>The type of the folder store entry.</returns>
    public FolderStoreEntryType Type { get; }

    /// <summary>
    /// Gets the identifier of the parent entity.
    /// </summary>
    /// <typeparam name="TKey">The type of the parent entity identifier.</typeparam>
    /// <returns>
    /// The identifier of the parent entity.
    /// </returns>
    public TKey ParentId { get; }
}

/// <summary>
/// Represents the type of a folder store entry.
/// </summary>
public enum FolderStoreEntryType
{
    /// <summary>
    /// Represents a file in a folder store.
    /// </summary>
    File,

    /// <summary>
    /// Represents the type of entry in the folder store.
    /// </summary>
    Folder
}