using System;
using System.Buffers;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink;

public interface IMavlinkFtpClient
{
    Task<OpenReadResult> OpenFileRead(string path, CancellationToken cancel = default);
    Task<ReadResult> Read(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);
}

public readonly struct ReadResult(byte readCount, ReadRequest request)
{
    public readonly ReadRequest Request = request;
    public readonly byte ReadCount = readCount;
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