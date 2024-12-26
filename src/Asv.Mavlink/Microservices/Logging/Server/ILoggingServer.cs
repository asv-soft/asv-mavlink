using System.Threading.Tasks;

namespace Asv.Mavlink;

/// <summary>
/// Represents a logging server that is responsible for sending logging data.
/// </summary>
public interface ILoggingServer:IMavlinkMicroserviceServer
{
    /// <summary>
    /// Gets the maximum data length for the property.
    /// </summary>
    /// <returns>The maximum data length as an integer.</returns>
    int MaxDataLength { get; }

    /// <summary>
    /// Sends logging data to the specified target system and component.
    /// </summary>
    /// <param name="targetSystemId">The target system ID.</param>
    /// <param name="targetComponentId">The target component ID.</param>
    /// <param name="seq">The sequence number.</param>
    /// <param name="firstMessageOffset">The offset of the first message.</param>
    /// <param name="data">The logging data to be sent.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    ValueTask SendLoggingData(byte targetSystemId, byte targetComponentId, ushort seq, byte firstMessageOffset,
        byte[] data);
}