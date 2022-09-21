using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2ParamsStatus : ISizedSpanSerializable
    {
        public uint DescriptionHash { get; set; }
        public uint Count { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Count = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            DescriptionHash = BinSerialize.ReadUInt(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, Count);
            BinSerialize.WriteUInt(ref buffer, DescriptionHash);
        }

        public int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(Count) +
                   sizeof(uint);
        }
    }
}
