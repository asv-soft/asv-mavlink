using System;
using System.Text.RegularExpressions;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2WorkModeStatusInfo : SpanKeyWithNameAndDescriptionType<byte>
    {
        public const string NameRegexString = "^[A-Za-z][A-Za-z0-9_]{1,15}$";
        public const int MaxDescriptionLength = 50;
        public static readonly Regex NameRegex = new(NameRegexString, RegexOptions.Compiled);

        public WorkModeStatusClassEnum Class { get; set; }

        protected override void InternalValidateName(string name)
        {
            if (NameRegex.IsMatch(name) == false)
                throw new ArgumentException(
                    $"Work mode status {nameof(Name)} '{name}' not match regex '{NameRegexString}'");
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WriteByte(ref buffer, (byte)Class);
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            Class = (WorkModeStatusClassEnum)BinSerialize.ReadByte(ref buffer);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() + sizeof(byte);
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
                    $"Work mode status {nameof(Description)} '{description}' must be not empty and less then {MaxDescriptionLength} symbols");
        }
    }
}
