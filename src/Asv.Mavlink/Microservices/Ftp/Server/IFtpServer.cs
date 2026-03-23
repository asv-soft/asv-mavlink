using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

/// <summary>
/// Delegate that opens a remote file for reading and returns a read session handle.
/// </summary>
/// <param name="path">Remote file path.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task that returns a <see cref="ReadHandle"/> describing the opened read session.</returns>
public delegate Task<ReadHandle> OpenFileReadDelegate(string path, CancellationToken cancel = default);

/// <summary>
/// Delegate that opens a remote file for writing and returns a write session handle.
/// </summary>
/// <param name="path">Remote file path.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task that returns a <see cref="WriteHandle"/> describing the opened write session.</returns>
public delegate Task<WriteHandle> OpenFileWriteDelegate(string path, CancellationToken cancel = default);

/// <summary>
/// Delegate that reads file data from an opened read session.
/// </summary>
/// <param name="request">Read request details (session, offset, etc.).</param>
/// <param name="buffer">Memory buffer to read data into.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task that returns a <see cref="ReadResult"/> containing read data information.</returns>
public delegate Task<ReadResult> FileReadDelegate(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);

/// <summary>
/// Delegate that renames (or moves) a file or directory.
/// </summary>
/// <param name="path1">Original path.</param>
/// <param name="path2">New path.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task representing the asynchronous operation.</returns>
public delegate Task RenameDelegate(string path1, string path2, CancellationToken cancel = default);

/// <summary>
/// Delegate that terminates a specific session.
/// </summary>
/// <param name="session">Session handle to terminate.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task representing the asynchronous operation.</returns>
public delegate Task TerminateSessionDelegate(byte session, CancellationToken cancel = default);

/// <summary>
/// Delegate that resets (terminates) all active sessions.
/// </summary>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task representing the asynchronous operation.</returns>
public delegate Task ResetSessionsDelegate(CancellationToken cancel = default);

/// <summary>
/// Delegate that lists entries in a directory.
/// </summary>
/// <param name="path">Directory path.</param>
/// <param name="offset">Offset to start listing from.</param>
/// <param name="buffer">Memory buffer to write entry list into.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task that returns the number of bytes written to the buffer.</returns>
public delegate Task<byte> ListDirectoryDelegate(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default);

/// <summary>
/// Delegate that creates a new directory.
/// </summary>
/// <param name="path">Directory path to create.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task representing the asynchronous operation.</returns>
public delegate Task CreateDirectory(string path, CancellationToken cancel = default);

/// <summary>
/// Delegate that creates a new file and returns its session handle.
/// </summary>
/// <param name="path">File path to create.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task that returns the session handle for the new file.</returns>
public delegate Task<byte> CreateFile(string path, CancellationToken cancel = default);

/// <summary>
/// Delegate that removes a file.
/// </summary>
/// <param name="path">File path to remove.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task representing the asynchronous operation.</returns>
public delegate Task RemoveFile(string path, CancellationToken cancel = default);

/// <summary>
/// Delegate that removes a directory.
/// </summary>
/// <param name="path">Directory path to remove.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task representing the asynchronous operation.</returns>
public delegate Task RemoveDirectory(string path, CancellationToken cancel = default);

/// <summary>
/// Delegate that calculates CRC32 checksum for a file.
/// </summary>
/// <param name="path">File path.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task that returns the CRC32 checksum.</returns>
public delegate Task<uint> CalcFileCrc32(string path, CancellationToken cancel = default);

/// <summary>
/// Delegate that truncates a file to a specified length.
/// </summary>
/// <param name="request">Truncate request details.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task representing the asynchronous operation.</returns>
public delegate Task TruncateFile(TruncateRequest request, CancellationToken cancel = default);

/// <summary>
/// Delegate that performs a burst read from an opened read session.
/// </summary>
/// <param name="request">Read request details.</param>
/// <param name="buffer">Memory buffer to read data into.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task that returns a <see cref="BurstReadResult"/> containing read data information.</returns>
public delegate Task<BurstReadResult> BurstReadFileDelegate(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);

/// <summary>
/// Delegate that writes data to an opened write session.
/// </summary>
/// <param name="request">Write request details.</param>
/// <param name="buffer">Memory buffer containing data to write.</param>
/// <param name="cancel">Cancellation token.</param>
/// <returns>A task representing the asynchronous operation.</returns>
public delegate Task WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancel = default);

/// <summary>
/// MAVLink FTP (File Transfer Protocol) server abstraction.
/// </summary>
/// <remarks>
/// The server is configured by assigning handler delegates for supported FTP operations.
/// An implementation is expected to invoke these delegates when handling incoming MAVLink FTP commands.
/// </remarks>
public interface IFtpServer:IMavlinkMicroserviceServer
{
    /// <summary>
    /// Sets the handler that renames (or moves) a file/directory.
    /// </summary>
    RenameDelegate? Rename { set; }
    
    /// <summary>
    /// Sets the handler that opens a file for reading.
    /// </summary>
    OpenFileReadDelegate? OpenFileRead { set; }
    
    /// <summary>
    /// Sets the handler that opens a file for writing.
    /// </summary>
    OpenFileWriteDelegate? OpenFileWrite { set; }
    
    /// <summary>
    /// Sets the handler that reads file data from an opened read session.
    /// </summary>
    FileReadDelegate? FileRead { set; }
    
    /// <summary>
    /// Sets the handler that terminates a session.
    /// </summary>
    TerminateSessionDelegate? TerminateSession { set; }
    
    /// <summary>
    /// Sets the handler that lists directory entries.
    /// </summary>
    ListDirectoryDelegate? ListDirectory { set; }
    
    /// <summary>
    /// Sets the handler that resets (terminates) all sessions.
    /// </summary>
    ResetSessionsDelegate? ResetSessions { set; }
    
    /// <summary>
    /// Sets the handler that creates a directory.
    /// </summary>
    CreateDirectory? CreateDirectory { set; }
    
    /// <summary>
    /// Sets the handler that creates a file.
    /// </summary>
    CreateFile? CreateFile { set; }
    
    /// <summary>
    /// Gets or sets the handler that removes a file.
    /// </summary>
    RemoveFile? RemoveFile { get; set; }
    
    /// <summary>
    /// Sets the handler that removes a directory.
    /// </summary>
    RemoveDirectory? RemoveDirectory { set; }
    
    /// <summary>
    /// Sets the handler that calculates CRC32 for a file.
    /// </summary>
    CalcFileCrc32? CalcFileCrc32 { set; }
    
    /// <summary>
    /// Sets the handler that truncates a file.
    /// </summary>
    TruncateFile? TruncateFile { set; }
    
    /// <summary>
    /// Sets the handler that performs burst reads.
    /// </summary>
    BurstReadFileDelegate? BurstReadFile { set; }
    
    /// <summary>
    /// Sets the handler that writes file data to an open write session.
    /// </summary>
    WriteFile? WriteFile { set; }
}