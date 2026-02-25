namespace Asv.Mavlink;

/// <summary>
/// Generic handle that binds a remote session id with a path.
/// </summary>
/// <param name="session">Remote session id.</param>
/// <param name="path">Remote path.</param>
public readonly struct CreateHandle(byte session, string path)
{
    /// <summary>
    /// Remote session id.
    /// </summary>
    public readonly byte Session = session;
    
    /// <summary>
    /// Remote path.
    /// </summary>
    public readonly string Path = path;
    
    public override string ToString() => $"CREATE_DIR(session: {Session}, path: {Path})";
}
