using System;

namespace Asv.Mavlink;


public class ListDataException : Exception
{
    public ListDataException()
    {
    }

    public ListDataException(string message) : base(message)
    {
    }

    public ListDataException(string message, Exception inner) : base(message, inner)
    {
    }
}