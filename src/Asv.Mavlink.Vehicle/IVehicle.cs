using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink
{
    

    

    public interface IVehicle:IDisposable
    {
        ushort FullId { get; }
        MavlinkClientIdentity Identity { get; }

        IMavlinkClient Mavlink { get; }

        IRxValue<VehicleInitState> InitState { get; }
        IRxValue<AutopilotVersionPayload> AutopilotVersion { get; }
        IRxValue<string> Name { get; }

        void StartListen();

        IRxValue<LinkState> Link { get; }
        IRxValue<double> PacketRateHz { get; }
        IRxValue<double> LinkQuality { get; }
        IRxValue<RadioLinkStatus> RadioStatus { get; }

        IRxValue<VehicleClass> Class { get; }

        /// <summary>
        /// The filtered global position (e.g. fused GPS and accelerometers). 
        /// </summary>
        IRxValue<GeoPoint> GlobalPosition { get; }

        IRxValue<GpsInfo> GpsInfo { get; }
        IRxValue<double> GpsGroundVelocity { get; }

        IRxValue<GpsInfo> Gps2Info { get; }
        IRxValue<double> Gps2GroundVelocity { get; }

        IRxValue<double> AltitudeAboveHome { get; }

        IRxValue<GeoPoint?> Home { get; }
        IRxValue<double?> HomeDistance { get; }
        Task RequestHome(CancellationToken cancel);

        IRxValue<bool> IsArmed { get; }
        IRxValue<TimeSpan> ArmedTime { get; }

        Task ArmDisarm(bool isArming, CancellationToken cancel, bool force = false);

        IRxValue<double?> CurrentBattery { get; }
        IRxValue<double> VoltageBattery { get; }
        IRxValue<double?> BatteryCharge { get; }

        IRxValue<double> Pitch { get; }
        IRxValue<double> Roll { get; }
        IRxValue<double> Yaw { get; }
        
        IRxValue<double> PitchSpeed { get; }
        IRxValue<double> RollSpeed { get; }
        IRxValue<double> YawSpeed { get; }

        IRxValue<VehicleStatusMessage> TextStatus { get; }
        IRxValue<double> CpuLoad { get; }
        IRxValue<double> DropRateCommunication { get; }
        
        IEnumerable<VehicleCustomMode> AvailableModes { get; }
        IRxValue<VehicleMode> Mode { get; }
        Task SetMode(VehicleMode mode, CancellationToken cancel);

        IMavlinkParameterClient Params { get; }

        IEnumerable<VehicleParamDescription> GetParamDescription();

        Task TakeOff(double altitude, CancellationToken cancel);

        IRxValue<GeoPoint?> GoToTarget { get; }
        Task GoToGlob(GeoPoint location, CancellationToken cancel, double? yawDeg = null);
        Task GoToGlobAndWait(GeoPoint location, IProgress<double> progress, double precisionMet, CancellationToken cancel);
        Task GoToGlobAndWaitWithoutAltitude(GeoPoint location, IProgress<double> progress, double precisionMet, CancellationToken cancel);

        Task FlyByLineGlob(GeoPoint start, GeoPoint stop, double precisionMet, CancellationToken cancel, Action firstPointComplete = null);
        Task DoLand(CancellationToken cancel);
        Task DoRtl(CancellationToken cancel);

        #region Region of Interest

        Task SetRoi(GeoPoint location, CancellationToken cancel);
        IRxValue<GeoPoint?> Roi { get; }
        Task ClearRoi(CancellationToken cancel);

        #endregion

        /// <summary>
        /// Request the reboot or shutdown of system components.
        /// </summary>
        /// <param name="ardupilot"></param>
        /// <param name="companion"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task PreflightRebootShutdown(AutopilotRebootShutdown ardupilot, CompanionRebootShutdown companion, CancellationToken cancel);

        #region Flight time statistic

        /// <summary>
        /// Flight time recorder statistic
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<FlightTimeStatistic> GetFlightTimeStatistic(CancellationToken cancel = default);
        /// <summary>
        /// Reset flight time stat
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task ResetFlightStatistic(CancellationToken cancel = default);

        #endregion

        #region FailSafe

        IEnumerable<FailSafeInfo> AvailableFailSafe { get; }
        Task<FailSafeState[]> ReadFailSafe(CancellationToken cancel = default);
        Task WriteFailSafe(IReadOnlyDictionary<string,bool> values,CancellationToken cancel = default);

        #endregion

        #region Missions

        

        Task SetCurrentMissionItem(ushort index, CancellationToken cancel = default);

        Task<MissionItem[]> DownloadMission(int attemptCount,CancellationToken cancel, Action<double> progress = null);
        Task ClearRemoteMission(int attemptCount, CancellationToken cancel);
        Task UploadMission(int attemptCount, CancellationToken cancel, Action<double> progress = null);

        MissionItem AddMissionItem();
        void RemoveMissionItem(ushort index);
        void CleatLocalMission();

        IObservable<IChangeSet<MissionItem, ushort>> MissionItems { get; }
        IRxValue<bool> IsMissionSynced { get; }
        IRxValue<ushort> MissionCurrent { get; }
        IRxValue<ushort> MissionReached { get; }
        IRxValue<double> AllMissionsDistance { get; }

        #endregion

        #region User-defined serial number

        IRxValue<ushort> SerialNumber { get; }
        Task<ushort> ReadSerialNumber(CancellationToken cancel = default);
        Task<ushort> WriteSerialNumber(ushort serialNumber, CancellationToken cancel = default);

        #endregion
    }


    public enum VehicleInitState
    {
        WaitConnection,
        Failed,
        InProgress,
        Complete
    }

    public enum VehicleClass
    {
        Unknown,
        Plane,
        Copter,
    }

    public class RadioStatusInfo
    {
        public RadioStatusInfo()
        {
        }

        public RadioStatusInfo(byte rssi, byte noise)
        {
            RSSI = rssi;
            if (rssi == 255)
            {
                RSSIPersent = 0;
                RSSIdBm = Double.NaN;
                RSSImW = Double.NaN;
            }
            else
            {
                RSSIPersent = 100.0 / 254.0 * rssi;
                RSSIdBm = rssi / 1.9 - 127;
                RSSImW = Math.Pow(10, (RSSIdBm - 30) / 10) * 1000;
            }

            Noise = noise;
            if (noise == 255)
            {
                NoisePercent = 0;
                NoisedBm = Double.NaN;
                NoisemW = Double.NaN;
            }
            else
            {
                NoisePercent = 100.0 / 254.0 * noise;
                NoisedBm = noise / 1.9 - 127;
                NoisemW = Math.Pow(10, (NoisedBm - 30) / 10) * 1000;
            }
        }


        public double RSSI { get; set; }
        public double RSSIPersent { get; set; }
        public double RSSIdBm { get; set; }
        public double RSSImW { get; set; }

        public double Noise { get; set; }
        public double NoisedBm { get; set; }
        public double NoisemW { get; set; }
        public double NoisePercent { get; set; }
    }

    public class RadioLinkStatus
    {
        public RadioLinkStatus()
        {
        }
        public RadioLinkStatus(RadioStatusPayload value)
        {
            Local = new RadioStatusInfo(value.Rssi, value.Noise);
            Remote = new RadioStatusInfo(value.Remrssi, value.Remnoise);
            CorrectedPackets = value.Fixed;
            FreeTxBufferSpace = value.Txbuf;
            ReceiveErrors = value.Rxerrors;
        }

        public ushort ReceiveErrors { get; set; }
        public byte FreeTxBufferSpace { get; set; }

        public ushort CorrectedPackets { get; set; }

        public RadioStatusInfo Local { get; set; }
        public RadioStatusInfo Remote { get; set; }

    }

    public class FailSafeInfo
    {
        public object Tag { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }

    public class FailSafeState
    {
        public FailSafeInfo Info { get; set; }
        public bool Value { get; set; }
    }

    public class FlightTimeStatistic
    {
        /// <summary>
        /// Holds the number of times the board has been booted
        /// </summary>
        public long BootCount { get; set; }
        /// <summary>
        /// Holds the total number of seconds that the board/vehicle has been flying (including all previous flights)
        /// </summary>
        public TimeSpan FlightTime { get; set; }
        /// <summary>
        /// Holds the total number of seconds that the board has been powered up (including all previous flights)
        /// </summary>
        public TimeSpan RunTime { get; set; }
    }

    public class VehicleStatusMessage
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public MavSeverity Type { get; set; }
    }

    public class GpsInfo
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public GpsInfo(GpsRawIntPayload rawGps)
        {
            if (rawGps.FixType == GpsFixType.GpsFixTypeNoGps) return;
            if (Enum.IsDefined(typeof(GpsFixType), rawGps.FixType) == false) return;


            Vdop = rawGps.Epv == ushort.MaxValue ? (double?)null : rawGps.Epv / 100D;
            Hdop = rawGps.Eph == ushort.MaxValue ? (double?)null : rawGps.Eph / 100D;
            Pdop = Hdop.HasValue && Vdop.HasValue
                ? Math.Sqrt(Hdop.Value * Hdop.Value + Vdop.Value * Vdop.Value)
                : default(double?);
            AltitudeMsl = rawGps.Alt / 1000D;
            AltitudeEllipsoid = rawGps.AltEllipsoid / 1000D;
            CourseOverGround = rawGps.Cog / 100D;

            FixType = rawGps.FixType;
            SatellitesVisible = rawGps.SatellitesVisible;
            var num = (long)(rawGps.TimeUsec * (double)1000 + (rawGps.TimeUsec >= 0.0 ? 0.5 : -0.5));
            if (num is > -315537897600000L and < 315537897600000L && rawGps.TimeUsec < 253402967000)
            {
                Time = Epoch.AddSeconds(rawGps.TimeUsec);
            }

        }

        public GpsInfo(Gps2RawPayload rawGps)
        {
            if (rawGps.FixType == GpsFixType.GpsFixTypeNoGps) return;
            if (Enum.IsDefined(typeof(GpsFixType), rawGps.FixType) == false) return;

            Vdop = rawGps.Epv == ushort.MaxValue ? (double?)null : rawGps.Epv / 100D;
            Hdop = rawGps.Eph == ushort.MaxValue ? (double?)null : rawGps.Eph / 100D;
            Pdop = Hdop.HasValue && Vdop.HasValue
                ? Math.Sqrt(Hdop.Value * Hdop.Value + Vdop.Value * Vdop.Value)
                : default(double?);
            AltitudeMsl = rawGps.Alt / 1000D;

            CourseOverGround = rawGps.Cog / 100D;
            FixType = rawGps.FixType;
            SatellitesVisible = rawGps.SatellitesVisible;

            // check because sometime argument out of range exception
            var num = (long)(rawGps.TimeUsec * (double)1000 + (rawGps.TimeUsec >= 0.0 ? 0.5 : -0.5));
            if (num > -315537897600000L && num < 315537897600000L)
            {
                Time = Epoch.AddSeconds(rawGps.TimeUsec);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public double CourseOverGround { get; set; }

        /// <summary>
        /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        /// </summary>
        public double? AltitudeEllipsoid { get; }

        /// <summary>
        /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
        /// </summary>
        public double AltitudeMsl { get; }

        public GpsFixType FixType { get; }
        public int SatellitesVisible { get; }
        public DateTime Time { get; }
        /// <summary>
        /// HDOP – horizontal dilution of precision
        /// </summary>
        public double? Hdop { get; }
        public DopStatusEnum HdopStatus => GpsInfoHelper.GetDopStatus(Hdop);

        /// <summary>
        /// position (3D) dilution of precision
        /// </summary>
        public double? Pdop { get; }
        public DopStatusEnum PdopStatus => GpsInfoHelper.GetDopStatus(Pdop);

        /// <summary>
        ///  vertical dilution of precision
        /// </summary>
        public double? Vdop { get; }
        public DopStatusEnum VdopStatus => GpsInfoHelper.GetDopStatus(Vdop);
    }

    /// <summary>
    /// https://en.wikipedia.org/wiki/Dilution_of_precision_(navigation)
    /// </summary>
    public enum DopStatusEnum
    {
        Unknown,
        Ideal,
        Excellent,
        Good,
        Moderate,
        Fair,
        Poor,
    }

    public static class GpsInfoHelper
    {
        public static DopStatusEnum GetDopStatus(double? value)
        {
            if (!value.HasValue) return DopStatusEnum.Unknown;
            if (value <= 1) return DopStatusEnum.Ideal;
            if (value <= 2) return DopStatusEnum.Excellent;
            if (value <= 5) return DopStatusEnum.Good;
            if (value <= 10) return DopStatusEnum.Moderate;
            if (value <= 20) return DopStatusEnum.Fair;
            return DopStatusEnum.Poor;
        }

        public static string GetDescription(this DopStatusEnum src)
        {
            switch (src)
            {
                case DopStatusEnum.Ideal:
                    return RS.GpsInfoHelper_GetDescription_Ideal;
                case DopStatusEnum.Excellent:
                    return RS.GpsInfoHelper_GetDescription_Excellent;
                case DopStatusEnum.Good:
                    return RS.GpsInfoHelper_GetDescription_Good;
                case DopStatusEnum.Moderate:
                    return RS.GpsInfoHelper_GetDescription_Moderate;
                case DopStatusEnum.Fair:
                    return RS.GpsInfoHelper_GetDescription_Fair;
                case DopStatusEnum.Poor:
                    return RS.GpsInfoHelper_GetDescription_Poor;
                case DopStatusEnum.Unknown:
                default:
                    return RS.GpsInfoHelper_GetDescription_Unknown;
            }
        }

        public static string GetDisplayName(this DopStatusEnum src)
        {
            switch (src)
            {
                case DopStatusEnum.Ideal:
                    return RS.GpsInfoHelper_GetDisplayName_Ideal;
                case DopStatusEnum.Excellent:
                    return RS.GpsInfoHelper_GetDisplayName_Excellent;
                case DopStatusEnum.Good:
                    return RS.GpsInfoHelper_GetDisplayName_Good;
                case DopStatusEnum.Moderate:
                    return RS.GpsInfoHelper_GetDisplayName_Moderate;
                case DopStatusEnum.Fair:
                    return RS.GpsInfoHelper_GetDisplayName_Fair;
                case DopStatusEnum.Poor:
                    return RS.GpsInfoHelper_GetDisplayName_Poor;
                case DopStatusEnum.Unknown:
                default:
                    return RS.GpsInfoHelper_GetDescription_Unknown;
            }
        }

        public static string GetShortDisplayName(this GpsFixType fixType)
        {
            switch (fixType)
            {
                case GpsFixType.GpsFixTypeNoGps:
                    return "No GPS";
                case GpsFixType.GpsFixTypeNoFix:
                    return "No Fix";
                case GpsFixType.GpsFixType2dFix:
                    return "2D Fix";
                case GpsFixType.GpsFixType3dFix:
                    return "3D Fix";
                case GpsFixType.GpsFixTypeDgps:
                    return "Dgps";
                case GpsFixType.GpsFixTypeRtkFloat:
                    return "RTK Float";
                case GpsFixType.GpsFixTypeRtkFixed:
                    return "RTK Fix";
                case GpsFixType.GpsFixTypeStatic:
                    return "Static";
                case GpsFixType.GpsFixTypePpp:
                    return "Ppp";
                default:
                    return RS.GpsInfoHelper_GetDescription_Unknown;
            }
        }
    }


    public enum AutopilotRebootShutdown
    {
        DoNothingForAutopilot = 0,
        RebootAutopilot = 1,
        ShutdownAutopilot = 2,
        RebootAutopilotAndKeepItInTheBootloaderUntilUpgraded = 3,
    }

    public enum CompanionRebootShutdown
    {
        DoNothingForOnboardComputer = 0,
        RebootOnboardComputer = 1,
        ShutdownOnboardComputer = 2,
        RebootOnboardComputerAndKeepItInTheBootloaderUntilUpgraded = 3,
    }


    public class VehicleCustomMode
    {
        public uint Value { get; set; }
        public string Name { get; set; }
    }

    public class VehicleMode
    {
        public MavModeFlag BaseMode { get; set; }
        public VehicleCustomMode CustomMode { get; set; }
    }
}
