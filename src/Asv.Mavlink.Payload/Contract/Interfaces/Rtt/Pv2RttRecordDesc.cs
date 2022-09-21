using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    [Flags]
    public enum Pv2RttRecordFlags : uint
    {
        NoFlags = 0b0000_0000,
        Reserved1 = 0b0000_0001,
        Reserved2 = 0b0000_0010,
        Reserved3 = 0b0000_0100,
        Reserved4 = 0b0000_1000,
        Reserved5 = 0b0001_0000,
        Reserved6 = 0b0010_0000,
        Reserved7 = 0b0100_0000,
        Reserved8 = 0b1000_0000
    }

    public class Pv2RttRecordDesc : ISizedSpanSerializable
    {
        private string _description;
        private string _name;

        public Pv2RttRecordDesc()
        {
        }

        public Pv2RttRecordDesc(ushort id, string name, string description,
            Pv2RttRecordFlags flags = Pv2RttRecordFlags.NoFlags)
        {
            Name = name;
            Description = description;
            Id = id;
            Flags = flags;
            ValidateSize();
        }

        public ushort Id { get; set; }
        public Pv2RttRecordFlags Flags { get; set; }

        public string Name
        {
            get => _name;
            set => ChunkStoreHelper.CheckAndSetName(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => Pv2RttInterface.CheckAndSetDescription(ref _description, value);
        }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Id = (ushort)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Flags = (Pv2RttRecordFlags)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Name = BinSerialize.ReadString(ref buffer);
            Description = BinSerialize.ReadString(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, Id);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)Flags);
            BinSerialize.WriteString(ref buffer, Name);
            BinSerialize.WriteString(ref buffer, Description);
        }

        public int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(Id) +
                   BinSerialize.GetSizeForPackedUnsignedInteger((uint)Flags) +
                   BinSerialize.GetSizeForString(Name) +
                   BinSerialize.GetSizeForString(Description);
        }

        public void ValidateSize()
        {
            var size = GetByteSize();
            if (size > Pv2RttInterface.MaxOnStreamDataSize)
                throw new Exception(
                    $"Max size of serialized record info must be less then '{Pv2RttInterface.MaxOnStreamDataSize}'. '{size}' bytes now.");
        }

        public override string ToString()
        {
            return $"id:{Id}, name:{Name}, flags:[{Flags:F}]";
        }
    }
}
