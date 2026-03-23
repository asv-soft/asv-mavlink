using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

namespace Asv.Mavlink;

/// <summary>
/// MAVLink FTP (File Transfer Protocol) client abstraction.
/// Provides remote filesystem operations over the MAVLink FTP microservice.
/// </summary>
/// <remarks>
/// Most methods return the raw <see cref="FileTransferProtocolPacket"/> response to allow callers to parse payload
/// and handle implementation-specific details.
/// </remarks>
public interface IFtpClient:IMavlinkMicroserviceClient
{
    /// <summary>
    /// Opens a file for reading on the remote filesystem.
    /// </summary>
    /// <param name="path">Remote file path.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns><see cref="ReadHandle"/> with session id and file size.</returns>
    Task<ReadHandle> OpenFileRead(string path, CancellationToken cancel = default);
    
    /// <summary>
    /// Opens a file for writing on the remote filesystem.
    /// </summary>
    /// <param name="path">Remote file path.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns><see cref="WriteHandle"/> with session id and file size.</returns>
    Task<WriteHandle> OpenFileWrite(string path, CancellationToken cancel = default);

    /// <summary>
    /// Creates a directory on the remote filesystem.
    /// </summary>
    /// <param name="path">Remote directory path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Raw <see cref="FileTransferProtocolPacket"/> response.</returns>
    public Task<FileTransferProtocolPacket> CreateDirectory(string path, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates an empty file on the remote filesystem.
    /// </summary>
    /// <param name="path">Remote file path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Raw <see cref="FileTransferProtocolPacket"/> response.</returns>
    public Task<FileTransferProtocolPacket> CreateFile(string path, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Resets (closes) all FTP sessions on the remote side.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Raw <see cref="FileTransferProtocolPacket"/> response.</returns>
    public Task<FileTransferProtocolPacket> ResetSessions(CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a directory at the specified path on the remote filesystem.
    /// </summary>
    /// <param name="path">Remote directory path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Raw <see cref="FileTransferProtocolPacket"/> response.</returns>
    public Task<FileTransferProtocolPacket> RemoveDirectory(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a file at the specified path on the remote filesystem.
    /// </summary>
    /// <param name="path">Remote file path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Raw <see cref="FileTransferProtocolPacket"/> response.</returns>
    public Task<FileTransferProtocolPacket> RemoveFile(string path, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Calculates CRC32 of a remote file.
    /// </summary>
    /// <param name="path">Remote file path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>CRC32 of the remote file.</returns>
    public Task<uint> CalcFileCrc32(string path, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Truncates a remote file to the specified byte offset.
    /// </summary>
    /// <param name="request">Truncate request (path and offset) of type <see cref="TruncateRequest"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Raw <see cref="FileTransferProtocolPacket"/> response.</returns>
    public Task<FileTransferProtocolPacket> TruncateFile(TruncateRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Writes a chunk of bytes into an already opened write session on the remote filesystem.
    /// </summary>
    /// <param name="request">Write request (session, offset, and size) of type <see cref="WriteRequest"/>.</param>
    /// <param name="buffer">Source buffer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Raw <see cref="FileTransferProtocolPacket"/> response.</returns>
    public Task<FileTransferProtocolPacket> WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Performs burst file read from the remote filesystem. Data is delivered to <paramref name="onBurstData"/> as packets arrive.
    /// </summary>
    /// <param name="request">Read request (session, offset, and size) of type <see cref="ReadRequest"/>.</param>
    /// <param name="onBurstData">Callback invoked for each received burst packet.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns><see cref="Task"/></returns>
    Task BurstReadFile(ReadRequest request, Action<FileTransferProtocolPacket> onBurstData, CancellationToken cancel = default);

    /// <summary>
    /// Reads a chunk of bytes from an already opened read session on the remote filesystem.
    /// </summary>
    /// <param name="request">Read request (session, offset, and size) of type <see cref="ReadRequest"/>.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns>Raw <see cref="FileTransferProtocolPacket"/> response.</returns>
    Task<FileTransferProtocolPacket> ReadFile(ReadRequest request, CancellationToken cancel = default);
    
    /// <summary>
    /// Renames or moves a file/directory on the remote filesystem.
    /// </summary>
    /// <param name="path1">Source path.</param>
    /// <param name="path2">Destination path (new name for the path).</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns>Raw <see cref="FileTransferProtocolPacket"/> response.</returns>
    Task<FileTransferProtocolPacket> Rename(string path1, string path2, CancellationToken cancel = default);
    
    /// <summary>
    /// Terminates an FTP session on the remote side.
    /// </summary>
    /// <param name="session">Session id.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns><see cref="Task"/></returns>
    Task TerminateSession(byte session, CancellationToken cancel = default);
    
    /// <summary>
    /// Lists a directory starting at the given offset cursor from the remote filesystem.
    /// </summary>
    /// <param name="path">Remote directory path.</param>
    /// <param name="offset">Paging offset.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns>Raw <see cref="FileTransferProtocolPacket"/> response.</returns>
    Task<FileTransferProtocolPacket> ListDirectory(string path, uint offset, CancellationToken cancel = default);
    
    /// <summary>
    /// Reads file data into a byte memory buffer from the remote filesystem.
    /// </summary>
    /// <param name="request">Read request (session, offset, and size) of type <see cref="ReadRequest"/>.</param>
    /// <param name="buffer">Destination buffer.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns>Struct of type <see cref="ReadResult"/>.</returns>
    public async Task<ReadResult> ReadFile(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)
    {
        var result = await ReadFile(request, cancel).ConfigureAwait(false);
        var readCount = result.ReadData(buffer);
        return new ReadResult(readCount, request);
    }
    
    /// <summary>
    /// Reads file data into an <see cref="IBufferWriter{T}"/> from the remote filesystem.
    /// </summary>
    /// <param name="request">Read request (session, offset, and size) of type <see cref="ReadRequest"/>.</param>
    /// <param name="buffer">Destination buffer writer.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns>Struct of type <see cref="ReadResult"/>.</returns>
    public async Task<ReadResult> ReadFile(ReadRequest request, IBufferWriter<byte> buffer, CancellationToken cancel = default)
    {
        var result = await ReadFile(request, cancel).ConfigureAwait(false);
        var readCount = result.ReadData(buffer);
        return new ReadResult(readCount, request);
    }

    /// <summary>
    /// Lists directory and writes raw bytes into an <see cref="IBufferWriter{T}"/> from the remote filesystem.
    /// </summary>
    /// <param name="path">Remote directory path.</param>
    /// <param name="offset">Paging offset.</param>
    /// <param name="buffer">Destination buffer writer.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns>Returns the number of bytes written.</returns>
    public async Task<byte> ListDirectory(string path, uint offset, IBufferWriter<byte> buffer, CancellationToken cancel = default)
    {
        var result = await ListDirectory(path, offset, cancel).ConfigureAwait(false);
        return result.ReadData(buffer);
    }

    /// <summary>
    /// Lists directory and writes decoded characters into an <see cref="IBufferWriter{T}"/> from the remote filesystem.
    /// </summary>
    /// /// <param name="path">Remote directory path.</param>
    /// <param name="offset">Paging offset.</param>
    /// <param name="buffer">Destination buffer writer.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns>Returns the number of bytes read from the payload.</returns>
    public async Task<byte> ListDirectory(string path, uint offset, IBufferWriter<char> buffer, CancellationToken cancel = default)
    {
        var result = await ListDirectory(path, offset, cancel).ConfigureAwait(false);
        return result.ReadDataAsString(buffer);
    }

    /// <summary>
    /// Lists directory and writes decoded characters into a <see cref="Memory{T}"/> from the remote filesystem.
    /// </summary>
    /// <param name="path">Remote directory path.</param>
    /// <param name="offset">Paging offset.</param>
    /// <param name="buffer">Destination buffer.</param>
    /// <param name="cancel">Cancellation token.</param>
    /// <returns>Returns the number of bytes read from the payload.</returns>
    public async Task<byte> ListDirectory(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default)
    {
        var result = await ListDirectory(path, offset, cancel).ConfigureAwait(false);
        return result.ReadDataAsString(buffer);
    }
}
