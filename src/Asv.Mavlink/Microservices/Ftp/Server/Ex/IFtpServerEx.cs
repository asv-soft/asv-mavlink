using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public interface IFtpServerEx : IDisposable
{
    IFtpServer Base { get; }
    
    public Task<ReadHandle> OpenFileRead(string path, CancellationToken cancel = default);
    public Task<ReadResult> FileRead(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);
    public Task Rename(string path1, string path2, CancellationToken cancel = default);
    public Task TerminateSession(byte session, CancellationToken cancel = default);
    public Task ResetSessions( CancellationToken cancel = default);
    public Task CreateDirectory(string path, CancellationToken cancel = default);
    public Task RemoveFile(string path, CancellationToken cancel = default);
    public Task RemoveDirectory(string path, CancellationToken cancel = default);
    public Task<int> CalcFileCrc32(string path, CancellationToken cancel = default);
    public Task TruncateFile(TruncateRequest request, CancellationToken cancel = default);
}

public class FtpSession : IDisposable
{
    public byte Id { get; }
    public bool IsOccupied { get; private set; }

    public ICollection<Stream> AffectedResources { get; }
    
    public FtpSession(byte id)
    {
        Id = id;
        IsOccupied = false;
        AffectedResources = new List<Stream>();
    }

    public void Open()
    {
        IsOccupied = true;
    }

    public void Close()
    {
        IsOccupied = false;
        Dispose();
    }
        
    public void Dispose()
    {
        ReleaseAllResources();
    }
        
    private void ReleaseAllResources()
    {
        foreach (var resource in AffectedResources)
        {
            resource.Dispose();
        }
            
        AffectedResources.Clear();
    }
}