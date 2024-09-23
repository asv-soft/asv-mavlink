using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;

namespace Asv.Mavlink;

public enum FtpEntryType
{
    File,
    Directory
}

public interface IFtpEntry
{
    string ParentId { get; }
    string Id { get; }
    string Name { get; }
    FtpEntryType Type { get; }
    uint Size { get; }
    int Level { get;  }
}



public interface IFtpClientEx
{
    IFtpEntry RootEntry { get; }
    IFtpClient Base { get; }
    IObservable<IChangeSet<IFtpEntry,string>> Entries { get; }
    Task RefreshEntries(IFtpEntry entry, bool recursive = true, CancellationToken cancel = default);
    Task DownloadFile(string filePath, Stream streamToSave, IProgress<double>? progress = null,
        CancellationToken cancel = default);
}