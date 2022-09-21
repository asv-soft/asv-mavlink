using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink.Payload.Digits
{
    public class Pv2IntParamValue : Pv2ParamValue
    {
        private int _rawValue;
        public override Pv2ParamTypeEnum Type => Pv2ParamTypeEnum.Int;

        public int RawValue
        {
            get => _rawValue;
            set => Interlocked.Exchange(ref _rawValue, value);
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WritePackedInteger(ref buffer, RawValue);
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            RawValue = BinSerialize.ReadPackedInteger(ref buffer);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() + BinSerialize.GetSizeForPackedInteger(RawValue);
        }

        public override void CopyFrom(Pv2ParamValue data)
        {
            RawValue = Pv2ParamInterface.CheckValueTypeAndCast<Pv2IntParamValue>(data).RawValue;
        }

        public override string ToString()
        {
            return $"[{Index}] {Type:G} RAW:{RawValue}";
        }
    }

    public class Pv2IntParamType : Pv2DigitParamType<Pv2IntParamValue, int>
    {
        public Pv2IntParamType()
        {
        }

        public Pv2IntParamType(string paramName, string description, string groupName, string formatString,
            string units, int min = int.MinValue, int max = int.MaxValue, int defaultValue = 0,
            Pv2ParamFlags flags = Pv2ParamFlags.NoFlags) : base(paramName, description, groupName, formatString, units,
            min, max, defaultValue, flags)
        {
            ValidateSize();
        }

        public override Pv2ParamTypeEnum TypeEnum => Pv2ParamTypeEnum.Int;


        protected override int DeserializeValue(ref ReadOnlySpan<byte> buffer)
        {
            return BinSerialize.ReadPackedInteger(ref buffer);
        }

        protected override void SerializeValue(ref Span<byte> buffer, int value)
        {
            BinSerialize.WritePackedInteger(ref buffer, value);
        }

        protected override int GetValueSize(int value)
        {
            return BinSerialize.GetSizeForPackedInteger(value);
        }

        protected override void InternalSetValue(Pv2IntParamValue paramValue, int value)
        {
            var delta = Max / 2 + Min / 2;
            paramValue.RawValue = value - delta;
        }

        protected override int InternalGetValue(Pv2IntParamValue paramValue)
        {
            var delta = Max / 2 + Min / 2;
            return paramValue.RawValue + delta;
        }
    }

    public static class Pv2IntParamTypeHelper
    {
        public static IObservable<int> FilterInt(this IObservable<Pv2ParamValueAndTypePair> src,
            Pv2IntParamType type)
        {
            return src.Filter<Pv2IntParamType, Pv2IntParamValue, int>(type);
        }

        public static int ReadInt(this IPv2ServerParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2IntParamType intType)
                throw new Exception($"Wrong type: want {nameof(Pv2IntParamType)}. Got {param.GetType().Name}");
            var paramResult = src.Read(param);
            return intType.GetValue(paramResult);
        }

        public static void WriteInt(this IPv2ServerParamsInterface src, Pv2ParamType param, int value)
        {
            if (param is not Pv2IntParamType intType)
                throw new Exception($"Wrong type: want {nameof(Pv2IntParamType)}. Got {param.GetType().Name}");
            src.Write(param, (_, v) => intType.SetValue(v, value));
        }

        public static int ReadInt(this IPv2ClientParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2IntParamType intType)
                throw new Exception($"Wrong type: want {nameof(Pv2UIntParamType)}. Got {param.GetType().Name}");
            var item = src.Read(param);
            return intType.GetValue(item);
        }

        public static async Task<int> WriteInt(this IPv2ClientParamsInterface src, Pv2ParamType param, int value,
            CancellationToken cancel = default)
        {
            if (param is not Pv2IntParamType intType)
                throw new Exception($"Wrong type: want {nameof(Pv2UIntParamType)}. Got {param.GetType().Name}");
            var result = await src.Write(param, (_, val) => intType.SetValue(val, value), cancel).ConfigureAwait(false);
            return intType.GetValue(result);
        }
    }
}
