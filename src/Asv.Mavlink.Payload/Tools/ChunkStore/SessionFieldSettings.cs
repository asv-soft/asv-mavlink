using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class SessionFieldSettings: ISizedSpanSerializable,IEquatable<SessionFieldSettings>
    {
        public SessionFieldSettings()
        {

        }

        public SessionFieldSettings(uint id,string name,ushort offset)
        {
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));
            Id = id;
            Name = name;
            Offset = offset;
        }

        public uint Id { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => ChunkStoreHelper.CheckAndSetName(ref _name,value);
        }

        public ushort Offset { get; set; }

        public virtual void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Id = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Name = BinSerialize.ReadString(ref buffer);
            Offset = BinSerialize.ReadUShort(ref buffer);
        }

        public virtual void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, Id);
            BinSerialize.WriteString(ref buffer, Name);
            BinSerialize.WriteUShort(ref buffer, Offset);
        }

        public virtual int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(Id) + BinSerialize.GetSizeForString(Name) + sizeof(ushort);
        }

        public override string ToString()
        {
            return $"{Name}[{Id}] offset={Offset}";
        }

        public bool Equals(SessionFieldSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _name == other._name && Id == other.Id && Offset == other.Offset;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SessionFieldSettings)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_name != null ? _name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Id.GetHashCode();
                hashCode = (hashCode * 397) ^ Offset.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(SessionFieldSettings left, SessionFieldSettings right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SessionFieldSettings left, SessionFieldSettings right)
        {
            return !Equals(left, right);
        }
    }
}
