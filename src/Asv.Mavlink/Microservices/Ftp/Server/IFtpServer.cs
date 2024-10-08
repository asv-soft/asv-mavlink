using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public delegate Task<ReadHandle> OpenFileReadDelegate(string path, CancellationToken cancel = default);
public delegate Task<WriteHandle> OpenFileWriteDelegate(string path, CancellationToken cancel = default);
public delegate Task<ReadResult> FileReadDelegate(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);
public delegate Task RenameDelegate(string path1, string path2, CancellationToken cancel = default);
public delegate Task TerminateSessionDelegate(byte session, CancellationToken cancel = default);
public delegate Task ResetSessionsDelegate(CancellationToken cancel = default);
public delegate Task<byte> ListDirectoryDelegate(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default);
public delegate Task CreateDirectory(string path, CancellationToken cancel = default);
public delegate Task<byte> CreateFile(string path, CancellationToken cancel = default);
public delegate Task RemoveFile(string path, CancellationToken cancel = default);
public delegate Task RemoveDirectory(string path, CancellationToken cancel = default);
public delegate Task<uint> CalcFileCrc32(string path, CancellationToken cancel = default);
public delegate Task TruncateFile(TruncateRequest request, CancellationToken cancel = default);
public delegate Task<BurstReadResult> BurstReadFileDelegate(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);
public delegate Task WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancel = default);

public interface IFtpServer
{
    RenameDelegate? Rename { set; }
    OpenFileReadDelegate? OpenFileRead { set; }
    OpenFileWriteDelegate? OpenFileWrite { set; }
    FileReadDelegate? FileRead { set; }
    TerminateSessionDelegate? TerminateSession { set; }
    ListDirectoryDelegate? ListDirectory { set; }
    ResetSessionsDelegate? ResetSessions { set; }
    CreateDirectory? CreateDirectory { set; }
    CreateFile? CreateFile { set; }
    RemoveFile? RemoveFile { get; set; }
    RemoveDirectory? RemoveDirectory { set; }
    CalcFileCrc32? CalcFileCrc32 { set; }
    TruncateFile? TruncateFile { set; }
    BurstReadFileDelegate? BurstReadFile { set; }
    WriteFile? WriteFile { set; }
}