using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.Vehicle;
using Geodesy;
using NLog;

namespace Asv.Mavlink
{

    public static class CopterModeHelper
    {
        public static string GetDisplayName(this CopterMode customMode)
        {
            switch (customMode)
            {
                case CopterMode.CopterModeStabilize:
                    return "Stabilize";
                case CopterMode.CopterModeAcro:
                    return "Acro";
                case CopterMode.CopterModeAltHold:
                    return "AltHold";
                case CopterMode.CopterModeAuto:
                    return "Auto";
                case CopterMode.CopterModeGuided:
                    return "Guided";
                case CopterMode.CopterModeLoiter:
                    return "Loiter";
                case CopterMode.CopterModeRtl:
                    return "RTL";
                case CopterMode.CopterModeCircle:
                    return "Circle";
                case CopterMode.CopterModeLand:
                    return "Land";
                case CopterMode.CopterModeDrift:
                    return "Drift";
                case CopterMode.CopterModeSport:
                    return "Sport";
                case CopterMode.CopterModeFlip:
                    return "Flip";
                case CopterMode.CopterModeAutotune:
                    return "Autotune";
                case CopterMode.CopterModePoshold:
                    return "Position hold";
                case CopterMode.CopterModeBrake:
                    return "Brake";
                case CopterMode.CopterModeThrow:
                    return "Throw";
                case CopterMode.CopterModeAvoidAdsb:
                    return "Avoid ADSB";
                case CopterMode.CopterModeGuidedNogps:
                    return "Guided no GPS";
                case CopterMode.CopterModeSmartRtl:
                    return "Smart RTL";
                default:
                    return customMode.ToString("G").Replace("CopterMode", string.Empty);
            }
        }
    }

    public class VehicleArdupilotCopter : VehicleArdupilot
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public VehicleArdupilotCopter(IMavlinkClient mavlink, VehicleBaseConfig config, bool disposeClient = true) : base(mavlink, config,disposeClient)
        {
        }

        protected override VehicleClass InterpretVehicleClass(HeartbeatPayload heartbeatPacket)
        {
            return VehicleClass.Copter;
        }

        #region Modes

        public override IEnumerable<VehicleCustomMode> AvailableModes => GetModes();
        private VehicleCustomMode[] _modes;
        

        private IEnumerable<VehicleCustomMode> GetModes()
        {
            return _modes ??= Enum.GetValues(typeof(CopterMode)).Cast<CopterMode>()
                .Select(_ => new VehicleCustomMode {Name = _.GetDisplayName(), Value = (uint) _}).ToArray();
        }

        protected override VehicleMode Interpret(HeartbeatPayload heartbeat)
        {
            var customMode = (CopterMode) (heartbeat.CustomMode);
            return new VehicleMode
            {
                BaseMode = heartbeat.BaseMode,
                CustomMode = new VehicleCustomMode
                {
                    Name = customMode.GetDisplayName(),
                    Value = (uint) customMode
                }
            };
        }

      

        #endregion

        #region GoTo

        public Task GoToRel(GlobalPosition location, CancellationToken cancel, double? yawDeg = null)
        {
            return GoTo(location, MavFrame.MavFrameGlobalRelativeAltInt, cancel, yawDeg);
        }

        private async Task GoTo(GlobalPosition location, MavFrame frame, CancellationToken cancel, double? yawDeg = null, double? vx = null, double? vy = null, double? vz = null)
        {
            await EnsureInGuidedMode(cancel).ConfigureAwait(false);
            

            var yaw = yawDeg.HasValue ? (float?)GeoMath.DegreesToRadians(yawDeg.Value) : null;
            await Mavlink.Common.SetPositionTargetGlobalInt(0, frame, cancel, (int)(location.Latitude.Degrees * 10000000), (int)(location.Longitude.Degrees * 10000000), (float)location.Elevation, yaw: yaw).ConfigureAwait(false);

        }

        protected override Task InternalGoToGlob(GlobalPosition location, CancellationToken cancel, double? yawDeg)
        {
            return GoTo(location, MavFrame.MavFrameGlobalInt, cancel, yawDeg);
        }

        #endregion

        protected override Task<bool> CheckGuidedMode(CancellationToken cancel)
        {
            return Task.FromResult(
                Mavlink.Heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
                Mavlink.Heartbeat.RawHeartbeat.Value.CustomMode == (int)CopterMode.CopterModeGuided);
        }

        protected override async Task EnsureInGuidedMode(CancellationToken cancel)
        {
            if (!await CheckGuidedMode(cancel).ConfigureAwait(false))
            {
                await Mavlink.Common.SetMode(1, (int)CopterMode.CopterModeGuided, cancel).ConfigureAwait(false);
            }
        }

        public override async Task FlyByLineGlob(GlobalPosition start, GlobalPosition stop, double precisionMet, CancellationToken cancel, Action firstPointComplete = null)
        {
            await GoToGlobAndWait(start, CallbackProgress<double>.Default, precisionMet, cancel).ConfigureAwait(false);
            firstPointComplete?.Invoke();
            await GoToGlobAndWait(stop, CallbackProgress<double>.Default, precisionMet, cancel).ConfigureAwait(false);
        }

        public override Task DoLand(CancellationToken cancel)
        {
            return Mavlink.Common.SetMode(1, (int)CopterMode.CopterModeLand, cancel);
        }

        public override Task DoRtl(CancellationToken cancel)
        {
            return Mavlink.Common.SetMode(1, (int)CopterMode.CopterModeRtl, cancel);
        }

        #region Fail safe

        public override IEnumerable<FailSafeInfo> AvailableFailSafe => _availableFailSafe;

        private static readonly FailSafeInfo[] _availableFailSafe = new FailSafeInfo[]
        {
            new FailSafeInfo { Name = "All",  Description = "",  DisplayName = "All", Tag=0  },
            new FailSafeInfo { Name = "Barometer",  Description = "",  DisplayName = "Barometer", Tag=1  },
            new FailSafeInfo { Name = "Compass",  Description = "",  DisplayName = "Compass", Tag=2  },
            new FailSafeInfo { Name = "GPSL",  Description = "",  DisplayName = "GPSL", Tag=3    },
            new FailSafeInfo { Name = "INS",  Description = "",  DisplayName = "INS", Tag=4  },
            new FailSafeInfo { Name = "Parameters",  Description = "",  DisplayName = "Parameters", Tag=5    },
            new FailSafeInfo { Name = "RCChann",  Description = "",  DisplayName = "RCChann", Tag=6  },
            new FailSafeInfo { Name = "BoardVoltage",  Description = "",  DisplayName = "BoardVoltage", Tag=7    },
            new FailSafeInfo { Name = "BatteryLevel",  Description = "",  DisplayName = "BatteryLevel", Tag=8    },
            new FailSafeInfo { Name = "LoggingAvailable",  Description = "",  DisplayName = "LoggingAvailable", Tag=9    },
            new FailSafeInfo { Name = "HardwareSafetySwit",  Description = "",  DisplayName = "HardwareSafetySwit", Tag=10 },
            new FailSafeInfo { Name = "GPSConfiguration",  Description = "",  DisplayName = "GPSConfiguration", Tag=11 },
            new FailSafeInfo { Name = "System",  Description = "",  DisplayName = "System", Tag=12 },
            new FailSafeInfo { Name = "Mission",  Description = "",  DisplayName = "Mission", Tag=13 },
            new FailSafeInfo { Name = "Rangefinder",  Description = "",  DisplayName = "Rangefinder", Tag=14 },
            new FailSafeInfo { Name = "Camera",  Description = "",  DisplayName = "Camera", Tag=15 },
            new FailSafeInfo { Name = "AuxAuth",  Description = "",  DisplayName = "AuxAuth", Tag=16 },
            new FailSafeInfo { Name = "VisualOdometry",  Description = "",  DisplayName = "VisualOdometry", Tag=17 },
            new FailSafeInfo { Name = "FFT",  Description = "",  DisplayName = "FFT", Tag=19 },
        };

        public override async Task<FailSafeState[]> ReadFailSafe(CancellationToken cancel = default)
        {
            _logger.Info("Begin to read failsafe state...");
            var value = await Mavlink.Params.ReadParam("ARMING_CHECK", 1, cancel).ConfigureAwait(false);
            Debug.Assert(value.IntegerValue.HasValue,"Something goes wrong. Need uin32");

            _logger.Info($"Ended to read failsafe state value = {value.IntegerValue}:");

            var bitArr = new BitArray(BitConverter.GetBytes((UInt32)value.IntegerValue.Value));
            var result = new FailSafeState[_availableFailSafe.Length];
            for (int i = 0; i < result.Length; i++)
            {
                _logger.Debug($"FailSafe {_availableFailSafe[i].Name} = {bitArr[(int)_availableFailSafe[i].Tag]}");
                result[i] = new FailSafeState
                {
                    Info = _availableFailSafe[i],
                    Value = bitArr[(int) _availableFailSafe[i].Tag]
                };
            }

            return result;
        }

        public override async Task WriteFailSafe(IReadOnlyDictionary<string, bool> values, CancellationToken cancel = default)
        {
            _logger.Info("Begin to write failsafe state...");
            var readed = await ReadFailSafe(cancel).ConfigureAwait(false);
            foreach (var pair in values)
            {
                var founded = readed.FirstOrDefault(_ => _.Info.Name == pair.Key);
                if (founded == null) throw new Exception($"Fail safe with key '{pair.Key}' not found. Available keys:{string.Join(",",_availableFailSafe.Select(_=>_.Name))}");
                _logger.Info($"Chane FailSafe value {pair.Key}: {founded.Value} => {pair.Value}");
                founded.Value = pair.Value;
            }

            var source = BitConverter.GetBytes((uint) (0));
            var bitArr = new BitArray(source);
            foreach (var item in readed)
            {
                bitArr[(int) item.Info.Tag] = item.Value;
            }
            bitArr.CopyTo(source, 0);
            var value = BitConverter.ToUInt32(source, 0);
            
            var result = await Mavlink.Params.WriteParam("ARMING_CHECK", value , cancel).ConfigureAwait(false);
            _logger.Info($"End to write failsafe state Send {value}. Readed {result.IntegerValue}");
        }

        #endregion

        #region Missions

        protected override Task<bool> CheckAutoMode(CancellationToken cancel)
        {
            return Task.FromResult(
                Mavlink.Heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
                Mavlink.Heartbeat.RawHeartbeat.Value.CustomMode == (int)CopterMode.CopterModeAuto);
        }

        protected override async Task EnsureInAutoMode(CancellationToken cancel)
        {
            if (!await CheckAutoMode(cancel).ConfigureAwait(false))
            {
                await Mavlink.Common.SetMode(1, (int)CopterMode.CopterModeAuto, cancel).ConfigureAwait(false);
            }
           
        }

        #endregion  

    }
}
