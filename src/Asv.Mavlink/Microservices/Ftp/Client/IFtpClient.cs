using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

/// <summary>
/// Creating only wrap methods, all business logic will be on the Ex level
/// </summary>
public interface IFtpClient
{
    #region Directory methods

    Task<FtpMessagePayload> ListDirectory(string path, uint offset, byte sequenceNumber, CancellationToken cancel);
    
    Task CreateDirectory(string path, CancellationToken cancel);
    
    Task RemoveDirectory(string path, CancellationToken cancel);
    #endregion
    
    #region File methods

    Task ReadFile(string readPath, string savePath, CancellationToken cancel);

    Task BurstReadFile(string path, string savePath, CancellationToken cancel);
    
    Task UploadFile(string filePath, CancellationToken cancel);
    
    Task RemoveFile(string path, CancellationToken cancel);
    
    Task TruncateFile(string path, int offset, CancellationToken cancel);
    
    #endregion

    #region Observables
    
    IObservable<IPacketV2<IPayload>> OnReceivedPacket { get; }

    #endregion
}

#region Enums
public enum NakError
{
    /// <summary> 
    /// No error 
    /// </summary> 
    None = 0,

    /// <summary> 
    /// Unknown failure 
    /// </summary> 
    Fail = 1,

    /// <summary> 
    /// Command failed, Err number sent back in PayloadHeader.data[1]. This is a file-system error number understood by the server operating system. 
    /// </summary> 
    FailErrno = 2,

    /// <summary> 
    /// Payload size is invalid 
    /// </summary> 
    InvalidDataSize = 3,

    /// <summary> 
    /// Session is not currently open 
    /// </summary> 
    InvalidSession = 4,

    /// <summary> 
    /// All available sessions are already in use. 
    /// </summary> 
    NoSessionsAvailable = 5,

    /// <summary> 
    /// Offset past end of file for ListDirectory and ReadFile commands. 
    /// </summary> 
    EOF = 6,

    /// <summary> 
    /// Unknown command / opcode 
    /// </summary> 
    UnknownCommand = 7,

    /// <summary> 
    /// File/directory already exists 
    /// </summary> 
    FileExists = 8,

    /// <summary> 
    /// File/directory is write protected 
    /// </summary> 
    FileProtected = 9,

    /// <summary> 
    /// File/directory not found 
    /// </summary> 
    FileNotFound = 10,

}

public enum OpCode
{
    /// <summary> 
    /// Ignored, always ACKed
    /// </summary> 
    None = 0,
    
    /// <summary> 
    /// "Terminates open Read session.
    /// - Closes the file associated with (session) and frees the session ID for re-use."
    /// </summary> 
    TerminateSession = 1,
    
    /// <summary> 
    /// "Terminates all open read sessions.
    /// - Clears all state held by the drone (server); closes all open files, etc.
    /// - Sends an ACK reply with no data."
    /// </summary> 
    ResetSessions = 2,
    
    /// <summary> 
    /// "List directory entry information (files, folders etc.) in <path>, starting from a specified entry index (<offset>).
    /// - Response is an ACK packet with one or more entries on success, otherwise a NAK packet with an error code.
    /// - Completion is indicated by a NACK with EOF in response to a requested index (offset) beyond the list of entries.
    /// - The directory is closed after the operation, so this leaves no state on the server."
    /// </summary> 
    ListDirectory = 3,
    
    /// <summary> 
    /// "Opens file at <path> for reading, returns <session>
    /// - The path is stored in the payload data. The drone opens the file (path) and allocates a session number. The file must exist.
    /// - An ACK packet must include the allocated session and the data size of the file to be opened (size)
    /// - A NAK packet must contain error information . Typical error codes for this command are NoSessionsAvailable, FileExists.
    /// - The file remains open after the operation, and must eventually be closed by Reset or Terminate."
    /// </summary> 
    OpenFileRO = 4,
    
    /// <summary> 
    /// "Reads <size> bytes from <offset> in <session>.
    /// - Seeks to (offset) in the file opened in (session) and reads (size) bytes into the result buffer.
    /// - Sends an ACK packet with the result buffer on success, otherwise a NAK packet with an error code. For short reads or reads beyond the end of a file, the (size) field in the ACK packet will indicate the actual number of bytes read.
    /// - Reads can be issued to any offset in the file for any number of bytes, so reconstructing portions of the file to deal with lost packets should be easy.
    /// - For best download performance, try to keep two Read packets in flight."
    /// </summary> 
    ReadFile = 5,
    
    /// <summary> 
    /// "Creates file at <path> for writing, returns <session>.
    /// - Creates the file (path) and allocates a session number. The file must not exist, but all parent directories must exist.
    /// - Sends an ACK packet with the allocated session number on success, or a NAK packet with an error code on error (i.e. FileExists if the path already exists).
    /// - The file remains open after the operation, and must eventually be closed by Reset or Terminate."
    /// </summary> 
    CreateFile = 6,
    
    /// <summary> 
    /// "Writes <size> bytes to <offset> in <session>.
    /// - Sends an ACK reply with no data on success, otherwise a NAK packet with an error code."
    /// </summary> 
    WriteFile = 7,
    
    /// <summary> 
    /// "Remove file at <path>.
    /// - ACK reply with no data on success.
    /// - NAK packet with error information on failure."
    /// </summary> 
    RemoveFile = 8,
    
    /// <summary> 
    /// "Creates directory at <path>.
    /// - Sends an ACK reply with no data on success, otherwise a NAK packet with an error code."
    /// </summary> 
    CreateDirectory = 9,
    
    /// <summary> 
    /// "Removes directory at <path>. The directory must be empty.
    /// - Sends an ACK reply with no data on success, otherwise a NAK packet with an error code."
    /// </summary> 
    RemoveDirectory = 10,
    
    /// <summary> 
    /// "Opens file at <path> for writing, returns <session>.
    /// - Opens the file (path) and allocates a session number. The file must exist.
    /// - Sends an ACK packet with the allocated session number on success, otherwise a NAK packet with an error code.
    /// - The file remains open after the operation, and must eventually be closed by Reset or Terminate."
    /// </summary> 
    OpenFileWO = 11,
    
    /// <summary> 
    /// "Truncate file at <path> to <offset> length.
    /// - Sends an ACK reply with no data on success, otherwise a NAK packet with an error code."
    /// </summary> 
    TruncateFile = 12,
    
    /// <summary> 
    /// "Rename <path1> to <path2>.
    /// - Sends an ACK reply the no data on success, otherwise a NAK packet with an error code (i.e. if the source path does not exist)."
    /// </summary> 
    Rename = 13,
    
    /// <summary> 
    /// "Calculate CRC32 for file at <path>.
    /// - Sends an ACK reply with the checksum on success, otherwise a NAK packet with an error code."
    /// </summary> 
    CalcFileCRC32 = 14,
    
    /// <summary> 
    /// Burst download session file.
    /// </summary> 
    BurstReadFile = 15,

    /// <summary>
    ///  ACK response.
    /// </summary>
    ACK = 128,
    
    /// <summary>
    ///  NAK response.
    /// </summary>
    NAK = 129,
}

public enum FileItemType
{
    File,
    Directory,
    Skip,
}

public enum ErrNo
{
    /// <summary>
    /// Operation not permitted
    /// </summary>
    EPERM = 1,

    /// <summary>
    /// No such file or directory
    /// </summary>
    ENOENT = 2,

    /// <summary>
    /// No such process
    /// </summary>
    ESRCH = 3,

    /// <summary>
    /// Interrupted system call
    /// </summary>
    EINTR = 4,

    /// <summary>
    /// Input/output error
    /// </summary>
    EIO = 5,

    /// <summary>
    /// No such device or address
    /// </summary>
    ENXIO = 6,

    /// <summary>
    /// Argument list too long
    /// </summary>
    E2BIG = 7,

    /// <summary>
    /// Exec format error
    /// </summary>
    ENOEXEC = 8,

    /// <summary>
    /// Bad file descriptor
    /// </summary>
    EBADF = 9,

    /// <summary>
    /// No child processes
    /// </summary>
    ECHILD = 10,

    /// <summary>
    /// Resource temporarily unavailable
    /// </summary>
    EAGAIN = 11,

    /// <summary>
    /// Cannot allocate memory
    /// </summary>
    ENOMEM = 12,

    /// <summary>
    /// Permission denied
    /// </summary>
    EACCES = 13,

    /// <summary>
    /// Bad address
    /// </summary>
    EFAULT = 14,

    /// <summary>
    /// Block device required
    /// </summary>
    ENOTBLK = 15,

    /// <summary>
    /// Device or resource busy
    /// </summary>
    EBUSY = 16,

    /// <summary>
    /// File exists
    /// </summary>
    EEXIST = 17,

    /// <summary>
    /// Invalid cross-device link
    /// </summary>
    EXDEV = 18,

    /// <summary>
    /// No such device
    /// </summary>
    ENODEV = 19,

    /// <summary>
    /// Not a directory
    /// </summary>
    ENOTDIR = 20,

    /// <summary>
    /// Is a directory
    /// </summary>
    EISDIR = 21,

    /// <summary>
    /// Invalid argument
    /// </summary>
    EINVAL = 22,

    /// <summary>
    /// Too many open files in system
    /// </summary>
    ENFILE = 23,

    /// <summary>
    /// Too many open files
    /// </summary>
    EMFILE = 24,

    /// <summary>
    /// Inappropriate ioctl for device
    /// </summary>
    ENOTTY = 25,

    /// <summary>
    /// Text file busy</summary>
    ETXTBSY = 26,

    /// <summary>
    /// File too large
    /// </summary>
    EFBIG = 27,

    /// <summary>
    /// No space left on device
    /// </summary>
    ENOSPC = 28,

    /// <summary>
    /// Illegal seek
    /// </summary>
    ESPIPE = 29,

    /// <summary>
    /// Read-only file system
    /// </summary>
    EROFS = 30,

    /// <summary>
    /// Too many links
    /// </summary>
    EMLINK = 31,

    /// <summary>
    /// Broken pipe
    /// </summary>
    EPIPE = 32,

    /// <summary>
    /// Numerical argument out of domain
    /// </summary>
    EDOM = 33,

    /// <summary>
    /// Numerical result out of range
    /// </summary>
    ERANGE = 34,

    /// <summary>
    /// Resource deadlock avoided
    /// </summary>
    EDEADLK = 35,

    /// <summary>
    /// File name too long
    /// </summary>
    ENAMETOOLONG = 36,

    /// <summary>
    /// No locks available
    /// </summary>
    ENOLCK = 37,

    /// <summary>
    /// Function not implemented
    /// </summary>
    ENOSYS = 38,

    /// <summary>
    /// Directory not empty
    /// </summary>
    ENOTEMPTY = 39,

    /// <summary>
    /// Too many levels of symbolic links
    /// </summary>
    ELOOP = 40,

    /// <summary>
    /// Resource temporarily unavailable
    /// </summary>
    EWOULDBLOCK = 11,

    /// <summary>
    /// No message of desired type
    /// </summary>
    ENOMSG = 42,

    /// <summary>
    /// Identifier removed
    /// </summary>
    EIDRM = 43,
    
    /// <summary>
    /// Channel number out of range
    /// </summary>
    ECHRNG = 44,

    /// <summary>
    /// Level 2 not synchronized
    /// </summary>
    EL2NSYNC = 45,

    /// <summary>
    /// Level 3 halted
    /// </summary>
    EL3HLT = 46,

    /// <summary>
    /// Level 3 reset
    /// </summary>
    EL3RST = 47,

    /// <summary>
    /// Link number out of range
    /// </summary>
    ELNRNG = 48,

    /// <summary>
    /// Protocol driver not attached
    /// </summary>
    EUNATCH = 49,

    /// <summary>
    /// No CSI structure available
    /// </summary>
    ENOCSI = 50,

    /// <summary>
    /// Level 2 halted
    /// </summary>
    EL2HLT = 51,

    /// <summary>
    /// Invalid exchange
    /// </summary>
    EBADE = 52,

    /// <summary>
    /// Invalid request descriptor
    /// </summary>
    EBADR = 53,

    /// <summary>
    /// Exchange full
    /// </summary>
    EXFULL = 54,

    /// <summary>
    /// No anode
    /// </summary>
    ENOANO = 55,

    /// <summary>
    /// Invalid request code
    /// </summary>
    EBADRQC = 56,

    /// <summary>
    /// Invalid slot
    /// </summary>
    EBADSLT = 57,

    /// <summary>
    /// Resource deadlock avoided
    /// </summary>
    EDEADLOCK = 35,

    /// <summary>
    /// Bad font file format
    /// </summary>
    EBFONT = 59,

    /// <summary>
    /// Device not a stream
    /// </summary>
    ENOSTR = 60,

    /// <summary>
    /// No data available
    /// </summary>
    ENODATA = 61,

    /// <summary>
    /// Timer expired
    /// </summary>
    ETIME = 62,

    /// <summary>
    /// Out of streams resources
    /// </summary>
    ENOSR = 63,

    /// <summary>
    /// Machine is not on the network
    /// </summary>
    ENONET = 64,

    /// <summary>
    /// Package not installed
    /// </summary>
    ENOPKG = 65,

    /// <summary>
    /// Object is remote
    /// </summary>
    EREMOTE = 66,

    /// <summary>
    /// Link has been severed
    /// </summary>
    ENOLINK = 67,

    /// <summary>
    /// Advertise error
    /// </summary>
    EADV = 68,

    /// <summary>
    /// Srmount error
    /// </summary>
    ESRMNT = 69,

    /// <summary>
    /// Communication error on send
    /// </summary>
    ECOMM = 70,

    /// <summary>
    /// Protocol error
    /// </summary>
    EPROTO = 71,

    /// <summary>
    /// Multihop attempted
    /// </summary>
    EMULTIHOP = 72,

    /// <summary>
    /// RFS specific error
    /// </summary>
    EDOTDOT = 73,

    /// <summary>
    /// Bad message
    /// </summary>
    EBADMSG = 74,

    /// <summary>
    /// Value too large for defined data type
    /// </summary>
    EOVERFLOW = 75,

    /// <summary>
    /// Name not unique on network
    /// </summary>
    ENOTUNIQ = 76,

    /// <summary>
    /// File descriptor in bad state
    /// </summary>
    EBADFD = 77,

    /// <summary>
    /// Remote address changed
    /// </summary>
    EREMCHG = 78,

    /// <summary>
    /// Can not access a needed shared library
    /// </summary>
    ELIBACC = 79,

    /// <summary>
    /// Accessing a corrupted shared library
    /// </summary>
    ELIBBAD = 80,

    /// <summary>
    /// .lib section in a.out corrupted
    /// </summary>
    ELIBSCN = 81,

    /// <summary>
    /// Attempting to link in too many shared libraries
    /// </summary>
    ELIBMAX = 82,

    /// <summary>
    /// Cannot exec a shared library directly
    /// </summary>
    ELIBEXEC = 83,

    /// <summary>
    /// Invalid or incomplete multibyte or wide character
    /// </summary>
    EILSEQ = 84,

    /// <summary>
    /// Interrupted system call should be restarted
    /// </summary>
    ERESTART = 85,

    /// <summary>
    /// Streams pipe error
    /// </summary>
    ESTRPIPE = 86,

    /// <summary>
    /// Too many users
    /// </summary>
    EUSERS = 87,

    /// <summary>
    /// Socket operation on non-socket
    /// </summary>
    ENOTSOCK = 88,

    /// <summary>
    /// Destination address required
    /// </summary>
    EDESTADDRREQ = 89,

    /// <summary>
    /// Message too long
    /// </summary>
    EMSGSIZE = 90,

    /// <summary>
    /// Protocol wrong type for socket
    /// </summary>
    EPROTOTYPE = 91,

    /// <summary>
    /// Protocol not available
    /// </summary>
    ENOPROTOOPT = 92,

    /// <summary>
    /// Protocol not supported
    /// </summary>
    EPROTONOSUPPORT = 93,

    /// <summary>
    /// Socket type not supported
    /// </summary>
    ESOCKTNOSUPPORT = 94,

    /// <summary>
    /// Operation not supported
    /// </summary>
    EOPNOTSUPP = 95,

    /// <summary>
    /// Protocol family not supported
    /// </summary>
    EPFNOSUPPORT = 96,

    /// <summary>
    /// Address family not supported by protocol
    /// </summary>
    EAFNOSUPPORT = 97,

    /// <summary>
    /// Address already in use
    /// </summary>
    EADDRINUSE = 98,

    /// <summary>
    /// Cannot assign requested address
    /// </summary>
    EADDRNOTAVAIL = 99,

    /// <summary>
    /// Network is down
    /// </summary>
    ENETDOWN = 100,

    /// <summary>
    /// Network is unreachable
    /// </summary>
    ENETUNREACH = 101,

    /// <summary>
    /// Network dropped connection on reset
    /// </summary>
    ENETRESET = 102,

    /// <summary>
    /// Software caused connection abort
    /// </summary>
    ECONNABORTED = 103,

    /// <summary>
    /// Connection reset by peer
    /// </summary>
    ECONNRESET = 104,

    /// <summary>
    /// No buffer space available
    /// </summary>
    ENOBUFS = 105,

    /// <summary>
    /// Transport endpoint is already connected
    /// </summary>
    EISCONN = 106,

    /// <summary>
    /// Transport endpoint is not connected
    /// </summary>
    ENOTCONN = 107,

    /// <summary>
    /// Cannot send after transport endpoint shutdown
    /// </summary>
    ESHUTDOWN = 108,

    /// <summary>
    /// Too many references: cannot splice
    /// </summary>
    ETOOMANYREFS = 109,

    /// <summary>
    /// Connection timed out
    /// </summary>
    ETIMEDOUT = 110,

    /// <summary>
    /// Connection refused
    /// </summary>
    ECONNREFUSED = 111,

    /// <summary>
    /// Host is down
    /// </summary>
    EHOSTDOWN = 112,

    /// <summary>
    /// No route to host
    /// </summary>
    EHOSTUNREACH = 113,

    /// <summary>
    /// Operation already in progress
    /// </summary>
    EALREADY = 114,

    /// <summary>
    /// Operation now in progress
    /// </summary>
    EINPROGRESS = 115,

    /// <summary>
    /// Stale file handle
    /// </summary>
    ESTALE = 116,

    /// <summary>
    /// Structure needs cleaning
    /// </summary>
    EUCLEAN = 117,

    /// <summary>
    /// Not a XENIX named type file
    /// </summary>
    ENOTNAM = 118,

    /// <summary>
    /// No XENIX semaphores available
    /// </summary>
    ENAVAIL = 119,

    /// <summary>
    /// Is a named type file
    /// </summary>
    EISNAM = 120,

    /// <summary>
    /// Remote I/O error
    /// </summary>
    EREMOTEIO = 121,

    /// <summary>
    /// Disk quota exceeded
    /// </summary>
    EDQUOT = 122,

    /// <summary>
    /// No medium found
    /// </summary>
    ENOMEDIUM = 123,

    /// <summary>
    /// Wrong medium type
    /// </summary>
    EMEDIUMTYPE = 124,

    /// <summary>
    /// Operation canceled
    /// </summary>
    ECANCELED = 125,

    /// <summary>
    /// Required key not available
    /// </summary>
    ENOKEY = 126,

    /// <summary>
    /// Key has expired
    /// </summary>
    EKEYEXPIRED = 127,

    /// <summary>
    /// Key has been revoked
    /// </summary>
    EKEYREVOKED = 128,

    /// <summary>
    /// Key was rejected by service
    /// </summary>
    EKEYREJECTED = 129,

    /// <summary>
    /// Owner died
    /// </summary>
    EOWNERDEAD = 130,

    /// <summary>
    /// State not recoverable
    /// </summary>
    ENOTRECOVERABLE = 131,

    /// <summary>
    /// Operation not possible due to RF-kill
    /// </summary>
    ERFKILL = 132,

    /// <summary>
    /// Memory page has hardware error
    /// </summary>
    EHWPOISON = 133,

    /// <summary>
    /// Operation not supported
    /// </summary>
    ENOTSUP = 95,
}
#endregion