using System;
using System.Collections.Generic;
using System.Linq;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Payload.Digits;

namespace Asv.Mavlink.Payload
{
    public static class Pv2RttInterface
    {
        public const string InterfaceName = "RTT";
        public const ushort InterfaceId = 3;

        public static readonly int MaxOnStreamDataSize =
            PayloadV2Helper.MaxMessageSize - Pv2RttStreamMetadata.MaxMetadataSizeWithOneGroup;

        

        public static readonly Pv2BoolParamType SendEnabled =
            new(nameof(SendEnabled), "Enabled or disabled sending rtt to gcs", InterfaceName, true, "Enabled",
                "Disabled", Pv2ParamFlags.ForAdvancedUsers);

        public static readonly Pv2UIntParamType SendTickTime =
            new(nameof(SendTickTime), "Sending tick time to GCS", InterfaceName, "{0}", "ms", 0, 30000, 1000,
                Pv2ParamFlags.ForAdvancedUsers);

        public static readonly Pv2UIntParamType RecordTickTime =
            new(nameof(RecordTickTime), "Global tick time (atom) for telemetry requester", InterfaceName, "{0}", "ms",
                0, 10000, 1000, Pv2ParamFlags.ForAdvancedUsers);


        public static MethodInfo<Pv2RttStreamData> OnStream = new(nameof(OnStream), 0, InterfaceName, InterfaceId);

        public static MethodInfo<SpanVoidType, Pv2RttStatus>
            Status = new(nameof(Status), 1, InterfaceName, InterfaceId);

        public static MethodInfo<SpanPacketUnsignedIntegerType, Pv2RttRecordDesc> ReadRecordDesc =
            new(nameof(ReadRecordDesc), 2, InterfaceName, InterfaceId);

        public static MethodInfo<SpanPacketUnsignedIntegerType, Pv2RttFieldResult> ReadFieldDesc =
            new(nameof(ReadFieldDesc), 3, InterfaceName, InterfaceId);

        public static MethodInfo<SessionSettings, Pv2RttSessionMetadataType> StartSession =
            new(nameof(StartSession), 4, InterfaceName, InterfaceId);

        public static MethodInfo<SpanVoidType, SpanBoolType> StopSession = new(nameof(StopSession), 5, InterfaceName,
            InterfaceId);

        public static MethodInfo<SpanVoidType, Pv2RttSessionStoreInfo> GetSessionStoreInfo =
            new(nameof(GetSessionStoreInfo), 7, InterfaceName, InterfaceId);

        public static MethodInfo<SpanPacketUnsignedIntegerType, SessionInfo> GetSessionInfo =
            new(nameof(GetSessionInfo), 8, InterfaceName, InterfaceId);

        public static MethodInfo<Pv2RttGetFieldsArgs, SessionFieldInfo> GetSessionFieldInfo =
            new(nameof(GetSessionFieldInfo), 9, InterfaceName, InterfaceId);

        public static MethodInfo<Pv2RttGetFieldsDataArgs, Pv2RttGetFieldsDataResult> GetSessionFieldData =
            new(nameof(GetSessionFieldData), 10, InterfaceName, InterfaceId);

        public static MethodInfo<SessionId, SessionInfo> GetSessionInfoByGuid =
            new(nameof(GetSessionInfoByGuid), 11, InterfaceName, InterfaceId);

        public static MethodInfo<SessionId, SpanBoolType> DeleteSessionByGuid =  new(nameof(DeleteSessionByGuid), 12, InterfaceName, InterfaceId);


        public static IEnumerable<Pv2ParamType> Params
        {
            get
            {
                yield return RecordTickTime;
                yield return SendTickTime;
                yield return SendEnabled;
            }
        }


        public static Pv2RttFieldDesc CreateFieldByType(Pv2RttFieldType fieldType)
        {
            switch (fieldType)
            {
                case Pv2RttFieldType.FixedPoint:
                    return new Pv2RttFixedPointDesc();
                default:
                    return new Pv2RttFieldDesc(); // TODO: change to unknown
            }
        }

        #region Name validation

        public const int MaxDescriptionLength = 100;
        public const int MaxUnitsStringLength = 10;
        public const int MaxFormatStringLength = 12;

        

        public static void CheckAndSetDescription(ref string desc, string value)
        {
            CheckDescription(value);
            desc = value;
        }

        public static void CheckDescription(string value)
        {
            if (value.IsEmpty() || value.Length > MaxDescriptionLength)
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
            if (string.IsNullOrWhiteSpace(value)) return;
            if (value.Length > MaxUnitsStringLength)
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"Units '{value}' must be less then {MaxUnitsStringLength} symbols");
        }

        public static void CheckAndSetFormatString(ref string formatString, string value)
        {
            CheckFormatString(value);
            formatString = value;
        }

        public static void CheckFormatString(string value)
        {
            if (value.IsEmpty() || value.Length > MaxFormatStringLength)
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"Format string '{value}' must be less then {MaxFormatStringLength} symbols");
        }

        #endregion

        #region Hash calculation

        public static uint CalculateHash(IEnumerable<Pv2RttFieldDesc> fields, IEnumerable<Pv2RttRecordDesc> records)
        {
            return records.OrderBy(_ => _.Id).CalculateCrc32QHash(fields.OrderBy(_ => _.FullId).CalculateCrc32QHash());
        }


        #endregion
    }
}
