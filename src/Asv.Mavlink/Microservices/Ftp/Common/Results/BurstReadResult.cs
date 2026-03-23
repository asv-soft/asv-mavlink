namespace Asv.Mavlink;

/// <summary>
/// Result of a burst-read chunk.
/// </summary>
/// <param name="readCount">Number of bytes in this chunk.</param>
/// <param name="isLastChunk">Indicates whether this is the last chunk in the burst stream.</param>
/// <param name="request">Original request of type <see cref="ReadRequest"/>.</param>
public readonly struct BurstReadResult(byte readCount, bool isLastChunk, ReadRequest request)
{
    /// <summary>
    /// Original read request.
    /// </summary>
    public readonly ReadRequest Request = request;
    
    /// <summary>
    /// Number of bytes in this chunk.
    /// </summary>
    public readonly byte ReadCount = readCount;

    /// <summary>
    /// Indicates that the stream is complete after this chunk.
    /// </summary>
    public readonly bool IsLastChunk = isLastChunk;
   
    public override string ToString() => $"BURSTREAD_RESP(read: {ReadCount}, {Request}, {IsLastChunk})";
}
