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
using NLog;

namespace Asv.Mavlink;


public interface IHierarchicalStoreFormat<TKey, out TFile>:IDisposable
    where TFile : IDisposable
{
    TKey RootFolderId { get; }
    IEqualityComparer<TKey> KeyComparer{ get; }
    bool TryGetFolderInfo(DirectoryInfo folderInfo,out TKey id, out string? displayName);
    string GetFileSystemFolderName(TKey id, string displayName);
    bool TryGetFileInfo(FileInfo fileInfo,out TKey id, out string displayName);
    string GetFileSystemFileName(TKey id, string displayName);
    TFile OpenFile(Stream stream);
    TFile CreateFile(Stream stream, TKey id, string name);
    void CheckFolderDisplayName(string name);
    TKey GenerateNewKey();
}

public class FileSystemHierarchicalStore<TKey, TFile>:DisposableOnceWithCancel,IHierarchicalStore<TKey, TFile> 
    where TKey : notnull
    where TFile : IDisposable
{
    private readonly string _rootFolder;
    private readonly IHierarchicalStoreFormat<TKey,TFile> _format;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly object _sync = new();
    private readonly RxValue<ushort> _count;
    private readonly RxValue<ulong> _size;
    private readonly Dictionary<TKey,FileSystemHierarchicalStoreEntry<TKey>> _entries;
    private readonly List<CachedFile<TKey,TFile>> _fileCache = new();
    private int _checkCacheInProgress;
    private readonly TimeSpan _fileCacheTime;

    public FileSystemHierarchicalStore(string rootFolder, IHierarchicalStoreFormat<TKey,TFile> format, TimeSpan? fileCacheTime)
    {
        if (format == null) throw new ArgumentNullException(nameof(format));
        format.DisposeItWith(Disposable);
        if (string.IsNullOrEmpty(rootFolder))
            throw new ArgumentException($"{nameof(rootFolder)} cannot be null or empty.", nameof(rootFolder));
        _fileCacheTime = fileCacheTime ?? TimeSpan.Zero;
        if (_fileCacheTime != TimeSpan.Zero)
        {
            Observable.Timer(_fileCacheTime, _fileCacheTime).Subscribe(CheckFileCacheForRottenFiles).DisposeItWith(Disposable);
        }
        _rootFolder = rootFolder;
        _format = format ?? throw new ArgumentNullException(nameof(format));
        _entries = new Dictionary<TKey, FileSystemHierarchicalStoreEntry<TKey>>(_format.KeyComparer);
        if (Directory.Exists(rootFolder) == false)
        {
            Logger.Warn($"Directory '{rootFolder}' not exist. Try create");
            Directory.CreateDirectory(rootFolder);
        }
        _count = new RxValue<ushort>().DisposeItWith(Disposable);
        _size = new RxValue<ulong>().DisposeItWith(Disposable);
        
        InternalUpdateEntries();
        
        Disposable.AddAction(ClearFileCache);

    }
    
    public IRxValue<ushort> Count => _count;
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
            if (_entries.TryGetValue(entry.Id, out _))
            {
                var newKey = _format.GenerateNewKey();
                var newName = _format.GetFileSystemFileName(newKey, entry.Name);
                var newFullName = Directory.GetParent(entry.FullPath).FullName;
                var newEntryFullPath = Path.Combine(newFullName, newName);
                
                File.Move(entry.FullPath, newEntryFullPath);
                
                var newEntry = new FileSystemHierarchicalStoreEntry<TKey>(newKey, entry.Name, entry.Type, entry.ParentId, newEntryFullPath);
                _entries.Add(newEntry.Id, newEntry);
            }
            else
            {
                _entries.Add(entry.Id, entry);
            }
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
                Logger.Warn($"Skip store folder '{folderInfo.FullName}'");
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
                Logger.Warn($"Skip store file '{fileInfo.FullName}'");
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
        Logger.Info($"Update statistics: items:{_count.Value}, size:{_size.Value}");
    }

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

    public TKey CreateFolder(TKey id, string name, TKey parentId)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("Folder name cannot be null or empty.", nameof(name));
        _format.CheckFolderDisplayName(name);
        lock (_sync)    
        {
            var rootFolder = _rootFolder;
            if (!_entries.ContainsKey(parentId) && !_format.KeyComparer.Equals(parentId, RootFolderId))
            {
                Logger.Error($"Folder with id {parentId} does not exist");
                throw new HierarchicalStoreException(name);
            }
            if (_entries.TryGetValue(parentId, out var parent))
            {
                rootFolder = parent.FullPath;
                parentId = parent.Id;
            }
            if (_entries.ContainsKey(id))
            {
                Logger.Error("Folder with id='{0}' already exist",id);
                throw new HierarchicalStoreFolderAlreadyExistException(name);
            }
            
            var newFolderNameInFileSystem = _format.GetFileSystemFolderName(id,name);
           
            if (_entries.Where(x => _format.KeyComparer.Equals(x.Value.ParentId, parentId))
                .Any(x => x.Value.Name == name))
            {
                Logger.Error("Folder with name='{0}' already exist at {1}",name,rootFolder);
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
            Logger.Info("Create folder {0}",name);
            _entries.Add(id,newEntry);
            return id;
        }
    }

    public void DeleteFolder(TKey id)
    {
        lock (_sync)
        {
            Logger.Info("Delete folder {0}",id);
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
            Logger.Info($"Rename folder {id} to {newName}");
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

    public void MoveFolder(TKey id, TKey newParentId)
    {
        lock (_sync)
        {
            Logger.Info("Move folder '{0}' to '{1}'", id, newParentId);
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
            Logger.Info("Delete file {0}",id);
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

    public TKey RenameFile(TKey id, string newName)
    {
        lock (_sync)
        {
            Logger.Info("Rename file {0} to {1}",id,newName);
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
            
            var nameAtFileSystem = _format.GetFileSystemFileName(id, newName);
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

    public void MoveFile(TKey id, TKey newParentId)
    {
        lock (_sync)
        {
            Logger.Info("Move file {0} to {1}",id,newParentId);
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
                Logger.Error("File with [{0}] not found", id);
                throw new FileNotFoundException($"File with [{id}] not found");
            }
            
            var fileInfo =  new FileInfo(entry.FullPath);
            if (fileInfo.Exists == false)
            {
                Logger.Error("File with [{0}]{1} not found", id, fileInfo.FullName);
                throw new FileNotFoundException($"File with [{id}]{fileInfo.FullName} not found", fileInfo.FullName);
            }
            return GetOrAddFileToCache(entry);
        }
    }

    public ICachedFile<TKey, TFile> CreateFile(TKey id, string name, TKey parentId)
    {
        lock (_sync)
        {
            if (_entries.ContainsKey(id))
            {
                Logger.Error("Entry {0} already exist",id);
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
                Logger.Error("File [{0}]{1} already exist", id, fileInfo.FullName);
                throw new HierarchicalStoreException($"File [{id}]{fileInfo.FullName} already exist");
            }
            var file = _format.CreateFile(File.Open(fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite), id, name);
            var wrapper = new CachedFile<TKey,TFile>(id,name, parentId,file,OnFileDisposed);
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
        Logger.Trace("Immediately remove file from cache: '{0}'", item.Id);
        item.ImmediateDispose();
        _fileCache.Remove(item);
        return true;
    }

    private ICachedFile<TKey, TFile> GetOrAddFileToCache(FileSystemHierarchicalStoreEntry<TKey> entry)
    {
        var exist = _fileCache.FirstOrDefault(_ => _.Id.Equals(entry.Id));
        if (exist != null)
        {
            exist.AddRef();
            Logger.Trace($"Return file '{entry.Name}'[{entry.Id}] from cache (ref count={exist.RefCount})");
        }
        else
        {
            var stream = File.Open(entry.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var file = _format.OpenFile(stream);
            exist = new CachedFile<TKey, TFile>(entry.Id, entry.Name, entry.ParentId, file, OnFileDisposed);
            exist.AddRef();
            Logger.Trace($"Add file '{entry.Name}'[{entry.Id}] to cache (ref count={exist.RefCount})");
            _fileCache.Add(exist);
        }

        return exist;
    }
    
    private ICachedFile<TKey, TFile> AddFileToCache(FileSystemHierarchicalStoreEntry<TKey> entry, CachedFile<TKey, TFile> wrapper)
    {
        wrapper.AddRef();
        Logger.Trace($"Add file '{entry.Name}'[{entry.Id}] to cache (ref count={wrapper.RefCount})");
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
                Logger.Trace("Remove file from cache: {0}", file.Id);
            }
        }
    }

    private void CheckFileCacheForRottenFiles(long delay)
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
                    .Where(file => DateTime.Now - file.DeadTimeBegin > _fileCacheTime).ToImmutableList();
                if (list.Count == 0) return;
                foreach (var file in list)
                {
                    file.ImmediateDispose();
                    _fileCache.Remove(file);
                    Logger.Trace("Remove file from cache: {0}", file.Id);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"CheckFileCacheForRottenFiles error:{ex.Message}");
        }
        finally
        {
            Interlocked.Exchange(ref _checkCacheInProgress, 0);
        }
    }
    
    #endregion
}