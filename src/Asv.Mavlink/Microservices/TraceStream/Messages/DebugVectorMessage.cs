namespace Asv.Mavlink;

/// <summary>
/// To debug something using a named 3D vector.
/// </summary>
public class DebugVectorMessage : ITraceMessage
{
    /// <summary>
    /// Name of the debug variable
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Timestamp (UNIX Epoch time or time since system boot).
    /// The receiving end can infer timestamp format (since 1.1.1970 or since system boot)
    /// by checking for the magnitude of the number.
    /// </summary>
    public ulong TimeUsec { get; set; }
    /// <summary>
    /// Coordinate X
    /// </summary>
    public float X { get; set; }
    /// <summary>
    /// Coordinate Y
    /// </summary>
    public float Y { get; set; }
    /// <summary>
    /// Coordinate Z
    /// </summary>
    public float Z { get; set; }
    
    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(TimeUsec)}: {TimeUsec}, {nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Z)}: {Z}";
    }
}