using Asv.IO;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a packet with a payload.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload.</typeparam>
    public interface IPacket<out TPayload>: ISizedSpanSerializable
        where TPayload:IPayload
    {
        /// <summary>
        /// Gets the maximum size of the object in bytes.
        /// </summary>
        /// <returns>
        /// The maximum size of the object in bytes.
        /// </returns>
        int GetMaxByteSize();

        /// <summary>
        /// Protocol magic marker.
        /// </summary>
        /// <value>
        /// The magic marker byte.
        /// </value>
        byte Magic { get; }

        /// <summary>
        /// Gets or sets the sequence of the packet.
        /// </summary>
        /// <remarks>
        /// The sequence is a byte value that represents the order of the packet in a series of packets.
        /// It is typically used to ensure the correct order of packets is maintained during transmission or processing.
        /// </remarks>
        byte Sequence { get; set; }

        /// <summary>
        /// Gets or sets the ID of the message sender system/aircraft.
        /// </summary>
        /// <remarks>
        /// The SystemId is a byte value that uniquely identifies the system or aircraft that is sending the message.
        /// </remarks>
        byte SystemId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the message sender component.
        /// </summary>
        /// <value>
        /// A byte representing the ID of the component.
        /// </value>
        byte ComponentId { get; set; }

        /// <summary>
        /// Gets the message id of the message.
        /// </summary>
        int MessageId { get; }

        /// <summary>
        /// Gets the message payload.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <returns>The payload of the message.</returns>
        TPayload Payload { get; }
    }
}
