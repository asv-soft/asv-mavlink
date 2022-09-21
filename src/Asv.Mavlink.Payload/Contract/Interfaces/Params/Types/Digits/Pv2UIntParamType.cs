using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink.Payload.Digits
{
    public class Pv2UIntParamValue : Pv2ParamValue
    {
        public override Pv2ParamTypeEnum Type => Pv2ParamTypeEnum.UInt;

        public uint RawValue { get; set; }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, RawValue);
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            RawValue = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() + BinSerialize.GetSizeForPackedUnsignedInteger(RawValue);
        }

        public override void CopyFrom(Pv2ParamValue data)
        {
            RawValue = Pv2ParamInterface.CheckValueTypeAndCast<Pv2UIntParamValue>(data).RawValue;
        }

        public override string ToString()
        {
            return $"{Index} {Type:G} RAW:{RawValue}";
        }
    }

    public class Pv2UIntParamType : Pv2DigitParamType<Pv2UIntParamValue, uint>
    {
        public Pv2UIntParamType()
        {
        }


        public Pv2UIntParamType(string paramName, string description, string groupName, string formatString,
            string units, uint min = uint.MinValue, uint max = uint.MaxValue, uint defaultValue = 0,
            Pv2ParamFlags flags = Pv2ParamFlags.NoFlags) : base(paramName, description, groupName, formatString, units,
            min, max, defaultValue, flags)
        {
            ValidateSize();
        }

        public override Pv2ParamTypeEnum TypeEnum => Pv2ParamTypeEnum.UInt;

        protected override void InternalSetValue(Pv2UIntParamValue paramValue, uint value)
        {
            paramValue.RawValue = value - Min;
        }

        protected override uint InternalGetValue(Pv2UIntParamValue paramValue)
        {
            return paramValue.RawValue + Min;
        }

        public override void ValidateValue(Pv2ParamValue data)
        {
            Validate(GetValue(data));
        }

        public override string ConvertToString(Pv2ParamValue value)
        {
            return FormatString.FormatWith(GetValue(value));
        }

        protected override uint DeserializeValue(ref ReadOnlySpan<byte> buffer)
        {
            return BinSerialize.ReadPackedUnsignedInteger(ref buffer);
        }

        protected override void SerializeValue(ref Span<byte> buffer, uint value)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, value);
        }

        protected override int GetValueSize(uint value)
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(value);
        }
    }

    public static class Pv2UIntParamTypeHelper
    {
        public static void WriteToConfigValue(this Pv2UIntParamType src, IConfiguration config, string configSuffix,
            uint value)
        {
            var val = src.CreateValue();
            src.SetValue(val, value);
            src.WriteToConfig(config, configSuffix, val);
        }

        public static uint ReadFromConfigValue(this Pv2UIntParamType src, IConfiguration config, string configSuffix)
        {
            return src.GetValue(src.ReadFromConfig(config, configSuffix));
        }

        public static IObservable<uint> FilterUint(this IObservable<Pv2ParamValueAndTypePair> src,
            Pv2UIntParamType type)
        {
            return src.Filter<Pv2UIntParamType, Pv2UIntParamValue, uint>(type);
        }

        public static uint ReadUInt(this IPv2ServerParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2UIntParamType uintType)
                throw new Exception($"Wrong type: want {nameof(Pv2UIntParamType)}. Got {param.GetType().Name}");
            var paramResult = src.Read(param);
            return uintType.GetValue(paramResult);
        }

        public static void WriteUInt(this IPv2ServerParamsInterface src, Pv2ParamType param, uint value)
        {
            if (param is not Pv2UIntParamType uintType)
                throw new Exception($"Wrong type: want {nameof(Pv2UIntParamType)}. Got {param.GetType().Name}");
            src.Write(param, (type, v) => uintType.SetValue(v, value));
        }

        public static uint ReadUInt(this IPv2ClientParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2UIntParamType uintType)
                throw new Exception($"Wrong type: want {nameof(Pv2UIntParamType)}. Got {param.GetType().Name}");
            var item = src.Read(param);
            return uintType.GetValue(item);
        }

        public static async Task<uint> WriteUInt(this IPv2ClientParamsInterface src, Pv2ParamType param, uint value,
            CancellationToken cancel = default)
        {
            if (param is not Pv2UIntParamType uintType)
                throw new Exception($"Wrong type: want {nameof(Pv2UIntParamType)}. Got {param.GetType().Name}");
            var result = await src.Write(param, (_, val) => uintType.SetValue(val, value), cancel)
                .ConfigureAwait(false);
            return uintType.GetValue(result);
        }
    }
}
