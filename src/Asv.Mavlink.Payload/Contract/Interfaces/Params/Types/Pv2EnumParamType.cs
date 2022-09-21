using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2EnumParamValue : Pv2ParamValue
    {
        public override Pv2ParamTypeEnum Type => Pv2ParamTypeEnum.Enum;

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
            RawValue = Pv2ParamInterface.CheckValueTypeAndCast<Pv2EnumParamValue>(data).RawValue;
        }

        public override string ToString()
        {
            return $"[{Index}] {Type:G} RAW:{RawValue}";
        }
    }

    public class Pv2EnumParamType : Pv2ParamType<string, Pv2EnumParamValue>
    {
        private readonly uint _defaultIndex;
        private List<string> _list;

        public Pv2EnumParamType()
        {
        }

        public Pv2EnumParamType(string paramName, string description, string groupName, string defaultValue,
            Pv2ParamFlags flags = Pv2ParamFlags.NoFlags, params string[] items) : base(paramName, description,
            groupName, defaultValue, flags)
        {
            foreach (var item in items.GroupBy(_ => _))
                if (item.Count() > 1)
                    throw new Exception($"Enum items '{item.Key}' not unique");

            if (items.Any(_ => string.IsNullOrWhiteSpace(_)))
                throw new Exception("Enum items contain null or white space item");

            if (items.Any(_ => _ == DefaultValue) == false)
                throw new Exception($"Items doesn't contain default value '{defaultValue}'");

            _list = new List<string>(items.Length);
            for (uint i = 0; i < items.Length; i++)
            {
                if (DefaultValue == items[i])
                    _defaultIndex = i;
                _list.Add(items[i]);
            }

            ValidateSize();
        }

        public IReadOnlyList<string> Items => _list;

        public override Pv2ParamTypeEnum TypeEnum => Pv2ParamTypeEnum.Enum;

        protected override void InternalSetValue(Pv2EnumParamValue paramValue, string value)
        {
            var index = _list.IndexOf(value);
            if (index < 0) throw new Exception($"Element of {value} not found in enum {this}");
            paramValue.RawValue = (uint)index;
        }

        protected override string InternalGetValue(Pv2EnumParamValue paramValue)
        {
            var index = paramValue.RawValue;
            return index >= _list.Count ? null : _list[(int)index];
        }

        public override bool IsValidValue(string value)
        {
            return _list.IndexOf(value) >= 0;
        }

        public override string GetValidationError(string value)
        {
            return $"Element of {value} not found in enum {this}";
        }

        public override string ConvertToString(Pv2ParamValue value)
        {
            return GetValue(value);
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)_list.Count);
            foreach (var t in Items) BinSerialize.WriteString(ref buffer, t);
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
            var count = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            _list = new List<string>((int)count);
            for (var i = 0; i < count; i++) _list.Add(BinSerialize.ReadString(ref buffer));
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() +
                   BinSerialize.GetSizeForPackedUnsignedInteger((uint)_list.Count) +
                   _list.Sum(BinSerialize.GetSizeForString);
        }
    }

    public static class Pv2EnumParamTypeHelper
    {
        public static IObservable<string> FilterEnum(this IObservable<Pv2ParamValueAndTypePair> src,
            Pv2EnumParamType type)
        {
            return src.Filter<Pv2EnumParamType, Pv2EnumParamValue, string>(type);
        }

        public static string ReadEnum(this IPv2ServerParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2EnumParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2EnumParamType)}. Got {param.GetType().Name}");
            var paramResult = src.Read(param);
            return type.GetValue(paramResult);
        }

        public static void WriteEnum(this IPv2ServerParamsInterface src, Pv2ParamType param, string value)
        {
            if (param is not Pv2EnumParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2EnumParamType)}. Got {param.GetType().Name}");
            src.Write(param, (_, v) => type.SetValue(v, value));
        }

        public static string ReadEnum(this IPv2ClientParamsInterface src, Pv2ParamType param)
        {
            if (param is not Pv2EnumParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2EnumParamType)}. Got {param.GetType().Name}");
            var item = src.Read(type);
            return type.GetValue(item);
        }

        public static async Task<string> WriteEnum(this IPv2ClientParamsInterface src, Pv2ParamType param, string value,
            CancellationToken cancel = default)
        {
            if (param is not Pv2EnumParamType type)
                throw new Exception($"Wrong type: want {nameof(Pv2EnumParamType)}. Got {param.GetType().Name}");
            var result = await src.Write(param, (_, val) => type.SetValue(val, value), cancel).ConfigureAwait(false);
            return type.GetValue(result);
        }
    }
}
