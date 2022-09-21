using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2RttSessionStoreInfo : ISizedSpanSerializable
    {
        public uint SessionCount { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            SessionCount = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, SessionCount);
        }

        public int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(SessionCount);
        }
    }
}
