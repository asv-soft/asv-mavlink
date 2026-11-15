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
        var listedEntries = await ListEntries(currentEntry.Path, cancel).ConfigureAwait(false);

        var staleKeys = _entryCache
            .Select(kv => kv.Key)
            .Where(key => key.StartsWith(currentEntry.Path, StringComparison.Ordinal))
            .ToList();
        foreach (var key in staleKeys)
        {
            _entryCache.Remove(key);
        }

        MaterializeParents(currentEntry);
        _entryCache.Add(currentEntry.Path, currentEntry);
        foreach (var entry in listedEntries)
        {
            _entryCache.Add(entry.Path, entry);
        }

        if (recursive)
        {
            foreach (var item in listedEntries.Where(entry => entry.Type == FtpEntryType.Directory))
            {
                await Refresh(item.Path, recursive, cancel).ConfigureAwait(false);
            }
        }
    }

    private async Task<IReadOnlyCollection<IFtpEntry>> ListEntries(
        string directoryPath,
        CancellationToken cancel
    )
    {
        // keyed by path: a listing that shifts between pages must not produce duplicates
        var listedEntries = new Dictionary<string, IFtpEntry>();
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
                    var charSize = await Base.ListDirectory(directoryPath, offset, array, cancel)
                        .ConfigureAwait(false);
                    // every record ('D', 'F' or skipped 'S') is null-terminated and advances the server-side offset
                    var recordCount = (uint)
                        array.AsSpan(0, charSize).Count(MavlinkFtpHelper.PathSeparator);
                    if (recordCount == 0)
                    {
                        // an empty page would request the same offset forever
                        break;
                    }

                    ParseEntries(
                        directoryPath,
                        new ReadOnlySequence<char>(array, 0, charSize),
                        listedEntries
                    );
                    offset += recordCount;
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

        return listedEntries.Values;
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
    
    private static void ParseEntries(
        string parentPath,
        ReadOnlySequence<char> data,
        Dictionary<string, IFtpEntry> listedEntries
    )
    {
        var rdr = new SequenceReader<char>(data);
        while (MavlinkFtpHelper.ParseFtpEntry(ref rdr, parentPath, out var entry))
        {
            Debug.Assert(entry != null);
            if (MavlinkFtpHelper.IgnorePaths.Contains(entry.Name))
                continue;
            listedEntries[entry.Path] = entry;
        }
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
        cancel.ThrowIfCancellationRequested();

        var canonicalPath = MavlinkFtpHelper.CreateFtpDirectoryFromPath(path).Path;
        MavlinkFtpHelper.CheckFolderPath(canonicalPath);

        // every removed entry is evicted from the cache right away:
        // if a later step fails, the cache must not keep what is already gone
        if (recursive)
        {
            foreach (var child in await ListEntries(canonicalPath, cancel).ConfigureAwait(false))
            {
                cancel.ThrowIfCancellationRequested();

                switch (child.Type)
                {
                    case FtpEntryType.Directory:
                        await RemoveDirectory(child.Path, recursive, cancel).ConfigureAwait(false);
                        break;
                    case FtpEntryType.File:
                        await Base.RemoveFile(child.Path, cancel).ConfigureAwait(false);
                        // the file may be cached under both path forms: "x" and "/x"
                        var serverPath = child.Path.TrimStart(MavlinkFtpHelper.DirectorySeparator);
                        _entryCache.Remove(serverPath);
                        _entryCache.Remove($"{MavlinkFtpHelper.DirectorySeparator}{serverPath}");
                        break;
                    default:
                        _logger.LogWarning("Unknown FTP entry type: {Type}", child.Type);
                        break;
                }
            }
        }

        await Base.RemoveDirectory(canonicalPath, cancel).ConfigureAwait(false);
        EvictRemovedDirectory(canonicalPath);
        _logger.LogInformation("Directory removed: {Path}", path);
    }

    private void EvictRemovedDirectory(string canonicalPath)
    {
        // the directory may be cached under both path forms: "x/" and "/x/"
        var serverPath = canonicalPath.TrimStart(MavlinkFtpHelper.DirectorySeparator);
        var staleKeys = _entryCache
            .Select(kv => kv.Key)
            .Where(key =>
                key.AsSpan()
                    .TrimStart(MavlinkFtpHelper.DirectorySeparator)
                    .StartsWith(serverPath, StringComparison.Ordinal)
            )
            .ToList();
        foreach (var key in staleKeys)
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
