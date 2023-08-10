#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.IO;
using NLog;

namespace Asv.Mavlink;

public class ListDataStore<TMetadata, TKey> : DisposableOnceWithCancel, IListDataStore<TMetadata, TKey> 
    where TMetadata : ISizedSpanSerializable, new() 
    where TKey : notnull
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly string _rootFolder;
    private readonly ListDataStoreFormat<TKey,TMetadata> _keyConverter;
    private readonly ListDataFileFormat _fileFormat;
    private readonly TimeSpan _fileCacheTime;
    private readonly Dictionary<TKey,ListDataStoreEntry<TKey>> _entries;
    private readonly object _sync = new();
    private readonly RxValue<ushort> _count;
    private readonly RxValue<ulong> _size;
    private readonly ReaderWriterLockSlim _fileCacheLockSlim = new();
    private readonly List<CachedFileWithRefCounter<TMetadata,TKey>> _fileCache = new();
    private int _checkCacheInProgress;

    public ListDataStore(string rootFolder, ListDataStoreFormat<TKey,TMetadata> keyConverter, ListDataFileFormat fileFormat, TimeSpan fileCacheTime)
    {
        if (string.IsNullOrEmpty(rootFolder))
            throw new ArgumentException("Value cannot be null or empty.", nameof(rootFolder));
        _rootFolder = rootFolder;
        _keyConverter = keyConverter ?? throw new ArgumentNullException(nameof(keyConverter));
        _keyConverter.DisposeItWith(Disposable);
        _fileFormat = fileFormat ?? throw new ArgumentNullException(nameof(fileFormat));
        _fileCacheTime = fileCacheTime;
        _entries = new Dictionary<TKey, ListDataStoreEntry<TKey>>(keyConverter.KeyComparer);
        _count = new RxValue<ushort>().DisposeItWith(Disposable);
        _size = new RxValue<ulong>().DisposeItWith(Disposable);
        if (Directory.Exists(_rootFolder) == false)
        {
            Directory.CreateDirectory(_rootFolder);
        }
        InternalUpdateEntries();
        Disposable.AddAction(ClearFileCache);
        Observable.Timer(_fileCacheTime, _fileCacheTime).Subscribe(CheckFileCacheForRottenFiles)
            .DisposeItWith(Disposable);
    }

   

    public IEnumerable<IListDataStoreEntry<TKey>> GetEntries()
    {
        lock (_sync)
        {
            return _entries.Values.ToImmutableList();
        }
    }

    public IRxValue<ushort> Count => _count;
    public IRxValue<ulong> Size => _size;
    
    private void InternalUpdateEntries()
    {
        Logger.Trace("Update entries");
        _entries.Clear();
        foreach (var entry in InternalGetEntries(null))
        {
            _entries.Add(entry.Id, entry);
        }
        InternalUpdateStatistics();
    }

    private void InternalUpdateStatistics()
    {
        _count.Value = (ushort)_entries.Count(x => x.Value.Type == StoreEntryType.File);
        _size.Value = (ulong)_entries.Where(x => x.Value.Type == StoreEntryType.File).Sum(x => new FileInfo(x.Value.FullPath).Length);
        Logger.Info($"Update statistics: rec:{_count.Value}, size:{_size.Value}");
    }

    private IEnumerable<ListDataStoreEntry<TKey>> InternalGetEntries(ListDataStoreEntry<TKey>? parentFolder)
    {
        
        foreach (var directory in Directory.EnumerateDirectories(parentFolder?.FullPath ?? _rootFolder))
        {
            var folderInfo = new DirectoryInfo(directory);
            if (_keyConverter.TryGetFolderKey(folderInfo, out var id) == false) continue;
            var entry = new ListDataStoreEntry<TKey>
            {
                Id = id,
                Name =_keyConverter.GetFolderName(folderInfo),
                Type = StoreEntryType.Folder,
                FullPath = folderInfo.FullName,
                ParentId = parentFolder == null ? _keyConverter.RootFolderId : parentFolder.Id,
            };
            yield return entry;
            foreach (var subEntry in InternalGetEntries(entry))
            {
                yield return subEntry;
            }
        }

        foreach (var file in Directory.EnumerateFiles(parentFolder?.FullPath ?? _rootFolder))
        {
            var fileInfo = new FileInfo(file);
            TMetadata metadata;
            ListDataFileFormat header;
            try
            {
                using var listFile = new ListDataFile<TMetadata>(File.Open(file, FileMode.OpenOrCreate, FileAccess.ReadWrite), _fileFormat, true);
                metadata = listFile.ReadMetadata();
                header = listFile.Header;
            }
            catch (Exception e)
            {
                Logger.Warn($"Skip store file:{e.Message}");
                continue;
            }
            
            if (_keyConverter.TyrGetFileKey(fileInfo,header, metadata, out var id) == false) continue;
            var entry = new ListDataStoreEntry<TKey>
            {
                Id = id,
                Name = _keyConverter.GetDisplayName(fileInfo, header, metadata),
                Type = StoreEntryType.File,
                FullPath = fileInfo.FullName,
                ParentId = parentFolder == null ? _keyConverter.RootFolderId : parentFolder.Id,
            };
            yield return entry;
        }
    }

    #region File cache

    private void ClearFileCache()
    {
        _fileCacheLockSlim.EnterWriteLock();
        try
        {
            foreach (var file in _fileCache)
            {
                file.ImmediateDispose();
            }
            _fileCache.Clear();
        }
        finally
        {
            _fileCacheLockSlim.ExitWriteLock();
        }
        _fileCacheLockSlim.Dispose();
    }

    private bool TryImmediatelyRemoveFromCache(TKey id)
    {
        try
        {
            _fileCacheLockSlim.EnterUpgradeableReadLock();
            if (_fileCache.Count == 0) return true;
            var item = _fileCache.FirstOrDefault(file => file.RefCount == 0 && file.Id.Equals(id));
            if (item == null) return true;
            
            if (item.RefCount != 0) return false;
            
            _fileCacheLockSlim.EnterWriteLock();
            Logger.Trace("Immediately remove file from cache: {0}", item.Id);
            item.ImmediateDispose();
            _fileCache.Remove(item);
            _fileCacheLockSlim.ExitWriteLock();
            return true;
        }
        finally
        {
            _fileCacheLockSlim.ExitUpgradeableReadLock();
        }
    }
    
    private CachedFileWithRefCounter<TMetadata,TKey> GetOrAddFileToCache(TKey id, Func<TKey,ListDataFile<TMetadata>> add)
    {
        try
        {
            _fileCacheLockSlim.EnterUpgradeableReadLock();
            var exist = _fileCache.FirstOrDefault(_ => _.Id.Equals(id));
            if (exist != null)
            {
                Logger.Trace($"Return file '{id}' from cache");
                exist.AddRef();
                return exist;
            }
            else
            {
                _fileCacheLockSlim.EnterWriteLock();
                var file = add(id);
                var wrapper = new CachedFileWithRefCounter<TMetadata, TKey>(file, id);
                wrapper.AddRef();
                Logger.Trace($"Add file '{id}' to cache");
                _fileCache.Add(wrapper);
                _fileCacheLockSlim.ExitWriteLock();
                return wrapper;
            }
        }
        finally
        {
            _fileCacheLockSlim.ExitUpgradeableReadLock();
        }
    }
    
    private void CheckFileCacheForRottenFiles(long delay)
    {
        if (Interlocked.CompareExchange(ref _checkCacheInProgress,1,0) != 0) return;
        try
        {
            _fileCacheLockSlim.EnterUpgradeableReadLock();
            if (_fileCache.Count == 0) return;
            var list = _fileCache.Where(file=>file.RefCount == 0).Where(file => DateTime.Now - file.DeadTimeBegin > _fileCacheTime).ToImmutableList();
            if (list.Count == 0) return;
            _fileCacheLockSlim.EnterWriteLock();
            foreach (var file in list)
            {
                file.ImmediateDispose();
                _fileCache.Remove(file);
                Logger.Trace("Remove file from cache: {0}", file.Id);
            }
            _fileCacheLockSlim.ExitWriteLock();
        }
        finally
        {
            _fileCacheLockSlim.ExitUpgradeableReadLock();
            Interlocked.Exchange(ref _checkCacheInProgress, 0);
        }
    }
    
    #endregion

    public bool MoveFolder(TKey id, TKey newParentId)
    {
        lock (_sync)
        {
            Logger.Info("Move folder '{0}' to '{1}'", id, newParentId);
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.Folder) return false;
            if (_entries.TryGetValue(newParentId, out var newParent) == false) return false;
            if (newParent.Type != StoreEntryType.Folder) return false;
            var newFullPath = Path.Combine(newParent.FullPath, entry.Name);
            Directory.Move(entry.FullPath, newFullPath);
            InternalUpdateEntries();
            return true;
        }
    }

    public bool ExistEntry(TKey id)
    {
        lock (_sync)
        {
            return _entries.ContainsKey(id);
        }
    }

    public TKey RootFolderId => _keyConverter.RootFolderId;

    public IReadOnlyList<TKey> GetFolders()
    {
        lock (_sync)
        {
            return _entries.Where(x => x.Value.Type == StoreEntryType.Folder).Select(x=>x.Key).ToImmutableList();
        }
    }

    public bool TryGetEntry(TKey id,[MaybeNullWhen(false)] out IListDataStoreEntry<TKey>? entry)
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

    public TKey CreateFolder(TKey parentId, string name)
    {
        lock (_sync)
        {
            var rootFolder = _rootFolder;
            if (_entries.TryGetValue(parentId, out var parent))
            {
                rootFolder = parent.FullPath;
                parentId = parent.Id;
            }
            var newFolderPath = Path.Combine(rootFolder, name);
            var folderInfo = new DirectoryInfo(newFolderPath);

            if (_keyConverter.TryGetFolderKey(folderInfo, out var newFolderId) == false)
            {
                Logger.Error("Couldn't create folder {0}", name);
                throw new InvalidOperationException($"Couldn't create folder {name}");
            }
            var newEntry = new ListDataStoreEntry<TKey>
            {
                Id = newFolderId,
                Name = name,
                Type = StoreEntryType.Folder,
                FullPath = newFolderPath,
                ParentId = parentId,
            };
            if (_entries.Where(x => _keyConverter.KeyComparer.Equals(x.Value.ParentId, parentId))
                .Any(x => x.Value.Name == name))
            {
                Logger.Error("Folder {0} already exist",name);
                throw new ListDataFolderAlreadyExistException(name);
            }
            Logger.Info("Create folder {0}",name);
            _entries.Add(newFolderId,newEntry);
            Directory.CreateDirectory(newFolderPath);
            
            return newFolderId;
        }
    }


    public bool DeleteFolder(TKey id)
    {
        lock (_sync)
        {
            Logger.Info("Delete folder {0}",id);
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.Folder) return false;
            var files = InternalGetSubFiles(id);
            foreach (var file in files)
            {
                if (TryImmediatelyRemoveFromCache(file) == false)
                {
                    throw new Exception("Can't rename folder: file in folder currently in use");
                }
            }
            Directory.Delete(entry.FullPath,true);
            InternalUpdateEntries();
            return true;
        }
    }

    private IEnumerable<TKey> InternalGetSubFiles(TKey folderId)
    {
        if (_entries.TryGetValue(folderId, out var folder) == false) yield break;
        foreach (var item in _entries)
        {
            if (item.Value.ParentId.Equals(folder.Id) == false) continue;
            if (item.Value.Type == StoreEntryType.File)
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

    public bool ExistFolder(TKey id)
    {
        lock (_sync)
        {
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.Folder) return false;
            return true;
        }
    }

    public bool RenameFolder(TKey id, string newName)
    {
        lock (_sync)
        {
            Logger.Info($"Rename folder {id} to {newName}");
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.Folder) return false;
            var rootFolder = Path.GetDirectoryName(entry.FullPath);
            var newFolderName = rootFolder != null ? Path.Combine(rootFolder, newName) : newName;
            var files = InternalGetSubFiles(id);
            foreach (var file in files)
            {
                if (TryImmediatelyRemoveFromCache(file))
                {
                    throw new Exception("Can't rename folder: file in folder currently in use");
                }
            }
            Directory.Move(entry.FullPath,newFolderName);
            InternalUpdateEntries();
            return true;
        }
    }

    public IReadOnlyList<IListDataStoreEntry<TKey>> GetFiles()
    {
        lock (_sync)
        {
            return _entries.Where(x => x.Value.Type == StoreEntryType.File)
                .Select(x=>x.Value).ToImmutableList();
        }
    }

    public bool TryGetFile(TKey id, out IListDataStoreEntry<TKey> entry)
    {
        entry = default;
        lock (_sync)
        {
            if (!_entries.TryGetValue(id, out var commonEntry)) return false;
            if (commonEntry.Type != StoreEntryType.File) return false;
            entry = commonEntry;
            return true;

        }
    }

    public ICachedFileWithRefCounter<TMetadata> Open(TKey id)
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

            return GetOrAddFileToCache(id,
                _ => new ListDataFile<TMetadata>(
                    File.Open(fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite), _fileFormat, true));
        }
    }

    public ICachedFileWithRefCounter<TMetadata> Create(TKey id, TKey parentId, Action<TMetadata> defaultMetadata)
    {
        lock (_sync)
        {
            if (_entries.ContainsKey(id))
            {
                Logger.Error("Entry {0} already exist",id);
                throw new Exception($"Entry {id} already exist");
            }
            
            var rootFolder = _rootFolder;
            if (_entries.TryGetValue(parentId, out var parent) == false)
            {
                parentId = _keyConverter.RootFolderId;
                rootFolder = _rootFolder;
            }
            else
            {
                rootFolder = parent.FullPath;
            }
                
            var name = _keyConverter.GetFileNameByKey(id);
            var fileInfo = new FileInfo(Path.Combine(rootFolder, name));
            if (fileInfo.Exists)
            {
                Logger.Error("File [{0}]{1} already exist", id, fileInfo.FullName);
                throw new Exception($"File [{id}]{fileInfo.FullName} already exist");
            }
            var file = new ListDataFile<TMetadata>(File.Open(fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite), _fileFormat, true);
            file.EditMetadata(defaultMetadata);
            _entries.Add(id, new ListDataStoreEntry<TKey>
            {
                Id = id,
                Name = _keyConverter.GetDisplayName(fileInfo,_fileFormat,file.ReadMetadata()),
                Type = StoreEntryType.File,
                ParentId = parentId,
                FullPath = fileInfo.FullName,
            });
            return GetOrAddFileToCache(id,_=>file);
        }
    }

    public bool DeleteFile(TKey id)
    {
        lock (_sync)
        {   
            Logger.Info("Delete file {0}",id);
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.File) return false;
            // we need to remove file from cache, if we want to modify it
            if (TryImmediatelyRemoveFromCache(id) == false) throw new Exception("Can't delete file: it is currently in use");
            File.Delete(entry.FullPath);
            _entries.Remove(id);
            return true;
        }
    }

    public bool ExistFile(TKey id)
    {
        lock (_sync)
        {
            return _entries.ContainsKey(id);
        }
    }

    public bool RenameFile(TKey id, string newName)
    {
        lock (_sync)
        {
            Logger.Info("Rename file {0} to {1}",id,newName);
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.File) return false;
            var rootFolder = Path.GetDirectoryName(entry.FullPath);
            var newFileName = rootFolder != null ? Path.Combine(rootFolder, newName) : newName;
            // we need to remove file from cache, if we want to modify it
            if (TryImmediatelyRemoveFromCache(id) == false) throw new Exception("Can't rename file: it is currently in use");
            File.Move(entry.FullPath,newFileName);
            entry.Name = newName;
            entry.FullPath = newFileName;
            return true;
        }
    }

    public bool MoveFile(TKey id, TKey newParentId)
    {
        lock (_sync)
        {
            Logger.Info("Move file {0} to {1}",id,newParentId);
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.File) return false;
            if (_entries.TryGetValue(newParentId, out var newParent) == false) return false;
            if (newParent.Type != StoreEntryType.Folder) return false;
            var newFileName = Path.Combine(newParent.FullPath, entry.Name);
            // we need to remove file from cache, if we want to modify it
            if (TryImmediatelyRemoveFromCache(id) == false) throw new Exception("Can't move file: it is currently in use");
            File.Move(entry.FullPath,newFileName);
            entry.FullPath = newFileName;
            entry.ParentId = newParentId;
            return true;
        }
    }
    
   
}

public class ListDataStoreEntry<TKey> : IListDataStoreEntry<TKey>
{
    public TKey Id { get; set; }
    public string Name { get;set; }
    public StoreEntryType Type { get;set; }
    public TKey ParentId { get;set; }
    public string FullPath { get;set; }
}

