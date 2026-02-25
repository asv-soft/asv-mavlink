namespace Asv.Mavlink;

/// <summary>
/// Parameters for a truncate operation.
/// </summary>
/// <param name="path">Remote file path.</param>
/// <param name="offset">New file length / truncate offset in bytes.</param>
public readonly struct TruncateRequest(string path, uint offset)
{
    /// <summary>
    /// Remote file path.
    /// </summary>
    public readonly string Path = path;
    
    /// <summary>
    /// New file length / truncate offset in bytes.
    /// </summary>
    public readonly uint Offset = offset;
    
    public override string ToString() => $"READ_REQ(path: {Path}, offset: {Offset})";
}
