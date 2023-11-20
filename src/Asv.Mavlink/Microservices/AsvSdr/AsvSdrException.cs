using System;

namespace Asv.Mavlink;


public class AsvSdrException : Exception
{
    public AsvSdrException()
    {
    }

    public AsvSdrException(string message) : base(message)
    {
    }

    public AsvSdrException(string message, Exception inner) : base(message, inner)
    {
    }
}
