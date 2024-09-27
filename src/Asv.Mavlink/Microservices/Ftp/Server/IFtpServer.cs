using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public delegate Task<ReadHandle> OpenFileReadDelegate(string path, CancellationToken cancel = default);
public delegate Task<ReadResult> FileReadDelegate(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);
public delegate Task RenameDelegate(string path1, string path2, CancellationToken cancel = default);
public delegate Task TerminateSessionDelegate(byte session, CancellationToken cancel = default);

public interface IFtpServer
{
    RenameDelegate? Rename { get; set; }
    OpenFileReadDelegate? OpenFileRead { set; }
    FileReadDelegate? FileRead { set; }
    TerminateSessionDelegate? TerminateSession { set; }
}