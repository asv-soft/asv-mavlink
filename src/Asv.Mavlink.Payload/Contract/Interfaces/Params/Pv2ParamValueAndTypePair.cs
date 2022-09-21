using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2ParamValueAndTypePair : ISizedSpanSerializable
    {
        public Pv2ParamValueAndTypePair(Pv2ParamType type, Pv2ParamValue value, uint index)
        {
            Index = index;
            Type = type;
            Value = value;
        }

        public Pv2ParamValueAndTypePair()
        {
        }

        public uint Index { get; set; }

        public Pv2ParamValue Value { get; set; }

        public Pv2ParamType Type { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Index = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            var typeEnum = (Pv2ParamTypeEnum)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Type = Pv2ParamInterface.CreateType(typeEnum);
            var isContainValue = BinSerialize.ReadBool(ref buffer);
            Type.Deserialize(ref buffer);
            Value = null;
            if (!isContainValue) return;
            Value = Pv2ParamInterface.CreateValue(typeEnum);
            Value.Deserialize(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, Index);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)Type.TypeEnum);
            var typeSize = Type.GetByteSize();
            var valueSize = int.MaxValue; // if value == null, we wouldn't serialize it
            if (Value != null) valueSize = Value.GetByteSize();
            if (Value == null || typeSize + valueSize + 1 > PayloadV2Helper.MaxMessageSize)
            {
                // send only type
                BinSerialize.WriteBool(ref buffer, false);
                Type.Serialize(ref buffer);
            }
            else
            {
                // send type + value
                BinSerialize.WriteBool(ref buffer, true);
                Type.Serialize(ref buffer);
                Value.Serialize(ref buffer);
            }
        }

        public int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(Index) +
                BinSerialize.GetSizeForPackedUnsignedInteger((uint)Type.TypeEnum) +
                1 /* Is value present flag (bool) */ + Type.GetByteSize() + Value?.GetByteSize() ?? 0;
        }

        public override string ToString()
        {
            return $"[{Index}] val:{Value} type:{Type}";
        }
    }
}
