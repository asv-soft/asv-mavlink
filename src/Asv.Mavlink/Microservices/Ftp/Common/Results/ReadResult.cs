namespace Asv.Mavlink;

/// <summary>
/// Result of a read operation into a caller-provided buffer.
/// </summary>
/// <param name="readCount">Number of bytes actually read.</param>
/// <param name="request">Original request of type <see cref="ReadRequest"/>.</param>
public readonly struct ReadResult(byte readCount, ReadRequest request)
{
    /// <summary>
    /// Original read request.
    /// </summary>
    public readonly ReadRequest Request = request;
    
    /// <summary>
    /// Number of bytes actually read.
    /// </summary>
    public readonly byte ReadCount = readCount;
   
    public override string ToString() => $"READ_RESP(read: {ReadCount}, {Request})";
}
