using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public class MavlinkFtpServerExConfig
{
    public required string RootDirectory { get; init; }
}

public class FtpServerEx : IFtpServerEx
{
    private readonly ILogger _logger;
    private readonly MavlinkFtpServerExConfig _config;
    private readonly IFileSystem _fileSystem;
    private readonly ConcurrentBag<FtpSession> _sessions;
    private readonly string _rootDirectory;
    public IFtpServer Base { get; }
    
    
    public FtpServerEx(MavlinkFtpServerExConfig config, IFtpServer @base, IFileSystem? fileSystem, ILogger? logger = null)
    {
        _config = config;
        _rootDirectory = _config.RootDirectory;
        _fileSystem = fileSystem ?? new FileSystem(); // if file system that was passed here is null we use default system.io
        _logger = logger ?? NullLogger.Instance;
        _sessions = [];
        
        Base = @base;
        Base.OpenFileRead = OpenFileRead;
        Base.TerminateSession = TerminateSession;
        Base.CreateDirectory = CreateDirectory;
        Base.CalcFileCrc32 = CalcFileCrc32;
        Base.TruncateFile = TruncateFile;
        Base.Rename = Rename;
        Base.FileRead = FileRead;
        Base.ResetSessions = ResetSessions;
        Base.BurstReadFile = BurstReadFile;
        Base.RemoveDirectory = RemoveDirectory;
        Base.RemoveFile = RemoveFile;
    }

    public Task<ReadHandle> OpenFileRead(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.OpenFileRO, NackError.None);
        }
        
        var fullPath = _fileSystem.Path.Combine(_rootDirectory, path);
        if (!_fileSystem.File.Exists(fullPath))
        {
            throw new FtpNackException(FtpOpcode.OpenFileRO, NackError.FileNotFound);
        }
        
        var session = OpenSession(FtpSession.SessionMode.OpenRead);
        var stream = _fileSystem.File.OpenRead(fullPath);
        var info = new FileInfo(fullPath);
        var file = _fileSystem.FileInfo.Wrap(info);
        
        if (file.Length > byte.MaxValue)
        {
            throw new FtpNackException(FtpOpcode.OpenFileRO, NackError.FileNotFound);
        }

        session.Stream = stream;
        
        var fileSize = (uint) file.Length;

        return Task.FromResult(new ReadHandle(session.Id, fileSize));
    }

    public Task<WriteHandle> OpenFileWrite(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.OpenFileWO, NackError.None);
        }
        
        var fullPath = _fileSystem.Path.Combine(_rootDirectory, path);
        if (!_fileSystem.File.Exists(fullPath))
        {
            throw new FtpNackException(FtpOpcode.OpenFileWO, NackError.FileNotFound);
        }
        
        var session = OpenSession(FtpSession.SessionMode.OpenWrite);
        var stream = _fileSystem.File.OpenWrite(fullPath);
        var info = new FileInfo(fullPath);
        var file = _fileSystem.FileInfo.Wrap(info);
        
        if (file.Length > byte.MaxValue)
        {
            throw new FtpNackException(FtpOpcode.OpenFileWO, NackError.FileNotFound);
        }

        session.Stream = stream;
        
        var fileSize = (uint) file.Length;

        return Task.FromResult(new WriteHandle(session.Id, fileSize));
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
            if (request.Skip > session.Stream.Length)
            {
                throw new FtpNackEndOfFileException(FtpOpcode.ReadFile);
            }
            
            var offset = Convert.ToInt32(request.Skip);
            var take = request.Take;
            var size = await session.Stream.ReadAsync(bytes, offset, take, cancel)
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
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.Rename, NackError.None);
        }
        
        var fullPath1 = _fileSystem.Path.Combine(_rootDirectory, path1);
        if (!_fileSystem.Path.Exists(fullPath1))
        {
            throw new FtpNackException(FtpOpcode.Rename, NackError.FileNotFound);
        }

        var fullPath2 = _fileSystem.Path.Combine(_rootDirectory, path2);
        if (_fileSystem.Path.HasExtension(fullPath2))
        {
            _fileSystem.File.Move(fullPath1, fullPath2);
        }
        else
        {
            _fileSystem.Directory.Move(fullPath1, fullPath2);
        }
        
        return Task.CompletedTask;
    }

    public async Task TerminateSession(byte session, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.None);
        }
        
        var existingSession =  _sessions.FirstOrDefault(
            s => s.Id == session && 
                 s.Mode is 
                     FtpSession.SessionMode.OpenRead or 
                     FtpSession.SessionMode.OpenWrite
        );

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
            if (session.Mode is FtpSession.SessionMode.OpenRead or FtpSession.SessionMode.OpenWrite)
            {
                await session.CloseAsync().ConfigureAwait(false);
            }
        }
    }

    public Task<byte> ListDirectory(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task CreateDirectory(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.CreateDirectory, NackError.None);
        }
        
        var fullPath = _fileSystem.Path.Combine(_rootDirectory, path);
        if (_fileSystem.Directory.Exists(fullPath))
        {
            throw new FtpNackException(FtpOpcode.CreateDirectory, NackError.FileExists);
        }

        _fileSystem.Directory.CreateDirectory(fullPath);
        
        return Task.CompletedTask;
    }

    public Task<byte> CreateFile(string path, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFile(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.None);
        }
        
        var filePath = _fileSystem.Path.Combine(_rootDirectory, path);
        if (!_fileSystem.File.Exists(filePath))
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.FileNotFound);
        }
        
        _fileSystem.File.Delete(filePath);
        
        return Task.CompletedTask;
    }

    public Task RemoveDirectory(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.RemoveDirectory, NackError.None);
        }
        
        var fullPath = _fileSystem.Path.Combine(_rootDirectory, path);
        if (!_fileSystem.Directory.Exists(fullPath))
        {
            throw new FtpNackException(FtpOpcode.RemoveDirectory, NackError.FileNotFound);
        }

        if (_fileSystem.Directory.EnumerateFileSystemEntries(fullPath).Any())
        {
            throw new FtpNackException(FtpOpcode.RemoveDirectory, NackError.Fail);
        }
        
        _fileSystem.Directory.Delete(fullPath);

        return Task.CompletedTask;
    }

    public async Task<uint> CalcFileCrc32(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.CalcFileCRC32, NackError.None);
        }
        
        var filePath = _fileSystem.Path.Combine(_rootDirectory, path);
        if (!_fileSystem.File.Exists(filePath))
        {
            throw new FtpNackException(FtpOpcode.CalcFileCRC32, NackError.FileNotFound);
        }

        var fileBytes = await _fileSystem.File.ReadAllBytesAsync(filePath, cancel).ConfigureAwait(false);
        
        var crc32 = Crc32Mavlink.Accumulate(fileBytes);

        return crc32;
    }

    public Task TruncateFile(TruncateRequest request, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.TruncateFile, NackError.None);
        }
        
        var filePath = _fileSystem.Path.Combine(_rootDirectory, request.Path);
        if (!_fileSystem.File.Exists(filePath))
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.FileNotFound);
        }
        
        var stream = _fileSystem.File.Open(filePath, FileMode.Truncate, FileAccess.Write, FileShare.Read);
        
        stream.SetLength(request.Offset);
        stream.Close();
        
        return Task.CompletedTask;
    }

    public async Task<BurstReadResult> BurstReadFile(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.BurstReadFile, NackError.None);
        }
        
        var session = _sessions.FirstOrDefault(s => s.Id == request.Session);

        if (session is null)
        {
            throw new FtpNackException(FtpOpcode.BurstReadFile, NackError.InvalidSession);
        }

        if (session.Stream is null)
        {
            throw new FtpNackException(FtpOpcode.BurstReadFile, NackError.FileNotFound);
        }
        
        var bytes = ArrayPool<byte>.Shared.Rent(request.Take);
        try
        {
            var isLastChunk = false;
            var offset = Convert.ToInt32(request.Skip);
            var take = request.Take;
            var size = await session.Stream.ReadAsync(bytes, offset, take, cancel)
                .ConfigureAwait(false);
            var realBytes = bytes[..size];
            realBytes.CopyTo(buffer);

            if (offset + take > session.Stream.Length)
            {
                isLastChunk = true;
            }

            return new BurstReadResult((byte) size, isLastChunk,  request);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(bytes);
        }
    }

    public Task WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
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

        int maxSessionNumber;
        if (_sessions.IsEmpty)
        {
            maxSessionNumber = -1;
        }
        else
        {
            maxSessionNumber = _sessions.Max(s => s.Id);
        }
        
        var session = new FtpSession((byte)(maxSessionNumber + 1));
        session.Open(mode);

        _sessions.Add(session);
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