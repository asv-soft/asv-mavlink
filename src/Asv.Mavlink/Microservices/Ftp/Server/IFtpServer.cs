using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public delegate Task<ReadHandle> OpenFileReadDelegate(string path, CancellationToken cancel = default);
public delegate Task<ReadResult> FileReadDelegate(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);
public delegate Task RenameDelegate(string path1, string path2, CancellationToken cancel = default);
public delegate Task TerminateSessionDelegate(byte session, CancellationToken cancel = default);
public delegate Task ResetSessionsDelegate( CancellationToken cancel = default);
public delegate Task CreateDirectory(string path, CancellationToken cancel = default);
public delegate Task RemoveFile(string path, CancellationToken cancel = default);
public delegate Task RemoveDirectory(string path, CancellationToken cancel = default);
public delegate Task<int> CalcFileCrc32(string path, CancellationToken cancel = default);
public delegate Task TruncateFile(TruncateRequest request, CancellationToken cancel = default);
public delegate Task<BurstReadResult> BurstReadFileDelegate(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);

public interface IFtpServer
{
    RenameDelegate? Rename { get; set; }
    OpenFileReadDelegate? OpenFileRead { set; }
    FileReadDelegate? FileRead { set; }
    TerminateSessionDelegate? TerminateSession { set; }
    ResetSessionsDelegate? ResetSessions { set; }
    CreateDirectory? CreateDirectory { set; get; }
    RemoveFile? RemoveFile { get; set; }
    RemoveDirectory? RemoveDirectory { set; get; }
    CalcFileCrc32? CalcFileCrc32 { get; set; }
    TruncateFile? TruncateFile { get; set; }
    BurstReadFileDelegate? BurstReadFile { set; }
}