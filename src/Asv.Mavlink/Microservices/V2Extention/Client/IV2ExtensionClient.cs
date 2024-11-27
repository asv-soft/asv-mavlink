using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a client that implements parts of the V2 payload specs in V1 frames for transitional support.
    /// </summary>
    public interface IV2ExtensionClient : IMavlinkMicroserviceClient
    {
        /// <summary>
        /// Gets the maximum size of data allowed.
        /// </summary>
        /// <value>
        /// The maximum size of data allowed.
        /// </value>
        int MaxDataSize { get; }

        /// <summary>
        /// Gets the OnData property of type IRxValue&lt;V2ExtensionPacket&gt;.
        /// </summary>
        /// <remarks>
        /// This property represents a reactive value that provides a stream of V2ExtensionPacket objects.
        /// The property can be subscribed to in order to receive updates whenever new data is available.
        /// </remarks>
        ReadOnlyReactiveProperty<V2ExtensionPacket> OnData { get; }

        /// <summary>
        /// Sends data to the target network ID using the specified message type.
        /// </summary>
        /// <param name="targetNetworkId">The target network ID.</param>
        /// <param name="messageType">The type of the message to be sent.</param>
        /// <param name="data">The data to be sent.</param>
        /// <param name="cancel">The cancellation token for cancelling the task.</param>
        /// <returns>A task representing the asynchronous operation of sending data.</returns>
        Task SendData(byte targetNetworkId, ushort messageType,
            byte[] data, CancellationToken cancel);
    }
}
