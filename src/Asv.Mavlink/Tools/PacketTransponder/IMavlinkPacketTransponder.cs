using System;
using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents the state of a packet transponder.
    /// </summary>
    public enum PacketTransponderState
    {
        /// <summary>
        /// Represents the state of a packet transponder when it is "Ok".
        /// </summary>
        Ok,

        /// <summary>
        /// Represents the state of a packet transponder when it has been skipped.
        /// </summary>
        /// <remarks>
        /// This state indicates that the packet transponder was skipped for processing.
        /// </remarks>
        Skipped,

        /// <summary>
        /// Represents the error state when sending a packet.
        /// </summary>
        ErrorToSend,
    }

    public interface IMavlinkPacketTransponder<out TPacket> : IDisposable
        where TPacket : MavlinkMessage, new()
    {

        void Start(TimeSpan dueTime, TimeSpan period);

        bool IsStarted { get; }

        ReadOnlyReactiveProperty<PacketTransponderState> State { get; }

        void Stop();

        void Set(Action<TPacket> changeCallback);
    }
}
