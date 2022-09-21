using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2RttGetFieldsArgs : ISizedSpanSerializable
    {
        public Pv2RttGetFieldsArgs()
        {
        }

        public Pv2RttGetFieldsArgs(SessionId sessionId, uint fieldIndex)
        {
            SessionId = sessionId;
            FieldIndex = fieldIndex;
        }

        public SessionId SessionId { get; set; }
        public uint FieldIndex { get; set; }

        public virtual void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            SessionId ??= new SessionId();
            SessionId.Deserialize(ref buffer);
            FieldIndex = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
        }

        public virtual void Serialize(ref Span<byte> buffer)
        {
            SessionId.Serialize(ref buffer);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, FieldIndex);
        }

        public virtual int GetByteSize()
        {
            return SessionId.GetByteSize() + BinSerialize.GetSizeForPackedUnsignedInteger(FieldIndex);
        }

        public override string ToString()
        {
            return $"[{SessionId}] index:{FieldIndex}";
        }
    }
}
