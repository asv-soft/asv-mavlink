#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

/// <summary>
/// Represents a vehicle mode.
/// </summary>
public class Px4Mode: ICustomMode
{
    public static Px4Mode Unknown = new("Unknown", "Unknown mode",true, 0, 0,0);
    
    public Px4Mode(string name, string description, bool internalMode, MavModeFlag mode, CustomMainMode customMode, CustomSubMode customSubMode)
    {
        Name = name;
        Description = description;
        InternalMode = internalMode;
        Mode = mode;
        CustomMode = customMode;
        CustomSubMode = customSubMode;
    }

    public CustomSubMode CustomSubMode { get; }

    public CustomMainMode CustomMode { get; }

    public MavModeFlag Mode { get; }

    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <returns>The name of the property.</returns>
    public string Name { get; }

    /// <summary>
    /// Gets the description of the property.
    /// </summary>
    /// <returns>
    /// A string representing the description.
    /// </returns>
    public string Description { get; }

    /// <summary>
    /// This flag indicates whether the vehicle can be set into this mode by the user command.
    /// You shouldn't show this mode in the user interface if this flag is true.
    /// </summary>
    /// <value>
    /// A boolean value that represents whether the vehicle can be set into this mode by the user command.
    /// </value>
    public bool InternalMode { get; }

    public void GetCommandLongArgs(out uint baseMode, out uint customMode, out uint customSubMode)
    {
        baseMode = (uint)Mode;
        customMode = (uint)CustomMode;
        customSubMode = (uint)CustomSubMode;
    }

    public bool IsCurrentMode(HeartbeatPayload? hb)
    {
        return Convert(hb) == this;
    }

    public bool IsCurrentMode(CommandLongPayload payload)
    {
        if ((uint)payload.Param1 != (uint)Mode) return false;
        if ((uint)payload.Param2 != (uint)CustomMode) return false;
        return (uint)payload.Param3 == (uint)CustomSubMode;
    }

    public void Fill(HeartbeatPayload hb)
    {
        hb.CustomMode = 
            ((uint)CustomMode << 16) | 
            ((uint)CustomSubMode << 24) | 
            (hb.CustomMode & 0xFFFF);
    }

    #region Static

    public static ImmutableArray<Px4Mode> AllModes { get; }

    static Px4Mode()
    {
        AllModes = [..WellKnownModes];
    }
    
    private static IEnumerable<Px4Mode> WellKnownModes 
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

        public static readonly Px4Mode Manual = new("Manual", "Manual", false,
        MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
        CustomMainMode.Px4CustomMainModeManual,
        CustomSubMode.Empty);

        public static readonly Px4Mode Stabilized = new("Stabilized","Stabilized",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled |
            MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode.Px4CustomMainModeStabilized,
            CustomSubMode.Empty);

        public static readonly Px4Mode Acro = new("Acro", "Acro", false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode.Px4CustomMainModeAcro,
            CustomSubMode.Empty);

        public static readonly Px4Mode Rattitude = new("Rattitude", "Rattitude", false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode.Px4CustomMainModeRattitude,
            CustomSubMode.Empty);
        
        public static readonly Px4Mode  Altctl = new("Altctl","Altctl",false, 
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode.Px4CustomMainModeAltctl,
            CustomSubMode.Empty);
        
        public static readonly Px4Mode  Posctl = new("Posctl","Posctl",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagManualInputEnabled,
            CustomMainMode.Px4CustomMainModePosctl,
            CustomSubMode.Empty);
        
        public static readonly Px4Mode  Loiter = new("Loiter","Loiter",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode.Px4CustomSubModeAutoLoiter);
        
        public static readonly Px4Mode  Mission = new("Mission","Mission",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode.Px4CustomSubModeAutoMission);
        
        public static readonly Px4Mode  RTL = new("RTL","RTL",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode.Px4CustomSubModeAutoRtl);
        
        public static readonly Px4Mode  Followme = new("Followme","Followme",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode.Px4CustomSubModeAutoFollowTarget);
        
        public static readonly Px4Mode  Offboard = new("Offboard","Offboard",false,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode.Px4CustomMainModeOffboard,
            CustomSubMode.Empty);
        
        public static readonly Px4Mode  TakeOff = new("TakeOff","TakeOff",true,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode.Px4CustomSubModeAutoTakeoff);
        
        public static readonly Px4Mode  Land = new("Land","Land",true,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode.Px4CustomSubModeAutoLand);
        
        public static readonly Px4Mode  Rtgs = new("Rtgs","Rtgs",true,
            MavModeFlag.MavModeFlagCustomModeEnabled | MavModeFlag.MavModeFlagAutoEnabled | MavModeFlag.MavModeFlagStabilizeEnabled | MavModeFlag.MavModeFlagGuidedEnabled,
            CustomMainMode.Px4CustomMainModeAuto,
            CustomSubMode.Px4CustomSubModeAutoRtgs);


        protected static Px4Mode Convert(HeartbeatPayload? hb)
        {
            if (hb == null) return Unknown;
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

            return Px4Mode.Unknown;
        }

        #endregion
}

public enum CustomMainMode:ushort
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

public enum CustomSubMode:ushort
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
