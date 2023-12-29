using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

/// <summary>
/// Represents an FTP client interface.
/// </summary>
public interface IFtpClient
{
    /// <summary>
    /// Gets an IObservable object that represents the event when a burst read packet is received.
    /// </summary>
    /// <value>
    /// An IObservable object that provides a stream of FtpMessagePayload objects representing the burst read packets.
    /// </value>
    /// <remarks>
    /// This property allows you to subscribe to the event when a burst read packet is received. The returned IObservable
    /// object provides a stream of FtpMessagePayload objects that can be consumed by subscribers. You can use the Subscribe
    /// method of the IObservable object to register a callback or handler to handle the burst read packets.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Subscribe to the OnBurstReadPacket event
    /// OnBurstReadPacket.Subscribe(payload =>
    /// {
    /// // Handle the burst read packet
    /// Console.WriteLine(payload.Data);
    /// });
    /// </code>
    /// </example>
    public IObservable<FtpMessagePayload> OnBurstReadPacket { get; }

    /// <summary>
    /// This method does something.
    /// </summary>
    /// <param name="cancel">The cancellation token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// The task result is an instance of <see cref="FtpMessagePayload"/>.
    /// </returns>
    /// <remarks>
    /// Add any additional information or notes about the method here.
    /// </remarks>
    /// <example>
    /// This is an example usage of the None method.
    /// <code>
    /// var cancelToken = new CancellationToken();
    /// var result = await None(cancelToken);
    /// </code>
    /// </example>
    Task<FtpMessagePayload> None(CancellationToken cancel);

    /// <summary>
    /// Terminates an FTP session.
    /// </summary>
    /// <param name="sequenceNumber">The sequence number of the session to be terminated.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous termination operation. The task result is an FtpMessagePayload object.</returns>
    /// <remarks>
    /// This method terminates the FTP session identified by the specified sequence number.
    /// The operation is performed asynchronously and can be canceled using the cancellation token.
    /// </remarks>
    Task<FtpMessagePayload> TerminateSession(byte sequenceNumber, CancellationToken cancel);

    /// <summary>
    /// Resets the FTP sessions asynchronously. </summary> <param name="cancel">The cancellation token to cancel the operation.</param> <returns>A task representing the asynchronous operation.</returns>
    /// /
    Task<FtpMessagePayload> ResetSessions(CancellationToken cancel);

    /// <summary>
    /// Opens a file in read-only mode from the specified path on an FTP server.
    /// </summary>
    /// <param name="path">The path of the file on the FTP server.</param>
    /// <param name="cancel">A token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result is a payload containing metadata
    /// for the opened file.
    /// </returns>
    Task<FtpMessagePayload> OpenFileRO(string path, CancellationToken cancel);

    /// <summary>
    /// Reads a file from an FTP server.
    /// </summary>
    /// <param name="size">The size of the file to read.</param>
    /// <param name="offset">The offset in the file to start reading from.</param>
    /// <param name="sessionNumber">The session number to use for the FTP connection.</param>
    /// <param name="cancel">A cancellation token to stop the operation if needed.</param>
    /// <returns>A task that represents the asynchronous file reading operation.
    /// The task result contains the payload of the FTP message.</returns>
    Task<FtpMessagePayload> ReadFile(byte size, uint offset, byte sessionNumber, CancellationToken cancel);

    /// <summary>
    /// Creates a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to be created.</param>
    /// <param name="cancel">The cancellation token to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation which completes with an FtpMessagePayload.</returns>
    Task<FtpMessagePayload> CreateFile(string path, CancellationToken cancel);

    /// <summary>
    /// Writes the provided byte array to the FTP server at the specified offset and session number.
    /// </summary>
    /// <param name="writeBuffer">The byte array to write to the server.</param>
    /// <param name="offset">The offset, in bytes, to start writing in the server.</param>
    /// <param name="sessionNumber">The session number to use for the write operation.</param>
    /// <param name="cancel">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation. The task result is a <see cref="FtpMessagePayload"/>
    /// object that contains information about the status of the write operation.
    /// </returns>
    Task<FtpMessagePayload> WriteFile(byte[] writeBuffer, uint offset, byte sessionNumber, CancellationToken cancel);

    /// <summary>
    /// Removes a file from the specified path.
    /// </summary>
    /// <param name="path">The path of the file to be removed.</param>
    /// <param name="cancel">The cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the payload message.</returns>
    /// <remarks>
    /// Use this method to remove a file from the specified path. The method returns a task that can be awaited for the operation to complete.
    /// If the operation is canceled using the provided cancellation token, a Canceled task will be returned.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to remove a file from the specified path:
    /// <code>
    /// using FtpClient;
    /// using System.Threading;
    /// var client = new FtpClient();
    /// CancellationTokenSource cts = new CancellationTokenSource();
    /// // Call the RemoveFile method to remove the file
    /// Task<FtpMessagePayload> removeTask = client.RemoveFile("/path/to/file.txt", cts.Token);
    /// try
    /// {
    /// // Wait for the task to complete
    /// await removeTask;
    /// // Check if the task was canceled
    /// if (removeTask.IsCanceled)
    /// {
    /// Console.WriteLine("The remove operation was canceled.");
    /// }
    /// else
    /// {
    /// // Get the result payload
    /// FtpMessagePayload payload = removeTask.Result;
    /// Console.WriteLine("File removed successfully.");
    /// }
    /// }
    /// catch (Exception ex)
    /// {
    /// Console.WriteLine($"An error occurred: {ex.Message}");
    /// }
    /// finally
    /// {
    /// // Dispose the cancellation token source
    /// cts.Dispose();
    /// }
    /// </code>
    /// </example>
    Task<FtpMessagePayload> RemoveFile(string path, CancellationToken cancel);

    /// <summary>
    /// Creates a directory at the specified path in the FTP server.
    /// </summary>
    /// <param name="path">The path where the directory should be created.</param>
    /// <param name="cancel">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the FTP message payload
    /// indicating the success or failure of the directory creation operation.
    /// </returns>
    Task<FtpMessagePayload> CreateDirectory(string path, CancellationToken cancel);

    /// <summary>
    /// Removes a directory from the FTP server.
    /// </summary>
    /// <param name="path">The path of the directory to remove.</param>
    /// <param name="cancel">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result represents the response from the FTP server.</returns>
    /// <remarks>The RemoveDirectory method is used to delete a directory from the FTP server. This operation is performed asynchronously.</remarks>
    Task<FtpMessagePayload> RemoveDirectory(string path, CancellationToken cancel);

    /// <summary>
    /// Opens a file without writing it using the given file path.
    /// </summary>
    /// <param name="path">The path of the file to open.</param>
    /// <param name="cancel">The cancellation token.</param>
    /// <returns>A task representing the operation. The task result is a <see cref="FtpMessagePayload"/> object.</returns>
    Task<FtpMessagePayload> OpenFileWO(string path, CancellationToken cancel);

    /// <summary>
    /// Retrieves a directory listing from the specified remote FTP server path.
    /// </summary>
    /// <param name="path">The remote FTP server path to list the directory contents.</param>
    /// <param name="offset">The offset to skip the specified number of directory entries.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="FtpMessagePayload"/> object that represents the directory listing.</returns>
    Task<FtpMessagePayload> ListDirectory(string path, uint offset, CancellationToken cancel);

    /// <summary>
    /// Truncates a file at the specified path by removing a given number of bytes from the beginning. </summary> <param name="path">The path of the file to be truncated.</param> <param name="offset">The number of bytes to be removed from the beginning of the file.</param> <param name="cancel">A cancellation token that can be used to cancel the truncation operation.</param>
    /// <returns>A task that represents the asynchronous truncation operation. The value of the task's result is a FtpMessagePayload object containing information about the truncation.</returns>
    /// /
    Task<FtpMessagePayload> TruncateFile(string path, uint offset, CancellationToken cancel);

    /// <summary>
    /// Renames a file or directory to a new name.
    /// </summary>
    /// <param name="pathToRename">The path of the file or directory to rename.</param>
    /// <param name="newPath">The new path or name of the file or directory.</param>
    /// <param name="cancel">A cancellation token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the FTP message payload indicating the success or failure of the renaming process.
    /// </returns>
    Task<FtpMessagePayload> Rename(string pathToRename, string newPath, CancellationToken cancel);

    /// Calculates the CRC32 checksum for the file specified by the given path.
    /// @param path The path of the file for which to calculate the CRC32 checksum.
    /// @param cancel A CancellationToken to cancel the operation if needed.
    /// @returns A Task of FtpMessagePayload representing the calculated CRC32 checksum.
    /// /
    Task<FtpMessagePayload> CalcFileCRC32(string path, CancellationToken cancel);
    Task<FtpMessagePayload> BurstReadFile(byte size, uint offset, byte sessionNumber, CancellationToken cancel);
}

#region Enums

/// <summary>
/// Represents the error codes for the Nak (Negative Acknowledgment) protocol.
/// </summary>
public enum NakError
{
    /// <summary>
    /// Enumeration representing error status codes for the Nak protocol.
    /// </summary>
    None = 0,

    /// <summary>Unknown failure</summary>
    Fail = 1,

    /// <summary>
    /// Command failed, Err number sent back in PayloadHeader.data[1].
    /// This is a file-system error number understood by the server operating system.
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
    /// Represents an end-of-file error.
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
    /// Represents the operation code for communication with a drone server.
    /// </summary>
    TerminateSession = 1,

    /// <summary> 
    /// "Terminates all open read sessions.
    /// - Clears all state held by the drone (server); closes all open files, etc.
    /// - Sends an ACK reply with no data."
    /// </summary> 
    ResetSessions = 2,

    /// <summary>
    /// The ListDirectory member of the OpCode enum represents a command to list directory entry information.
    /// </summary>
    ListDirectory = 3,

    /// <summary>
    /// Enum member representing the opcode for opening a file in read-only mode.
    /// </summary>
    OpenFileRO = 4,

    /// <summary>
    /// Represents the ReadFile operation of the OpCode enum.
    /// </summary>
    /// <remarks>
    /// - Reads <size> bytes from <offset> in <session>.
    /// - Seeks to (offset) in the file opened in (session) and reads (size) bytes into the result buffer.
    /// - Sends an ACK packet with the result buffer on success, otherwise a NAK packet with an error code. For short reads or reads beyond the end of a file, the (size) field in the ACK
    /// packet will indicate the actual number of bytes read.
    /// - Reads can be issued to any offset in the file for any number of bytes, so reconstructing portions of the file to deal with lost packets should be easy.
    /// - For best download performance, try to keep two Read packets in flight.
    /// </remarks>
    ReadFile = 5,

    /// <summary>
    /// Represents the CreateFile opcode, which is used to create a file at a specified path for writing.
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
    /// Calculates the CRC32 checksum for a file at the specified path.
    /// </summary>
    /// <remarks>
    /// The CalcFileCRC32 OpCode is used to calculate the cyclic redundancy check (CRC32) for a file located at the specified path.
    /// The CRC32 checksum is a widely used error-detection code that is commonly used to validate the integrity of data.
    /// The drone (server) will send an ACK packet with the CRC32 checksum on success, or a NAK packet with an error code on failure.
    /// The file must exist for the calculation to be successful.
    /// </remarks>
    CalcFileCRC32 = 14,

    /// <summary>
    /// Represents the burst download session file operation.
    /// </summary>
    BurstReadFile = 15,

    /// <summary>
    /// ACK response.
    /// </summary>
    ACK = 128,

    /// <summary>
    /// NAK response.
    /// </summary>
    NAK = 129,
}

/// Represents the type of an FTP entry.
/// /
public enum FtpEntryType
{
    /// <summary>
    /// Represents the different types of FTP entries.
    /// </summary>
    File = 'F',

    /// <summary>
    /// Represents the type of entry in an FTP server.
    /// </summary>
    Directory = 'D',
    Skip = 'S',
}
#endregion