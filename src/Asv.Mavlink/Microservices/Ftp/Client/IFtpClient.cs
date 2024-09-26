using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IFtpClient
{
    Task<ReadHandle> OpenFileRead(string path, CancellationToken cancel = default);

    public Task<FileTransferProtocolPacket> CreateDirectory(string path, CancellationToken cancellationToken = default);
    public Task<FileTransferProtocolPacket> ResetSessions(CancellationToken cancellationToken = default);
    public Task<FileTransferProtocolPacket> RemoveDirectory(string path, CancellationToken cancellationToken = default);
    public Task<FileTransferProtocolPacket> RemoveFile(string path, CancellationToken cancellationToken = default);

    public Task<FileTransferProtocolPacket> CalcFileCrc32(string path, CancellationToken cancellationToken = default);

    
    Task BurstReadFile(ReadRequest request,
        Action<FileTransferProtocolPacket> onBurstData,
        CancellationToken cancel = default);
    Task<FileTransferProtocolPacket> ReadFile(ReadRequest request, CancellationToken cancel = default);
    Task TerminateSession(byte session, CancellationToken cancel = default);
    Task<FileTransferProtocolPacket> ListDirectory(string path, uint offset, CancellationToken cancel = default);
    public async Task<ReadResult> ReadFile(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)
    {
        var result = await ReadFile(request, cancel).ConfigureAwait(false);
        var readCount = result.ReadData(buffer);
        return new ReadResult(readCount, request);
    }
    public async Task<ReadResult> ReadFile(ReadRequest request, IBufferWriter<byte> buffer, CancellationToken cancel = default)
    {
        var result = await ReadFile(request, cancel).ConfigureAwait(false);
        var readCount = result.ReadData(buffer);
        return new ReadResult(readCount, request);
    }
    async Task<byte> ListDirectory(string path, uint offset, IBufferWriter<byte> buffer, CancellationToken cancel = default)
    {
        var result = await ListDirectory(path, offset, cancel).ConfigureAwait(false);
        return result.ReadData(buffer);
    }

    async Task<byte> ListDirectory(string path, uint offset, IBufferWriter<char> buffer, CancellationToken cancel = default)
    {
        var result = await ListDirectory(path, offset, cancel).ConfigureAwait(false);
        return result.ReadDataAsString(buffer);
    }

    async Task<byte> ListDirectory(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default)
    {
        var result = await ListDirectory(path, offset, cancel).ConfigureAwait(false);
        return result.ReadDataAsString(buffer);
    }
   
}

public readonly struct ReadRequest(byte session, uint skip, byte take)
{
    public readonly byte Session = session;
    public readonly uint Skip = skip;
    public readonly byte Take = take;
    public override string ToString() => $"READ_REQ(skip: {Skip}, take: {Take}, session:{Session})";
}

public readonly struct ReadResult(byte readCount, ReadRequest request)
{
    public readonly ReadRequest Request = request;
    public readonly byte ReadCount = readCount;
   
    public override string ToString() => $"READ_RESP(read: {ReadCount}, {Request})";
}

public readonly struct CrcDelegate(string crc)
{
    
}


public readonly struct ReadHandle(byte session, uint size)
{
    public readonly byte Session = session;
    public readonly uint Size = size;
    public override string ToString() => $"READ_FILE(session: {Session}, size: {StringExtensions.BytesToString(Size)})";
}

public readonly struct CreateHandle(byte session, string path)
{
public readonly byte Session = session;
public readonly string Path = path;
public override string ToString() => $"CREATE_DIR(session: {Session}, path: {path})";
}