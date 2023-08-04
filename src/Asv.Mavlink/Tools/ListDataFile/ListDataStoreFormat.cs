#nullable enable
using System;
using System.Collections.Generic;
using System.IO;

namespace Asv.Mavlink;

public abstract class ListDataStoreFormat<TKey>:IDisposable
{
    
    public abstract TKey RootFolderId { get; }
    public abstract bool TyrGetFileKey(FileInfo file, out TKey id);
    public abstract string GetFileName(FileInfo fileInfo);
    public abstract IEqualityComparer<TKey> KeyComparer { get; }
    public abstract bool TryGetFolderKey(DirectoryInfo directory, out TKey id);
    public abstract string GetFolderName(DirectoryInfo folderInfo);
    public abstract void Dispose();
}