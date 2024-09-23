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
using System.IO.Pipelines;
using DotNext.Buffers;
using DotNext.IO;

namespace Asv.Mavlink;

public class FtpDirectory : IFtpEntry
{
    public static readonly IFtpEntry Root = new FtpDirectory();
    
    public const char Separator = '/';
    public FtpDirectory(IFtpEntry parent, ReadOnlySpan<char> data)
    {
        Name =  data.ToString();
        ParentPath = parent.Path;
        Path = $"{ParentPath}{Separator}{Name}";
    }

    private FtpDirectory()
    {
        Name =  string.Empty;
        ParentPath = string.Empty;
        Path = $"{ParentPath}{Separator}{Name}";
    }
    public string ParentPath { get; }
    public string Path { get; }
    public string Name { get; }
    public FtpEntryType Type => FtpEntryType.Directory;
    public uint Size { get; } = 0;

    public override string ToString()
    {
        return Path;
    }
}
public class FtpFile : IFtpEntry
{
    public FtpFile(IFtpEntry parent, ReadOnlySpan<char> data)
    {
        Name = data.ToString();
        ParentPath = parent.Path;
        Path = $"{ParentPath}{FtpDirectory.Separator}{Name}";
    }
    public string ParentPath { get; }
    public string Path { get; }
    public string Name { get; }
    public FtpEntryType Type => FtpEntryType.File;
    public uint Size { get; }
    public override string ToString()
    {
        return Path;
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
        _entryCache = new SourceCache<IFtpEntry, string>(x => x.Path);
        _entryCache.AddOrUpdate(FtpDirectory.Root);
    }
    
    public IFtpClient Base { get; }
    public IObservable<IChangeSet<IFtpEntry, string>> Entries => _entryCache.Connect();

    public async Task RefreshEntries(IFtpEntry entry, CancellationToken cancel = default)
    {
        var offset = 0U;
        using var buffer = new SparseBufferWriter<char>();
        var parsedItems = new List<IFtpEntry>();
        while (true)
        {
            try
            {
                var size = await Base.ListDirectory(entry.Path, offset++, buffer, cancel).ConfigureAwait(false);
                while (ParseEntry(entry, new SequenceReader<char>(buffer.ToReadOnlySequence()), out var parsed))
                {
                    Debug.Assert(parsed != null);
                    _entryCache.AddOrUpdate(parsed);
                    if (parsed.Type == FtpEntryType.Directory)
                    {
                        parsedItems.Add(parsed);    
                    }
                }
                if (size < MavlinkFtpHelper.MaxDataSize) break;
            }
            catch (FtpEndOfFileException)
            {
                break;
            }
        }

        foreach (var item in parsedItems)
        {
            await RefreshEntries(item, cancel).ConfigureAwait(false);
        }
    }

    private static bool ParseEntry(IFtpEntry parent, SequenceReader<char> rdr, out IFtpEntry? entry)
    {
        while (true)
        {
            if (rdr.TryReadTo(out ReadOnlySpan<char> line, '\0') == false) continue;
            if (line.Length == 0) continue;
            switch (line[0])
            {
                case 'D':
                    line = line[1..].Trim('.');
                    if (line.Length == 0) continue;
                    entry = new FtpDirectory(parent, line);
                    return true;
                case 'F':
                    line = line[1..].Trim('.');
                    if (line.Length == 0) continue;
                    entry = new FtpFile(parent, line);
                    return true;
            }
        }
        return false;
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