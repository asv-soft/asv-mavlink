namespace Asv.Mavlink;

/// <summary>
/// Interface for an extended FTP server.
/// </summary>
public interface IFtpServerEx
{
    /// <summary>
    /// Gets the FTP server object.
    /// </summary>
    /// <returns>
    /// The FTP server object.
    /// </returns>
    public IFtpServer Server { get; }

    /// <summary>
    /// This method handles the reset sessions operation for a specific device identified by the given identity.
    /// It takes the device identity and the FTP message payload as parameters.
    /// </summary>
    /// <param name="identity">The identity of the device.</param>
    /// <param name="ftpMessagePayload">The FTP message payload to be processed.</param>
    /// </returns>
    public void OnResetSessions(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Reads a burst file from an FTP server using the specified device identity and FTP message payload.
    /// </summary>
    /// <param name="identity">The device identity.</param>
    /// <param name="ftpMessagePayload">The FTP message payload.</param>
    public void OnBurstReadFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Calculates the CRC32 checksum for a file.
    /// </summary>
    /// <param name="identity">The identity of the device.</param>
    /// <param name="ftpMessagePayload">The FTP message payload containing the file.</param>
    public void OnCalcFileCRC32(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Handles the truncation of a file in response to an FTP message.
    /// </summary>
    /// <param name="identity">The identity of the device associated with the file.</param>
    /// <param name="ftpMessagePayload">The payload of the FTP message.</param>
    public void OnTruncateFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Event handler called when a file is opened without a corresponding file watcher.
    /// </summary>
    /// <param name="identity">The identity of the device.</param>
    /// <param name="ftpMessagePayload">The payload of the FTP message.</param>
    public void OnOpenFileWO(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Called when a directory is removed from the FTP server.
    /// </summary>
    /// <param name="identity">The identity of the device.</param>
    /// <param name="ftpMessagePayload">The payload of the FTP message.</param>
    public void OnRemoveDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Creates a directory on the FTP server for the specified device identity and FTP message payload.
    /// </summary>
    /// <param name="identity">The device identity.</param>
    /// <param name="ftpMessagePayload">The FTP message payload.</param>
    public void OnCreateDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Removes a file from the FTP server.
    /// </summary>
    /// <param name="identity">
    /// The device identity associated with the FTP server.
    /// </param>
    /// <param name="ftpMessagePayload">
    /// The payload of the FTP message containing information about the file to be removed.
    /// </param>
    /// <remarks>
    /// This method removes a file from the FTP server using the provided device identity and FTP message payload.
    /// The FTP message payload should contain the necessary details such as the file name, path, or any other required information to locate the file on the server.
    /// </remarks>
    public void RemoveFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Event handler for writing file on the FTP server.
    /// </summary>
    /// <param name="identity">The identity of the FTP device.</param>
    /// <param name="ftpMessagePayload">The payload containing the FTP message.</param>
    public void OnWriteFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Handles the event when a file is created.
    /// </summary>
    /// <param name="identity">The identity of the device.</param>
    /// <param name="ftpMessagePayload">The payload of the FTP message.</param>
    public void OnCreateFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Event handler that is invoked when a file has been read.
    /// </summary>
    /// <param name="identity">The identity of the device.</param>
    /// <param name="ftpMessagePayload">The FTP message payload representing the file contents.</param>
    public void OnReadFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Handles the event when a file is opened in read-only mode.
    /// </summary>
    /// <param name="identity">The device identity.</param>
    /// <param name="ftpMessagePayload">The FTP message payload.</param>
    public void OnOpenFileRO(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Handles the "List Directory" command received from an FTP client. </summary> <param name="identity">The device identity associated with the FTP client.</param> <param name="ftpMessagePayload">The payload of the FTP message received.</param> <remarks>
    /// This method is called when an FTP client sends a "List Directory" command to the server.
    /// It is responsible for listing the files and directories present in the specified directory
    /// and sending the list back to the client. </remarks>
    /// /
    public void OnListDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Handles the termination of a session.
    /// </summary>
    /// <param name="identity">The device identity associated with the terminated session.</param>
    /// <param name="ftpMessagePayload">The FTP message payload received from the terminated session.</param>
    public void OnTerminateSession(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);

    /// <summary>
    /// Handles the rename event for a device identity.
    /// </summary>
    /// <param name="identity">The device identity associated with the event.</param>
    /// <param name="ftpMessagePayload">The FTP message payload containing the rename information.</param>
    public void OnRename(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
}