namespace Asv.Mavlink;

/// <summary>
/// Parameters for a single write operation inside an already opened write session.
/// </summary>
/// <param name="session">Remote session id.</param>
/// <param name="skip">Byte offset from the beginning of the file.</param>
/// <param name="take">Number of bytes to write from the provided buffer.</param>
public readonly struct WriteRequest(byte session, uint skip, byte take)
{
    /// <summary>
    /// Remote session id.
    /// </summary>
    public readonly byte Session = session;
    
    /// <summary>
    /// Byte offset from the beginning of the file.
    /// </summary>
    public readonly uint Skip = skip;
    
    /// <summary>
    /// Number of bytes to write from the provided buffer.
    /// </summary>
    public readonly byte Take = take;
    
    public override string ToString() => $"WRITE_REQ(skip: {Skip}, take: {Take}, session:{Session})";
}
