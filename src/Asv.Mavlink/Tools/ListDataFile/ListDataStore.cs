#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Asv.Common;
using Asv.IO;
using NLog;

namespace Asv.Mavlink;

public class ListDataStore<TMetadata, TKey> : DisposableOnceWithCancel, IListDataStore<TMetadata, TKey> 
    where TMetadata : ISizedSpanSerializable, new() where TKey : notnull
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly string _rootFolder;
    private readonly ListDataStoreFormat<TKey,TMetadata> _keyConverter;
    private readonly ListDataFileFormat _fileFormat;
    private readonly Dictionary<TKey,ListDataStoreEntry<TKey>> _entries;
    private readonly object _sync = new();
    private readonly RxValue<ushort> _count;
    private readonly RxValue<ulong> _size;
    

    public ListDataStore(string rootFolder, ListDataStoreFormat<TKey,TMetadata> keyConverter, ListDataFileFormat fileFormat)
    {
        if (string.IsNullOrEmpty(rootFolder))
            throw new ArgumentException("Value cannot be null or empty.", nameof(rootFolder));
        _rootFolder = rootFolder;
        _keyConverter = keyConverter ?? throw new ArgumentNullException(nameof(keyConverter));
        _keyConverter.DisposeItWith(Disposable);
        _fileFormat = fileFormat ?? throw new ArgumentNullException(nameof(fileFormat));
        _entries = new Dictionary<TKey, ListDataStoreEntry<TKey>>(keyConverter.KeyComparer);
        _count = new RxValue<ushort>().DisposeItWith(Disposable);
        _size = new RxValue<ulong>().DisposeItWith(Disposable);
        
        InternalUpdateEntries();
        if (Directory.Exists(_rootFolder) == false)
        {
            Directory.CreateDirectory(_rootFolder);
        }
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

        foreach (var file in Directory.EnumerateFiles(_rootFolder))
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

    

    public bool MoveFolder(TKey id, TKey newParentId)
    {
        lock (_sync)
        {
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
                throw new ListDataFolderAlreadyExistException(name);
            }
            
            _entries.Add(newFolderId,newEntry);
            Directory.CreateDirectory(newFolderPath);
            
            return newFolderId;
        }
    }


    public bool DeleteFolder(TKey id)
    {
        lock (_sync)
        {
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.Folder) return false;
            Directory.Delete(entry.FullPath,true);
            InternalUpdateEntries();
            return true;
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
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.Folder) return false;
            var rootFolder = Path.GetDirectoryName(entry.FullPath);
            var newFolderName = rootFolder != null ? Path.Combine(rootFolder, newName) : newName;
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

    public IListDataFile<TMetadata> Open(TKey id)
    {
        lock (_sync)
        {
            if (_entries.TryGetValue(id, out var entry) == false)
            {
                throw new FileNotFoundException($"File with [{id}] not found");
            }
            
            var fileInfo =  new FileInfo(entry.FullPath);
            if (fileInfo.Exists == false)
            {
                throw new FileNotFoundException($"File with [{id}]{fileInfo.FullName} not found", fileInfo.FullName);
            }
            var file = new ListDataFile<TMetadata>(File.Open(fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite), _fileFormat, true);
            return file;
        }
    }

    public IListDataFile<TMetadata> Create(TKey id, TKey parentId, Action<TMetadata> defaultMetadata)
    {
        lock (_sync)
        {
            if (_entries.ContainsKey(id))
            {
                throw new Exception($"Entry {id} already exist");
            }
            
            var rootFolder = _rootFolder;
            if (_entries.TryGetValue(parentId, out var parent) == false)
            {
                parentId = _keyConverter.RootFolderId;
                rootFolder = _rootFolder;
            }
            var name = _keyConverter.GetFileNameByKey(id);
            var fileInfo = new FileInfo(Path.Combine(rootFolder, name));
            if (fileInfo.Exists)
            {
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
            return file;
        }
    }

    public bool DeleteFile(TKey id)
    {
        lock (_sync)
        {   
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.File) return false;
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
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.File) return false;
            var rootFolder = Path.GetDirectoryName(entry.FullPath);
            var newFileName = rootFolder != null ? Path.Combine(rootFolder, newName) : newName;
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
            if (_entries.TryGetValue(id, out var entry) == false) return false;
            if (entry.Type != StoreEntryType.File) return false;
            if (_entries.TryGetValue(newParentId, out var newParent) == false) return false;
            if (newParent.Type != StoreEntryType.Folder) return false;
            var newFileName = Path.Combine(newParent.FullPath, entry.Name);
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