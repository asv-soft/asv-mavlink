using System;

namespace Asv.Mavlink
{
    //                         MAVLink 2 packet
    // [STX|LEN|INC|CMP|SEQ|SYS|COMP|MSG_ID| PAYLOAD | CHECKSUM      | SIGNATURE    |
    //   0   1   2   3   4   5   6    7-9   10-(10+N)  (11+N)-(12+N)   (13+N)-(26+N)



    
    public static class PacketV2Helper
    {
        /// <summary>
        /// Protocol-specific start-of-text (STX) marker used to indicate the beginning of a new packet. 
        /// Any system that does not understand protocol version will skip the packet.
        /// </summary>
        public const byte MagicMarkerV2 = 0xFD;
        /// <summary>
        /// The maximum packet length is 279 bytes for a signed message that uses the whole payload.
        /// </summary>
        public const int PacketV2MaxSize = 279;

        /// <summary>
        /// Packet frame byte size without Payload and Signature
        /// </summary>
        public const int PacketV2FrameSize = 13;
        public const int PaylodStartIndexInFrame = PacketV2FrameSize - /*STX*/1 - 2 /*CRC*/ ;
        /// <summary>
        /// Size of signature, if present
        /// </summary>
        public const int SignatureByteSize = 13;

        public static void SetMessageId(byte[] buffer, int frameStartIndex, int messageId)
        {
            buffer[frameStartIndex + 7] = (byte)((messageId) & 0xFF);
            buffer[frameStartIndex + 8] = (byte)((messageId >> 8) & 0xFF);
            buffer[frameStartIndex + 9] = (byte)((messageId >> 16) & 0xFF);
        }

        public static int GetMessageId(byte[] buffer, int frameStartIndex)
        {
            int messageId = buffer[7 + frameStartIndex];
            messageId |= buffer[8 + frameStartIndex] << 8;
            messageId |= buffer[9 + frameStartIndex] << 16;
            return messageId;
        }

        public static bool CheckSignaturePresent(byte[] buffer, int inx)
        {
            return (GetIncompatFlags(buffer,inx) & 0x01) != 0;
        }

        public static void SetStx(Span<byte> buffer)
        {
            buffer[0] = MagicMarkerV2;
        }

        public static void SetStx(byte[] buffer, int inx)
        {
            buffer[inx] = MagicMarkerV2;
        }
        public static bool CheckStx(byte[] buffer, int inx)
        {
            return buffer[inx] == MagicMarkerV2;
        }

        public static void VerifyStx(byte[] buffer, int inx)
        {
            if (!CheckStx(buffer, inx))
                throw new MavlinkException(string.Format(RS.WheelKnownConstant_VerifyStx_Unknown_STX_value, MagicMarkerV2, buffer[inx]));
        }

        public static byte GetPayloadSize(byte[] buffer, int inx)
        {
            return buffer[inx + 1];
        }

        public static void SetPayloadSize(byte[] buffer, int inx, byte size)
        {
            buffer[inx + 1] = size;
        }

        public static byte GetIncompatFlags(byte[] buffer, int inx)
        {
            return buffer[inx + 2];
        }

        public static void SetIncompatFlags(byte[] buffer, int inx, byte value)
        {
            buffer[inx + 2] = value;
        }

        public static byte GetCompatFlags(byte[] buffer, int inx)
        {
            return buffer[inx + 3];
        }
        public static void SetCompatFlags(byte[] buffer, int inx, byte value)
        {
            buffer[inx + 3] = value;
        }

        public static byte GetSequence(byte[] buffer, int inx)
        {
            return buffer[inx + 4];
        }
        public static void SetSequence(byte[] buffer, int inx, byte value)
        {
            buffer[inx + 4] = value;
        }

        public static byte GetSystemId(byte[] buffer, int inx)
        {
            return buffer[inx + 5];
        }
        public static void SetSystemId(byte[] buffer, int inx, byte value)
        {
            buffer[inx + 5] = value;
        }

        public static byte GetComponenId(byte[] buffer, int inx)
        {
            return buffer[inx + 6];
        }
        public static void SetComponenId(byte[] buffer, int inx, byte value)
        {
            buffer[inx + 6] = value;
        }

        public static void VerifyCrc(byte[] buffer, int inx, byte crcEtra)
        {
            var payloadSize = GetPayloadSize(buffer, inx);
            var calcCrc = CalcCrc(buffer, inx, crcEtra, payloadSize);
            ushort crc = buffer[inx + PaylodStartIndexInFrame + payloadSize];
            crc |= (ushort)(buffer[inx + PaylodStartIndexInFrame + payloadSize+1] << 8);
            if (crc != calcCrc)
                throw new MavlinkException(string.Format(RS.PacketV2Helper_VerifyCrc_Bad_X25Crc, calcCrc, crc));
        }

        private static ushort CalcCrc(byte[] buffer, int inx, byte crcEtra, byte payloadSize)
        {
            var crc = X25Crc.Accumulate(buffer, X25Crc.CrcSeed, inx + 1, 9);
            crc = X25Crc.Accumulate(buffer,crc, 10, payloadSize);

            crc = X25Crc.Accumulate(crcEtra, crc);
            return crc;
        }

        public static int GetSignatureStartIndex(byte[] buffer, int inx)
        {
            return inx + PaylodStartIndexInFrame + GetPayloadSize(buffer, inx) + /*CRC*/ 2;
        }

        public static void SetCrc(byte[] buffer, int inx, byte crcEtra)
        {
            var payloadSize = GetPayloadSize(buffer, inx);
            var crc = CalcCrc(buffer, inx, crcEtra, payloadSize);
            buffer[inx + PaylodStartIndexInFrame + payloadSize] = (byte) (crc & 0xFF);
            buffer[inx + PaylodStartIndexInFrame + payloadSize+1] = (byte)(crc >> 8 & 0xFF);
        }
    }
}
