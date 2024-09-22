using System;

namespace Asv.Mavlink;

public class FtpException : Exception
{
    public FtpException()
    {
    }

    public FtpException(string message) : base(message)
    {
    }

    public FtpException(string message, Exception inner) : base(message, inner)
    {
    }
}

public class FtpNackException : Exception
{
    public FtpNackException(FtpOpcode action,NackError err) 
        : base($"Error to {action}: {MavlinkFtpHelper.GetErrorMessage(err)}")
    {
        
    }
    public FtpNackException(FtpOpcode action,NackError err, byte additionalError) 
        : base($"Error to {action}: {MavlinkFtpHelper.GetErrorMessage(err)} with file-system specific error code {additionalError}")
    {
        
    }

    
}

