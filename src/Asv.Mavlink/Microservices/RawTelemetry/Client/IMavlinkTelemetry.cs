using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IMavlinkTelemetry
    {
        IRxValue<RadioStatusPayload> RawRadioStatus { get; }
        IRxValue<SysStatusPayload> RawSysStatus { get; }
        IRxValue<GpsRawIntPayload> RawGpsRawInt { get; }
        IRxValue<Gps2RawPayload> RawGps2Raw { get; }
        IRxValue<HighresImuPayload> RawHighresImu { get; }
        IRxValue<ExtendedSysStatePayload> RawExtendedSysState { get; }
        IRxValue<AltitudePayload> RawAltitude { get; }
        IRxValue<BatteryStatusPayload> RawBatteryStatus { get; }
        IRxValue<AttitudePayload> RawAttitude { get; }
        IRxValue<VfrHudPayload> RawVfrHud { get; }
        IRxValue<HomePositionPayload> RawHome { get; }
        IRxValue<StatustextPayload> RawStatusText { get; }
        IRxValue<GlobalPositionIntPayload> RawGlobalPositionInt { get; }
        IRxValue<PositionTargetGlobalIntPayload> RawPositionTargetGlobalInt { get; }



    }
}
