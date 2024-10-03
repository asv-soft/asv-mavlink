using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public class FtpServerEx : IFtpServerEx
{
    private readonly TimeProvider _timeProvider;
    private readonly ILogger _logger;
    private readonly SourceCache<IFtpEntry,string> _entryCache;
    private readonly List<FtpSession> _sessions;
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
    private IReadOnlyCollection<string> _rootDirectories;
    public IFtpServer Base { get; }
    
    
    public FtpServerEx(IFtpServer @base, IReadOnlyCollection<string> rootDirectories, TimeProvider? timeProvider = null, ILogger? logger = null)
    {
        _rootDirectories = rootDirectories;
        _timeProvider = timeProvider ?? TimeProvider.System;
        _logger = logger ?? NullLogger.Instance;
        _entryCache = new SourceCache<IFtpEntry, string>(x => x.Path);
        _sessions = new List<FtpSession>(256);
        for (byte i = 0; i < _sessions.Count; i++)
        {
            _sessions.Add(new FtpSession(i));
        }
        
        Base = @base;
        Base.OpenFileRead = OpenFileRead;
        Base.TerminateSession = TerminateSession;
    }

    public Task<ReadHandle> OpenFileRead(string path, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(path))
        {
            throw new FtpNackException(FtpOpcode.OpenFileRO, NackError.FileNotFound);
        }
        
        var session = OpenSession();
        var stream = File.OpenRead(path);
        session.AddResource(stream);
        var fileSize = (uint) new FileInfo(path).Length;
        
        if (cancellationToken.IsCancellationRequested)
        {
            session.Close();
            
            throw new OperationCanceledException(cancellationToken);
        }

        return Task.FromResult(new ReadHandle(session.Id, fileSize));
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
        var existingSession =  _sessions.FirstOrDefault(s => s.Id == session);

        if (existingSession is null)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.Fail);
        }

        if (!existingSession.IsOccupied)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.InvalidSession);
        }
        
        await existingSession.CloseAsync().ConfigureAwait(false);
    }

    public Task ResetSessions(CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task CreateDirectory(string path, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFile(string path, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveDirectory(string path, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> CalcFileCrc32(string path, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task TruncateFile(TruncateRequest request, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    private FtpSession OpenSession()
    {
        var availableSession = _sessions.FirstOrDefault(s => s.IsOccupied == false);

        if (availableSession is null)
        {
            throw new FtpNackException(FtpOpcode.Nak, NackError.NoSessionsAvailable);
        }
        
        availableSession.Open();
        return availableSession;
    }

    public void Dispose()
    {
        _entryCache.Dispose();
        foreach (var session in _sessions)
        {
            session.Close();
        }
    }
    
    public async ValueTask DisposeAsync()
    {
        _entryCache.Dispose();
        foreach (var session in _sessions)
        {
            await session.CloseAsync().ConfigureAwait(false);
        }
    }
}