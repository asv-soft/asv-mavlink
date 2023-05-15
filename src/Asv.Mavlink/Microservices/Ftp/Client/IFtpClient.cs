using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

/// <summary>
/// Creating only wrap methods, all business logic will be on the Ex level
/// </summary>
public interface IFtpClient
{
    public IObservable<FtpMessagePayload> OnBurstReadPacket { get; }
    Task<FtpMessagePayload> None(CancellationToken cancel);
    Task<FtpMessagePayload> TerminateSession(byte sequenceNumber, CancellationToken cancel);
    Task<FtpMessagePayload> ResetSessions(CancellationToken cancel);
    Task<FtpMessagePayload> OpenFileRO(string path, CancellationToken cancel);
    Task<FtpMessagePayload> ReadFile(byte size, uint offset, byte sessionNumber, CancellationToken cancel);
    Task<FtpMessagePayload> CreateFile(string path, CancellationToken cancel);
    Task<FtpMessagePayload> WriteFile(byte[] writeBuffer, uint offset, byte sessionNumber, CancellationToken cancel);
    Task<FtpMessagePayload> RemoveFile(string path, CancellationToken cancel);
    Task<FtpMessagePayload> CreateDirectory(string path, CancellationToken cancel);
    Task<FtpMessagePayload> RemoveDirectory(string path, CancellationToken cancel);
    Task<FtpMessagePayload> OpenFileWO(string path, CancellationToken cancel);
    Task<FtpMessagePayload> ListDirectory(string path, uint offset, CancellationToken cancel);
    Task<FtpMessagePayload> TruncateFile(string path, uint offset, CancellationToken cancel);
    Task<FtpMessagePayload> Rename(string pathToRename, string newPath, CancellationToken cancel);
    Task<FtpMessagePayload> CalcFileCRC32(string path, CancellationToken cancel);
    Task<FtpMessagePayload> BurstReadFile(byte size, uint offset, byte sessionNumber, CancellationToken cancel);
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

public enum FtpEntryType
{
    File = 'F',
    Directory = 'D',
    Skip = 'S',
}
#endregion