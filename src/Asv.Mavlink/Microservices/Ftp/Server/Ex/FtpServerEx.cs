using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
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
    private bool _disposed;
    public IFtpServer Base { get; }
    
    
    public FtpServerEx(MavlinkFtpServerExConfig config, 
        IFtpServer @base, 
        IFileSystem? fileSystem = null, 
        TimeProvider? timeProvider = null,
        ILoggerFactory? logFactory = null)
    {
        _config = config;
        _rootDirectory = _config.RootDirectory;
        _fileSystem = fileSystem ?? new FileSystem(); // if file system that was passed here is null we use default system.io
        logFactory??=NullLoggerFactory.Instance;
        _logger = logFactory.CreateLogger<FtpServerEx>();
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
        Base.ListDirectory = ListDirectory;
        Base.OpenFileWrite = OpenFileWrite;
        Base.WriteFile = WriteFile;
        Base.CreateFile = CreateFile;
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

        session.Stream = stream;
        
        var fileSize = (uint) stream.Length;

        return Task.FromResult(new ReadHandle(session.Id, fileSize));
    }

    public Task<WriteHandle> OpenFileWrite(string path, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.OpenFileWO, NackError.None);
        }
        
        var fullPath = _fileSystem.Path.Combine(_rootDirectory, path);
        
        var session = OpenSession(FtpSession.SessionMode.OpenWrite);
        var stream = _fileSystem.File.OpenWrite(fullPath);

        session.Stream = stream;
        
        var fileSize = (uint) stream.Length;

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
        
        if (request.Skip > session.Stream.Length)
        {
            throw new FtpNackEndOfFileException(FtpOpcode.ReadFile);
        }
            
        var temp = buffer[..request.Take];
        session.Stream.Position = request.Skip;
        var size = await session.Stream.ReadAsync(temp, cancel).ConfigureAwait(false);

        return new ReadResult((byte) size, request);
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
                 s.Mode is not FtpSession.SessionMode.Free
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
            if (session.Mode is not FtpSession.SessionMode.Free)
            {
                await session.CloseAsync().ConfigureAwait(false);
            }
        }
    }

    public Task<byte> ListDirectory(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.ListDirectory, NackError.None);
        }

        var fullPath = _fileSystem.Path.Combine(_rootDirectory, path);
        if (!_fileSystem.Path.Exists(fullPath))
        {
            throw new FtpNackException(FtpOpcode.ListDirectory, NackError.FileNotFound);
        }
        
        uint currentIndex = 0;
        var infos = new List<IFileSystemInfo>();
        var dirInfo = new DirectoryInfo(fullPath);
        var directory = _fileSystem.DirectoryInfo.Wrap(dirInfo);
        var dirInfos = directory.GetFileSystemInfos();

        if (offset >= dirInfos.Length)
        {
            throw new FtpNackEndOfFileException(FtpOpcode.ListDirectory);
        }
        
        foreach (var info in dirInfos)
        {
            if (currentIndex >= offset)
            {
                infos.Add(info);
            }
            
            currentIndex++;
        }

        var result = new List<string>();
        foreach (var entry in infos)
        {
            if (entry.Extension.Length > 0)
            {
                var file = (IFileInfo) entry;
                result.Add($"F{file.Name}\t{file.Length}\0");
                continue;
            }
            
            result.Add($"D{entry.Name}\0");
        }

        var sb = new StringBuilder(0, MavlinkFtpHelper.MaxDataSize);
        foreach (var str in result)
        {
            if (sb.Length + str.Length > sb.MaxCapacity)
            {
                break;
            }
            
            sb.Append(str);
        }

        if (sb.Length == 0)
        {
            throw new FtpNackException(FtpOpcode.ListDirectory, NackError.Fail);
        }
        
        sb.ToString().CopyTo(buffer.Span);
        
        return Task.FromResult((byte) sb.Length);
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
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.CreateFile, NackError.None);
        }
        
        var fullPath = _fileSystem.Path.Combine(_rootDirectory, path);
        if (_fileSystem.File.Exists(fullPath))
        {
            var stream = _fileSystem.File.Open(fullPath, FileMode.Truncate, FileAccess.Write, FileShare.Read);
            stream.SetLength(0);
            stream.Close();

            throw new FtpNackException(FtpOpcode.CreateFile, NackError.FileExists);
        }

        var file = _fileSystem.File.Create(fullPath);
        var session = OpenSession(FtpSession.SessionMode.OpenReadWrite);
        session.Stream = file;
        
        return Task.FromResult(session.Id);
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
        if (request.Offset == 0 )
        {
            throw new FtpNackException(FtpOpcode.TruncateFile, NackError.InvalidDataSize);
        }
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.TruncateFile, NackError.None);
        }
        
        var filePath = _fileSystem.Path.Combine(_rootDirectory, request.Path);
        if (!_fileSystem.File.Exists(filePath))
        {
            throw new FtpNackException(FtpOpcode.TruncateFile, NackError.FileNotFound);
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
        
        var isLastChunk = request.Skip + request.Take >= session.Stream.Length;

        session.Stream.Position = request.Skip;
        var temp = buffer[..request.Take];
        var size = await session.Stream.ReadAsync(temp, cancel).ConfigureAwait(false);

        return new BurstReadResult((byte)size, isLastChunk, request);
    }

    public async Task WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancel = default)
    {
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.WriteFile, NackError.None);
        }
        
        var session = _sessions.FirstOrDefault(s => s.Id == request.Session);

        if (session is null)
        {
            throw new FtpNackException(FtpOpcode.WriteFile, NackError.InvalidSession);
        }

        if (session.Stream is null)
        {
            throw new FtpNackException(FtpOpcode.WriteFile, NackError.FileNotFound);
        }
        
        var temp = buffer[..request.Take];
        session.Stream.Position = request.Skip;
        await session.Stream.WriteAsync(temp, cancel).ConfigureAwait(false);
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

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            foreach (var session in _sessions)
            {
                session.Close();
            }
            _sessions.Clear();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (!_disposed)
        {
            foreach (var session in _sessions)
            {
                await session.CloseAsync().ConfigureAwait(false);
            }
            _sessions.Clear();

            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    ~FtpServerEx()
    {
        Dispose(disposing: false);
    }
}