using System;
using System.Collections.Generic;
using System.Linq;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class SessionSettings:IEqualityComparer<SessionSettings>, ISizedSpanSerializable
    {
        private string _name;

        public SessionSettings()
        {
            
        }

        public SessionSettings(string name, params string[] tags)
        {
            if (tags == null) throw new ArgumentNullException(nameof(tags));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            Tags = new HashSet<string>(tags);
            foreach (var tag in tags)
            {
                ChunkStoreHelper.CheckName(tag);
            }
            Name = name;
        }

        public string Name
        {
            get => _name;
            set => ChunkStoreHelper.CheckAndSetName(ref _name, value);
        }

        public HashSet<string> Tags { get; set; }

        public override string ToString()
        {
            return $"name:{Name},tags:[{string.Join("|", Tags)}]";
        }

        public virtual void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Name = BinSerialize.ReadString(ref buffer);
            var count = BinSerialize.ReadByte(ref buffer);
            Tags = new HashSet<string>();
            for (var i = 0; i < count; i++)
            {
                Tags.Add(BinSerialize.ReadString(ref buffer));
            }
        }

        public virtual void Serialize(ref Span<byte> buffer)
        {
            if (Tags.Count > byte.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(Tags));
            BinSerialize.WriteString(ref buffer, Name);
            if (Tags == null)
            {
                BinSerialize.WriteByte(ref buffer, 0);
                return;
            }
            BinSerialize.WriteByte(ref buffer, (byte)Tags.Count);
            foreach (var tag in Tags)
            {
                BinSerialize.WriteString(ref buffer, tag);
            }
        }

        public virtual int GetByteSize()
        {
            return BinSerialize.GetSizeForString(Name) +
                   sizeof(byte) +
                   (Tags?.Sum(BinSerialize.GetSizeForString) ?? 0);
        }

        

        public bool Equals(SessionSettings x, SessionSettings y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Name == y.Name && Equals(x.Tags, y.Tags);
        }

        public int GetHashCode(SessionSettings obj)
        {
            unchecked
            {
                return ((obj.Name != null ? obj.Name.GetHashCode() : 0) * 397) ^ (obj.Tags != null ? obj.Tags.GetHashCode() : 0);
            }
        }
    }
}
