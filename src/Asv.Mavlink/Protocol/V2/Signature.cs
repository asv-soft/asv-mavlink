using System;
using Asv.IO;

namespace Asv.Mavlink
{
    public class Signature : ISignature
    {
        private bool _isPresent;

        public bool IsPresent
        {
            get { return _isPresent; }
            set
            {
                _isPresent = value;
                ByteSize = value ? MavlinkV2Protocol.SignatureByteSize : 0;
            }
        }

        public int ByteSize { get; private set; }
        public byte LinkId { get; set; }
        public ulong Timestamp { get; set; }
        public ulong Sign { get; set; }

        public int Deserialize(byte[] buffer, int offset)
        {
            IsPresent = true;
            LinkId = buffer[offset];
            Timestamp = buffer[offset + 1];
            Timestamp |= (ulong)buffer[offset + 2] << 8;
            Timestamp |= (ulong)buffer[offset + 3] << 16;
            Timestamp |= (ulong)buffer[offset + 4] << 24;
            Timestamp |= (ulong)buffer[offset + 5] << 32;
            Timestamp |= (ulong)buffer[offset + 6] << 40;

            Sign = buffer[offset + 7];
            Sign |= (ulong)buffer[offset + 8] << 8;
            Sign |= (ulong)buffer[offset + 9] << 16;
            Sign |= (ulong)buffer[offset + 10] << 24;
            Sign |= (ulong)buffer[offset + 11] << 32;
            Sign |= (ulong)buffer[offset + 12] << 40;
            return ByteSize;
        }

        public int GetMaxByteSize() => MavlinkV2Protocol.SignatureByteSize;

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            IsPresent = true;
            LinkId = BinSerialize.ReadByte(ref buffer);
            Timestamp = BinSerialize.ReadByte(ref buffer);
            Timestamp |= (ulong)BinSerialize.ReadByte(ref buffer) << 8;
            Timestamp |= (ulong)BinSerialize.ReadByte(ref buffer) << 16;
            Timestamp |= (ulong)BinSerialize.ReadByte(ref buffer) << 24;
            Timestamp |= (ulong)BinSerialize.ReadByte(ref buffer) << 32;
            Timestamp |= (ulong)BinSerialize.ReadByte(ref buffer) << 40;

            Sign = BinSerialize.ReadByte(ref buffer);
            Sign |= (ulong)BinSerialize.ReadByte(ref buffer) << 8;
            Sign |= (ulong)BinSerialize.ReadByte(ref buffer) << 16;
            Sign |= (ulong)BinSerialize.ReadByte(ref buffer) << 24;
            Sign |= (ulong)BinSerialize.ReadByte(ref buffer) << 32;
            Sign |= (ulong)BinSerialize.ReadByte(ref buffer) << 40;
        }

        public void Serialize(ref Span<byte> buffer)
        {
            if (!IsPresent) return;
            BinSerialize.WriteByte(ref buffer,LinkId);

            BinSerialize.WriteByte(ref buffer, (byte)(Timestamp & 0xFF));
            BinSerialize.WriteByte(ref buffer, (byte)(Timestamp >> 8 & 0xFF));
            BinSerialize.WriteByte(ref buffer, (byte)(Timestamp >> 16 & 0xFF));
            BinSerialize.WriteByte(ref buffer, (byte)(Timestamp >> 24 & 0xFF));
            BinSerialize.WriteByte(ref buffer, (byte)(Timestamp >> 32 & 0xFF));
            BinSerialize.WriteByte(ref buffer, (byte)(Timestamp >> 40 & 0xFF));

            BinSerialize.WriteByte(ref buffer, (byte)(Sign & 0xFF));
            BinSerialize.WriteByte(ref buffer, (byte)(Sign >> 8 & 0xFF));
            BinSerialize.WriteByte(ref buffer, (byte)(Sign >> 16 & 0xFF));
            BinSerialize.WriteByte(ref buffer, (byte)(Sign >> 24 & 0xFF));
            BinSerialize.WriteByte(ref buffer, (byte)(Sign >> 32 & 0xFF));
            BinSerialize.WriteByte(ref buffer, (byte)(Sign >> 40 & 0xFF));
        }

        public int Serialize(byte[] buffer, int offset)
        {
            if (!IsPresent) return 0;
            buffer[offset] = LinkId;
            buffer[offset + 1] = (byte) (Timestamp & 0xFF);
            buffer[offset + 2] = (byte)(Timestamp >> 8 & 0xFF);
            buffer[offset + 3] = (byte)(Timestamp >> 16 & 0xFF);
            buffer[offset + 4] = (byte)(Timestamp >> 24 & 0xFF);
            buffer[offset + 5] = (byte)(Timestamp >> 32 & 0xFF);
            buffer[offset + 6] = (byte)(Timestamp >> 40 & 0xFF);


            buffer[offset + 7] = (byte)(Sign & 0xFF);
            buffer[offset + 8] = (byte)(Sign >> 8 & 0xFF);
            buffer[offset + 9] = (byte)(Sign >> 16 & 0xFF);
            buffer[offset + 10] = (byte)(Sign >> 24 & 0xFF);
            buffer[offset + 11] = (byte)(Sign >> 32 & 0xFF);
            buffer[offset + 12] = (byte)(Sign >> 40 & 0xFF);
            return MavlinkV2Protocol.SignatureByteSize;
        }
    }
}
