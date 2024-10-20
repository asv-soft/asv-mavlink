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

public sealed class FtpSession : IDisposable, IAsyncDisposable
{
    public enum SessionMode
    {
        Free,
        Unknown,
        OpenRead,
        OpenWrite,
        OpenReadWrite,
    }
    
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
    
    public byte Id { get; }
    public SessionMode Mode { get; private set; }
    
    public FtpSession(byte id)
    {
        Id = id;
        Mode = SessionMode.Free;
    }

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
        
    public void Dispose()
    {
        ReleaseAllResources();
    }
    
    public async ValueTask DisposeAsync()
    {
        await ReleaseAllResourcesAsync().ConfigureAwait(false);
    }
        
    private void ReleaseAllResources()
    {
        _stream?.Dispose();
    }
    
    private async Task ReleaseAllResourcesAsync()
    {
        if (_stream is null)
        {
            return;
        }
        
        await _stream.DisposeAsync().ConfigureAwait(false);
    }
}