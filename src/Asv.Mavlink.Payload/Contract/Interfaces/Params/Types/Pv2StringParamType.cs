using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2StringParamValue : Pv2ParamValue
    {
        public override Pv2ParamTypeEnum Type => Pv2ParamTypeEnum.String;

        public string RawValue { get; set; } = string.Empty;

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WriteString(ref buffer, RawValue);
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            RawValue = BinSerialize.ReadString(ref buffer);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() + BinSerialize.GetSizeForString(RawValue);
        }

        public override void CopyFrom(Pv2ParamValue data)
        {
            RawValue = Pv2ParamInterface.CheckValueTypeAndCast<Pv2StringParamValue>(data).RawValue;
        }

        public override string ToString()
        {
            return $"{Index} {Type:G} RAW:{RawValue}";
        }
    }

    public class Pv2StringParamType : Pv2ParamType<string, Pv2StringParamValue>
    {
        public Pv2StringParamType()
        {
        }

        public Pv2StringParamType(string paramName, string description, string groupName, string defaultValue,
            uint maxLength = 200, Pv2ParamFlags flags = Pv2ParamFlags.NoFlags) : base(paramName, description, groupName,
            defaultValue, flags)
        {
            MaxLength = maxLength;
            if (MaxLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxLength));
            ValidateSize();
        }

        public uint MaxLength { get; private set; }

        public override Pv2ParamTypeEnum TypeEnum => Pv2ParamTypeEnum.String;

        protected override void InternalSetValue(Pv2StringParamValue paramValue, string value)
        {
            paramValue.RawValue = value;
        }

        protected override string InternalGetValue(Pv2StringParamValue paramValue)
        {
            return paramValue.RawValue;
        }

        public override string ConvertToString(Pv2ParamValue value)
        {
            return GetValue(value);
        }

        // public override string GetValue(Pv2ParamValue paramValue)
        // {
        //     return Pv2ParamInterface.CheckValueTypeAndCast<Pv2StringParamValue>(paramValue).RawValue;
        // }
        //
        // public override void SetValue(Pv2ParamValue paramValue, string value)
        // {
        //     var intParamValue = Pv2ParamInterface.CheckValueTypeAndCast<Pv2StringParamValue>(paramValue);
        //     Validate(value);
        //     intParamValue.RawValue = value;
        // }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, MaxLength);
        }

        protected override string DeserializeValue(ref ReadOnlySpan<byte> buffer)
        {
            return BinSerialize.ReadString(ref buffer);
        }

        protected override void SerializeValue(ref Span<byte> buffer, string value)
        {
            BinSerialize.WriteString(ref buffer, value);
        }

        protected override int GetValueSize(string value)
        {
            return BinSerialize.GetSizeForString(value);
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            MaxLength = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() + BinSerialize.GetSizeForPackedUnsignedInteger(MaxLength);
        }

        public override bool IsValidValue(string value)
        {
            if (value == null) return false;
            return !(value.Length > MaxLength);
        }

        public override string GetValidationError(string value)
        {
            if (value == null) return "Value is null";
            if (value.Length > MaxLength) return $"Value must be less then {MaxLength} chars (now {value.Length})";
            return null;
        }
    }

    public static class Pv2StringParamTypeHelper
    {
        public static string GetValueFromConfig(this Pv2StringParamType src, IConfiguration config,
            string configSuffix)
        {
            return src.GetValue(src.ReadFromConfig(config, configSuffix));
        }

        public static IObservable<string> FilterString(this IObservable<Pv2ParamValueAndTypePair> src,
            Pv2StringParamType type)
        {
            return src.Filter<Pv2StringParamType, Pv2StringParamValue, string>(type);
        }

        public static string ReadString(this IPv2ServerParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2StringParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2StringParamType)}. Got {param.GetType().Name}");
            var paramResult = src.Read(param);
            return type.GetValue(paramResult);
        }

        public static void WriteString(this IPv2ServerParamsInterface src, Pv2ParamType param, string value)
        {
            if (param is not Pv2StringParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2StringParamType)}. Got {param.GetType().Name}");
            src.Write(param, (_, v) => type.SetValue(v, value));
        }

        public static string ReadString(this IPv2ClientParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2StringParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2StringParamType)}. Got {param.GetType().Name}");
            var item = src.Read(param);
            return type.GetValue(item);
        }

        public static async Task<string> WriteString(this IPv2ClientParamsInterface src, Pv2ParamType param,
            string value, CancellationToken cancel = default)
        {
            if (param is not Pv2StringParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2StringParamType)}. Got {param.GetType().Name}");
            var result = await src.Write(param, (_, val) => type.SetValue(val, value), cancel).ConfigureAwait(false);
            return type.GetValue(result);
        }
    }
}
