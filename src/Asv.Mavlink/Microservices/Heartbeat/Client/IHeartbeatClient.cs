using Asv.Common;
using Asv.Mavlink.Minimal;

using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a client that sends and receives heartbeats.
    /// </summary>
    public interface IHeartbeatClient : IMavlinkMicroserviceClient
    {
        /// <summary>
        /// Gets the full ID of the property.
        /// </summary>
        /// <value>
        /// The full ID.
        /// </value>
        ushort FullId { get; }

        /// <summary>
        /// Gets the raw heartbeat value as an <see cref="ReadOnlyReactiveProperty{T}"/> of type <see cref="HeartbeatPayload"/>.
        /// </summary>
        /// <value>
        /// The raw heartbeat value.
        /// </value>
        ReadOnlyReactiveProperty<HeartbeatPayload?> RawHeartbeat { get; }

        /// <summary>
        /// Gets the packet rate in Hz.
        /// </summary>
        /// <returns>The value of the packet rate in Hz.</returns>
        ReadOnlyReactiveProperty<double> PacketRateHz { get; }

        /// <summary>
        /// Gets the quality of the link as an interface to receive a read-only observable stream of double values.
        /// </summary>
        ReadOnlyReactiveProperty<double> LinkQuality { get; }

        /// <summary>
        /// Gets the ReactiveProperty instance representing the state of a link.
        /// </summary>
        /// <value>
        /// The ReactiveProperty instance representing the state of the link.
        /// </value>
        ILinkIndicator Link { get; }
    }

    
}