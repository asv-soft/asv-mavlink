using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public class FtpDirectory : IFtpEntry
{
    public FtpDirectory(string name, string parentPath)
    {
        ParentPath = parentPath;
        Name = name;
        Path = $"{ParentPath}{Name}{MavlinkFtpHelper.DirectorySeparator}";
    }
    public FtpDirectory(string name)
        :this(name,string.Empty)
    {
        
    }
    
    public string ParentPath { get; }
    public string Path { get; }
    public string Name { get; }
    public FtpEntryType Type => FtpEntryType.Directory;

    public override string ToString() => $"[D] {Path}";
}
public class FtpFile : IFtpEntry
{
    public FtpFile(string name, uint size, string parentPath)
    {
        Name = name;
        Size = size;
        ParentPath = parentPath;
        Path = $"{ParentPath}{Name}";
    }
    public string ParentPath { get; }
    public string Path { get; }
    public string Name { get; }
    public FtpEntryType Type => FtpEntryType.File;
    public uint Size { get; }
    public override string ToString() => $"[F] {Path} (size: {Size})";
}

public class FtpClientEx : IFtpClientEx
{
    private readonly ILogger _logger;
    private readonly SourceCache<IFtpEntry,string> _entryCache;
    
    public FtpClientEx(IFtpClient @base, ILogger? logger = null)
    {
        _logger = logger ?? NullLogger.Instance;
        Base = @base;
        _entryCache = new SourceCache<IFtpEntry, string>(x => x.Path);
    }

    public IFtpClient Base { get; }
    public IObservable<IChangeSet<IFtpEntry, string>> Entries => _entryCache.Connect();

    public async Task Refresh(string path, bool recursive = true, CancellationToken cancel = default)
    {
        var currentEntry = MavlinkFtpHelper.CreateFtpDirectoryFromPath(path);
        _entryCache.AddOrUpdate(currentEntry);
        var offset = 0U;
        var parsedItems = new List<IFtpEntry>();
        var array = ArrayPool<char>.Shared.Rent(MavlinkFtpHelper.FtpEncoding.GetMaxCharCount(MavlinkFtpHelper.MaxDataSize));
        try
        {
            while (true)
            {
                try
                {
                    byte charSize;
                    if (MavlinkFtpHelper.IsRootPath(path))
                    {
                        charSize = await Base.ListDirectory(path, offset, array, cancel).ConfigureAwait(false);
                    }
                    else
                    {
                        charSize = await Base.ListDirectory(path.TrimEnd(MavlinkFtpHelper.DirectorySeparator), offset, array, cancel).ConfigureAwait(false);    
                    }
                    
                    var seq = new ReadOnlySequence<char>(array, 0, charSize);
                    offset += ParseEntries(currentEntry.Path, seq, parsedItems);
                }
                catch (FtpEndOfFileException)
                {
                    break;
                }
            }
        }
        finally
        {
            ArrayPool<char>.Shared.Return(array);
        }
        
        if (recursive)
        {
            foreach (var item in parsedItems)
            {
                await Refresh(item.Path,recursive, cancel).ConfigureAwait(false);
            }    
        }
        
    }

    private uint ParseEntries(string parentPath, ReadOnlySequence<char> data, List<IFtpEntry> parsedDirectoryItems)
    {
        var rdr = new SequenceReader<char>(data);
        var count = 0U;
        while (MavlinkFtpHelper.ParseFtpEntry(ref rdr,parentPath, out var entry))
        {
            count++;
            Debug.Assert(entry != null);
            if (MavlinkFtpHelper.IgnorePaths.Contains(entry.Name)) continue;
            _entryCache.AddOrUpdate(entry);
            if (entry.Type == FtpEntryType.Directory)
            {
                parsedDirectoryItems.Add(entry);
            }
        }
        return count;
    }

    public async Task DownloadFile(string filePath, IBufferWriter<byte> streamToSave, IProgress<double>? progress = null, CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        var file = await Base.OpenFileRead(filePath, cancel).ConfigureAwait(false);
        var skip = 0;
        var take = MavlinkFtpHelper.MaxDataSize;
        progress.Report(0);
        try
        {
            while (true)
            {
                if (file.Size - skip < take)
                {
                    take = (byte)(file.Size - skip);
                }
                var request = new ReadRequest(file.Session, (uint)skip, take);
                try
                {
                    var result = await Base.ReadFile(request, streamToSave, cancel).ConfigureAwait(false);
                    skip += result.ReadCount;
                    progress.Report((double)skip / file.Size);
                    
                }
                catch (FtpEndOfFileException e)
                {
                    break;
                }
            }
        }
        finally
        {
            await Base.TerminateSession(file.Session, cancel).ConfigureAwait(false);
        }
    }
    public async Task DownloadFile(string filePath,Stream streamToSave, IProgress<double>? progress = null, CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        var file = await Base.OpenFileRead(filePath, cancel).ConfigureAwait(false);
        var skip = 0;
        var take = MavlinkFtpHelper.MaxDataSize;
        progress.Report(0);
        var buffer = ArrayPool<byte>.Shared.Rent(MavlinkFtpHelper.MaxDataSize);
        try
        {
            while (true)
            {
                if (file.Size - skip < take)
                {
                    take = (byte)(file.Size - skip);
                }
                var request = new ReadRequest(file.Session, (uint)skip, take);
                try
                {
                    var mem = new Memory<byte>(buffer, 0, take);
                    var result = await Base.ReadFile(request, mem, cancel).ConfigureAwait(false);
                    await streamToSave.WriteAsync(mem, cancel).ConfigureAwait(false);
                    skip += result.ReadCount;
                    progress.Report((double)skip / file.Size);
                    
                }
                catch (FtpEndOfFileException e)
                {
                    break;
                }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
            await Base.TerminateSession(file.Session, cancel).ConfigureAwait(false);
        }
        
    }
}