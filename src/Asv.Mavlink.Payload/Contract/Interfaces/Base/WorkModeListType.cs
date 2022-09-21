using System;
using System.Collections.Generic;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class WorkModeListType : ISizedSpanSerializable
    {
        public uint DescHash { get; set; }
        public SpanByteArrayType WorkModes { get; set; }

        public WorkModeListType()
        {
            
        }

        public WorkModeListType(uint hash,IEnumerable<byte> workModes)
        {
            DescHash = hash;
            WorkModes = new SpanByteArrayType(workModes);
        }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            DescHash = BinSerialize.ReadUInt(ref buffer);
            WorkModes = new SpanByteArrayType();
            WorkModes.Deserialize(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,DescHash);
            WorkModes.Serialize(ref buffer);
        }

        public int GetByteSize()
        {
            return sizeof(uint) + WorkModes.GetByteSize();
        }

        public override string ToString()
        {
            return $"hash:{DescHash},modes:[{string.Join(",",WorkModes.Items)}]";
        }
    }
}
