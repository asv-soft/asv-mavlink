using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public static class PayloadV2Helper
    {
        public const ushort DefaultRequestSuccessMessageType = 32773;
        public const ushort DefaultResponseSuccessMessageType = 32774;
        public const ushort DefaultErrorMessageType = 32772;
        public const int MaxHeaderSize = 11;
        public const int MaxMessageSize = 249 - MaxHeaderSize;
        public static int MaxErrorMessageStringCharSize = 200;

        public static void WriteHeader(ref Span<byte> span, ushort methodInterfaceId, ushort methodMethodId,
            byte requestId)
        {
            BinSerialize.WriteByte(ref span, requestId);
            BinSerialize.WritePackedUnsignedInteger(ref span, methodInterfaceId);
            BinSerialize.WritePackedUnsignedInteger(ref span, methodMethodId);
        }

        public static int GetHeaderByteSize(ushort methodInterfaceId, ushort methodMethodId, byte sequence)
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(methodInterfaceId) +
                   BinSerialize.GetSizeForPackedUnsignedInteger(methodMethodId) + sizeof(byte);
        }

        public static void ReadHeader(ref ReadOnlySpan<byte> span, out ushort methodInterfaceId,
            out ushort methodMethodId, out byte requestId)
        {
            requestId = BinSerialize.ReadByte(ref span);
            var val = BinSerialize.ReadPackedUnsignedInteger(ref span);
            if (val > ushort.MaxValue)
                throw new Exception($"Interface ID is more then max value {val} > {ushort.MaxValue}");
            methodInterfaceId = (ushort)val;
            val = BinSerialize.ReadPackedUnsignedInteger(ref span);
            if (val > ushort.MaxValue)
                throw new Exception($"Method ID is more then max value {val} > {ushort.MaxValue}");
            methodMethodId = (ushort)val;
        }

        public static uint GetMessageId(ushort interfaceId, ushort methodId)
        {
            return ((uint)interfaceId << 16) | methodId;
        }
    }
}
