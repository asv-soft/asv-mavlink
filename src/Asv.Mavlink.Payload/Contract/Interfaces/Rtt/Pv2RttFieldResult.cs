using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2RttFieldResult : ISizedSpanSerializable
    {
        public Pv2RttFieldResult()
        {
        }

        public Pv2RttFieldResult(Pv2RttFieldDesc desc)
        {
            FieldType = desc.Type;
            Desc = desc;
        }

        public Pv2RttFieldType FieldType { get; set; }

        public Pv2RttFieldDesc Desc { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            FieldType = (Pv2RttFieldType)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Desc = Pv2RttInterface.CreateFieldByType(FieldType);
            Desc.Deserialize(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)FieldType);
            Desc.Serialize(ref buffer);
        }

        public int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger((uint)FieldType) + Desc.GetByteSize();
        }

        public override string ToString()
        {
            return Desc.ToString();
        }
    }
}
