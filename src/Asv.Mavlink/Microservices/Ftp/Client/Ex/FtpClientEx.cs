using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ObservableCollections;
using ZLogger;

namespace Asv.Mavlink;

/// <inheritdoc cref="IFtpClientEx" />
public class FtpClientEx : MavlinkMicroserviceClient, IFtpClientEx
{
    private readonly ILogger _logger;
    private readonly ObservableDictionary<string, IFtpEntry> _entryCache;
    private static readonly TimeSpan DefaultBurstTimeout = TimeSpan.FromSeconds(5);

    public FtpClientEx(IFtpClient client)
        : base(MavlinkFtpHelper.FtpMicroserviceExName, client.Identity, client.Core)
    {
        _logger = client.Core.LoggerFactory.CreateLogger<FtpClientEx>();
        Base = client;
        _entryCache = new ObservableDictionary<string, IFtpEntry>();
    }

    public IFtpClient Base { get; }
    public IReadOnlyObservableDictionary<string, IFtpEntry> Entries => _entryCache;

    public async Task<int> BurstDownloadFile(
        string filePath,
        Stream streamToSave,
        IProgress<double>? progress = null,
        byte partSize = MavlinkFtpHelper.MaxDataSize,
        CancellationToken cancel = default
    )
    {
        cancel.ThrowIfCancellationRequested();
        if (partSize > MavlinkFtpHelper.MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(
                nameof(partSize),
                $"Max data size is {MavlinkFtpHelper.MaxDataSize}"
            );
        }

        progress ??= new Progress<double>();
        var file = await Base.OpenFileRead(filePath, cancel).ConfigureAwait(false);
        var total = 0U;
        progress.Report(0);

        var offsetsToRead = new HashSet<uint>((int)(file.Size / partSize + 1));
        for (var i = 0; i < file.Size; i += partSize)
        {
            offsetsToRead.Add((uint)i);
        }

        var buffer = ArrayPool<byte>.Shared.Rent(partSize);
        try
        {
            // start burst read
            await Base.BurstReadFile(
                    new ReadRequest(file.Session, 0, partSize),
                    p =>
                    {
                        cancel.ThrowIfCancellationRequested();
                        var offset = p.ReadOffset();
                        offsetsToRead.Remove(offset);
                        var size = p.ReadSize();
                        p.ReadData(buffer);
                        progress.Report(Interlocked.Add(ref total, size) / (double)file.Size);
                        lock (buffer)
                        {
                            streamToSave.Position = offset;
                            streamToSave.Write(buffer, 0, size);
                        }
                    },
                    cancel
                )
                .ConfigureAwait(false);

            // check if we skip some data
            var manualDownloadCount = offsetsToRead.Count;
            if (offsetsToRead.Count > 0)
            {
                _logger.ZLogWarning(
                    $"Burst read file {filePath} failed some packets (cnt {offsetsToRead.Count}). Try to read manually"
                );
                foreach (var offset in offsetsToRead)
                {
                    cancel.ThrowIfCancellationRequested();
                    var request = new ReadRequest(file.Session, offset, partSize);
                    try
                    {
                        var result2 = await Base.ReadFile(request, buffer, cancel)
                            .ConfigureAwait(false);
                        streamToSave.Position = offset;
                        await streamToSave
                            .WriteAsync(buffer, 0, result2.ReadCount, cancel)
                            .ConfigureAwait(false);
                        progress.Report(
                            Interlocked.Add(ref total, result2.ReadCount) / (double)file.Size
                        );
                    }
                    catch (FtpNackEndOfFileException)
                    {
                        break;
                    }
                }
            }

            progress.Report(1);
            return manualDownloadCount;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
            await Base.TerminateSession(file.Session, cancel).ConfigureAwait(false);
        }
    }

    public async Task<int> BurstDownloadFile(
        string filePath,
        IBufferWriter<byte> bufferToSave,
        IProgress<double>? progress = null,
        byte partSize = MavlinkFtpHelper.MaxDataSize,
        CancellationToken cancel = default
    )
    {
        cancel.ThrowIfCancellationRequested();
        if (partSize > MavlinkFtpHelper.MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(
                nameof(partSize),
                $"Max data size is {MavlinkFtpHelper.MaxDataSize}"
            );
        }

        progress ??= new Progress<double>();
        var file = await Base.OpenFileRead(filePath, cancel).ConfigureAwait(false);
        var total = 0U;
        progress.Report(0);

        var offsetsToRead = new HashSet<uint>((int)(file.Size / partSize + 1));
        for (var i = 0; i < file.Size; i += partSize)
        {
            offsetsToRead.Add((uint)i);
        }

        try
        {
            // start burst read
            await Base.BurstReadFile(
                    new ReadRequest(file.Session, 0, partSize),
                    p =>
                    {
                        cancel.ThrowIfCancellationRequested();
                        var offset = p.ReadOffset();
                        offsetsToRead.Remove(offset);
                        var size = p.ReadSize();
                        p.ReadData(bufferToSave);
                        progress.Report(Interlocked.Add(ref total, size) / (double)file.Size);
                    },
                    cancel
                )
                .ConfigureAwait(false);

            // check if we skip some data
            var manualDownloadCount = offsetsToRead.Count;
            if (offsetsToRead.Count > 0)
            {
                _logger.ZLogWarning(
                    $"Burst read file {filePath} failed some packets (cnt {offsetsToRead.Count}). Try to read manually"
                );
                foreach (var offset in offsetsToRead)
                {
                    cancel.ThrowIfCancellationRequested();
                    var request = new ReadRequest(file.Session, offset, partSize);
                    try
                    {
                        var result2 = await Base.ReadFile(request, bufferToSave, cancel)
                            .ConfigureAwait(false);
                        progress.Report(
                            Interlocked.Add(ref total, result2.ReadCount) / (double)file.Size
                        );
                    }
                    catch (FtpNackEndOfFileException)
                    {
                        break;
                    }
                }
            }

            progress.Report(1);
            return manualDownloadCount;
        }
        finally
        {
            await Base.TerminateSession(file.Session, cancel).ConfigureAwait(false);
        }
    }

    public async Task Refresh(
        string path,
        bool recursive = true,
        CancellationToken cancel = default
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        cancel.ThrowIfCancellationRequested();

        var currentEntry = MavlinkFtpHelper.CreateFtpDirectoryFromPath(path);

        var listedEntries = new List<IFtpEntry>();
        var parsedItems = new List<IFtpEntry>();
        var offset = 0U;
        var array = ArrayPool<char>.Shared.Rent(
            MavlinkFtpHelper.FtpEncoding.GetMaxCharCount(MavlinkFtpHelper.MaxDataSize)
        );
        try
        {
            while (true)
            {
                cancel.ThrowIfCancellationRequested();
                try
                {
                    var charSize = await Base.ListDirectory(
                            currentEntry.Path,
                            offset,
                            array,
                            cancel
                        )
                        .ConfigureAwait(false);
                    var seq = new ReadOnlySequence<char>(array, 0, charSize);
                    offset += ParseEntries(currentEntry.Path, seq, listedEntries, parsedItems);
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

        var existingEntries = _entryCache
            .Where(entry => entry.Value.Path.StartsWith(currentEntry.Path, StringComparison.OrdinalIgnoreCase))
            .Select(entry => entry.Value.Path)
            .ToList();

        foreach (var entryPath in existingEntries)
        {
            _entryCache.Remove(entryPath);
        }

        MaterializeParents(currentEntry);
        _entryCache.Add(currentEntry.Path, currentEntry);
        foreach (var entry in listedEntries)
        {
            _entryCache.Add(entry.Path, entry);
        }

        if (recursive)
        {
            foreach (var item in parsedItems)
            {
                await Refresh(item.Path, recursive, cancel).ConfigureAwait(false);
            }
        }
    }

    private void MaterializeParents(IFtpEntry entry)
    {
        var parentPath = entry.ParentPath;
        while (!string.IsNullOrEmpty(parentPath) && !_entryCache.ContainsKey(parentPath))
        {
            var parent = MavlinkFtpHelper.CreateFtpDirectoryFromPath(parentPath);
            _entryCache.Add(parent.Path, parent);
            parentPath = parent.ParentPath;
        }
    }

    private static uint ParseEntries(
        string parentPath,
        ReadOnlySequence<char> data,
        List<IFtpEntry> listedEntries,
        List<IFtpEntry> parsedDirectoryItems
    )
    {
        var rdr = new SequenceReader<char>(data);
        var count = 0U;
        while (MavlinkFtpHelper.ParseFtpEntry(ref rdr, parentPath, out var entry))
        {
            count++;
            Debug.Assert(entry != null);
            if (MavlinkFtpHelper.IgnorePaths.Contains(entry.Name))
                continue;
            listedEntries.Add(entry);
            if (entry.Type == FtpEntryType.Directory)
            {
                parsedDirectoryItems.Add(entry);
            }
        }

        return count;
    }

    public async Task DownloadFile(
        string filePath,
        IBufferWriter<byte> bufferToSave,
        IProgress<double>? progress = null,
        byte partSize = MavlinkFtpHelper.MaxDataSize,
        CancellationToken cancel = default
    )
    {
        cancel.ThrowIfCancellationRequested();
        progress ??= new Progress<double>();
        var file = await Base.OpenFileRead(filePath, cancel).ConfigureAwait(false);
        var skip = 0;
        if (partSize > MavlinkFtpHelper.MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(
                nameof(partSize),
                $"Max data size is {MavlinkFtpHelper.MaxDataSize}"
            );
        }

        if (partSize == 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(partSize),
                "Part size must be greater than 0"
            );
        }

        var take = partSize;
        progress.Report(0);
        try
        {
            while (file.Size > skip)
            {
                cancel.ThrowIfCancellationRequested();
                if (file.Size - skip < take)
                {
                    take = (byte)(file.Size - skip);
                }

                var request = new ReadRequest(file.Session, (uint)skip, take);
                try
                {
                    var result = await Base.ReadFile(request, bufferToSave, cancel)
                        .ConfigureAwait(false);
                    skip += result.ReadCount;
                    progress.Report((double)skip / file.Size);
                }
                catch (FtpNackEndOfFileException)
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

    public async Task DownloadFile(
        string filePath,
        Stream streamToSave,
        IProgress<double>? progress = null,
        byte partSize = MavlinkFtpHelper.MaxDataSize,
        CancellationToken cancel = default
    )
    {
        cancel.ThrowIfCancellationRequested();
        progress ??= new Progress<double>();
        var file = await Base.OpenFileRead(filePath, cancel).ConfigureAwait(false);

        var skip = 0;
        if (partSize > MavlinkFtpHelper.MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(
                nameof(partSize),
                $"Max data size is {MavlinkFtpHelper.MaxDataSize}"
            );
        }

        if (partSize == 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(partSize),
                "Part size must be greater than 0"
            );
        }

        var take = partSize;
        progress.Report(0);
        var buffer = ArrayPool<byte>.Shared.Rent(MavlinkFtpHelper.MaxDataSize);
        try
        {
            while (file.Size > skip)
            {
                cancel.ThrowIfCancellationRequested();
                if (file.Size - skip < take)
                {
                    take = (byte)(file.Size - skip);
                }

                var request = new ReadRequest(file.Session, (uint)skip, take);
                try
                {
                    var mem = new Memory<byte>(buffer, 0, take);
                    var result = await Base.ReadFile(request, mem, cancel).ConfigureAwait(false);
                    await streamToSave
                        .WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, result.ReadCount), cancel)
                        .ConfigureAwait(false);
                    skip += result.ReadCount;
                    progress.Report((double)skip / file.Size);
                }
                catch (FtpNackEndOfFileException)
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

    public async Task UploadFile(
        string filePath,
        Stream streamToUpload,
        IProgress<double>? progress = null,
        CancellationToken cancel = default
    )
    {
        cancel.ThrowIfCancellationRequested();
        progress ??= new Progress<double>();
        var file = await Base.CreateFile(filePath, cancel).ConfigureAwait(false);
        var session = file.ReadSession();
        var totalWritten = 0u;
        var buffer = ArrayPool<byte>.Shared.Rent(MavlinkFtpHelper.MaxDataSize);

        try
        {
            while (true)
            {
                cancel.ThrowIfCancellationRequested();
                var bytesRead = (uint)
                    await streamToUpload
                        .ReadAsync(buffer.AsMemory(0, MavlinkFtpHelper.MaxDataSize), cancel)
                        .ConfigureAwait(false);
                if (bytesRead <= 0)
                    break;

                var request = new WriteRequest(session, totalWritten, (byte)bytesRead);
                var memory = new Memory<byte>(buffer, 0, (int)bytesRead);

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
    
    public async Task RemoveDirectory(
        string path,
        bool recursive = false,
        CancellationToken cancel = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        MavlinkFtpHelper.CheckFolderPath(path);
        cancel.ThrowIfCancellationRequested();

        var canonicalPath = MavlinkFtpHelper.CreateFtpDirectoryFromPath(path).Path;

        if (recursive)
        {
            await RemoveDirectoryRecursive(canonicalPath).ConfigureAwait(false);
        }
        else
        {
            await Base.RemoveDirectory(canonicalPath, cancel).ConfigureAwait(false);
        }

        EvictRemovedSubtree(canonicalPath);
        _logger.LogInformation("Directory removed: {Path}", path);
        return;

        async Task RemoveDirectoryRecursive(string dirPath)
        {
            await Refresh(dirPath, recursive: false, cancel).ConfigureAwait(false);

            var children = _entryCache
                .Where(kv => kv.Value.ParentPath == dirPath)
                .Select(kv => kv.Value)
                .ToList();

            foreach (var child in children)
            {
                cancel.ThrowIfCancellationRequested();

                switch (child.Type)
                {
                    case FtpEntryType.Directory:
                        await RemoveDirectoryRecursive(child.Path).ConfigureAwait(false);
                        break;
                    case FtpEntryType.File:
                        await Base.RemoveFile(child.Path, cancel).ConfigureAwait(false);
                        _entryCache.Remove(child.Path);
                        break;
                    default:
                        _logger.LogWarning("Unknown FTP entry type: {Type}", child.Type);
                        break;
                }
            }

            await Base.RemoveDirectory(dirPath, cancel).ConfigureAwait(false);
            _entryCache.Remove(dirPath);
        }
    }

    private void EvictRemovedSubtree(string canonicalPath)
    {
        if (MavlinkFtpHelper.IsRootPath(canonicalPath))
        {
            _entryCache.Clear();
            return;
        }

        var serverPrefix = canonicalPath.TrimStart(MavlinkFtpHelper.DirectorySeparator);
        var affected = _entryCache
            .Where(kv =>
                kv.Key.TrimStart(MavlinkFtpHelper.DirectorySeparator)
                    .StartsWith(serverPrefix, StringComparison.Ordinal)
            )
            .Select(kv => kv.Key)
            .ToList();

        foreach (var key in affected)
        {
            _entryCache.Remove(key);
        }
    }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _entryCache.Clear();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await base.DisposeAsyncCore().ConfigureAwait(false);
        _entryCache.Clear();
    }

    #endregion
}
