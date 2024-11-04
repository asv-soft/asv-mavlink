using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public interface IFtpServerEx : IDisposable, IAsyncDisposable
{
    IFtpServer Base { get; }
    
    public Task<ReadHandle> OpenFileRead(string path, CancellationToken cancel = default);
    public Task<WriteHandle> OpenFileWrite(string path, CancellationToken cancel = default);
    public Task<ReadResult> FileRead(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);
    public Task Rename(string path1, string path2, CancellationToken cancel = default);
    public Task TerminateSession(byte session, CancellationToken cancel = default);
    public Task ResetSessions(CancellationToken cancel = default);
    public Task<byte> ListDirectory(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default);
    public Task CreateDirectory(string path, CancellationToken cancel = default);
    public Task<byte> CreateFile(string path, CancellationToken cancel = default);
    public Task RemoveFile(string path, CancellationToken cancel = default);
    public Task RemoveDirectory(string path, CancellationToken cancel = default);
    public Task<uint> CalcFileCrc32(string path, CancellationToken cancel = default);
    public Task TruncateFile(TruncateRequest request, CancellationToken cancel = default);
    public Task<BurstReadResult> BurstReadFile(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);
    public Task WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancel = default);
}

public sealed class FtpSession(byte id) : IDisposable, IAsyncDisposable
{
    public enum SessionMode
    {
        Free,
        Unknown,
        OpenRead,
        OpenWrite,
        OpenReadWrite,
    }
    
    private bool _disposed;
    private Stream? _stream;
    public Stream? Stream
    {
        get => _stream;
        set
        {
            if (Mode == SessionMode.Free )
            {
                throw new FtpException("Session is not at work");
            }
            
            _stream = value;
        }
    }
    
    public byte Id { get; } = id;
    public SessionMode Mode { get; private set; } = SessionMode.Free;

    public void Open(SessionMode mode = SessionMode.Unknown)
    {
        if (Mode != SessionMode.Free)
        {
            throw new FtpException("Session is already opened");
        }
        
        Mode = mode;
    }

    public void Close()
    {
        Mode = SessionMode.Free;
        Dispose();
    }
    
    public async Task CloseAsync()
    {
        Mode = SessionMode.Free;
        await DisposeAsync().ConfigureAwait(false);
    }
    
    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _stream?.Dispose();
        }
        _disposed = true;
    }
        
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    
    private async ValueTask DisposeAsyncCore()
    {
        if (!_disposed)
        {
            if (_stream is not null)
            {
                await _stream.DisposeAsync().ConfigureAwait(false);
            }
            
            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    ~FtpSession()
    {
        Dispose(disposing: false);
    }
}