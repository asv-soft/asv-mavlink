using System;

namespace Asv.Mavlink;

/// <summary>
/// Represents an exception thrown when a MAVLink FTP request is rejected with a NAK (negative acknowledgment).
/// </summary>
/// <remarks>
/// This exception provides the originating FTP <see cref="Action"/> (opcode) and the specific <see cref="NackError"/>
/// returned by the remote endpoint. If the error is <see cref="Asv.Mavlink.NackError.FailErrno"/>, the optional
/// <see cref="FsErrorCode"/> contains the file-system-specific error code supplied by the server.
/// </remarks>
public class FtpNackException : Exception
{
    /// <summary>
    /// Gets the FTP opcode (action) that caused the NAK response.
    /// </summary>
    public FtpOpcode Action { get; }
    
    /// <summary>
    /// Gets the NAK error code reported by the FTP server.
    /// </summary>
    public NackError NackError { get; }
    
    /// <summary>
    /// Gets the file-system-specific error code when <see cref="NackError"/> is <see cref="Asv.Mavlink.NackError.FailErrno"/>;
    /// otherwise, <see langword="null"/>.
    /// </summary>
    public byte? FsErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FtpNackException"/> class with a protocol-level NAK error code.
    /// </summary>
    /// <param name="action">The FTP opcode (action) that failed.</param>
    /// <param name="nackError">The NAK error code returned by the server.</param>
    public FtpNackException(FtpOpcode action, NackError nackError) 
        : base($"Error to {action}: {MavlinkFtpHelper.GetErrorMessage(nackError)}")
    {
        Action = action;
        NackError = nackError;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="FtpNackException"/> class for <see cref="Asv.Mavlink.NackError.FailErrno"/>
    /// and captures the server-provided file-system error code.
    /// </summary>
    /// <param name="action">The FTP opcode (action) that failed.</param>
    /// <param name="fsErrorCode">The file-system-specific error code returned by the server.</param>
    public FtpNackException(FtpOpcode action, byte fsErrorCode) 
        : base($"Error to {action}: {MavlinkFtpHelper.GetErrorMessage(NackError.FailErrno)} with file-system specific error code {fsErrorCode}")
    {
        Action = action;
        NackError = NackError.FailErrno;
        FsErrorCode = fsErrorCode;
    }
}