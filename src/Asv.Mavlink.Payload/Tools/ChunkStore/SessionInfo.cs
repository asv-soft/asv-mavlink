using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class SessionInfo:ISizedSpanSerializable
    {
        public SessionInfo()
        {
            
        }

        public SessionInfo(SessionMetadata metadata, uint fieldsCount, uint itemsCount, uint dataSize, DateTime created)
        {
            Metadata = metadata;
            FieldsCount = fieldsCount;
            ItemsCount = itemsCount;
            DataSize = dataSize;
            Created = created;
        }

        public SessionMetadata Metadata { get; set; }
        public uint FieldsCount { get; set; }
        public uint ItemsCount { get; set; }
        public uint DataSize { get; set; }
        public DateTime Created { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Metadata ??= new SessionMetadata();
            Metadata.Deserialize(ref buffer);
            FieldsCount = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            ItemsCount = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            DataSize = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Created = DateTime.FromBinary(BinSerialize.ReadLong(ref buffer));
        }

        public void Serialize(ref Span<byte> buffer)
        {
            Metadata.Serialize(ref buffer);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, FieldsCount);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, ItemsCount);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, DataSize);
            BinSerialize.WriteLong(ref buffer,Created.ToBinary());
        }

        public int GetByteSize()
        {
            return Metadata.GetByteSize()
                   + BinSerialize.GetSizeForPackedUnsignedInteger(FieldsCount)
                   + BinSerialize.GetSizeForPackedUnsignedInteger(ItemsCount)
                   + BinSerialize.GetSizeForPackedUnsignedInteger(DataSize)
                   + sizeof(long);
        }

        public override string ToString()
        {
            return $"{Metadata} created:{Created:g} records={FieldsCount}, data:{ItemsCount}, size:{DataSize}";
        }
    }
}
