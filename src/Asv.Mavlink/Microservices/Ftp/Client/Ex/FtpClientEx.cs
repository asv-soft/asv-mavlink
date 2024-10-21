using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class FtpClientEx : IFtpClientEx
{
    private readonly TimeProvider _timeProvider;
    private readonly ILogger _logger;
    private readonly SourceCache<IFtpEntry,string> _entryCache;
    private static readonly TimeSpan DefaultBurstTimeout = TimeSpan.FromSeconds(5);
    
    public FtpClientEx(IFtpClient @base,TimeProvider? timeProvider = null, ILogger? logger = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;
        _logger = logger ?? NullLogger.Instance;
        Base = @base;
        _entryCache = new SourceCache<IFtpEntry, string>(x => x.Path);
    }

    public IFtpClient Base { get; }
    public IObservable<IChangeSet<IFtpEntry, string>> Entries => _entryCache.Connect();

    public async Task BurstDownloadFile(string filePath,Stream streamToSave, IProgress<double>? progress = null, byte partSize = MavlinkFtpHelper.MaxDataSize, CancellationToken cancel = default)
    {
        if (partSize > MavlinkFtpHelper.MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(nameof(partSize), $"Max data size is {MavlinkFtpHelper.MaxDataSize}");
        }
        progress ??= new Progress<double>();
        var file = await Base.OpenFileRead(filePath, cancel).ConfigureAwait(false);
        var total = 0U;
        progress.Report(0);
        
        
        var offsetsToRead = new HashSet<uint>((int)(file.Size / partSize +1));
        for (var i = 0; i < file.Size; i+=partSize)
        {
            offsetsToRead.Add((uint)i);
        }
        var buffer = ArrayPool<byte>.Shared.Rent(partSize);
        try
        {
            // start burst read
            await Base.BurstReadFile(new ReadRequest(file.Session, 0, partSize), p =>
            {
                lock (buffer)
                {
                    var offset = p.ReadOffset();
                    Debug.Assert(offsetsToRead.Remove(offset));
                    var size = p.ReadSize();
                    p.ReadData(buffer);
                    progress.Report(Interlocked.Add(ref total, size) / (double)file.Size);
                    streamToSave.Position = offset;
                    streamToSave.Write(buffer, 0, size);
                }
            }, cancel).ConfigureAwait(false);
           
            // check if we skip some data
            if (offsetsToRead.Count > 0)
            {
                _logger.ZLogWarning($"Burst read file {filePath} failed some packets (cnt {offsetsToRead.Count}). Try to read manually");
                foreach (var offset in offsetsToRead)
                {
                    var request = new ReadRequest(file.Session, offset, partSize);
                    try
                    {
                        var result2 = await Base.ReadFile(request, buffer, cancel).ConfigureAwait(false);
                        streamToSave.Position = offset;
                        await streamToSave.WriteAsync(buffer, 0, result2.ReadCount, cancel).ConfigureAwait(false);
                        progress.Report(Interlocked.Add(ref total,result2.ReadCount)/(double)file.Size);
                    }
                    catch (FtpNackEndOfFileException)
                    {
                        break;
                    }
                }
            }
            progress.Report(1);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
            await Base.TerminateSession(file.Session, cancel).ConfigureAwait(false);
        }
    }
    
    public async Task Refresh(string path, bool recursive = true, CancellationToken cancel = default)
    {
        var existingEntries = _entryCache.Items
            .Where(entry => entry.Path.StartsWith(path, StringComparison.OrdinalIgnoreCase))
            .Select(entry => entry.Path)
            .ToList();
        
        foreach (var entryPath in existingEntries)
        {
            _entryCache.RemoveKey(entryPath);
        }
        
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
                        // special case for root path: we don't need to trim '/' on root path
                        charSize = await Base.ListDirectory(path, offset, array, cancel).ConfigureAwait(false);
                    }
                    else
                    {
                        // path must be trimmed to avoid '/' at the end: e.g. /log/ -> /log
                        charSize = await Base.ListDirectory(path.TrimEnd(MavlinkFtpHelper.DirectorySeparator), offset, array, cancel).ConfigureAwait(false);    
                    }
                    
                    var seq = new ReadOnlySequence<char>(array, 0, charSize);
                    offset += ParseEntries(currentEntry.Path, seq, parsedItems);
                }
                catch (FtpNackEndOfFileException)
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

    
    public async Task DownloadFile(string filePath, IBufferWriter<byte> bufferToSave, IProgress<double>? progress = null, CancellationToken cancel = default)
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
                    var result = await Base.ReadFile(request, bufferToSave, cancel).ConfigureAwait(false);
                    skip += result.ReadCount;
                    progress.Report((double)skip / file.Size);
                    
                }
                catch (FtpNackEndOfFileException e)
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
                catch (FtpNackEndOfFileException e)
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
    
    public async Task UploadFile(string filePath, Stream streamToUpload, IProgress<double>? progress = null, CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        var writeHandle = await Base.OpenFileWrite(filePath, cancel).ConfigureAwait(false);
        var session = writeHandle.Session;
        var totalWritten = 0L;
        var buffer = ArrayPool<byte>.Shared.Rent(MavlinkFtpHelper.MaxDataSize);

        try
        {
            while (true)
            {
                int maxChunkSize = Math.Min(MavlinkFtpHelper.MaxDataSize, byte.MaxValue);
                var bytesRead = await streamToUpload.ReadAsync(buffer.AsMemory(0, maxChunkSize), cancel).ConfigureAwait(false);
                if (bytesRead == 0) break;

                var request = new WriteRequest(session, (uint)totalWritten, (byte)bytesRead);
                var memory = new Memory<byte>(buffer, 0, bytesRead);

                await Base.WriteFile(request, memory, cancel).ConfigureAwait(false);

                totalWritten += bytesRead;
                progress.Report((double)totalWritten / streamToUpload.Length);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
            await Base.TerminateSession(session, cancel).ConfigureAwait(false);
        }
    }
}
