using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;

namespace Asv.Mavlink;

public enum FtpEntryType
{
    File,
    Directory
}

public interface IFtpEntry
{
    string ParentPath { get; }
    string Path { get; }
    string Name { get; }
    FtpEntryType Type { get; }
}



public interface IFtpClientEx
{
    IFtpClient Base { get; }
    IReadOnlyObservableDictionary<string, IFtpEntry> Entries { get; }
    Task Refresh(string path, bool recursive = true, CancellationToken cancel = default);
    Task DownloadFile(string filePath, Stream streamToSave, IProgress<double>? progress = null,byte partSize = MavlinkFtpHelper.MaxDataSize,
        CancellationToken cancel = default);
    Task DownloadFile(string filePath, IBufferWriter<byte> bufferToSave, IProgress<double>? progress = null,byte partSize = MavlinkFtpHelper.MaxDataSize,
        CancellationToken cancel = default);
    Task UploadFile(string filePath, Stream streamToUpload, IProgress<double>? progress = null,
        CancellationToken cancel = default);
    Task<int> BurstDownloadFile(string filePath, Stream streamToSave, IProgress<double>? progress = null,
        byte partSize = MavlinkFtpHelper.MaxDataSize, CancellationToken cancel = default);
    Task<int> BurstDownloadFile(string filePath, IBufferWriter<byte> bufferToSave, IProgress<double>? progress = null,
        byte partSize = MavlinkFtpHelper.MaxDataSize, CancellationToken cancel = default);
    Task RemoveDirectory(string path, bool recursive = true, CancellationToken cancel = default);
}