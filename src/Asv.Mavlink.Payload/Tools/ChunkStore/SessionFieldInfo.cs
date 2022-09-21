using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class SessionFieldInfo:ISizedSpanSerializable
    {
        public SessionFieldInfo()
        {
            
        }

        public SessionFieldInfo(SessionRecordMetadata metadata,uint sizeInBytes, uint count)
        {
            Metadata = metadata;
            SizeInBytes = sizeInBytes;
            Count = count;
        }

        public SessionRecordMetadata Metadata { get; set; }
        public uint SizeInBytes { get; set; }
        public uint Count { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Metadata ??= new SessionRecordMetadata();
            Metadata.Deserialize(ref buffer);
            SizeInBytes = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Count = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            Metadata.Serialize(ref buffer);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, SizeInBytes);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, Count);
        }

        public int GetByteSize()
        {
            return Metadata.GetByteSize()
                   + BinSerialize.GetSizeForPackedUnsignedInteger(SizeInBytes)
                   + BinSerialize.GetSizeForPackedUnsignedInteger(Count);
        }

        public override string ToString()
        {
            return $"{Metadata} count:{Count}, size:{SizeInBytes}";
        }
    }
}
