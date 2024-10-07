using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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

    public async Task<ReadHandle> OpenFileRead(string path, CancellationToken cancel = default)
    {
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
        
        if (cancel.IsCancellationRequested)
        {
            await session.CloseAsync().ConfigureAwait(false);

            throw new FtpNackException(FtpOpcode.OpenFileRO, NackError.None);
        }

        return new ReadHandle(session.Id, fileSize);
    }

    public Task<ReadResult> FileRead(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task Rename(string path1, string path2, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public async Task TerminateSession(byte session, CancellationToken cancel = default)
    {
        var existingSession =  _sessions.FirstOrDefault(s => s.Id == session && s.Mode == FtpSession.SessionMode.OpenRead);

        if (existingSession is null)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.Fail);
        }

        if (existingSession.Mode == FtpSession.SessionMode.Free)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.InvalidSession);
        }
        
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.None);
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
        var filePath = Path.Combine(_rootDirectory, path);
        if (!File.Exists(filePath))
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.FileNotFound);
        }
        
        File.Delete(filePath);
        
        if (cancel.IsCancellationRequested)
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.None);
        }
        
        return Task.CompletedTask;
    }

    public Task RemoveDirectory(string path, CancellationToken cancel = default)
    {
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

    public Task<int> CalcFileCrc32(string path, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task TruncateFile(TruncateRequest request, CancellationToken cancel = default)
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