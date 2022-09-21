using System;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public abstract class Pv2DigitParamType<TParamValue, TValue> : Pv2ParamType<TValue, TParamValue>
        where TValue : IComparable<TValue>, IComparable
        where TParamValue : Pv2ParamValue
    {
        private string _formatString = string.Empty;
        private string _units = string.Empty;

        internal Pv2DigitParamType()
        {
        }

        protected Pv2DigitParamType(string paramName, string description, string groupName, string formatString,
            string units, TValue min, TValue max, TValue defaultValue, Pv2ParamFlags flags) : base(paramName,
            description, groupName, defaultValue, flags)
        {
            FormatString = formatString;
            Units = units;
            Min = min;
            Max = max;
            if (Max.CompareTo(Min) < 0) throw new ArgumentException($"Max < Min {Max} <= {Min}");
            Validate(DefaultValue);
        }

        public string FormatString
        {
            get => _formatString;
            protected set => Pv2ParamInterface.CheckAndSetFormatString(ref _formatString, value);
        }

        public string Units
        {
            get => _units;
            protected set => Pv2ParamInterface.CheckAndSetUnits(ref _units, value);
        }

        public TValue Min { get; private set; }

        public TValue Max { get; private set; }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            _formatString = BinSerialize.ReadString(ref buffer);
            _units = BinSerialize.ReadString(ref buffer);
            Min = DeserializeValue(ref buffer);
            Max = DeserializeValue(ref buffer);
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WriteString(ref buffer, FormatString);
            BinSerialize.WriteString(ref buffer, Units);
            SerializeValue(ref buffer, Min);
            SerializeValue(ref buffer, Max);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() +
                   BinSerialize.GetSizeForString(FormatString) +
                   BinSerialize.GetSizeForString(Units) +
                   GetValueSize(Min) +
                   GetValueSize(Max);
        }

        public override string ConvertToString(Pv2ParamValue value)
        {
            return FormatString.FormatWith(GetValue(value));
        }

        public override bool IsValidValue(TValue value)
        {
            return value.CompareTo(Max) <= 0 && value.CompareTo(Min) >= 0;
        }

        public override string GetValidationError(TValue value)
        {
            if (value.CompareTo(Max) > 0) return $"value must be less then {FormatString.FormatWith(Max)}";
            if (value.CompareTo(Min) < 0) return $"value must be more then {FormatString.FormatWith(Min)}";
            return null;
        }

        public override string ToString()
        {
            return
                $"{GroupName}.{ParamName}[{TypeEnum:G}] {FormatString.FormatWith(Min)} to {FormatString.FormatWith(Max)}";
        }
    }
}
