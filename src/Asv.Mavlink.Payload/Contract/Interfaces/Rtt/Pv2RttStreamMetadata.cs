using System;
using System.Collections.Generic;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    [Flags]
    public enum Pv2StreamFlags : byte
    {
        NoFlags = 0b0000_0000,
        RecordEnabled = 0b0000_0001,
        Reserved2 = 0b0000_0010,
        Reserved3 = 0b0000_0100,
        Reserved4 = 0b0000_1000,
        Reserved5 = 0b0001_0000,
        Reserved6 = 0b0010_0000,
        Reserved7 = 0b0100_0000,
        Reserved8 = 0b1000_0000
    }


    public class Pv2RttStreamMetadata : ISizedSpanSerializable
    {
        public const int
            MetadataSizeWithoutGroupIds =
                SessionId.MaxByteSize + 7; /* SessionId + counter(5) + Flags(1) + GroupCount(1) */

        public const int
            MaxMetadataSizeWithOneGroup =
                SessionId.MaxByteSize + 9; /* SessionId +  counter(5) + Flags(1) + GroupCount(1) + GroupId(2) */

        public SessionId SessionId { get; set; }
        public uint Counter { get; set; }
        public Pv2StreamFlags Flags { get; set; }
        public List<ushort> Groups { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            SessionId ??= new SessionId();
            SessionId.Deserialize(ref buffer);
            Counter = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Flags = (Pv2StreamFlags)BinSerialize.ReadByte(ref buffer);
            var count = BinSerialize.ReadByte(ref buffer);
            Groups = new List<ushort>(count);
            for (var i = 0; i < count; i++) Groups.Add(BinSerialize.ReadUShort(ref buffer));
        }

        public void Serialize(ref Span<byte> buffer)
        {
            SessionId.Serialize(ref buffer);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, Counter);
            BinSerialize.WriteByte(ref buffer, (byte)Flags);
            BinSerialize.WriteByte(ref buffer, (byte)Groups.Count);
            foreach (var group in Groups) BinSerialize.WriteUShort(ref buffer, group);
        }

        public int GetByteSize()
        {
            return SessionId.GetByteSize() +
                   BinSerialize.GetSizeForPackedUnsignedInteger(Counter) +
                   sizeof(byte) /* Flags */ +
                   sizeof(byte) /* Count */ +
                   (Groups?.Count * sizeof(ushort) ?? 0);
        }
    }
}
