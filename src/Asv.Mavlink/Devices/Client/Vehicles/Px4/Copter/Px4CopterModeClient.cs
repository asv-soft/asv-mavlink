using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class Px4CopterModeClient  : ModeClient
{
    #region Static

    public static ImmutableArray<OpMode> AllModes { get; }

    static Px4CopterModeClient()
    {
        var wellKnownModes = new HashSet<uint>(WellKnownModes.Select(x=>x.CustomMode));
        var allModes = new List<OpMode>();
        allModes.AddRange(WellKnownModes);
        AllModes = [..allModes];
    }
    
    private static IEnumerable<OpMode> WellKnownModes 
    {
        get
        {
            yield return Manual;
            yield return Stabilized;
            yield return Acro;
            yield return Rattitude;
            yield return Altctl;
            yield return Posctl;
            yield return Loiter;
            yield return Mission;
            yield return RTL;
            yield return Followme;
            yield return Offboard;
        }
    }

        public static readonly OpMode Manual = new("Manual", "Manual", false,
        MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
        (uint)CustomMainMode.Px4CustomMainModeManual,
        (uint)CustomSubMode.Empty);

        public static readonly OpMode Stabilized = new("Stabilized","Stabilized",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled |
            MavModeFlag.MavModeFlagManualInputEnabled,
            (uint)CustomMainMode.Px4CustomMainModeStabilized,
            (uint)CustomSubMode.Empty);

        public static readonly OpMode Acro = new("Acro", "Acro", false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            (uint)CustomMainMode.Px4CustomMainModeAcro,
            (uint)CustomSubMode.Empty);

        public static readonly OpMode Rattitude = new("Rattitude", "Rattitude", false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            (uint)CustomMainMode.Px4CustomMainModeRattitude,
            (uint)CustomSubMode.Empty);
        
        public static readonly OpMode  Altctl = new("Altctl","Altctl",false, 
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            (uint)CustomMainMode.Px4CustomMainModeAltctl,
            (uint)CustomSubMode.Empty);
        
        public static readonly OpMode  Posctl = new("Posctl","Posctl",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            (uint)CustomMainMode.Px4CustomMainModePosctl,
            (uint)CustomSubMode.Empty);
        
        public static readonly OpMode  Loiter = new("Loiter","Loiter",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            (uint)CustomMainMode.Px4CustomMainModeAuto,
            (uint)CustomSubMode.Px4CustomSubModeAutoLoiter);
        
        public static readonly OpMode  Mission = new("Mission","Mission",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            (uint)CustomMainMode.Px4CustomMainModeAuto,
            (uint)CustomSubMode.Px4CustomSubModeAutoMission);
        
        public static readonly OpMode  RTL = new("RTL","RTL",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            (uint)CustomMainMode.Px4CustomMainModeAuto,
            (uint)CustomSubMode.Px4CustomSubModeAutoRtl);
        
        public static readonly OpMode  Followme = new("Followme","Followme",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            (uint)CustomMainMode.Px4CustomMainModeAuto,
            (uint)CustomSubMode.Px4CustomSubModeAutoFollowTarget);
        
        public static readonly OpMode  Offboard = new("Offboard","Offboard",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            (uint)CustomMainMode.Px4CustomMainModeOffboard,
            (uint)CustomSubMode.Empty);
        
        public static readonly OpMode  TakeOff = new("TakeOff","TakeOff",true,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            (uint)CustomMainMode.Px4CustomMainModeAuto,
            (uint)CustomSubMode.Px4CustomSubModeAutoTakeoff);
        
        public static readonly OpMode  Land = new("Land","Land",true,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            (uint)CustomMainMode.Px4CustomMainModeAuto,
            (uint)CustomSubMode.Px4CustomSubModeAutoLand);
        
        public static readonly OpMode  Rtgs = new("Rtgs","Rtgs",true,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            (uint)CustomMainMode.Px4CustomMainModeAuto,
            (uint)CustomSubMode.Px4CustomSubModeAutoRtgs);
    
    #endregion
    
    public Px4CopterModeClient(IHeartbeatClient heartbeat, ICommandClient command) 
        : base(heartbeat, command)
    {
    }

    protected override OpMode Convert(HeartbeatPayload? hb)
    {
        if (hb == null) return OpMode.Unknown;
        var customMainMode = (CustomMainMode)((hb.CustomMode & 0xFF0000) >> 16);
        var customSubMode = (CustomSubMode)((hb.CustomMode & 0xFF000000) >> 24);
        var mode = hb.BaseMode;

        if (mode.HasFlag(MavModeFlag.MavModeFlagManualInputEnabled)) // manual modes
        {
            switch (customMainMode)
            {
                case CustomMainMode.Px4CustomMainModeManual:
                    return Manual;
                case CustomMainMode.Px4CustomMainModeAcro:
                    return Acro;
                case CustomMainMode.Px4CustomMainModeRattitude:
                    return Rattitude;
                case CustomMainMode.Px4CustomMainModeStabilized:
                    return Stabilized;
                case CustomMainMode.Px4CustomMainModeAltctl:
                    return Altctl;
                case CustomMainMode.Px4CustomMainModePosctl:
                    return Posctl;
            }
        }
        else if (mode.HasFlag(MavModeFlag.MavModeFlagAutoEnabled)) // auto mode
        {
            if (customMainMode.HasFlag(CustomMainMode.Px4CustomMainModeAuto))
            {
                switch (customSubMode)
                {
                    case CustomSubMode.Px4CustomSubModeAutoMission:
                        return Mission;
                    case CustomSubMode.Px4CustomSubModeAutoTakeoff:
                        return TakeOff;
                    case CustomSubMode.Px4CustomSubModeAutoLoiter:
                        return Loiter;
                    case CustomSubMode.Px4CustomSubModeAutoFollowTarget:
                        return Followme;
                    case CustomSubMode.Px4CustomSubModeAutoRtl:
                        return RTL;
                    case CustomSubMode.Px4CustomSubModeAutoLand:
                        return Land;
                    case CustomSubMode.Px4CustomSubModeAutoRtgs:
                        return Rtgs;
                }

                if ((int)customMainMode == (int)CustomMainMode.Px4CustomMainModeOffboard)
                {
                    return Offboard;
                }
            }
        }

        return OpMode.Unknown;
    }

    public override IEnumerable<OpMode> AvailableModes => AllModes;
}


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
    Px4CustomSubModeAutoRtl = 5,
    Px4CustomSubModeAutoLand = 6,
    Px4CustomSubModeAutoRtgs = 7,
    Px4CustomSubModeAutoFollowTarget = 8,
}