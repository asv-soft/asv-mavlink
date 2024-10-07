using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public class MavlinkFtpServerExConfig
{
    public byte SessionsCount { get; set; } = 1;
}

public class FtpServerEx : IFtpServerEx
{
    private readonly TimeProvider _timeProvider;
    private readonly ILogger _logger;
    private readonly MavlinkFtpServerExConfig _config;
    private readonly ConcurrentBag<FtpSession> _sessions;
    private readonly string _rootDirectory;
    public IFtpServer Base { get; }
    
    
    public FtpServerEx(MavlinkFtpServerExConfig config, IFtpServer @base, string rootDirectory, TimeProvider? timeProvider = null, ILogger? logger = null)
    {
        _config = config;
        _rootDirectory = Path.GetFullPath(rootDirectory);
        _timeProvider = timeProvider ?? TimeProvider.System;
        _logger = logger ?? NullLogger.Instance;
        _sessions = [];
        
        Base = @base;
        Base.OpenFileRead = OpenFileRead;
        Base.TerminateSession = TerminateSession;
    }

    public Task<ReadHandle> OpenFileRead(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.OpenFileRO, NackError.None);
        }
        
        var filePath = Path.Combine(_rootDirectory, path);
        if (!File.Exists(filePath))
        {
            throw new FtpNackException(FtpOpcode.OpenFileRO, NackError.FileNotFound);
        }
        
        var session = OpenSession(FtpSession.SessionMode.OpenRead);
        var stream = File.OpenRead(filePath);
        var file = new FileInfo(filePath);
        
        if (file.Length > byte.MaxValue)
        {
            throw new FtpNackException(FtpOpcode.OpenFileRO, NackError.FileNotFound);
        }

        session.Stream = stream;
        
        var fileSize = (uint) file.Length;

        return Task.FromResult(new ReadHandle(session.Id, fileSize));
    }

    public async Task<ReadResult> FileRead(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.ReadFile, NackError.None);
        }
        
        var session = _sessions.FirstOrDefault(s => s.Id == request.Session);

        if (session is null)
        {
            throw new FtpNackException(FtpOpcode.ReadFile, NackError.InvalidSession);
        }

        if (session.Stream is null)
        {
            throw new FtpNackException(FtpOpcode.ReadFile, NackError.FileNotFound);
        }
        
        var bytes = ArrayPool<byte>.Shared.Rent(request.Take);
        try
        {
            var size = await session.Stream.ReadAsync(bytes, (int)request.Skip, request.Take, cancel)
                .ConfigureAwait(false);
            var realBytes = bytes[..size];
            realBytes.CopyTo(buffer);

            return new ReadResult((byte) size, request);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(bytes);
        }
    }

    public Task Rename(string path1, string path2, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task TerminateSession(byte session, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.None);
        }
        
        var existingSession =  _sessions.FirstOrDefault(s => s.Id == session && s.Mode == FtpSession.SessionMode.OpenRead);

        if (existingSession is null)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.Fail);
        }

        if (existingSession.Mode == FtpSession.SessionMode.Free)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.InvalidSession);
        }
        
        await existingSession.CloseAsync().ConfigureAwait(false);
    }

    public async Task ResetSessions(CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.ResetSessions, NackError.None);
        }
        
        foreach (var session in _sessions)
        {
            if (session.Mode == FtpSession.SessionMode.OpenRead)
            {
                await session.CloseAsync().ConfigureAwait(false);
            }
        }
    }

    public Task CreateDirectory(string path, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFile(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.None);
        }
        
        var filePath = Path.Combine(_rootDirectory, path);
        if (!File.Exists(filePath))
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.FileNotFound);
        }
        
        File.Delete(filePath);
        
        return Task.CompletedTask;
    }

    public Task RemoveDirectory(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.RemoveDirectory, NackError.None);
        }
        
        var directoryPath = Path.Combine(_rootDirectory, path);
        if (!Directory.Exists(directoryPath))
        {
            throw new FtpNackException(FtpOpcode.RemoveDirectory, NackError.FileNotFound);
        }

        if (Directory.EnumerateFileSystemEntries(directoryPath).Any())
        {
            throw new FtpNackException(FtpOpcode.RemoveDirectory, NackError.Fail);
        }
        
        Directory.Delete(directoryPath);

        return Task.CompletedTask;
    }

    public async Task<uint> CalcFileCrc32(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.CalcFileCRC32, NackError.None);
        }
        
        var filePath = Path.Combine(_rootDirectory, path);
        if (!File.Exists(filePath))
        {
            throw new FtpNackException(FtpOpcode.CalcFileCRC32, NackError.FileNotFound);
        }

        var fileBytes = await File.ReadAllBytesAsync(filePath, cancel).ConfigureAwait(false);
        
        var crc32 = Crc32Mavlink.Accumulate(fileBytes);

        return crc32;
    }

    public Task TruncateFile(TruncateRequest request, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.TruncateFile, NackError.None);
        }
        
        var filePath = Path.Combine(_rootDirectory, request.Path);
        if (!File.Exists(filePath))
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.FileNotFound);
        }
        
        var stream = File.Open(filePath, FileMode.Truncate, FileAccess.Write, FileShare.Read);
        
        stream.SetLength(request.Offset);
        stream.Close();
        
        return Task.CompletedTask;
    }

    private FtpSession OpenSession(FtpSession.SessionMode mode = FtpSession.SessionMode.Unknown)
    {
        var freeSession = _sessions.FirstOrDefault(s => s.Mode == FtpSession.SessionMode.Free);

        if (freeSession is not null)
        {
            freeSession.Open(mode);
            return freeSession;
        }
        
        if (_sessions.Count > byte.MaxValue)
        {
            throw new FtpNackException(FtpOpcode.Nak, NackError.NoSessionsAvailable);
        }

        var maxSessionNumber = _sessions.Max(s => s.Id);
        var session = new FtpSession((byte)(maxSessionNumber + 1));
        session.Open(mode);

        _sessions.Add(new FtpSession((byte)(maxSessionNumber+1)));
        return session;
    }

    public void Dispose()
    {
        foreach (var session in _sessions)
        {
            session.Close();
        }
        
        _sessions.Clear();
    }
    
    public async ValueTask DisposeAsync()
    {
        foreach (var session in _sessions)
        {
            await session.CloseAsync().ConfigureAwait(false);
        }
        
        _sessions.Clear();
    }
}