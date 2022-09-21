using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public abstract class Pv2ParamValue : ISizedSpanSerializable
    {
        public uint Index { get; set; }
        public abstract Pv2ParamTypeEnum Type { get; }

        public virtual void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Index = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
        }

        public virtual void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, Index);
        }

        public virtual int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(Index);
        }

        public void ValidateSize()
        {
            var size = GetByteSize();
            if (size > PayloadV2Helper.MaxMessageSize)
                throw new Exception(
                    $"Size of param value with index {Index} more then max. {size} > {PayloadV2Helper.MaxMessageSize}");
        }

        public abstract void CopyFrom(Pv2ParamValue data);
    }
}
