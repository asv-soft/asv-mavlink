using System.Threading;

namespace Asv.Mavlink
{
    public interface IPacketSequenceCalculator
    {
        byte GetNextSequenceNumber();
    }

    public class PacketSequenceCalculator : IPacketSequenceCalculator
    {
        private volatile int _seq;

        public byte GetNextSequenceNumber()
        {
            return (byte)(Interlocked.Increment(ref _seq) % 255);
        }
    }
}
