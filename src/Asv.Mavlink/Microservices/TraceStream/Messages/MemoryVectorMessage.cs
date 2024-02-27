namespace Asv.Mavlink;

/// <summary>
/// Raw controller memory.
/// The use of this message is discouraged for normal packets,
/// but a quite efficient way for testing new messages and getting experimental debug output.
/// </summary>
public class MemoryVectorMessage : ITraceMessage
{
    /// <summary>
    /// Starting address of the debug variables
    /// </summary>
    public ushort Address { get; set; }
    /// <summary>
    /// Version code of the type variable. 0=unknown, type ignored and assumed int16_t. 1=as below
    /// </summary>
    public byte Version { get; set; }
    /// <summary>
    /// Type code of the memory variables. for ver = 1: 0=16 x int16_t, 1=16 x uint16_t, 2=16 x Q15, 3=16 x 1Q14
    /// </summary>
    public byte Type { get; set; }
    /// <summary>
    /// Memory contents at specified address
    /// </summary>
    public sbyte[] Value { get; set; }
    
    public override string ToString()
    {
        
        return $"{nameof(Address)}: {Address}, {nameof(Version)}: {Version}, {nameof(Type)}: {Type}, {nameof(Value)}: [{string.Join(", ", Value)}]";
    }
}