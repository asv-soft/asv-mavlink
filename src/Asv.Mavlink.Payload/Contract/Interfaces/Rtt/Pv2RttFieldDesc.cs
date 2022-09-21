using System;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    [Flags]
    public enum Pv2RttFieldFlags : uint
    {
        NoFlags = 0b0000_0000,
        Indexed = 0b0000_0001,
        Reserved2 = 0b0000_0010,
        Reserved3 = 0b0000_0100,
        Reserved4 = 0b0000_1000,
        Reserved5 = 0b0001_0000,
        Reserved6 = 0b0010_0000,
        Reserved7 = 0b0100_0000,
        Reserved8 = 0b1000_0000
    }

    public enum Pv2RttFieldType : ushort
    {
        Unknown,
        FixedPoint
    }

    public class Pv2RttFieldDesc : ISizedSpanSerializable
    {
        private string _description;
        private string _formatString;
        private string _fullName;
        private string _groupName;
        private string _name;
        private string _units;

        public Pv2RttFieldDesc()
        {
        }

        public Pv2RttFieldDesc(byte id, string name, string description, string units, string formatString,
            Pv2RttFieldFlags flags, object defaultValue, Pv2RttRecordDesc group)
        {
            Id = id;
            Name = name;
            Description = description;
            Units = units;
            FormatString = formatString;
            Flags = flags;
            GroupId = group.Id;
            GroupName = group.Name;
            // ReSharper disable once VirtualMemberCallInConstructor
            ValidateValue(defaultValue);
            DefaultValue = defaultValue;
            ValidateSize();
        }

        public string GroupName
        {
            get => _groupName;
            private set
            {
                _fullName = null;
                _groupName = value;
            }
        }

        public string FullName => _fullName ??= $"{GroupName}.{Name}";
        public virtual Pv2RttFieldType Type { get; }

        public byte Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _fullName = null;
                ChunkStoreHelper.CheckAndSetName(ref _name, value);
            }
        }

        public string Description
        {
            get => _description;
            set => Pv2RttInterface.CheckAndSetDescription(ref _description, value);
        }

        public string Units
        {
            get => _units;
            set => Pv2RttInterface.CheckAndSetUnits(ref _units, value);
        }

        public string FormatString
        {
            get => _formatString;
            set => Pv2RttInterface.CheckAndSetFormatString(ref _formatString, value);
        }

        public Pv2RttFieldFlags Flags { get; private set; }

        public ushort GroupId { get; private set; }

        public object DefaultValue { get; private set; }

        public uint FullId => ((uint)GroupId << 8) | Id;

        public virtual void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            // We wouldn't serialize and deserialize 'Type' field here. It would be serialized upper level for selecting class.
            Id = BinSerialize.ReadByte(ref buffer);
            Name = BinSerialize.ReadString(ref buffer);
            GroupName = BinSerialize.ReadString(ref buffer);
            Description = BinSerialize.ReadString(ref buffer);
            Units = BinSerialize.ReadString(ref buffer);
            FormatString = BinSerialize.ReadString(ref buffer);
            Flags = (Pv2RttFieldFlags)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            GroupId = (ushort)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            var bitIndex = 0;
            DefaultValue = DeserializeValue(buffer, ref bitIndex);
        }

        public virtual void Serialize(ref Span<byte> buffer)
        {
            // We wouldn't serialize and deserialize 'Type' field here. It would be serialized upper level for selecting class.
            BinSerialize.WriteByte(ref buffer, Id);
            BinSerialize.WriteString(ref buffer, Name);
            BinSerialize.WriteString(ref buffer, GroupName);
            BinSerialize.WriteString(ref buffer, Description);
            BinSerialize.WriteString(ref buffer, Units);
            BinSerialize.WriteString(ref buffer, FormatString);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)Flags);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, GroupId);
            var bitIndex = 0;
            SerializeValue(buffer, DefaultValue, ref bitIndex);
        }

        public virtual int GetByteSize()
        {
            return sizeof(byte) +
                   BinSerialize.GetSizeForString(Name) +
                   BinSerialize.GetSizeForString(GroupName) +
                   BinSerialize.GetSizeForString(Description) +
                   BinSerialize.GetSizeForString(Units) +
                   BinSerialize.GetSizeForString(FormatString) +
                   BinSerialize.GetSizeForPackedUnsignedInteger((uint)Flags) +
                   BinSerialize.GetSizeForPackedUnsignedInteger(GroupId) +
                   GetValueMaxByteSize();
        }

        public void ValidateSize()
        {
            var size = GetByteSize();
            if (size > Pv2RttInterface.MaxOnStreamDataSize)
                throw new Exception(
                    $"Max size of serialized field info must be less then '{Pv2RttInterface.MaxOnStreamDataSize}'. '{size}' bytes now.");
        }

        public virtual void ValidateValue(object value)
        {
            throw new NotImplementedException();
        }

        public virtual int GetValueMaxByteSize()
        {
            throw new NotImplementedException();
        }

        public virtual string ConvertToString(object value)
        {
            return FormatString.FormatWith(value);
        }


        public virtual void SerializeValue(Span<byte> data, object value, ref int bitIndex)
        {
            throw new NotImplementedException();
        }

        public virtual object DeserializeValue(ReadOnlySpan<byte> data, ref int bitIndex)
        {
            throw new NotImplementedException();
        }

        public virtual int GetValueBitSize(object value)
        {
            throw new NotImplementedException();
        }

        
    }
}
