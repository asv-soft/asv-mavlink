using System;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Payload.Digits;

namespace Asv.Mavlink.Payload
{
    public static class Pv2ParamInterface
    {
        public const int MaxSendDataHeaderSize = 10;
        public const int MaxValidByteSize = PayloadV2Helper.MaxMessageSize - MaxSendDataHeaderSize;

        public const string InterfaceName = "CFG";
        public const ushort InterfaceId = 1;

        

        public static MethodInfo<SpanVoidType, Pv2ParamsStatus> Status = new(nameof(Status), 0, InterfaceName,
            InterfaceId);

        public static MethodInfo<SpanPacketUnsignedIntegerType, Pv2ParamValueAndTypePair> ReadType =
            new(nameof(ReadType), 1, InterfaceName, InterfaceId);

        public static MethodInfo<SpanPacketUnsignedIntegerType, Pv2ParamValueItem> ReadValue =
            new(nameof(ReadValue), 2, InterfaceName, InterfaceId);

        public static MethodInfo<SpanVoidType, Pv2ParamValueList> UpdateEvent = new(nameof(UpdateEvent), 3,
            InterfaceName, InterfaceId);

        public static MethodInfo<Pv2ParamValueItem, Pv2ParamValueItem> Write = new(nameof(Write), 4, InterfaceName,
            InterfaceId);

        public static Pv2ParamValue CreateValue(Pv2ParamTypeEnum type)
        {
            return type switch
            {
                Pv2ParamTypeEnum.Int => new Pv2IntParamValue(),
                Pv2ParamTypeEnum.UInt => new Pv2UIntParamValue(),
                Pv2ParamTypeEnum.Double => new Pv2DoubleParamValue(),
                Pv2ParamTypeEnum.Float => new Pv2FloatParamValue(),
                Pv2ParamTypeEnum.String => new Pv2StringParamValue(),
                Pv2ParamTypeEnum.Bool => new Pv2BoolParamValue(),
                Pv2ParamTypeEnum.Enum => new Pv2EnumParamValue(),
                Pv2ParamTypeEnum.Flags => new Pv2FlagsParamValue(),
                Pv2ParamTypeEnum.Unknown => new Pv2ParamValueUnknown(),
                _ => new Pv2ParamValueUnknown()
            };
        }

        public static Pv2ParamType CreateType(Pv2ParamTypeEnum type)
        {
            return type switch
            {
                Pv2ParamTypeEnum.Int => new Pv2IntParamType(),
                Pv2ParamTypeEnum.UInt => new Pv2UIntParamType(),
                Pv2ParamTypeEnum.Double => new Pv2DoubleParamType(),
                Pv2ParamTypeEnum.Float => new Pv2FloatParamType(),
                Pv2ParamTypeEnum.String => new Pv2StringParamType(),
                Pv2ParamTypeEnum.Bool => new Pv2BoolParamType(),
                Pv2ParamTypeEnum.Enum => new Pv2EnumParamType(),
                Pv2ParamTypeEnum.Flags => new Pv2FlagsParamType(),
                Pv2ParamTypeEnum.Unknown => new Pv2ParamTypeUnknown("Unknown", "Unknown type", "Unknown"),
                _ => new Pv2ParamTypeUnknown("Unknown", "Unknown type", "Unknown")
            };
        }

        public static T CheckValueTypeAndCast<T>(Pv2ParamValue value)
            where T : Pv2ParamValue
        {
            if (value is not T result)
                throw new ArgumentException(nameof(value),
                    $"Unexpected param value type: want {typeof(T).Name}. Got {value.GetType().Name}");
            return result;
        }


        #region Name validation

        

        public const int MaxDescriptionLength = 100;
        public const int MaxUnitsStringLength = 10;
        public const int MaxFormatStringLength = 10;


        public static void CheckAndSetName(ref string name, string value)
        {
            CheckName(value);
            name = value;
        }

        public static void CheckName(string name)
        {
            if (ChunkStoreHelper.NameRegex.IsMatch(name) == false)
                throw new ArgumentException(
                    $"Param name '{name}' not match regex '{ChunkStoreHelper.NameRegexString}')");
        }

        public static void CheckAndSetDescription(ref string desc, string value)
        {
            CheckDescription(value);
            desc = value;
        }

        public static void CheckDescription(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            if (value.Length > MaxDescriptionLength)
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"Description '{value}' must be less then {MaxDescriptionLength} symbols");
        }

        public static void CheckAndSetUnits(ref string units, string value)
        {
            CheckUnits(value);
            units = value;
        }

        public static void CheckUnits(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            if (value.Length > MaxUnitsStringLength)
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"Units '{value}' must be less then {MaxUnitsStringLength} symbols");
        }

        public static void CheckAndSetFormatString(ref string units, string value)
        {
            CheckFormatString(value);
            units = value;
        }

        public static void CheckFormatString(string value)
        {
            if (value.IsEmpty() || value.Length > MaxFormatStringLength)
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"Format string '{value}' must be less then {MaxFormatStringLength} symbols");
        }

        #endregion
    }
}
