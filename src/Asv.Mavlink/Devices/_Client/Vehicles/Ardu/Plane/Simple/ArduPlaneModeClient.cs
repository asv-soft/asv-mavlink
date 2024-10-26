using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class ArduPlaneModeClient(IHeartbeatClient heartbeat, ICommandClient command) : ModeClient(heartbeat, command)
{
    #region Static

    public static ImmutableArray<OpMode> AllModes { get; }
 
    static ArduPlaneModeClient()
    {
        var wellKnownModes = new HashSet<uint>(WellKnownModes.Select(m=>m.CustomMode));
        var allModes = new List<OpMode>();
        allModes.AddRange(WellKnownModes);
        foreach (var copterMode in Enum.GetValues<PlaneMode>())
        {
            if (wellKnownModes.Contains((uint)copterMode)) continue;
            // this is not "well known" mode, try to create description from enum
            allModes.Add(new OpMode(copterMode.ToString("G"), String.Empty, true, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)copterMode,0));
        }
        AllModes = [..allModes];
    }
    private static IEnumerable<OpMode> WellKnownModes 
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

    /// <summary>
    /// Manual control surface movement, passthrough
    /// </summary>
    public static OpMode Manual      = new("Manual", RS.ArdupilotPlaneMode_Manual_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint)PlaneMode.PlaneModeManual,0);
    /// <summary>
    /// Gently turns aircraft
    /// </summary>
    public static OpMode Circle      = new("Circle", RS.ArdupilotPlaneMode_Circle_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeCircle,0);
    /// <summary>
    /// Wing-leveling on stick release
    /// </summary>
    public static OpMode Stabilize   = new("Stabilize", RS.ArdupilotPlaneMode_Stabilize_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeStabilize,0);
    /// <summary>
    /// Manual control up to roll and pitch limits
    /// </summary>
    public static OpMode Training    = new("Training", RS.ArdupilotPlaneMode_Training_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeTraining,0);
    /// <summary>
    /// Rate controlled mode with no attitude limits
    /// </summary>
    public static OpMode Acro        = new("Acro", RS.ArdupilotPlaneMode_Acro_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeAcro,0);
    /// <summary>
    /// Roll and pitch follow stick input, up to set limits
    /// </summary>
    public static OpMode FlyByWireA  = new("Fly by wire A", RS.ArdupilotPlaneMode_FlyByWireA_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeFlyByWireA,0);
    /// <summary>
    /// Like FBWA, but with automatic height and speed control
    /// </summary>
    public static OpMode FlyByWireB  = new("Fly by wire B", RS.ArdupilotPlaneMode_FlyByWireB_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeFlyByWireB,0);
    /// <summary>
    /// Like FBWB, but with ground course tracking and terrain following
    /// </summary>
    public static OpMode Cruise      = new("Cruise", RS.ArdupilotPlaneMode_Cruise_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeCruise,0);
    /// <summary>
    /// Like FBWA, but learns attitude tuning while flying
    /// </summary>
    public static OpMode Autotune    = new("Autotune", RS.ArdupilotPlaneMode_Autotune_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeAutotune,0);
    /// <summary>
    /// Follows Mission
    /// </summary>
    public static OpMode Auto        = new("Auto", RS.ArdupilotPlaneMode_Auto_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeAuto,0);
    /// <summary>
    /// Returns to and circles home or rally point
    /// </summary>
    public static OpMode Rtl         = new("RTL", RS.ArdupilotPlaneMode_Rtl_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeRtl,0);
    /// <summary>
    /// Circles point where mode switched
    /// </summary>
    public static OpMode Loiter      = new("Loiter", RS.ArdupilotPlaneMode_Loiter_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeLoiter,0);
    /// <summary>
    /// Circles user defined point from GCS
    /// </summary>
    public static OpMode Guided      = new("Guided", RS.ArdupilotPlaneMode_Guided_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeGuided,0);
    
    #endregion

    protected override OpMode Convert(HeartbeatPayload hb)
    {
        return AllModes.FirstOrDefault(x => x.CustomMode == hb.CustomMode) ?? OpMode.Unknown;
    }

    public override IEnumerable<OpMode> AvailableModes => AllModes;
    
}