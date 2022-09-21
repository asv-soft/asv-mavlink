using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2MissionInfo : ISizedSpanSerializable
    {
        public ushort Count { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Count = BinSerialize.ReadUShort(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer,Count);
        }

        public int GetByteSize()
        {
            return sizeof(ushort);
        }

        public override string ToString()
        {
            return $"count:{Count}";
        }
    }
}
