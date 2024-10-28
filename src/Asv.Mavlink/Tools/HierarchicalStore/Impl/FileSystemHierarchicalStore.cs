#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

/// <summary>
/// Interface representing a hierarchical store format
/// </summary>
/// <typeparam name="TKey">Type of key identifying each file or folder</typeparam>
/// <typeparam name="TFile">Type of file</typeparam>
public interface IHierarchicalStoreFormat<TKey, out TFile> : IDisposable where TFile : IDisposable
{
    /// <summary>
    /// Gets the Identifier of the root folder.
    /// </summary>
    TKey RootFolderId { get; }

    /// <summary>
    /// Gets the Key Comparer for this store format.
    /// </summary>
    IEqualityComparer<TKey> KeyComparer { get; }

    /// <summary>
    /// Tries to get the information of a folder.
    /// </summary>
    /// <param name="folderInfo">The DirectoryInfo object representing the folder.</param>
    /// <param name="id">[out] When this method returns, contains the unique identifier of the folder if it is found; otherwise, the default value for the type of the identifier.</param>
    /// <param name="displayName">[out] When this method returns, contains the display name of the folder if it is found; otherwise, null.</param>
    /// <returns>Returns true if the folder information is successfully retrieved; otherwise, false.</returns>
    bool TryGetFolderInfo(DirectoryInfo folderInfo, out TKey id, out string? displayName);

    /// <summary>
    /// Get the folder name in the file system.
    /// </summary>
    string GetFileSystemFolderName(TKey id, string displayName);

    /// <summary>
    /// Try to get file information.
    /// </summary>
    bool TryGetFileInfo(FileInfo fileInfo, out TKey id, out string displayName);

    /// <summary>
    /// Get file name from the file system.
    /// </summary>
    string GetFileSystemFileName(TKey id, string displayName);

    /// <summary>
    /// Open a file.
    /// </summary>
    TFile OpenFile(Stream stream);

    /// <summary>
    /// Create a new file.
    /// </summary>
    TFile CreateFile(Stream stream, TKey id, string name);

    /// <summary>
    /// Check if folder name is valid.
    /// </summary>
    void CheckFolderDisplayName(string name);
}


public class FileSystemHierarchicalStore<TKey, TFile>:DisposableOnceWithCancel,IHierarchicalStore<TKey, TFile> 
    where TKey : notnull
    where TFile : IDisposable
{
    private readonly string _rootFolder;
    private readonly IHierarchicalStoreFormat<TKey,TFile> _format;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger _logger;
    private readonly object _sync = new();
    private readonly RxValue<ushort> _count;
    private readonly RxValue<ulong> _size;
    private readonly Dictionary<TKey,FileSystemHierarchicalStoreEntry<TKey>> _entries;
    private readonly List<CachedFile<TKey,TFile>> _fileCache = new();
    private int _checkCacheInProgress;
    private readonly TimeSpan _fileCacheTime;

    public FileSystemHierarchicalStore(string rootFolder, IHierarchicalStoreFormat<TKey,TFile> format, TimeSpan? fileCacheTime, TimeProvider? timeProvider = null, ILoggerFactory? logFactory = null)
    {
        logFactory??=NullLoggerFactory.Instance;
        _logger = logFactory.CreateLogger<FileSystemHierarchicalStore<TKey, TFile>>();
        if (format == null) throw new ArgumentNullException(nameof(format));
        _timeProvider = timeProvider ?? TimeProvider.System;
        format.DisposeItWith(Disposable);
        if (string.IsNullOrEmpty(rootFolder))
            throw new ArgumentException($"{nameof(rootFolder)} cannot be null or empty.", nameof(rootFolder));
        _fileCacheTime = fileCacheTime ?? TimeSpan.Zero;
        if (_fileCacheTime != TimeSpan.Zero)
        {
            _timeProvider.CreateTimer(CheckFileCacheForRottenFiles,null,_fileCacheTime,_fileCacheTime).DisposeItWith(Disposable);
        }
        _rootFolder = rootFolder;
        _format = format ?? throw new ArgumentNullException(nameof(format));
        
        _entries = new Dictionary<TKey, FileSystemHierarchicalStoreEntry<TKey>>(_format.KeyComparer);
        if (Directory.Exists(rootFolder) == false)
        {
            _logger.ZLogWarning($"Directory '{rootFolder}' not exist. Try create");
            Directory.CreateDirectory(rootFolder);
        }
        _count = new RxValue<ushort>().DisposeItWith(Disposable);
        _size = new RxValue<ulong>().DisposeItWith(Disposable);
        
        InternalUpdateEntries();
        
        Disposable.AddAction(ClearFileCache);

    }
    
    public IRxValue<ushort> Count => _count;

    /// <summary>
    /// Gets the size value as an observable.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IRxValue{ulong}"/> representing the size value.
    /// </value>
    public IRxValue<ulong> Size => _size;

    public void UpdateEntries()
    {
        lock (_sync)
        {
            InternalUpdateEntries();
        }    
    }
    
    #region Internal update items

    /// <summary>
    /// This method should call from lock
    /// </summary>
    private void InternalUpdateEntries()
    {
        _entries.Clear();
        foreach (var entry in InternalGetEntries(null))
        {
            _entries.Add(entry.Id, entry);
        }
        InternalUpdateStatistics();    
    }
    
    private IEnumerable<FileSystemHierarchicalStoreEntry<TKey>> InternalGetEntries(FileSystemHierarchicalStoreEntry<TKey>? parentFolder)
    {
        foreach (var directory in Directory.EnumerateDirectories(parentFolder?.FullPath ?? _rootFolder))
        {
            var folderInfo = new DirectoryInfo(directory);
            Debug.Assert(folderInfo.Exists);
            if (_format.TryGetFolderInfo(folderInfo,out var id, out var displayName) == false)
            {
                _logger.ZLogWarning($"Skip store folder '{folderInfo.FullName}'");
                continue;
            }
            Debug.Assert(displayName.IsNullOrWhiteSpace() == false);
            var entry = new FileSystemHierarchicalStoreEntry<TKey>(id, displayName, FolderStoreEntryType.Folder,
                parentFolder == null ? _format.RootFolderId : parentFolder.Id, folderInfo.FullName);
            yield return entry;
            foreach (var subEntry in InternalGetEntries(entry))
            {
                yield return subEntry;
            }
        }

        foreach (var file in Directory.EnumerateFiles(parentFolder?.FullPath ?? _rootFolder))
        {
            var fileInfo = new FileInfo(file);
            Debug.Assert(fileInfo.Exists);
            if (_format.TryGetFileInfo(fileInfo, out var id, out var displayName) == false)
            {
                _logger.ZLogWarning($"Skip store file '{fileInfo.FullName}'");
                continue;
            }
            
            Debug.Assert(displayName.IsNullOrWhiteSpace() == false);
            var entry = new FileSystemHierarchicalStoreEntry<TKey>(id, displayName, FolderStoreEntryType.File,
                parentFolder == null ? _format.RootFolderId : parentFolder.Id, fileInfo.FullName);
            yield return entry;
        }
    }
    
    private void InternalUpdateStatistics()
    {
        _count.Value = (ushort)_entries.Count(x => x.Value.Type == FolderStoreEntryType.File);
        _size.Value = (ulong)_entries.Where(x => x.Value.Type == FolderStoreEntryType.File).Sum(x => new FileInfo(x.Value.FullPath).Length);
        //Logger.Info($"Update statistics: items:{_count.Value}, size:{_size.Value}");
    }

    /// <summary>
    /// Retrieves all sub-files of a given folder.
    /// </summary>
    /// <param name="folderId">The identifier of the folder.</param>
    /// <returns>An enumerable collection of sub-file identifiers.</returns>
    /// <remarks>
    /// The method recursively retrieves all sub-files within a folder.
    /// If the folder identified by <paramref name="folderId"/> does not exist in the internal collection,
    /// an empty collection is returned.
    /// </remarks>
    /// <typeparam name="TKey">The type of the folder identifier.</typeparam>
    /// <returns>An <see cref="IEnumerable{T}"/> of <typeparamref name="TKey"/>.</returns>
    private IEnumerable<TKey> InternalGetSubFiles(TKey folderId)
    {
        if (_entries.TryGetValue(folderId, out var folder) == false) yield break;
        foreach (var item in _entries)
        {
            if (item.Value.ParentId.Equals(folder.Id) == false) continue;
            if (item.Value.Type == FolderStoreEntryType.File)
            {
                yield return item.Value.Id;
            }
            else
            {
                foreach (var file in InternalGetSubFiles(item.Value.Id))
                {
                    yield return file;
                }
            }
        }
        
    }
    
    #endregion

    #region Entries

    public IReadOnlyList<IHierarchicalStoreEntry<TKey>> GetEntries()
    {
        lock (_sync)
        {
            return _entries.Values.ToImmutableList();
        }
    }

    /// <summary>
    /// Tries to get the entry with the specified id from the hierarchical store.
    /// </summary>
    /// <typeparam name="TKey">The type of the id.</typeparam>
    /// <param name="id">The id of the entry to get.</param>
    /// <param name="entry">When this method returns, contains the entry associated with the specified id, if the id is found; otherwise, null. This parameter is passed uninitialized.</param>
    /// <returns>true if the entry is found and retrieved successfully; otherwise, false.</returns>
    public bool TryGetEntry(TKey id, out IHierarchicalStoreEntry<TKey>? entry)
    {
        entry = null;
        lock (_sync)
        {
            if (_entries.TryGetValue(id, out var localEntry))
            {
                entry = localEntry;
                return true;
            }
            entry = default;
            return false;
        }
    }

    public bool EntryExists(TKey id)
    {
        lock (_sync)
        {
            return _entries.ContainsKey(id);
        }
    }

    #endregion
   
    #region Folders

    public TKey RootFolderId => _format.RootFolderId;
    
    public IReadOnlyList<IHierarchicalStoreEntry<TKey>> GetFolders()
    {
        lock (_sync)
        {
            return _entries.Where(x => x.Value.Type == FolderStoreEntryType.Folder).Select(x=>x.Value).ToImmutableList();
        }
    }

    /// <summary>
    /// Creates a new folder in the hierarchical store.
    /// </summary>
    /// <param name="id">The unique identifier for the folder.</param>
    /// <param name="name">The name of the folder to create.</param>
    /// <param name="parentId">The unique identifier of the parent folder.</param>
    /// <returns>
    /// The unique identifier of the created folder.
    /// </returns>
    /// <exception cref="System.ArgumentException">
    /// Thrown when <paramref name="name"/> is null or empty.
    /// </exception>
    /// <exception cref="HierarchicalStoreException">
    /// Thrown when the folder with <paramref name="parentId"/> does not exist.
    /// </exception>
    /// <exception cref="HierarchicalStoreFolderAlreadyExistException">
    /// Thrown when a folder with the same name already exists under the parent folder.
    /// </exception>
    public TKey CreateFolder(TKey id, string name, TKey parentId)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("Folder name cannot be null or empty.", nameof(name));
        _format.CheckFolderDisplayName(name);
        lock (_sync)    
        {
            var rootFolder = _rootFolder;
            if (!_entries.ContainsKey(parentId) && !_format.KeyComparer.Equals(parentId, RootFolderId))
            {
                _logger.ZLogError($"Folder with id {parentId} does not exist");
                throw new HierarchicalStoreException(name);
            }
            if (_entries.TryGetValue(parentId, out var parent))
            {
                rootFolder = parent.FullPath;
                parentId = parent.Id;
            }
            if (_entries.ContainsKey(id))
            {
                _logger.ZLogError($"Folder with id {id} already exist");
                throw new HierarchicalStoreFolderAlreadyExistException(name);
            }
            
            var newFolderNameInFileSystem = _format.GetFileSystemFolderName(id,name);
           
            if (_entries.Where(x => _format.KeyComparer.Equals(x.Value.ParentId, parentId))
                .Any(x => x.Value.Name == name))
            {
                _logger.ZLogError($"Folder with name='{name}' already exist at {rootFolder}");
                throw new HierarchicalStoreFolderAlreadyExistException(name);
            }
            
            var newFolderPath = Path.Combine(rootFolder, newFolderNameInFileSystem);
            var info = Directory.CreateDirectory(newFolderPath);
            Debug.Assert(info.Exists);
            Debug.Assert(_format.TryGetFolderInfo(info, out var newId, out var displayName));
            Debug.Assert(_format.KeyComparer.Equals(newId,id));
            Debug.Assert(displayName == name);
            var newEntry =
                new FileSystemHierarchicalStoreEntry<TKey>(id, name, FolderStoreEntryType.Folder, parentId,
                    newFolderPath);
            _logger.ZLogInformation($"Create folder {name}");
            _entries.Add(id,newEntry);
            return id;
        }
    }

    public void DeleteFolder(TKey id)
    {
        lock (_sync)
        {
            _logger.ZLogInformation($"Delete folder {id}");
            if (_entries.TryGetValue(id, out var entry) == false)
            { 
                throw new HierarchicalStoreException($"Folder with id='{id}' not found");
            }

            if (entry.Type != FolderStoreEntryType.Folder)
            {
                throw new HierarchicalStoreException($"Entry '{id}' is not folder");
            }
            var files = InternalGetSubFiles(id);
            foreach (var file in files)
            {
                if (TryImmediatelyRemoveFromCache(file) == false)
                {
                    throw new HierarchicalStoreException("Can't rename folder: file in folder currently in use");
                }
            }
            Directory.Delete(entry.FullPath,true);
            InternalUpdateEntries();
        }
    }

    /// <summary>
    /// Checks if a folder with the specified ID exists in the folder store.
    /// </summary>
    /// <param name="id">The ID of the folder to check.</param>
    /// <returns>True if the folder exists, otherwise false.</returns>
    public bool FolderExists(TKey id)
    {
        lock (_sync)
        {
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != FolderStoreEntryType.Folder) return false;
            return true;
        }
    }

    public void RenameFolder(TKey id, string newName)
    {
        lock (_sync)
        {
            _logger.ZLogInformation($"Rename folder {id} to {newName}");
            if (_entries.TryGetValue(id, out var entry) == false)
            {   
                throw new HierarchicalStoreException($"Folder with id '{id}' not found");
            }

            if (entry.Type != FolderStoreEntryType.Folder)
            {
                throw new HierarchicalStoreException($"Entry '{id}' is not folder");
            }

            var parentFolder = _rootFolder;
            if (_entries.TryGetValue(entry.ParentId, out var parent))
            {
                if (parent.Type == FolderStoreEntryType.Folder)
                {
                    parentFolder = parent.FullPath;
                }
            }

            var folderNameAtFileSystem = _format.GetFileSystemFolderName(id, newName);
            
            var newFolderPath= Path.Combine(parentFolder, folderNameAtFileSystem);
            var files = InternalGetSubFiles(id);
            foreach (var file in files)
            {
                if (TryImmediatelyRemoveFromCache(file) == false)
                {
                    throw new HierarchicalStoreException($"Can't rename folder: file '{file}' in folder currently in use");
                }
            }
            Directory.Move(entry.FullPath,newFolderPath);
            InternalUpdateEntries();
        }
    }

    /// <summary>
    /// Moves a folder to a new parent folder.
    /// </summary>
    /// <typeparam name="TKey">The type of the folder ID.</typeparam>
    /// <param name="id">The ID of the folder to move.</param>
    /// <param name="newParentId">The ID of the new parent folder.</param>
    /// <exception cref="HierarchicalStoreException">
    /// <para>Thrown if the folder with the specified <paramref name="id"/> is not found.</para>
    /// <para>Thrown if the entry with the specified <paramref name="id"/> is not a folder.</para>
    /// <para>Thrown if the parent folder with the specified <paramref name="newParentId"/> is not found.</para>
    /// <para>Thrown if the parent entry with the specified <paramref name="newParentId"/> is not a folder.</para>
    /// </exception>
    public void MoveFolder(TKey id, TKey newParentId)
    {
        lock (_sync)
        {
            _logger.ZLogInformation($"Move folder '{id}' to '{newParentId}'");
            if (_entries.TryGetValue(id, out var entry) == false)
            {
                throw new HierarchicalStoreException($"Folder with id='{id}' not found");
            }
            if (entry.Type != FolderStoreEntryType.Folder)
            {
                throw new HierarchicalStoreException($"Entry '{id}' is not folder");
            }

            if (_entries.TryGetValue(newParentId, out var newParent) == false)
            {
                throw new HierarchicalStoreException($"Parent folder '{newParentId}' not found");
            }

            if (newParent.Type != FolderStoreEntryType.Folder)
            {
                throw new HierarchicalStoreException("Parent entry is not folder");
            }
            var nameAtFileSystem = _format.GetFileSystemFolderName(entry.Id, entry.Name);
            var newFullPath = Path.Combine(newParent.FullPath, nameAtFileSystem);
            Directory.Move(entry.FullPath, newFullPath);
            InternalUpdateEntries();
        }
    }
    
    #endregion
    
    #region Files
    
    public IReadOnlyList<IHierarchicalStoreEntry<TKey>> GetFiles()
    {
        lock (_sync)
        {
            return _entries.Where(x => x.Value.Type == FolderStoreEntryType.File)
                .Select(x=>x.Value).ToImmutableList();
        }
    }

    public bool TryGetFile(TKey id, out IHierarchicalStoreEntry<TKey> entry)
    {
        entry = default;
        lock (_sync)
        {
            if (!_entries.TryGetValue(id, out var commonEntry)) return false;
            if (commonEntry.Type != FolderStoreEntryType.File) return false;
            entry = commonEntry;
            return true;

        }
    }

    public void DeleteFile(TKey id)
    {
        lock (_sync)
        {   
            _logger.ZLogInformation($"Delete file {id}");
            if (_entries.TryGetValue(id, out var entry) == false)
            {
                throw new HierarchicalStoreException($"File with id='{id}' not found");
            }

            if (entry.Type != FolderStoreEntryType.File)
            {
                throw new HierarchicalStoreException($"Entry '{id}' is not file");
            }
            // we need to remove file from cache, if we want to modify it
            if (TryImmediatelyRemoveFromCache(id) == false)
            {
                throw new HierarchicalStoreException($"Can't delete file '{id}': it is currently in use");
            }
            File.Delete(entry.FullPath);
            _entries.Remove(id);
        }
    }

    public bool FileExists(TKey id)
    {
        lock (_sync)
        {
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            return entry.Type == FolderStoreEntryType.File;
        }
    }

    /// <summary>
    /// Renames a file with the specified ID to the new name.
    /// </summary>
    /// <param name="id">The ID of the file to be renamed.</param>
    /// <param name="newName">The new name for the file.</param>
    /// <returns>The ID of the renamed file.</returns>
    public TKey RenameFile(TKey id, string newName)
    {
        lock (_sync)
        {
            _logger.ZLogInformation($"Rename file {id} to {newName}");
            if (_entries.TryGetValue(id, out var entry) == false)
            {
                throw new HierarchicalStoreException("File with id='{id}' not found");
            }

            if (entry.Type != FolderStoreEntryType.File)
            {
                throw new HierarchicalStoreException("Entry '{id}' is not file");
            }
            var parentFolder = _rootFolder;
            if (_entries.TryGetValue(entry.ParentId, out var parent))
            {
                if (parent.Type == FolderStoreEntryType.Folder)
                {
                    parentFolder = parent.FullPath;
                }
            }
            
            var nameAtFileSystem = _format.GetFileSystemFileName(id,newName);
            var newFilePath = Path.Combine(parentFolder, nameAtFileSystem);
            // we need to remove file from cache, if we want to modify it
            if (TryImmediatelyRemoveFromCache(id) == false)
            {
                throw new HierarchicalStoreException("Can't rename file: it is currently in use");
            }

            if (newFilePath.Equals(entry.FullPath) == false)
            {
                File.Move(entry.FullPath,newFilePath);
            }
            _entries.Remove(id);
            _entries.Add(id,new FileSystemHierarchicalStoreEntry<TKey>(id,newName, FolderStoreEntryType.File,entry.ParentId,newFilePath));
            return id;
        }
    }

    /// <summary>
    /// Moves a file with the specified ID to a new parent folder with the specified ID.
    /// </summary>
    /// <typeparam name="TKey">The type of the ID.</typeparam>
    /// <param name="id">The ID of the file to be moved.</param>
    /// <param name="newParentId">The ID of the new parent folder.</param>
    /// <exception cref="HierarchicalStoreException">Thrown if the file with the given ID is not found,
    /// if the entry with the given ID is not a file, or if the parent entry is not a folder.</exception>
    public void MoveFile(TKey id, TKey newParentId)
    {
        lock (_sync)
        {
            _logger.ZLogInformation($"Move file {id} to {newParentId}");
            if (_entries.TryGetValue(id, out var entry) == false)
            {
                throw new HierarchicalStoreException($"File with id='{id}' not found");
            }
            if (entry.Type != FolderStoreEntryType.File)
            {
                throw new HierarchicalStoreException($"Entry '{id}' is not file");
            }

            var parentPath = _rootFolder;
            if (_entries.TryGetValue(newParentId, out var newParent))
            {
                if (newParent.Type != FolderStoreEntryType.Folder)
                {
                    throw new HierarchicalStoreException("Parent entry is not folder");
                }
                parentPath = newParent.FullPath;
            }
            var nameAtFileSystem = _format.GetFileSystemFileName(id,entry.Name);
            var newFilePath = Path.Combine(parentPath, nameAtFileSystem);
            // we need to remove file from cache, if we want to modify it
            if (TryImmediatelyRemoveFromCache(id) == false)
            {
                throw new HierarchicalStoreException("Can't rename file: it is currently in use");
            }
           
            if (newFilePath.Equals(entry.FullPath) == false)
            {
                File.Move(entry.FullPath,newFilePath);    
            }
            InternalUpdateEntries();
        }
    }

    public ICachedFile<TKey, TFile> OpenFile(TKey id)
    {
        lock (_sync)
        {
            if (_entries.TryGetValue(id, out var entry) == false)
            {
                _logger.ZLogError($"File with id '{id}' not found");
                throw new FileNotFoundException($"File with [{id}] not found");
            }
            
            var fileInfo =  new FileInfo(entry.FullPath);
            if (fileInfo.Exists == false)
            {
                _logger.ZLogError($"File with [{id}]{fileInfo.FullName} not found");
                throw new FileNotFoundException($"File with [{id}]{fileInfo.FullName} not found", fileInfo.FullName);
            }
            return GetOrAddFileToCache(entry);
        }
    }

    /// <summary>
    /// Creates a new file in the hierarchical store.
    /// </summary>
    /// <typeparam name="TKey">The type of the file's ID.</typeparam>
    /// <typeparam name="TFile">The type of the file.</typeparam>
    /// <param name="id">The ID of the file.</param>
    /// <param name="name">The name of the file.</param>
    /// <param name="parentId">The ID of the parent folder.</param>
    /// <returns>The created file with its associated cache.</returns>
    /// <exception cref="HierarchicalStoreException">Thrown when the ID or file name already exists.</exception>
    public ICachedFile<TKey, TFile> CreateFile(TKey id, string name, TKey parentId)
    {
        lock (_sync)
        {
            if (_entries.ContainsKey(id))
            {
                _logger.ZLogError($"Entry '{id}' already exist");
                throw new HierarchicalStoreException($"Entry {id} already exist");
            }
            
            var rootFolder = _rootFolder;
            if (_entries.TryGetValue(parentId, out var parent) == false)
            {
                parentId = _format.RootFolderId;
                rootFolder = _rootFolder;
            }
            else
            {
                rootFolder = parent.FullPath;
            }
            var fileNameAtFileSystem = _format.GetFileSystemFileName(id,name);
            var newFilePath = Path.Combine(rootFolder, fileNameAtFileSystem);
            var fileInfo = new FileInfo(newFilePath);
            if (fileInfo.Exists)
            {
                _logger.ZLogError($"File [{id}]{fileInfo.FullName} already exist");
                throw new HierarchicalStoreException($"File [{id}]{fileInfo.FullName} already exist");
            }
            var file = _format.CreateFile(File.Open(fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite), id, name);
            var wrapper = new CachedFile<TKey,TFile>(id,name, parentId,file,OnFileDisposed, _timeProvider);
            var entry = new FileSystemHierarchicalStoreEntry<TKey>(id, name, FolderStoreEntryType.File, parentId,
                fileInfo.FullName);
            _entries.Add(id,entry);
            return AddFileToCache(entry,wrapper);
        }
    }

    #endregion

    #region File cache

    private void ClearFileCache()
    {
        foreach (var file in _fileCache)
        {
            file.ImmediateDispose();
        }
        _fileCache.Clear();
    }

    private bool TryImmediatelyRemoveFromCache(TKey id)
    {
        if (_fileCache.Count == 0) return true;
        var item = _fileCache.FirstOrDefault(file => file.Id.Equals(id));
        if (item == null) return true;
            
        if (item.RefCount != 0) return false;
        _logger.ZLogTrace($"Immediately remove file from cache: '{item.Id}'");
        item.ImmediateDispose();
        _fileCache.Remove(item);
        return true;
    }

    private ICachedFile<TKey, TFile> GetOrAddFileToCache(FileSystemHierarchicalStoreEntry<TKey> entry)
    {
        var exist = _fileCache.FirstOrDefault(f => f.Id.Equals(entry.Id));
        if (exist != null)
        {
            exist.AddRef();
            _logger.ZLogTrace($"Return file '{entry.Name}'[{entry.Id}] from cache (ref count={exist.RefCount})");
        }
        else
        {
            var stream = File.Open(entry.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var file = _format.OpenFile(stream);
            exist = new CachedFile<TKey, TFile>(entry.Id, entry.Name, entry.ParentId, file, OnFileDisposed, _timeProvider);
            exist.AddRef();
            _logger.ZLogTrace($"Add file '{entry.Name}'[{entry.Id}] to cache (ref count={exist.RefCount})");
            _fileCache.Add(exist);
        }

        return exist;
    }
    
    private ICachedFile<TKey, TFile> AddFileToCache(FileSystemHierarchicalStoreEntry<TKey> entry, CachedFile<TKey, TFile> wrapper)
    {
        wrapper.AddRef();
        _logger.ZLogTrace($"Add file '{entry.Name}'[{entry.Id}] to cache (ref count={wrapper.RefCount})");
        _fileCache.Add(wrapper);
        return wrapper;
    }
    

    private void OnFileDisposed(CachedFile<TKey, TFile> file)
    {
        lock (_sync)
        {
            // we remove immediately only if file cache time is zero
            // otherwise we wait for check cache timer to delete it
            if (_fileCacheTime == TimeSpan.Zero)
            {
                file.ImmediateDispose();
                _fileCache.Remove(file);
                _logger.ZLogTrace($"Remove file from cache: {file.Id}");
            }
        }
    }

    /// <summary>
    /// Checks the file cache for rotten files and removes them.
    /// </summary>
    /// <param name="delay">The delay in milliseconds to wait before checking the cache again if it is already in progress.</param>
    private void CheckFileCacheForRottenFiles(object? state)
    {
        if (Interlocked.CompareExchange(ref _checkCacheInProgress,1,0) != 0) return;
        
        try
        {
            // check before lock (optimization)
            if (_fileCache.Count == 0) return;
            lock (_sync)
            {
                // check again after lock (for thread safety)
                if (_fileCache.Count == 0) return;
                // try to remove all files with ref count == 0 and dead time > cache time
                var list = _fileCache.Where(file => file.RefCount == 0)
                    .Where(file => _timeProvider.GetElapsedTime(file.DeadTimeBegin) > _fileCacheTime).ToImmutableList();
                if (list.Count == 0) return;
                foreach (var file in list)
                {
                    file.ImmediateDispose();
                    _fileCache.Remove(file);
                    _logger.ZLogTrace($"Remove file from cache: {file.Id}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex,$"CheckFileCacheForRottenFiles error:{ex.Message}");
        }
        finally
        {
            Interlocked.Exchange(ref _checkCacheInProgress, 0);
        }
    }
    
    #endregion
}