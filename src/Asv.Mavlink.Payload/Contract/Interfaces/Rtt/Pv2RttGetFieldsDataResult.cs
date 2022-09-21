using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2RttGetFieldsDataResult : ISizedSpanSerializable
    {
        public const int HeaderSize = 10; /* max size of PackedUnsignedInteger * 2 */
        public const int MaxDataSize = PayloadV2Helper.MaxMessageSize - HeaderSize;

        public Pv2RttGetFieldsDataResult()
        {
        }

        public Pv2RttGetFieldsDataResult(uint count, byte[] resultArray, uint arraySize)
        {
            Count = count;
            Data = new byte[arraySize];
            Buffer.BlockCopy(resultArray, 0, Data, 0, Data.Length);
        }

        public byte[] Data { get; set; }
        public uint Count { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Count = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            var arraySize = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Data = BinSerialize.ReadBlock(ref buffer, (int)arraySize);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, Count);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)Data.Length);
            BinSerialize.WriteBlock(ref buffer, new ReadOnlySpan<byte>(Data));
        }

        public int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(Count) +
                   BinSerialize.GetSizeForPackedUnsignedInteger((uint)Data.Length) +
                   Data.Length /* byte data size in array*/;
        }

        public override string ToString()
        {
            return $"count:{Count} data:byte[{Data.Length}]";
        }
    }
}
