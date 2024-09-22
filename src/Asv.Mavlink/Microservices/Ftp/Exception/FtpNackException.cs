using System;

namespace Asv.Mavlink;

public class FtpNackException : Exception
{
    public FtpOpcode Action { get; }
    public NackError NackError { get; }
    public byte? FsErrorCode { get; }

    public FtpNackException(FtpOpcode action,NackError nackError) 
        : base($"Error to {action}: {MavlinkFtpHelper.GetErrorMessage(nackError)}")
    {
        Action = action;
        NackError = nackError;
    }
    public FtpNackException(FtpOpcode action, byte fsErrorCode) 
        : base($"Error to {action}: {MavlinkFtpHelper.GetErrorMessage(NackError.FailErrno)} with file-system specific error code {fsErrorCode}")
    {
        Action = action;
        NackError = NackError.FailErrno;
        FsErrorCode = fsErrorCode;
    }

    
}