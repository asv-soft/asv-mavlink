using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2RttGetFieldsDataArgs : ISizedSpanSerializable
    {
        public Pv2RttGetFieldsDataArgs()
        {
        }

        public Pv2RttGetFieldsDataArgs(SessionId id, uint fieldId, uint startIndex, uint take)
        {
            SessionId = id;
            FieldId = fieldId;
            StartIndex = startIndex;
            Take = take;
        }

        public SessionId SessionId { get; set; }
        public uint FieldId { get; set; }
        public uint StartIndex { get; set; }
        public uint Take { get; set; }

        public void Serialize(ref Span<byte> buffer)
        {
            SessionId.Serialize(ref buffer);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, FieldId);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, StartIndex);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, Take);
        }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            SessionId ??= new SessionId();
            SessionId.Deserialize(ref buffer);
            FieldId = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            StartIndex = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Take = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
        }

        public int GetByteSize()
        {
            return SessionId.GetByteSize() +
                   BinSerialize.GetSizeForPackedUnsignedInteger(FieldId) +
                   BinSerialize.GetSizeForPackedUnsignedInteger(StartIndex) +
                   BinSerialize.GetSizeForPackedUnsignedInteger(Take);
        }

        public override string ToString()
        {
            return $"id:{SessionId}, fieldId:{FieldId}, start:{StartIndex}, take:{Take}";
        }
    }
}
