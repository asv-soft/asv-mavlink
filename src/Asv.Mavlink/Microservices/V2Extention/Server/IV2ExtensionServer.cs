using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a V2 extension server that communicates with clients by implementing parts of the V2 payload specifications in V1 frames for transitional support.
    /// </summary>
    public interface IV2ExtensionServer : IMavlinkMicroserviceServer
    {
        /// <summary>
        /// Gets the property OnData which is of type IRxValue&lt;V2ExtensionPacket&gt;.
        /// </summary>
        /// <remarks>
        /// This property represents the event handler for handling data received events.
        /// </remarks>
        /// <returns>
        /// The IRxValue&lt;V2ExtensionPacket&gt; object.
        /// </returns>
        ReadOnlyReactiveProperty<V2ExtensionPacket> OnData { get; }

        /// <summary>
        /// Sends data to a target system.
        /// </summary>
        /// <param name="targetSystemId">The target system ID.</param>
        /// <param name="targetComponentId">The target component ID.</param>
        /// <param name="targetNetworkId">The target network ID.</param>
        /// <param name="messageType">The message type.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="cancel">Cancellation token.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// This method sends data to a specified target system using the provided target system ID, target component ID,
        /// target network ID, message type, and data to send.
        /// The cancellation token can be used to cancel the operation.
        /// </remarks>
        Task SendData(byte targetSystemId, byte targetComponentId, byte targetNetworkId, ushort messageType,
            byte[] data, CancellationToken cancel);
    }
}
