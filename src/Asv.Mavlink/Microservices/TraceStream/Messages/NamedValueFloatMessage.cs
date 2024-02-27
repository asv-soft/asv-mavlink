namespace Asv.Mavlink;

/// <summary>
/// A key-value pair as float.
/// The use of this message is discouraged for normal packets,
/// but a quite efficient way for testing new messages and getting experimental debug output.
/// </summary>
public class NamedValueFloatMessage : ITraceMessage
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
    /// Floating point value
    /// </summary>
    public float Value { get; set; }

    public override string ToString()
    {
        return $"{nameof(TimeBoot)}: {TimeBoot}, {nameof(Name)}: {Name}, {nameof(Value)}: {Value}";
    }
}