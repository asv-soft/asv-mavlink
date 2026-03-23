using Asv.Common;

namespace Asv.Mavlink;

/// <summary>
/// Handle that describes an opened remote write session (session id and remote file size).
/// </summary>
/// <param name="session">Remote session id.</param>
/// <param name="size">Remote file size in bytes.</param>
public readonly struct WriteHandle(byte session, uint size)
{
    /// <summary>
    /// Remote session id.
    /// </summary>
    public readonly byte Session = session;
    
    /// <summary>
    /// Remote file size in bytes.
    /// </summary>
    public readonly uint Size = size;
    
    public override string ToString() => $"WRITE_FILE(session: {Session}, size: {StringExtensions.BytesToString(Size)})";
}
