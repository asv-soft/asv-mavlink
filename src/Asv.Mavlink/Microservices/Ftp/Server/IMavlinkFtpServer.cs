using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public delegate Task<OpenReadResult> OpenFileReadDelegate(string path, CancellationToken cancel = default);
public delegate Task<ReadResult> FileReadDelegate(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);

public interface IMavlinkFtpServer
{
    OpenFileReadDelegate? OpenFileRead { set; }
    FileReadDelegate? FileRead { set; }
}