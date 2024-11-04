namespace Asv.Mavlink;

/// <summary>
/// Represents a server device that supports Ftp (File transfer protocol) functionality.
/// </summary>
public interface IFtpServerDevice
{
    /// <summary>
    /// Gets the Ftp server.
    /// </summary>
    /// <value>
    /// The Ftp server.
    /// </value>
    IFtpServerEx Ftp { get; }
}