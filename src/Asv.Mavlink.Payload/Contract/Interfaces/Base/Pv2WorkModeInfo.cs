using System;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2WorkModeInfo : SpanKeyWithNameAndDescriptionType<byte>
    {
        public Pv2WorkModeInfo()
        {
            
        }

        public Pv2WorkModeInfo(byte id, IWorkModeFactory modeFactory)
        {
            Id = id;
            Description = modeFactory.Description;
            Name = modeFactory.Name;
            Icon = modeFactory.Icon;
        }

        public const byte MaxWorkModeCount = 32;
        public const byte MaxWorkModeStatusCount = 8;

        
        public const int MaxDescriptionLength = 50;

        public IconTypeEnum Icon { get; set; }

        protected override void InternalValidateName(string name)
        {
            if (ChunkStoreHelper.NameRegex.IsMatch(name) == false)
                throw new ArgumentException(
                    $"Work mode {nameof(Name)} '{name}' not match regex '{ChunkStoreHelper.NameRegexString}'");
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)Icon);
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            Icon = (IconTypeEnum)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() + BinSerialize.GetSizeForPackedUnsignedInteger((uint)Icon);
        }

        protected override byte InternalReadKey(ref ReadOnlySpan<byte> buffer)
        {
            return BinSerialize.ReadByte(ref buffer);
        }

        protected override void InternalWriteKey(ref Span<byte> buffer, byte id)
        {
            BinSerialize.WriteByte(ref buffer, id);
        }

        protected override int InternalGetSizeKey(byte id)
        {
            return sizeof(byte);
        }

        protected override void InternalValidateDescription(string description)
        {
            if (description.IsEmpty() || description.Length > MaxDescriptionLength)
                throw new ArgumentOutOfRangeException(nameof(Description), description,
                    $"Work mode {nameof(Description)} '{description}' must be not empty and less then {MaxDescriptionLength} symbols");
        }
    }
}
