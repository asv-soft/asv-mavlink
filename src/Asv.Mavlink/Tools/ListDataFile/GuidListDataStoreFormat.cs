#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Asv.Mavlink;

public class GuidListDataStoreFormat : ListDataStoreFormat<Guid>
{
  
    class Comp : IEqualityComparer<Guid>
    {
        public bool Equals(Guid x, Guid y)
        {
            return x == y;
        }
        public int GetHashCode(Guid obj)
        {
            return obj.GetHashCode();
        }
    }

    private static IEqualityComparer<Guid> GuidComparer { get; } = new Comp();
    
    private readonly MD5 _md5 = MD5.Create();

    public override bool TryGetFolderKey(DirectoryInfo directory, out Guid id)
    {
        var hash = _md5.ComputeHash(Encoding.UTF8.GetBytes(directory.FullName));
        id = new Guid(hash);
        return true;
    }

    public override string GetFolderName(DirectoryInfo folderInfo)
    {
        return folderInfo.Name;
    }

    public override Guid RootFolderId => Guid.Empty;

    public override bool TyrGetFileKey(FileInfo file, out Guid id)
    {
        var str = file.Name;
        return Guid.TryParse(str, out id);
    }

    public override string GetFileName(FileInfo fileInfo)
    {
        return fileInfo.Name;
    }

    public override IEqualityComparer<Guid> KeyComparer => GuidComparer;

    public override void Dispose() => _md5.Dispose();
}