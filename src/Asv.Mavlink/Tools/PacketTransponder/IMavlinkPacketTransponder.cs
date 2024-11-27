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

    /// Represents a Mavlink packet transponder.
    /// @typeparam TPacket The type of packet.
    /// @typeparam TPayload The type of payload.
    /// /
    public interface IMavlinkPacketTransponder<TPacket, out TPayload> : IDisposable
        where TPacket : IPacketV2<TPayload>, new()
        where TPayload : IPayload, new()
    {
        /// <summary>
        /// Starts a timer that calls the specified method on a recurring basis, using the specified due time and time interval.
        /// </summary>
        /// <param name="dueTime">The amount of time to delay before the method is invoked. Specify TimeSpan.Zero to invoke the method immediately.</param>
        /// <param name="period">The time interval between invocations of the method. Specify TimeSpan.Zero to disable periodic invocations.</param>
        /// <remarks>
        /// The Start method starts a timer that calls the specified method on a recurring basis, using the specified due time and time interval.
        /// The dueTime parameter specifies the amount of time to delay before the first invocation of the method. Specify TimeSpan.Zero to invoke the method immediately.
        /// The period parameter specifies the time interval between invocations of the method. Specify TimeSpan.Zero to disable periodic invocations.
        /// </remarks>
        void Start(TimeSpan dueTime, TimeSpan period);

        /// <summary>
        /// Gets a value indicating whether the property is started or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if the property is started; otherwise, <c>false</c>.
        /// </value>
        bool IsStarted { get; }

        /// <summary>
        /// The current state of the packet transponder.
        /// </summary>
        /// <remarks>
        /// This property provides access to the <see cref="IRxValue{T}"/> instance representing the state of the packet transponder.
        /// The state can be read from this instance, but cannot be directly modified.
        /// To update the state, use the appropriate methods provided by the packet transponder class.
        /// </remarks>
        /// <returns>
        /// An <see cref="IRxValue{T}"/> instance representing the state of the packet transponder.
        /// </returns>
        ReadOnlyReactiveProperty<PacketTransponderState> State { get; }

        /// <summary>
        /// Stops the execution of the program.
        /// </summary>
        /// <remarks>
        /// This method will cease all ongoing operations and halt the execution of the program.
        /// </remarks>
        void Stop();

        /// <summary>
        /// Sets the change callback for the specified payload type.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <param name="changeCallback">The action to be called when the payload changes.</param>
        void Set(Action<TPayload> changeCallback);
    }
}
