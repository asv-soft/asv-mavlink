using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
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
    private readonly ICollection<Stream> _affectedStreams;
    public byte Id { get; }
    public bool IsOccupied { get; private set; }
    
    public FtpSession(byte id)
    {
        Id = id;
        IsOccupied = false;
        _affectedStreams = new List<Stream>();
    }

    public void AddResource<TResource>(TResource resource)
    {
        if (!IsOccupied)
        {
            throw new Exception("Session is not in work"); // TODO: make proper exception class
        }
        
        switch (resource)
        {
            case Stream stream:
                _affectedStreams.Add(stream);
                break;
            default:
                throw new Exception("Resource type is unknown for session"); // TODO: make proper exception class
        }
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
        foreach (var resource in _affectedStreams)
        {
            resource.Dispose();
        }
            
        _affectedStreams.Clear();
    }
}