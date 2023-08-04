using System;
using System.IO;

namespace Asv.Mavlink;

public class AsvSdrRecordStore : ListDataStore<AsvSdrRecordFileMetadata, Guid>, IAsvSdrStore
{
    public static ListDataStoreFormat<Guid> StoreFormat { get; } = new GuidListDataStoreFormat();
    
    public AsvSdrRecordStore(string rootFolder) : base(rootFolder, StoreFormat, AsvSdrRecordFile.FileFormat)
    {
        
    }
}

public class AsvSdrRecordFile : ListDataFile<AsvSdrRecordFileMetadata>, IAsvSdrRecordFile
{
    public static readonly ListDataFileFormat FileFormat = new()
    {
        Version = "1.0.0",
        Type = "AsvSdrRecordFile",
        MetadataMaxSize = 1024 * 4,
        ItemMaxSize = 256,
    };

    public AsvSdrRecordFile(Stream stream, bool disposeSteam) : base(stream, FileFormat, disposeSteam)
    {
        
    }
}