using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2RttStatus : ISizedSpanSerializable
    {
        public uint FieldsCount { get; set; }
        public uint RecordsCount { get; set; }
        public uint DescriptionHash { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            FieldsCount = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            RecordsCount = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            DescriptionHash = BinSerialize.ReadUInt(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, FieldsCount);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, RecordsCount);
            BinSerialize.WriteUInt(ref buffer, DescriptionHash);
        }

        public int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(FieldsCount) +
                   BinSerialize.GetSizeForPackedUnsignedInteger(RecordsCount) +
                   sizeof(uint);
        }
    }
}
