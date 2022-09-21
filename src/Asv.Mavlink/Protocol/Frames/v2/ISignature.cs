using Asv.IO;

namespace Asv.Mavlink
{
    public interface ISignature: ISpanSerializable
    {
        /// <summary>
        /// Maximum size in bytes
        /// </summary>
        int GetMaxByteSize();
        /// <summary>
        /// Indicate, that sign is present
        /// </summary>
        bool IsPresent { get; set; }
        /// <summary>
        /// Size in bytes
        /// </summary>
        int ByteSize { get; }
        /// <summary>
        /// ID of link on which packet is sent. Normally this is the same as the channel.
        /// </summary>
        byte LinkId { get; set; }
        /// <summary>
        /// Timestamp in 10 microsecond units since 1st January 2015 GMT time. This must monotonically increase for every message on a particular link. Note that means the timestamp may get ahead of the actual time if the packet rate averages more than 100,000 packets per second.
        /// </summary>
        ulong Timestamp { get; set; }
        /// <summary>
        /// A 48 bit signature for the packet, based on the complete packet, timestamp, and secret key.
        /// </summary>
        ulong Sign { get; set; }
    }
}
