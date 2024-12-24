using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.Ardupilotmega;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;

namespace Asv.Mavlink;

public class ArduPlaneMode:ICustomMode
{
    #region Static

    public static ImmutableArray<ArduPlaneMode> AllModes { get; }
 
    static ArduPlaneMode()
    {
        var wellKnownModes = new HashSet<PlaneMode>(WellKnownQuadPlaneModes.Select(m=>m.CustomMode));
        var allModes = new List<ArduPlaneMode>();
        allModes.AddRange(WellKnownQuadPlaneModes);
        foreach (var mode in Enum.GetValues<PlaneMode>())
        {
            if (wellKnownModes.Contains(mode)) continue;
            // this is not "well known" mode, try to create description from enum
            allModes.Add(new ArduPlaneMode(mode.ToString("G"), string.Empty, true, MavModeFlag.MavModeFlagCustomModeEnabled, mode));
        }
        AllModes = [..allModes];
    }
    private static IEnumerable<ArduPlaneMode> WellKnownPlaneModes 
    {
        get
        {
            yield return Manual;
            yield return Circle;
            yield return Stabilize;
            yield return Training;
            yield return Acro;
            yield return FlyByWireA;
            yield return FlyByWireB;
            yield return Cruise;
            yield return Autotune;
            yield return Auto;
            yield return Rtl;
            yield return Loiter;
            yield return Guided;
        }
    }
    private static IEnumerable<ArduPlaneMode> WellKnownQuadPlaneModes 
    {
        get
        {
            yield return Manual;
            yield return Circle;
            yield return Stabilize;
            yield return Training;
            yield return Acro;
            yield return FlyByWireA;
            yield return FlyByWireB;
            yield return Cruise;
            yield return Autotune;
            yield return Auto;
            yield return Rtl;
            yield return Loiter;
            yield return Guided;
            yield return Qstabilize;
            yield return Qhover;
            yield return Qloiter;
            yield return Qland;
            yield return Qrtl;
            yield return Qautotune;
            yield return LoiterToLand;
            yield return Qacro;
        }
    }

    public static readonly ArduPlaneMode Unknown      = new("Unknown", "Unknown plane mode", true, MavModeFlag.MavModeFlagCustomModeEnabled, (PlaneMode)uint.MaxValue);
    /// <summary>
    /// Manual control surface movement, passthrough
    /// </summary>
    public static readonly ArduPlaneMode Manual      = new("Manual", RS.ArdupilotPlaneMode_Manual_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeManual);
    /// <summary>
    /// Gently turns aircraft
    /// </summary>
    public static readonly ArduPlaneMode Circle      = new("Circle", RS.ArdupilotPlaneMode_Circle_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeCircle);
    /// <summary>
    /// Wing-leveling on stick release
    /// </summary>
    public static readonly ArduPlaneMode Stabilize   = new("Stabilize", RS.ArdupilotPlaneMode_Stabilize_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeStabilize);
    /// <summary>
    /// Manual control up to roll and pitch limits
    /// </summary>
    public static readonly ArduPlaneMode Training    = new("Training", RS.ArdupilotPlaneMode_Training_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeTraining);
    /// <summary>
    /// Rate controlled mode with no attitude limits
    /// </summary>
    public static readonly ArduPlaneMode Acro        = new("Acro", RS.ArdupilotPlaneMode_Acro_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeAcro);
    /// <summary>
    /// Roll and pitch follow stick input, up to set limits
    /// </summary>
    public static readonly ArduPlaneMode FlyByWireA  = new("Fly by wire A", RS.ArdupilotPlaneMode_FlyByWireA_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeFlyByWireA);
    /// <summary>
    /// Like FBWA, but with automatic height and speed control
    /// </summary>
    public static readonly ArduPlaneMode FlyByWireB  = new("Fly by wire B", RS.ArdupilotPlaneMode_FlyByWireB_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeFlyByWireB);
    /// <summary>
    /// Like FBWB, but with ground course tracking and terrain following
    /// </summary>
    public static readonly ArduPlaneMode Cruise      = new("Cruise", RS.ArdupilotPlaneMode_Cruise_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeCruise);
    /// <summary>
    /// Like FBWA, but learns attitude tuning while flying
    /// </summary>
    public static readonly ArduPlaneMode Autotune    = new("Autotune", RS.ArdupilotPlaneMode_Autotune_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeAutotune);
    /// <summary>
    /// Follows Mission
    /// </summary>
    public static readonly ArduPlaneMode Auto        = new("Auto", RS.ArdupilotPlaneMode_Auto_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeAuto);
    /// <summary>
    /// Returns to and circles home or rally point
    /// </summary>
    public static readonly ArduPlaneMode Rtl         = new("RTL", RS.ArdupilotPlaneMode_Rtl_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeRtl);
    /// <summary>
    /// Circles point where mode switched
    /// </summary>
    public static readonly ArduPlaneMode Loiter      = new("Loiter", RS.ArdupilotPlaneMode_Loiter_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeLoiter);
    /// <summary>
    /// Circles user defined point from GCS
    /// </summary>
    public static readonly ArduPlaneMode Guided      = new("Guided", RS.ArdupilotPlaneMode_Guided_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeGuided);
    /// <summary>
    ///  Allows you to fly your vehicle manually, but self-levels the roll and pitch axis
    /// </summary>
    public static readonly ArduPlaneMode Qstabilize  = new("Quad Stabilize", RS.ArdupilotPlaneMode_Qstabilize_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeQstabilize);
    /// <summary>
    /// QuadPlane maintains a consistent altitude while allowing roll, pitch, and yaw to be controlled normally
    /// </summary>
    public static readonly ArduPlaneMode Qhover      = new("Quad Hover", RS.ArdupilotPlaneMode_Qhover_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeQhover);
    /// <summary>
    /// Automatically attempts to maintain the current location, heading and altitude
    /// </summary>
    public static readonly ArduPlaneMode Qloiter     = new("Quad Loiter", RS.ArdupilotPlaneMode_Qloiter_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeQloiter);
    /// <summary>
    /// Attempts to bring the QuadPlane straight down at the position the vehicle is located when the mode is entered
    /// </summary>
    public static readonly ArduPlaneMode Qland       = new("Quad Land", RS.ArdupilotPlaneMode_Qland_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeQland);
    /// <summary>
    /// Navigates QuadPlane from its current position to hover above the home position and then land
    /// </summary>
    public static readonly ArduPlaneMode Qrtl        = new("Quad RTL", RS.ArdupilotPlaneMode_Qrtl_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeQrtl);
    /// <summary>
    /// Automated pitch and bank procedure to improve control loops
    /// </summary>
    public static readonly ArduPlaneMode Qautotune   = new("Quad Autotune", RS.ArdupilotPlaneMode_Qautotune_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeQautotune);
    /// <summary>
    /// This mode will perform a descending fixed wing LOITER down to Q_RTL_ALT and then switch to QLAND mode.
    /// </summary>
    public static readonly ArduPlaneMode LoiterToLand = new("Loiter To QLand", RS.ArdupilotPlaneMode_Loiter_To_QLand_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeQloiter);
    /// <summary>
    /// A quadplane mode for advanced users that provides rate based stabilization like Copter ACRO
    /// </summary>
    public static readonly ArduPlaneMode Qacro = new("Quad Acro", RS.ArdupilotPlaneMode_QAcro_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, PlaneMode.PlaneModeQacro);
    
    #endregion
    
    public ArduPlaneMode(string name, string description, bool internalMode, MavModeFlag mode, PlaneMode customMode)
    {
        Mode = mode;
        CustomMode = customMode;
        Name = name;
        Description = description;
        InternalMode = internalMode;
    }
    public MavModeFlag Mode { get; }
    public PlaneMode CustomMode { get; }
    public string Name { get; }
    public string Description { get; }
    public bool InternalMode { get; }
    

    public void GetCommandLongArgs(out uint baseMode, out uint customMode, out uint customSubMode)
    {
        baseMode = (uint)Mode;
        customMode = (uint)CustomMode;
        customSubMode = 0;
    }

    public bool IsCurrentMode(HeartbeatPayload? hb)
    {
        return hb?.CustomMode == (uint)CustomMode;
    }

    public bool IsCurrentMode(CommandLongPayload payload)
    {
        return (uint)payload.Param2 == (uint)CustomMode;
    }

    public void Fill(HeartbeatPayload hb)
    {
        hb.CustomMode = (uint)CustomMode;
        hb.BaseMode = Mode;
    }
}