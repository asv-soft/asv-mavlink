namespace Asv.Mavlink;

/// <summary>
/// Represents a specialized <see cref="FtpNackException"/> that indicates an end-of-file (EOF) condition
/// returned by a MAVLink FTP server.
/// </summary>
/// <param name="action">The FTP opcode (action) for which the server reported EOF.</param>
public class FtpNackEndOfFileException(FtpOpcode action) : FtpNackException(action, NackError.EOF);