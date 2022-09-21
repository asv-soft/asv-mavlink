using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2FlagsParamValue : Pv2ParamValue
    {
        public override Pv2ParamTypeEnum Type => Pv2ParamTypeEnum.Flags;

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
            RawValue = Pv2ParamInterface.CheckValueTypeAndCast<Pv2FlagsParamValue>(data).RawValue;
        }

        public override string ToString()
        {
            return $"{Index} {Type:G} RAW:{Convert.ToString(RawValue, 2).PadLeft(8, '0')}";
        }
    }

    public class Pv2FlagsParamType : Pv2ParamType<Asv.Common.UintBitArray, Pv2FlagsParamValue>
    {
        private string _falseString;

        private string _trueString;

        public Pv2FlagsParamType()
        {
        }

        public Pv2FlagsParamType(string paramName, string description, string groupName, string trueTitle = "On",
            string falseTitle = "Off", Pv2ParamFlags flags = Pv2ParamFlags.NoFlags, params (string, bool)[] items) :
            base(paramName, description, groupName, new Asv.Common.UintBitArray(0, items.Length), flags)
        {
            TrueTitle = trueTitle;
            FalseTitle = falseTitle;
            if (items.Length > 32)
                throw new Exception($"Max flags is more then max size: {items.Length} > {sizeof(uint)}");

            foreach (var item in items.GroupBy(_ => _.Item1))
                if (item.Count() > 1)
                    throw new Exception($"Enum items '{item.Key}' not unique");

            if (items.Any(_ => _.Item1.IsNullOrWhiteSpace()))
                throw new Exception("Enum items contain null or white space item");

            foreach (var item in items)
                if (ChunkStoreHelper.NameRegex.IsMatch(item.Item1) == false)
                    throw new Exception(
                        $"Flag item of {this} name error: '{item}' not match regex '{ChunkStoreHelper.NameRegex}'");

            DefaultValue = new Asv.Common.UintBitArray(items.Select(_ => _.Item2));
            Items = items.Select(_ => _.Item1).ToList();

            ValidateSize();
        }

        public override Pv2ParamTypeEnum TypeEnum => Pv2ParamTypeEnum.Flags;

        public IReadOnlyList<string> Items { get; private set; }

        public string TrueTitle
        {
            get => _trueString;
            private set => Pv2ParamInterface.CheckAndSetUnits(ref _trueString, value);
        }

        public string FalseTitle
        {
            get => _falseString;
            private set => Pv2ParamInterface.CheckAndSetUnits(ref _falseString, value);
        }

        protected override void InternalReadFromConfig(IConfiguration config, string configSuffix, Pv2ParamValue value)
        {
            var result = new UintBitArray(0, Items.Count);
            for (var i = 0; i < Items.Count; i++)
            {
                var bit = config.Get(string.Concat(configSuffix, "_", FullName, "_", Items[i]), DefaultValue[i]);
                result[i] = bit;
            }

            SetValue(value, result);
        }

        protected override void InternalWriteToConfig(IConfiguration config, string configSuffix, Pv2ParamValue value)
        {
            var values = GetValue(value);
            for (var i = 0; i < Items.Count; i++)
                config.Set(string.Concat(configSuffix, "_", FullName, "_", Items[i]), values[i]);
        }

        protected override void InternalSetValue(Pv2FlagsParamValue paramValue, UintBitArray value)
        {
            paramValue.RawValue = value.Value;
        }

        protected override UintBitArray InternalGetValue(Pv2FlagsParamValue paramValue)
        {
            return new UintBitArray(paramValue.RawValue, Items.Count);
        }

        public override bool IsValidValue(UintBitArray value)
        {
            return true;
        }

        public override string GetValidationError(UintBitArray value)
        {
            return null;
        }

        public override string ConvertToString(Pv2ParamValue value)
        {
            var flags = GetValue(value);
            var sb = new StringBuilder();
            for (var i = 0; i < flags.Size; i++)
                if (flags[i])
                    sb.Append(Items[i]).Append(",");
            return sb.ToString();
        }


        protected override Asv.Common.UintBitArray DeserializeValue(ref ReadOnlySpan<byte> buffer)
        {
            return new Asv.Common.UintBitArray(BinSerialize.ReadPackedUnsignedInteger(ref buffer), Items.Count);
        }

        protected override void SerializeValue(ref Span<byte> buffer, Asv.Common.UintBitArray value)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, value.Value);
        }

        protected override int GetValueSize(Asv.Common.UintBitArray value)
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(value.Value);
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteString(ref buffer, TrueTitle);
            BinSerialize.WriteString(ref buffer, FalseTitle);
            BinSerialize.WriteByte(ref buffer, (byte)Items.Count);
            foreach (var t in Items) BinSerialize.WriteString(ref buffer, t);
            base.Serialize(ref buffer); // <----- !!! must be at the end
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            TrueTitle = BinSerialize.ReadString(ref buffer);
            FalseTitle = BinSerialize.ReadString(ref buffer);
            var count = BinSerialize.ReadByte(ref buffer);
            var list = new List<string>();
            for (var i = 0; i < count; i++) list.Add(BinSerialize.ReadString(ref buffer));
            Items = list;
            base.Deserialize(ref buffer); // <----- !!! must be at the end
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() +
                   BinSerialize.GetSizeForString(TrueTitle) +
                   BinSerialize.GetSizeForString(FalseTitle) +
                   1 + /*count*/
                   Items.Sum(BinSerialize.GetSizeForString);
        }
    }

    public static class Pv2FlagsParamTypeHelper
    {
        public static IObservable<UintBitArray> FilterFlags(this IObservable<Pv2ParamValueAndTypePair> src,
            Pv2FlagsParamType type)
        {
            return src.Filter<Pv2FlagsParamType, Pv2FlagsParamValue, Asv.Common.UintBitArray>(type);
        }

        public static Asv.Common.UintBitArray ReadFlagValue(this IPv2ServerParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2FlagsParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2FlagsParamType)}. Got {param.GetType().Name}");
            var paramResult = src.Read(param);
            return type.GetValue(paramResult);
        }

        public static void WriteFlagValue(this IPv2ServerParamsInterface src, Pv2ParamType param, Asv.Common.UintBitArray value)
        {
            if (param is not Pv2FlagsParamType type)  
                throw new Exception($"Wrong type: want {nameof(Pv2FlagsParamType)}. Got {param.GetType().Name}");
            src.Write(param, (_, v) => type.SetValue(v, value));
        }

        public static Asv.Common.UintBitArray ReadFlagValue(this IPv2ClientParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2FlagsParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2FlagsParamType)}. Got {param.GetType().Name}");
            var item = src.Read(param);
            return type.GetValue(item);
        }

        public static async Task<Asv.Common.UintBitArray> WriteFlagValue(this IPv2ClientParamsInterface src, Pv2ParamType param,
            Asv.Common.UintBitArray value, CancellationToken cancel = default)
        {
            if (param is not Pv2FlagsParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2FlagsParamType)}. Got {param.GetType().Name}");
            var result = await src.Write(param, (_, val) => type.SetValue(val, value), cancel).ConfigureAwait(false);
            return type.GetValue(result);
        }
    }
}
