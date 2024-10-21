using System;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;

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
        /// Gets the raw heartbeat value as an <see cref="IRxValue{T}"/> of type <see cref="HeartbeatPayload"/>.
        /// </summary>
        /// <value>
        /// The raw heartbeat value.
        /// </value>
        IRxValue<HeartbeatPayload> RawHeartbeat { get; }

        /// <summary>
        /// Gets the packet rate in Hz.
        /// </summary>
        /// <returns>The value of the packet rate in Hz.</returns>
        IRxValue<double> PacketRateHz { get; }

        /// <summary>
        /// Gets the quality of the link as an interface to receive a read-only observable stream of double values.
        /// </summary>
        IRxValue<double> LinkQuality { get; }

        /// <summary>
        /// Gets the RxValue instance representing the state of a link.
        /// </summary>
        /// <value>
        /// The RxValue instance representing the state of the link.
        /// </value>
        IRxValue<LinkState> Link { get; }
    }

    
}