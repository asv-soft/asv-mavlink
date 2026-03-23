namespace Asv.Mavlink;

/// <summary>
/// Parameters for a single read operation inside an already opened read session.
/// </summary>
/// <param name="session">Remote session id.</param>
/// <param name="skip">Byte offset from the beginning of the file.</param>
/// <param name="take">Requested number of bytes to read (must fit MAVLink FTP payload limits).</param>
public readonly struct ReadRequest(byte session, uint skip, byte take)
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
    /// Requested number of bytes to read.
    /// </summary>
    public readonly byte Take = take;
    
    public override string ToString() => $"READ_REQ(skip: {Skip}, take: {Take}, session:{Session})";
}
