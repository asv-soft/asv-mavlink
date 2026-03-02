using System;

namespace Asv.Mavlink;

/// <summary>
/// Represents base class for all Ftp exceptions
/// </summary>
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