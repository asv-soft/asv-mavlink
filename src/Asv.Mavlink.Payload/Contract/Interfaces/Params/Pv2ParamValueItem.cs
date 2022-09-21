using System;
using System.Diagnostics;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2ParamValueItem : ISizedSpanSerializable
    {
        public Pv2ParamValueItem(Pv2ParamValue value)
        {
            Value = value;
        }

        public Pv2ParamValueItem()
        {
        }

        public Pv2ParamValue Value { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var type = (Pv2ParamTypeEnum)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Value = Pv2ParamInterface.CreateValue(type);
            Value.Deserialize(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            Debug.Assert(Value != null, nameof(Value) + " != null");
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)Value.Type);
            Value.Serialize(ref buffer);
        }

        public int GetByteSize()
        {
            return Value.GetByteSize() + BinSerialize.GetSizeForPackedUnsignedInteger((uint)Value.Type);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
