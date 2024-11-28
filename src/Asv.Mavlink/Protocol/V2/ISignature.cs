using Asv.IO;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a signature for a packet.
    /// </summary>
    public interface ISignature: ISpanSerializable
    {
        /// <summary>
        /// Gets the maximum size in bytes.
        /// </summary>
        /// <returns>The maximum size in bytes.</returns>
        int GetMaxByteSize();

        /// <summary>
        /// Gets or sets a boolean value indicating whether the sign is present.
        /// </summary>
        bool IsPresent { get; set; }

        /// <summary>
        /// Gets the size in bytes.
        /// </summary>
        int ByteSize { get; }

        /// <summary>
        /// Gets or sets the ID of the link on which the packet is sent.
        /// </summary>
        /// <value>
        /// The ID of the link.
        /// </value>
        byte LinkId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp in 10 microsecond units since 1st January 2015 GMT time.
        /// This must monotonically increase for every message on a particular link.
        /// Note that means the timestamp may get ahead of the actual time if the packet rate averages more than 100,000 packets per second.
        /// </summary>
        /// <value>
        /// The timestamp in 10 microsecond units.
        /// </value>
        ulong Timestamp { get; set; }

        /// <summary>
        /// Gets or sets a 48-bit signature for the packet, based on the complete packet, timestamp, and secret key.
        /// </summary>
        ulong Sign { get; set; }
    }
}
