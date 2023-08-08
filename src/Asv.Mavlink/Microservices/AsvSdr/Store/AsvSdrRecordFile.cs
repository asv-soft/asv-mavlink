using System;
using System.IO;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class AsvSdrRecordStore : ListDataStore<AsvSdrRecordFileMetadata, Guid>, IAsvSdrStore
{
    public static AsvSdrListDataStoreFormat StoreFormat { get; } = new ();
    
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
        MetadataMaxSize = 78  /*size of AsvSdrRecordPayload */ + sizeof(ushort) /* size of tag list */ + 100 * 57 /* max 100 tag * size of AsvSdrRecordTagPayload */, 
        ItemMaxSize = 256,
    };

    public AsvSdrRecordFile(Stream stream, bool disposeSteam) : base(stream, FileFormat, disposeSteam)
    {
        
    }
}