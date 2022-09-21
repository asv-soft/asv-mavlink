using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink.Payload.Digits
{
    public class Pv2DoubleParamValue : Pv2ParamValue
    {
        public override Pv2ParamTypeEnum Type => Pv2ParamTypeEnum.Double;

        public double RawValue { get; set; }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WriteDouble(ref buffer, RawValue);
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            RawValue = BinSerialize.ReadDouble(ref buffer);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() + sizeof(double);
        }

        public override void CopyFrom(Pv2ParamValue data)
        {
            RawValue = Pv2ParamInterface.CheckValueTypeAndCast<Pv2DoubleParamValue>(data).RawValue;
        }

        public override string ToString()
        {
            return $"[{Index}] {Type:G} RAW:{RawValue}";
        }
    }

    public class Pv2DoubleParamType : Pv2DigitParamType<Pv2DoubleParamValue, double>
    {
        public Pv2DoubleParamType()
        {
        }

        public Pv2DoubleParamType(string paramName, string description, string groupName, string formatString,
            string units, double min = double.MinValue, double max = double.MaxValue, double defaultValue = 0,
            Pv2ParamFlags flags = Pv2ParamFlags.NoFlags) : base(paramName, description, groupName, formatString, units,
            min, max, defaultValue, flags)
        {
            ValidateSize();
        }

        public override Pv2ParamTypeEnum TypeEnum => Pv2ParamTypeEnum.Double;

        protected override double DeserializeValue(ref ReadOnlySpan<byte> buffer)
        {
            return BinSerialize.ReadDouble(ref buffer);
        }

        protected override void SerializeValue(ref Span<byte> buffer, double value)
        {
            BinSerialize.WriteDouble(ref buffer, value);
        }

        protected override int GetValueSize(double min)
        {
            return sizeof(double);
        }

        protected override void InternalSetValue(Pv2DoubleParamValue paramValue, double value)
        {
            paramValue.RawValue = value;
        }

        protected override double InternalGetValue(Pv2DoubleParamValue paramValue)
        {
            return paramValue.RawValue;
        }
    }

    public static class Pv2DoubleParamTypeHelper
    {
        public static IObservable<double> FilterDouble(this IObservable<Pv2ParamValueAndTypePair> src,
            Pv2DoubleParamType type)
        {
            return src.Filter<Pv2DoubleParamType, Pv2DoubleParamValue, double>(type);
        }

        public static double ReadDouble(this IPv2ServerParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2DoubleParamType uintType)
                throw new Exception($"Wrong type: want {nameof(Pv2DoubleParamType)}. Got {param.GetType().Name}");
            var paramResult = src.Read(param);
            return uintType.GetValue(paramResult);
        }

        public static void WriteDouble(this IPv2ServerParamsInterface src, Pv2ParamType param, double value)
        {
            if (param is not Pv2DoubleParamType uintType)
                throw new Exception($"Wrong type: want {nameof(Pv2DoubleParamType)}. Got {param.GetType().Name}");
            src.Write(param, (_, v) => uintType.SetValue(v, value));
        }

        public static double ReadDouble(this IPv2ClientParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2DoubleParamType uintType)
                throw new Exception($"Wrong type: want {nameof(Pv2DoubleParamType)}. Got {param.GetType().Name}");
            var item = src.Read(param);
            return uintType.GetValue(item);
        }

        public static async Task<double> WriteDouble(this IPv2ClientParamsInterface src, Pv2ParamType param,
            double value, CancellationToken cancel = default)
        {
            if (param is not Pv2DoubleParamType uintType)
                throw new Exception($"Wrong type: want {nameof(Pv2DoubleParamType)}. Got {param.GetType().Name}");
            var result = await src.Write(param, (_, val) => uintType.SetValue(val, value), cancel)
                .ConfigureAwait(false);
            return uintType.GetValue(result);
        }
    }
}
