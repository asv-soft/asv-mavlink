using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

/// <summary>
/// Extended MAVLink FTP server microservice.
/// Provides high-level methods for file and directory operations over MAVLink FTP.
/// </summary>
public interface IFtpServerEx : IMavlinkMicroserviceServer
{
    /// <summary>
    /// Gets the base FTP server instance.
    /// </summary>
    IFtpServer Base { get; }
    
    /// <summary>
    /// Opens a file for reading.
    /// </summary>
    /// <param name="path">The path of the file to open.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ReadHandle"/> for the file.</returns>
    public Task<ReadHandle> OpenFileRead(string path, CancellationToken cancel = default);

    /// <summary>
    /// Opens a file for writing.
    /// </summary>
    /// <param name="path">The path of the file to open.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="WriteHandle"/> for the file.</returns>
    public Task<WriteHandle> OpenFileWrite(string path, CancellationToken cancel = default);

    /// <summary>
    /// Reads data from a file using a read request.
    /// </summary>
    /// <param name="request">The read request containing session and offset information.</param>
    /// <param name="buffer">The buffer to store the read data.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ReadResult"/>.</returns>
    public Task<ReadResult> FileRead(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);

    /// <summary>
    /// Renames a file or directory.
    /// </summary>
    /// <param name="path1">The current path of the file or directory.</param>
    /// <param name="path2">The new path of the file or directory.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task Rename(string path1, string path2, CancellationToken cancel = default);

    /// <summary>
    /// Terminates a specific FTP session.
    /// </summary>
    /// <param name="session">The session ID to terminate.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task TerminateSession(byte session, CancellationToken cancel = default);

    /// <summary>
    /// Resets all active FTP sessions.
    /// </summary>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task ResetSessions(CancellationToken cancel = default);

    /// <summary>
    /// Lists the contents of a directory.
    /// </summary>
    /// <param name="path">The path of the directory to list.</param>
    /// <param name="offset">The offset within the directory listing.</param>
    /// <param name="buffer">The buffer to store the directory listing as characters.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of bytes written to the buffer.</returns>
    public Task<byte> ListDirectory(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default);

    /// <summary>
    /// Creates a new directory.
    /// </summary>
    /// <param name="path">The path of the directory to create.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task CreateDirectory(string path, CancellationToken cancel = default);

    /// <summary>
    /// Creates a new file.
    /// </summary>
    /// <param name="path">The path of the file to create.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the session ID for the newly created file.</returns>
    public Task<byte> CreateFile(string path, CancellationToken cancel = default);

    /// <summary>
    /// Removes a file.
    /// </summary>
    /// <param name="path">The path of the file to remove.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task RemoveFile(string path, CancellationToken cancel = default);

    /// <summary>
    /// Removes a directory.
    /// </summary>
    /// <param name="path">The path of the directory to remove.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task RemoveDirectory(string path, CancellationToken cancel = default);

    /// <summary>
    /// Calculates the CRC32 checksum of a file.
    /// </summary>
    /// <param name="path">The path of the file.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the CRC32 checksum.</returns>
    public Task<uint> CalcFileCrc32(string path, CancellationToken cancel = default);

    /// <summary>
    /// Truncates a file to a specific length.
    /// </summary>
    /// <param name="request">The truncate request containing path and length information.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task TruncateFile(TruncateRequest request, CancellationToken cancel = default);

    /// <summary>
    /// Performs a burst read from a file.
    /// </summary>
    /// <param name="request">The read request containing session and offset information.</param>
    /// <param name="buffer">The buffer to store the read data.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="BurstReadResult"/>.</returns>
    public Task<BurstReadResult> BurstReadFile(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);

    /// <summary>
    /// Writes data to a file.
    /// </summary>
    /// <param name="request">The write request containing session and offset information.</param>
    /// <param name="buffer">The buffer containing the data to write.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancel = default);
}
