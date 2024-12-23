#nullable enable
using System;
using System.IO;
using System.IO.Abstractions;
using Asv.IO;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class AsvSdrListDataStoreFormat() : GuidHierarchicalStoreFormat<IListDataFile<AsvSdrRecordFileMetadata>>(".sdr")
{
    public override IListDataFile<AsvSdrRecordFileMetadata> OpenFile(Stream stream)
    {
        return new ListDataFile<AsvSdrRecordFileMetadata>(stream, AsvSdrHelper.FileFormat, true);
    }

    public override IListDataFile<AsvSdrRecordFileMetadata> CreateFile(Stream stream, Guid id, string name)
    {
        var file = new ListDataFile<AsvSdrRecordFileMetadata>(stream, AsvSdrHelper.FileFormat, true);
        file.EditMetadata(metadata=>
        {
            MavlinkTypesHelper.SetGuid(metadata.Info.RecordGuid, id);
            MavlinkTypesHelper.SetString(metadata.Info.RecordName,name);
        });
        return file;
    }

    public override void Dispose()
    {
        
    }
}

public class AsvSdrStore : FileSystemHierarchicalStore<Guid, IListDataFile<AsvSdrRecordFileMetadata>>
{
    public AsvSdrStore(string rootFolder, 
        TimeSpan? fileCacheTime, 
        ILoggerFactory? logFactory,
        TimeProvider? timeProvider, 
        IFileSystem? fileSystem) 
        : base(rootFolder, AsvSdrHelper.StoreFormat, fileCacheTime, logFactory, timeProvider,fileSystem)
    {
        
    }
}

