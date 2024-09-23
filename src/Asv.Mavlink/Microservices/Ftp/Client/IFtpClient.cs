using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public interface IFtpClient
{
    Task<byte> ListDirectory(string path, uint offset, IBufferWriter<byte> buffer, CancellationToken cancel = default);
    Task<byte> ListDirectory(string path, uint offset, IBufferWriter<char> buffer, CancellationToken cancel = default);
    Task<byte> ListDirectory(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default);
    Task<OpenReadResult> OpenFileRead(string path, CancellationToken cancel = default);
    Task<ReadResult> ReadFile(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);
    Task<ReadResult> ReadFile(ReadRequest request, IBufferWriter<byte> buffer, CancellationToken cancel = default);
    Task TerminateSession(byte session, CancellationToken cancel = default);

   
}

public readonly struct ReadResult(byte readCount, ReadRequest request)
{
    public readonly ReadRequest Request = request;
    public readonly byte ReadCount = readCount;
   
    public override string ToString() => $"Request: {Request}, ReadCount: {ReadCount}";
}

public readonly struct ReadRequest(byte session, uint skip, byte take)
{
    public readonly byte Session = session;
    public readonly uint Skip = skip;
    public readonly byte Take = take;
    public override string ToString() => $"Session: {Session}, Skip: {Skip}, Take: {Take}";
}

public readonly struct OpenReadResult(byte session, uint size)
{
    public readonly byte Session = session;
    public readonly uint Size = size;
    public override string ToString() => $"Session: {Session}, Size: {Size}";
}