using System;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    

    public class MavlinkTelemetry : MavlinkMicroserviceClient, IMavlinkTelemetry
    {
        private readonly RxValue<SysStatusPayload> _sysStatus = new();
        private readonly RxValue<GpsRawIntPayload> _gpsRawInt = new();
        private readonly RxValue<Gps2RawPayload> _gps2Raw = new();
        private readonly RxValue<HighresImuPayload> _highresImu = new();
        private readonly RxValue<VfrHudPayload> _vfrHud = new();
        private readonly RxValue<AttitudePayload> _attitude = new();
        private readonly RxValue<BatteryStatusPayload> _batteryStatus = new();
        private readonly RxValue<AltitudePayload> _altitude = new();
        private readonly RxValue<ExtendedSysStatePayload> _extendedSysState = new();
        private readonly RxValue<HomePositionPayload> _home = new();
        private readonly RxValue<StatustextPayload> _statusText = new();
        private readonly RxValue<GlobalPositionIntPayload> _globalPositionInt = new();
        private readonly RxValue<PositionTargetGlobalIntPayload> _positionTargetGlobalInt = new();
        private readonly RxValue<RadioStatusPayload> _radioStatus = new();

        public MavlinkTelemetry(IMavlinkV2Connection connection, MavlinkClientIdentity config,
            IPacketSequenceCalculator seq):base(connection,config,seq,"RTT")
        {
            HandleSystemStatus();
            HandleGps();
            HandleHighresImu();
            HandleVfrHud();
            HandleAttitude();
            HandleBatteryStatus();
            HandleAltitude();
            HandleExtendedSysState();
            HandleHome();
            HandleGlobalPositionInt();
            HandleRadioStatus();
        }

        

        public IRxValue<RadioStatusPayload> RawRadioStatus => _radioStatus;
        public IRxValue<SysStatusPayload> RawSysStatus => _sysStatus;
        public IRxValue<GpsRawIntPayload> RawGpsRawInt => _gpsRawInt;
        public IRxValue<Gps2RawPayload> RawGps2Raw => _gps2Raw;
        public IRxValue<HighresImuPayload> RawHighresImu => _highresImu;
        public IRxValue<ExtendedSysStatePayload> RawExtendedSysState => _extendedSysState;
        public IRxValue<AltitudePayload> RawAltitude => _altitude;
        public IRxValue<BatteryStatusPayload> RawBatteryStatus => _batteryStatus;
        public IRxValue<AttitudePayload> RawAttitude => _attitude;
        public IRxValue<VfrHudPayload> RawVfrHud => _vfrHud;
        public IRxValue<HomePositionPayload> RawHome => _home;
        public IRxValue<StatustextPayload> RawStatusText => _statusText;
        public IRxValue<GlobalPositionIntPayload> RawGlobalPositionInt => _globalPositionInt;
        public IRxValue<PositionTargetGlobalIntPayload> RawPositionTargetGlobalInt => _positionTargetGlobalInt;


        private void HandleRadioStatus()
        {
            Filter<RadioStatusPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_radioStatus).DisposeItWith(Disposable);
            Disposable.Add(_globalPositionInt);
        }

        private void HandleGlobalPositionInt()
        {
            Filter<PositionTargetGlobalIntPacket>().Select(_ => _.Payload)
                .Subscribe(_positionTargetGlobalInt).DisposeItWith(Disposable);

            Filter<GlobalPositionIntPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_globalPositionInt).DisposeItWith(Disposable);

            Disposable.Add(_positionTargetGlobalInt);
            Disposable.Add(_radioStatus);

        }

        private void HandleHome()
        {
            Filter<HomePositionPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_=>_home.OnNext(_)).DisposeItWith(Disposable);
           

            Disposable.Add(_home);
        }

        private void HandleExtendedSysState()
        {
            Filter<ExtendedSysStatePacket>()
                .Select(_ => _.Payload)
                .Subscribe(_extendedSysState).DisposeItWith(Disposable);
            Disposable.Add(_extendedSysState);
        }

        private void HandleAltitude()
        {
            Filter<AltitudePacket>()
                .Select(_ => _.Payload)
                .Subscribe(_altitude).DisposeItWith(Disposable);
            Disposable.Add(_altitude);
        }

        private void HandleBatteryStatus()
        {
            Filter<BatteryStatusPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_batteryStatus).DisposeItWith(Disposable);
            Disposable.Add(_batteryStatus);
        }

        private void HandleAttitude()
        {
            Filter<AttitudePacket>()
                .Select(_ => _.Payload)
                .Subscribe(_attitude).DisposeItWith(Disposable);
            Disposable.Add(_attitude);
        }

        private void HandleVfrHud()
        {
            Filter<VfrHudPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_vfrHud).DisposeItWith(Disposable);
            Disposable.Add(_vfrHud);
        }

        private void HandleHighresImu()
        {
            Filter<HighresImuPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_highresImu).DisposeItWith(Disposable);
            Disposable.Add(_highresImu);
        }

        private void HandleGps()
        {
            Filter<GpsRawIntPacket>()
                .Select(_ => _.Payload).Subscribe(_gpsRawInt).DisposeItWith(Disposable);
            Disposable.Add(_gpsRawInt);

            Filter<Gps2RawPacket>()
                .Select(_ => _.Payload).Subscribe(_gps2Raw).DisposeItWith(Disposable);
            Disposable.Add(_gps2Raw);
        }

        private void HandleSystemStatus()
        {
            Filter<SysStatusPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_sysStatus).DisposeItWith(Disposable);
            Filter<StatustextPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_statusText).DisposeItWith(Disposable);

            Disposable.Add(_sysStatus);
            Disposable.Add(_statusText);
        }
    }
}
