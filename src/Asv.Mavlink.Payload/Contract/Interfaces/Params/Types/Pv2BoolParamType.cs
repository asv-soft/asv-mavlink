using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2BoolParamValue : Pv2ParamValue
    {
        public override Pv2ParamTypeEnum Type => Pv2ParamTypeEnum.Bool;

        public bool RawValue { get; set; }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WriteBool(ref buffer, RawValue);
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            RawValue = BinSerialize.ReadBool(ref buffer);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() + sizeof(bool);
        }

        public override void CopyFrom(Pv2ParamValue data)
        {
            RawValue = Pv2ParamInterface.CheckValueTypeAndCast<Pv2BoolParamValue>(data).RawValue;
        }

        public override string ToString()
        {
            return $"[{Index}] {Type:G} RAW:{RawValue}";
        }
    }

    public class Pv2BoolParamType : Pv2ParamType<bool, Pv2BoolParamValue>
    {
        private string _falseString;
        private string _trueString;


        public Pv2BoolParamType()
        {
        }

        public Pv2BoolParamType(string paramName, string description, string groupName, bool defaultValue = false,
            string trueString = "On", string falseString = "Off", Pv2ParamFlags flags = Pv2ParamFlags.NoFlags) : base(
            paramName, description, groupName, defaultValue, flags)
        {
            TrueString = trueString;
            FalseString = falseString;
            ValidateSize();
        }

        public string TrueString
        {
            get => _trueString;
            private set => Pv2ParamInterface.CheckAndSetUnits(ref _trueString, value);
        }

        public string FalseString
        {
            get => _falseString;
            private set => Pv2ParamInterface.CheckAndSetUnits(ref _falseString, value);
        }

        public override Pv2ParamTypeEnum TypeEnum => Pv2ParamTypeEnum.Bool;

        protected override bool DeserializeValue(ref ReadOnlySpan<byte> buffer)
        {
            return BinSerialize.ReadBool(ref buffer);
        }

        protected override void SerializeValue(ref Span<byte> buffer, bool value)
        {
            BinSerialize.WriteBool(ref buffer, value);
        }

        protected override int GetValueSize(bool value)
        {
            return sizeof(byte);
        }

        public override string ConvertToString(Pv2ParamValue value)
        {
            return GetValue(value) ? TrueString : FalseString;
        }

        protected override void InternalSetValue(Pv2BoolParamValue paramValue, bool value)
        {
            paramValue.RawValue = value;
        }

        protected override bool InternalGetValue(Pv2BoolParamValue paramValue)
        {
            return paramValue.RawValue;
        }

        public override bool IsValidValue(bool value)
        {
            return true;
        }

        public override string GetValidationError(bool value)
        {
            return null;
        }


        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WriteString(ref buffer, TrueString);
            BinSerialize.WriteString(ref buffer, FalseString);
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            TrueString = BinSerialize.ReadString(ref buffer);
            FalseString = BinSerialize.ReadString(ref buffer);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() +
                   BinSerialize.GetSizeForString(TrueString) +
                   BinSerialize.GetSizeForString(FalseString);
        }

        public override string ToString()
        {
            return $"{GroupName}.{ParamName} {TypeEnum:G} [{TrueString}\\{FalseString}]";
        }
    }

    public static class Pv2BoolParamTypeHelper
    {
        public static bool GetValueFromConfig(this Pv2BoolParamType src, IConfiguration config,
            string configSuffix)
        {
            return src.GetValue(src.ReadFromConfig(config, configSuffix));
        }

        public static IObservable<bool> FilterBool(this IObservable<Pv2ParamValueAndTypePair> src,
            Pv2BoolParamType type)
        {
            return src.Filter<Pv2BoolParamType, Pv2BoolParamValue, bool>(type);
        }

        public static bool ReadBool(this IPv2ServerParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2BoolParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2BoolParamType)}. Got {param.GetType().Name}");
            var paramResult = src.Read(param);
            return type.GetValue(paramResult);
        }

        public static void WriteBool(this IPv2ServerParamsInterface src, Pv2ParamType param, bool value)
        {
            if (param is not Pv2BoolParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2BoolParamType)}. Got {param.GetType().Name}");
            src.Write(param, (_, v) => type.SetValue(v, value));
        }

        public static bool ReadBool(this IPv2ClientParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2BoolParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2BoolParamType)}. Got {param.GetType().Name}");
            var item = src.Read(param);
            return type.GetValue(item);
        }

        public static async Task<bool> WriteBool(this IPv2ClientParamsInterface src, Pv2ParamType param, bool value,
            CancellationToken cancel = default)
        {
            if (param is not Pv2BoolParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2BoolParamType)}. Got {param.GetType().Name}");
            var result = await src.Write(param, (_, val) => type.SetValue(val, value), cancel).ConfigureAwait(false);
            return type.GetValue(result);
        }
    }
}
