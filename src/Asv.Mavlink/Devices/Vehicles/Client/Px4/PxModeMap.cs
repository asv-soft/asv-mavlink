using System;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink
{
    public enum CustomMainMode
    {
        Empty = 0,
        Px4CustomMainModeManual = 1,
        Px4CustomMainModeAltctl = 2,
        Px4CustomMainModePosctl = 3,
        Px4CustomMainModeAuto = 4,
        Px4CustomMainModeAcro = 5,
        Px4CustomMainModeOffboard = 6,
        Px4CustomMainModeStabilized = 7,
        Px4CustomMainModeRattitude = 8,
    }

    public enum CustomSubMode
    {
        Empty = 0,
        Px4CustomSubModeAutoReady = 1,
        Px4CustomSubModeAutoTakeoff = 2,
        Px4CustomSubModeAutoLoiter = 3,
        Px4CustomSubModeAutoMission = 4,
        Px4CustomSubModeAutoRTL = 5,
        Px4CustomSubModeAutoLand = 6,
        Px4CustomSubModeAutoRtgs = 7,
        Px4CustomSubModeAutoFollowTarget = 8,
    }

    public enum Px4CustomMode
    {
        Manual,
        Stabilized,
        Acro,
        Rattitude,
        Altctl,
        Posctl,
        Loiter,
        Mission,
        RTL,
        Followme,
        Offboard,
        Unknwon,
        TakeOff,
        Land,
        Rtgs
    }

    public static class Px4ModeHelper
    {
        public static Px4VehicleMode Create(this Px4CustomMode mode)
        {
            switch (mode)
            {
                case Px4CustomMode.Manual: return Manual;
                case Px4CustomMode.Stabilized: return Stabilized;
                case Px4CustomMode.Acro: return Acro;
                case Px4CustomMode.Rattitude: return Rattitude;
                case Px4CustomMode.Altctl: return Altctl;
                case Px4CustomMode.Posctl: return Posctl;
                case Px4CustomMode.Loiter: return Loiter;
                case Px4CustomMode.Mission: return Mission;
                case Px4CustomMode.RTL: return RTL;
                case Px4CustomMode.Followme: return Followme;
                case Px4CustomMode.Offboard: return Offboard;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static Px4CustomMode GetMode(this Px4VehicleMode mode)
        {
            if (mode.ModeFlag.HasFlag(MavModeFlag.MavModeFlagManualInputEnabled)) // manual modes
            {
                switch (mode.CustomMainMode)
                {
                    case CustomMainMode.Px4CustomMainModeManual:
                        return Px4CustomMode.Manual;
                    case CustomMainMode.Px4CustomMainModeAcro:
                        return Px4CustomMode.Acro;
                    case CustomMainMode.Px4CustomMainModeRattitude:
                        return Px4CustomMode.Rattitude;
                    case CustomMainMode.Px4CustomMainModeStabilized:
                        return Px4CustomMode.Stabilized;
                    case CustomMainMode.Px4CustomMainModeAltctl:
                        return Px4CustomMode.Altctl;
                    case CustomMainMode.Px4CustomMainModePosctl:
                        return Px4CustomMode.Posctl;
                }
            }
            else if (mode.ModeFlag.HasFlag(MavModeFlag.MavModeFlagAutoEnabled)) // auto mode
            {
                if (mode.CustomMainMode.HasFlag(CustomMainMode.Px4CustomMainModeAuto))
                {
                    switch (mode.CustomSubMode)
                    {
                        case CustomSubMode.Px4CustomSubModeAutoMission:
                            return Px4CustomMode.TakeOff;
                        case CustomSubMode.Px4CustomSubModeAutoTakeoff:
                            return Px4CustomMode.Mission;
                        case CustomSubMode.Px4CustomSubModeAutoLoiter:
                            return Px4CustomMode.Loiter;
                        case CustomSubMode.Px4CustomSubModeAutoFollowTarget:
                            return Px4CustomMode.Followme;
                        case CustomSubMode.Px4CustomSubModeAutoRTL:
                            return Px4CustomMode.RTL;
                        case CustomSubMode.Px4CustomSubModeAutoLand:
                            return Px4CustomMode.Land;
                        case CustomSubMode.Px4CustomSubModeAutoRtgs:
                            return Px4CustomMode.Rtgs;
                    }

                    if ((int) mode.CustomSubMode == (int) CustomMainMode.Px4CustomMainModeOffboard)
                    {
                        return Px4CustomMode.Offboard;
                    }
                }
            }
            
            return Px4CustomMode.Unknwon;

        }

        public static Px4VehicleMode Manual = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModeManual,
            CustomSubMode = CustomSubMode.Empty,
        };
        public static Px4VehicleMode Stabilized = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModeStabilized,
            CustomSubMode = CustomSubMode.Empty,
        };
        public static Px4VehicleMode Acro = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModeAcro,
            CustomSubMode = CustomSubMode.Empty,
        };
        public static Px4VehicleMode Rattitude = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModeRattitude,
            CustomSubMode = CustomSubMode.Empty,
        };
        public static Px4VehicleMode Altctl = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModeAltctl,
            CustomSubMode = CustomSubMode.Empty,
        };
        public static Px4VehicleMode Posctl = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModePosctl,
            CustomSubMode = CustomSubMode.Empty,
        };
        public static Px4VehicleMode Loiter = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode = CustomSubMode.Px4CustomSubModeAutoLoiter,
        };
        public static Px4VehicleMode Mission = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode = CustomSubMode.Px4CustomSubModeAutoMission,
        };
        public static Px4VehicleMode RTL = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode = CustomSubMode.Px4CustomSubModeAutoRTL,
        };
        public static Px4VehicleMode Followme = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode = CustomSubMode.Px4CustomSubModeAutoFollowTarget,
        };
        public static Px4VehicleMode Offboard = new Px4VehicleMode
        {
            ModeFlag = MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode = CustomMainMode.Px4CustomMainModeOffboard,
            CustomSubMode = CustomSubMode.Empty,
        };


    }

    public class Px4VehicleMode:IEquatable<Px4VehicleMode>
    {
        public Px4VehicleMode()
        {
            
        }

        public Px4VehicleMode(HeartbeatPayload payload)
        {
            CustomMainMode = (CustomMainMode) ((payload.CustomMode & 0xFF0000) >> 16);
            CustomSubMode = (CustomSubMode) ((payload.CustomMode & 0xFF000000) >> 24);
            ModeFlag = payload.BaseMode;
        }
        public Px4CustomMode Px4Mode => this.GetMode();
        public MavModeFlag ModeFlag { get; set; }
        public CustomMainMode CustomMainMode { get; set; }
        public CustomSubMode CustomSubMode { get; set; }

        public bool Equals(Px4VehicleMode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ModeFlag == other.ModeFlag && CustomMainMode == other.CustomMainMode && CustomSubMode == other.CustomSubMode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Px4VehicleMode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) ModeFlag;
                hashCode = (hashCode * 397) ^ (int) CustomMainMode;
                hashCode = (hashCode * 397) ^ (int) CustomSubMode;
                return hashCode;
            }
        }
    }

    
}
