#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Asv.Mavlink;

public class AsvSdrListDataStoreFormat : ListDataStoreFormat<Guid, AsvSdrRecordFileMetadata>
{
    private const string DefaultExtension = ".sdr";
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

  
    public override IEqualityComparer<Guid> KeyComparer => GuidComparer;

    public override void Dispose() => _md5.Dispose();
    
    public override bool TyrGetFileKey(FileInfo fileInfo, ListDataFileFormat header, AsvSdrRecordFileMetadata metadata,
        out Guid id)
    {
        var ext = Path.GetExtension(fileInfo.Name);
        if (ext != DefaultExtension)
        {
            id = default;
            return false;
        }

        var name = Path.GetFileNameWithoutExtension(fileInfo.Name);
        if (Guid.TryParse(name, out id) == false) return false;
        var metadataGuid = MavlinkTypesHelper.GetGuid(metadata.Info.RecordGuid);
        return metadataGuid == id;
    }

    

    public override string GetFileNameByKey(Guid id)
    {
        return $"{id:N}{DefaultExtension}";
    }

    public override string GetDisplayName(FileInfo fileInfo, ListDataFileFormat header, AsvSdrRecordFileMetadata metadata)
    {
        return MavlinkTypesHelper.GetString(metadata.Info.RecordName);
    }
}