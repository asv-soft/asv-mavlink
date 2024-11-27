using System.Threading;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents an interface for calculating the next sequence number of a packet.
    /// </summary>
    public interface IPacketSequenceCalculator
    {
        /// <summary>
        /// Retrieves the next sequence number.
        /// </summary>
        /// <returns>The next sequence number as a byte value.</returns>
        byte GetNextSequenceNumber();
    }

    public class PacketSequenceCalculator : IPacketSequenceCalculator
    {
        /// <summary>
        /// The current sequence number.
        /// </summary>
        /// <remarks>
        /// This variable is used to keep track of the current sequence number.
        /// </remarks>
        private volatile uint _seq;

        /// <summary>
        /// Retrieves the next sequence number.
        /// </summary>
        /// <returns>
        /// A byte representing the next sequence number.
        /// </returns>
        public byte GetNextSequenceNumber()
        {
            return (byte)(Interlocked.Increment(ref _seq) % 255);
        }
    }
}
