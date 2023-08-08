#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using Asv.IO;

namespace Asv.Mavlink;

public abstract class ListDataStoreFormat<TKey,TMetadata>:IDisposable
    where TMetadata : ISizedSpanSerializable, new()
{
    
    public abstract TKey RootFolderId { get; }
    public abstract IEqualityComparer<TKey> KeyComparer { get; }
    public abstract bool TryGetFolderKey(DirectoryInfo directory, out TKey id);
    public abstract string GetFolderName(DirectoryInfo folderInfo);
    public abstract void Dispose();
    public abstract bool TyrGetFileKey(FileInfo fileInfo, ListDataFileFormat header, TMetadata metadata, out TKey id);
    public abstract string GetFileNameByKey(TKey id);
    public abstract string GetDisplayName(FileInfo fileInfo, ListDataFileFormat header, TMetadata metadata);

    
}