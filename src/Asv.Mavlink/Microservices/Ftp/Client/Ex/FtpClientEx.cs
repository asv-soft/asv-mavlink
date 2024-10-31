using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class FtpClientEx : IFtpClientEx, IDisposable, IMavlinkMicroserviceClient
{
    private readonly ILogger _logger;
    private readonly SourceCache<IFtpEntry,string> _entryCache;
    private static readonly TimeSpan DefaultBurstTimeout = TimeSpan.FromSeconds(5);
    
    public FtpClientEx(IFtpClient client)
    {
        _logger = client.Core.Log.CreateLogger<FtpClientEx>();
        Base = client;
        _entryCache = new SourceCache<IFtpEntry, string>(x => x.Path);
    }
    public string Name => $"{Base.Name}Ex";
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
            // TODO: add timeout handle
            
            // start burst read
            await Base.BurstReadFile(new ReadRequest(file.Session, 0, partSize), p =>
            {
                var offset = p.ReadOffset();
                Debug.Assert(offsetsToRead.Remove(offset));
                var size = p.ReadSize();
                p.ReadData(buffer);
                progress.Report(Interlocked.Add(ref total, size) / (double)file.Size);
                lock (buffer)
                {
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

    public void Dispose()
    {
        _entryCache.Dispose();
    }

    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
}