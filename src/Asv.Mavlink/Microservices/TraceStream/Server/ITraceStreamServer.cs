namespace Asv.Mavlink;

public interface ITraceStreamServer:IMavlinkMicroserviceServer
{
    /// <summary>
    /// Sends a DebugVectorMessage.
    /// </summary>
    /// <param name="debugVectorMessage">Defined debug vector message</param>
    /// <returns>True if the send operation was successful; otherwise, false.</returns>
    bool AddMessage(DebugVectorMessage debugVectorMessage);
    
    /// <summary>
    /// Sends a MemoryVectorMessage.
    /// </summary>
    /// <param name="memoryVectorMessage">Defined memory vector message</param>
    /// <returns>True if the send operation was successful; otherwise, false.</returns>
    bool AddMessage(MemoryVectorMessage memoryVectorMessage);
    
    /// <summary>
    /// Sends a NamedValueIntMessage.
    /// </summary>
    /// <param name="namedValueIntMessage">Defined name value int message</param>
    /// <returns>True if the send operation was successful; otherwise, false.</returns>
    bool AddMessage(NamedValueIntMessage namedValueIntMessage);
    
    /// <summary>
    /// Sends a NamedValueFloatMessage.
    /// </summary>
    /// <param name="namedValueFloatMessage">Defined name value float message</param>
    /// <returns>True if the send operation was successful; otherwise, false.</returns>
    bool AddMessage(NamedValueFloatMessage namedValueFloatMessage);
}