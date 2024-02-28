namespace Asv.Mavlink;

/// <summary>
/// A key-value pair as integer.
/// The use of this message is discouraged for normal packets,
/// but a quite efficient way for testing new messages and getting experimental debug output.
/// </summary>
public class NamedValueIntMessage : ITraceMessage
{
    /// <summary>
    /// Timestamp in milliseconds (time since system boot).
    /// </summary>
    public uint TimeBoot { get; set; }
    /// <summary>
    /// Name of the debug variable
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Signed integer value
    /// </summary>
    public int Value { get; set; }
    
    public override string ToString()
    {
        return $"{nameof(TimeBoot)}: {TimeBoot}, {nameof(Name)}: {Name}, {nameof(Value)}: {Value}";
    }
}