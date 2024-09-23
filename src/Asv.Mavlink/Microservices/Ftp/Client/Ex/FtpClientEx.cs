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
    
    
    public FtpDirectory(string name)
    {
        ParentId = string.Empty;
        Name = name;
        Id = $"{Name}{MavlinkFtpHelper.DirectorySeparator}";
        Level = 0;
    }

    public int Level { get;  }

    public FtpDirectory(ReadOnlySpan<char> data,IFtpEntry parent)
    {
        Name =  data.Trim(MavlinkFtpHelper.DirectorySeparator).ToString();
        ParentId = parent.Id;
        Id = $"{ParentId}{Name}{MavlinkFtpHelper.DirectorySeparator}";
        Level = parent.Level + 1;
    }

    
    public string ParentId { get; }
    public string Id { get; }
    public string Name { get; }
    public FtpEntryType Type => FtpEntryType.Directory;
    public uint Size { get; } = 0;

    public override string ToString()
    {
        return Id;
    }
}
public class FtpFile : IFtpEntry
{
    public FtpFile(IFtpEntry parent, ReadOnlySpan<char> data)
    {
        var tabIndex = data.IndexOf(MavlinkFtpHelper.FileSizeSeparator);
        Name = data[..tabIndex].Trim(MavlinkFtpHelper.DirectorySeparator).ToString();
        Size = uint.Parse(data[(tabIndex + 1)..]);
        ParentId = parent.Id;
        Id = $"{ParentId}{Name}";
        Level = parent.Level + 1;
    }
    public string ParentId { get; }
    public string Id { get; }
    public string Name { get; }
    public FtpEntryType Type => FtpEntryType.File;
    public uint Size { get; }
    public int Level { get; }

    public override string ToString()
    {
        return Id;
    }
}

public class FtpClientEx : IFtpClientEx
{
    private readonly ILogger _logger;
    private readonly SourceCache<IFtpEntry,string> _entryCache;
    
    public FtpClientEx(IFtpClient @base, ILogger? logger = null)
    {
        _logger = logger ?? NullLogger.Instance;
        Base = @base;
        _entryCache = new SourceCache<IFtpEntry, string>(x => x.Id);
        _entryCache.AddOrUpdate(MavlinkFtpHelper.Root);
    }

    public IFtpEntry RootEntry => MavlinkFtpHelper.Root;
    public IFtpClient Base { get; }
    public IObservable<IChangeSet<IFtpEntry, string>> Entries => _entryCache.Connect();

    public async Task RefreshEntries(IFtpEntry entry, bool recursive = true, CancellationToken cancel = default)
    {
        if (MavlinkFtpHelper.IgnorePaths.Contains(entry.Name)) return;
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
                    if (entry == MavlinkFtpHelper.Root)
                    {
                        charSize = await Base.ListDirectory(entry.Id, offset, array, cancel).ConfigureAwait(false);
                    }
                    else
                    {
                        charSize = await Base.ListDirectory(entry.Id.TrimEnd(MavlinkFtpHelper.DirectorySeparator), offset, array, cancel).ConfigureAwait(false);    
                    }
                    
                    var seq = new ReadOnlySequence<char>(array, 0, charSize);
                    offset += ParseEntries(entry,seq,parsedItems);
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
                await RefreshEntries(item,recursive, cancel).ConfigureAwait(false);
            }    
        }
        
    }

    private uint ParseEntries(IFtpEntry entry, ReadOnlySequence<char> data, List<IFtpEntry> parsedDirectoryItems)
    {
        var rdr = new SequenceReader<char>(data);
        var count = 0U;
        while (ParseEntry(entry,ref rdr, out var parsed))
        {
            count++;
            Debug.Assert(parsed != null);
            _entryCache.AddOrUpdate(parsed);
            if (parsed.Type == FtpEntryType.Directory)
            {
                parsedDirectoryItems.Add(parsed);
            }
        }
        return count;
    }

    private static bool ParseEntry(IFtpEntry parent,ref SequenceReader<char> rdr, out IFtpEntry? entry)
    {
        entry = null;
        while (true)
        {
            if (rdr.TryReadTo(out ReadOnlySpan<char> line, MavlinkFtpHelper.PathSeparator) == false) return false;
            if (line.Length == 0) continue;
            switch (line[0])
            {
                case MavlinkFtpHelper.DirectoryChar:
                    line = line[1..];
                    if (line.Length == 0) continue;
                    entry = new FtpDirectory(line,parent);
                    return true;
                case MavlinkFtpHelper.FileChar:
                    line = line[1..];
                    if (line.Length == 0) continue;
                    entry = new FtpFile(parent, line);
                    return true;
            }
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