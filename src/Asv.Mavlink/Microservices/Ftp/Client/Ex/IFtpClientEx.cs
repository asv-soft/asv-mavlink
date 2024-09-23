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
    string ParentPath { get; }
    string Path { get; }
    string Name { get; }
    FtpEntryType Type { get; }
    uint Size { get; }
}



public interface IFtpClientEx
{
    IFtpClient Base { get; }
    IObservable<IChangeSet<IFtpEntry,string>> Entries { get; }
    Task RefreshEntries(IFtpEntry entry, CancellationToken cancel = default);
    Task DownloadFile(string filePath, Stream streamToSave, IProgress<double>? progress = null,
        CancellationToken cancel = default);
}