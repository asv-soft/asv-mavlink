using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink.Payload.Digits
{
    public class Pv2FloatParamValue : Pv2ParamValue
    {
        public override Pv2ParamTypeEnum Type => Pv2ParamTypeEnum.Float;

        public float RawValue { get; set; }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WriteFloat(ref buffer, RawValue);
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            RawValue = BinSerialize.ReadFloat(ref buffer);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() + sizeof(float);
        }

        public override void CopyFrom(Pv2ParamValue data)
        {
            RawValue = Pv2ParamInterface.CheckValueTypeAndCast<Pv2FloatParamValue>(data).RawValue;
        }

        public override string ToString()
        {
            return $"{Index} {Type:G} RAW:{RawValue}";
        }
    }

    public class Pv2FloatParamType : Pv2DigitParamType<Pv2FloatParamValue, float>
    {
        public Pv2FloatParamType()
        {
        }

        public Pv2FloatParamType(string paramName, string description, string groupName, string formatString,
            string units, float min = float.MinValue, float max = float.MaxValue, float defaultValue = 0,
            Pv2ParamFlags flags = Pv2ParamFlags.NoFlags) : base(paramName, description, groupName, formatString, units,
            min, max, defaultValue, flags)
        {
            ValidateSize();
        }

        public override Pv2ParamTypeEnum TypeEnum => Pv2ParamTypeEnum.Float;

        protected override float DeserializeValue(ref ReadOnlySpan<byte> buffer)
        {
            return BinSerialize.ReadFloat(ref buffer);
        }

        protected override void SerializeValue(ref Span<byte> buffer, float value)
        {
            BinSerialize.WriteFloat(ref buffer, value);
        }

        protected override int GetValueSize(float value)
        {
            return sizeof(float);
        }

        protected override void InternalSetValue(Pv2FloatParamValue paramValue, float value)
        {
            paramValue.RawValue = value;
        }

        protected override float InternalGetValue(Pv2FloatParamValue paramValue)
        {
            return paramValue.RawValue;
        }
    }

    public static class Pv2FloatParamTypeHelper
    {
        public static IObservable<float> FilterFloat(this IObservable<Pv2ParamValueAndTypePair> src,
            Pv2FloatParamType type)
        {
            return src.Filter<Pv2FloatParamType, Pv2FloatParamValue, float>(type);
        }

        public static float ReadFloat(this IPv2ServerParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2FloatParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2FloatParamType)}. Got {param.GetType().Name}");
            var paramResult = src.Read(param);
            return type.GetValue(paramResult);
        }

        public static void WriteFloat(this IPv2ServerParamsInterface src, Pv2ParamType param, float value)
        {
            if (param is not Pv2FloatParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2FloatParamType)}. Got {param.GetType().Name}");
            src.Write(param, (_, v) => type.SetValue(v, value));
        }

        public static float ReadFloat(this IPv2ClientParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2FloatParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2FloatParamType)}. Got {param.GetType().Name}");
            var item = src.Read(param);
            return type.GetValue(item);
        }

        public static async Task<float> WriteFloat(this IPv2ClientParamsInterface src, Pv2ParamType param, float value,
            CancellationToken cancel = default)
        {
            if (param is not Pv2FloatParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2FloatParamType)}. Got {param.GetType().Name}");
            var result = await src.Write(param, (_, val) => type.SetValue(val, value), cancel).ConfigureAwait(false);
            return type.GetValue(result);
        }
    }
}
