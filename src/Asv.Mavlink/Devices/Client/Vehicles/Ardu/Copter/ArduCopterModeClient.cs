using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.Ardupilotmega;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.V2.Ardupilotmega;


namespace Asv.Mavlink;

public class ArduCopterModeClient : ModeClient
{
    #region Static

    public static ImmutableArray<OpMode> AllModes { get; }
 
    static ArduCopterModeClient()
    {
        var wellKnownModes = new HashSet<uint>(WellKnownModes.Select(m=>m.CustomMode));
        var allModes = new List<OpMode>();
        allModes.AddRange(WellKnownModes);
        foreach (var copterMode in Enum.GetValues<CopterMode>())
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
            yield return Stabilize;
            yield return Acro;
            yield return AltHold;
            yield return Auto;
            yield return Guided;
            yield return Loiter;
            yield return Rtl;
            yield return Circle;
            yield return Land;
            yield return Drift;
            yield return Sport;
            yield return Flip;
            yield return AutoTune;
            yield return PosHold;
            yield return PosHold;
            yield return Brake;
            yield return Throw;
            yield return GuidedNoGps;
            yield return AvoidAdsb;
            yield return SmartRtl;
        }
    }

    /// <summary>
    /// Wing-leveling on stick release
    /// </summary>
    public static OpMode Stabilize   = new("Stabilize", RS.ArdupilotCopterMode_Stabilize_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeStabilize,0);
    /// <summary>
    /// Holds attitude, no self-level
    /// </summary>
    public static OpMode Acro        = new("Acro", RS.ArdupilotCopterMode_Acro_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeAcro,0);
    /// <summary>
    /// Holds altitude and self-levels the roll & pitch
    /// </summary>
    public static OpMode AltHold     = new("AltHold", RS.ArdupilotCopterMode_AltHold_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeAltHold,0);
    /// <summary>
    /// Executes pre-defined mission
    /// </summary>
    public static OpMode Auto        = new("Auto", RS.ArdupilotCopterMode_Auto_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeAuto,0);
    /// <summary>
    /// Navigates to single points commanded by GCS
    /// </summary>
    public static OpMode Guided      = new("Guided", RS.ArdupilotCopterMode_Guided_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeGuided,0);
    /// <summary>
    /// Holds altitude and position, uses GPS for movements
    /// </summary>
    public static OpMode Loiter      = new("Loiter", RS.ArdupilotCopterMode_Loiter_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeLoiter,0);
    /// <summary>
    /// Returns above takeoff location, may also include landing
    /// </summary>
    public static OpMode Rtl         = new("RTL", RS.ArdupilotCopterMode_Rtl_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeRtl,0);
    /// <summary>
    /// Automatically circles a point in front of the vehicle
    /// </summary>
    public static OpMode Circle      = new("Circle", RS.ArdupilotCopterMode_Circle_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeCircle,0);
    /// <summary>
    /// Reduces altitude to ground level, attempts to go straight down
    /// </summary>
    public static OpMode Land        = new("Land", RS.ArdupilotCopterMode_Land_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeLand,0);
    /// <summary>
    /// Like stabilize, but coordinates yaw with roll like a plane
    /// </summary>
    public static OpMode Drift       = new("Drift", RS.ArdupilotCopterMode_Drift_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeDrift,0);
    /// <summary>
    /// Alt-hold, but holds pitch & roll when sticks centered
    /// </summary>
    public static OpMode Sport       = new("Sport", RS.ArdupilotCopterMode_Sport_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeSport,0);
    /// <summary>
    /// Rises and completes an automated flip
    /// </summary>
    public static OpMode Flip        = new("Flip", RS.ArdupilotCopterMode_Flip_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeFlip,0);
    /// <summary>
    /// Automated pitch and bank procedure to improve control loops
    /// </summary>
    public static OpMode AutoTune    = new("AutoTune", RS.ArdupilotCopterMode_AutoTune_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeAutotune,0);
    /// <summary>
    /// Like loiter, but manual roll and pitch when sticks not centered
    /// </summary>
    public static OpMode PosHold     = new("PosHold", RS.ArdupilotCopterMode_PosHold_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModePoshold,0);
    /// <summary>
    /// Brings copter to an immediate stop
    /// </summary>
    public static OpMode Brake       = new("Brake", RS.ArdupilotCopterMode_Brake_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeBrake,0);
    /// <summary>
    /// Holds position after a throwing takeoff
    /// </summary>
    public static OpMode Throw       = new("Throw", RS.ArdupilotCopterMode_Throw_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeThrow,0);
    /// <summary>
    /// Same as Guided, but uses only attitude targets for navigation
    /// </summary>
    public static OpMode GuidedNoGps = new("GuidedNoGps", RS.ArdupilotCopterMode_GuidedNoGps_Description, true, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeGuidedNogps,0);
    /// <summary>
    /// ADS-B based avoidance of manned aircraft. Should not be set-up as a pilot selectable flight mode.
    /// </summary>
    public static OpMode AvoidAdsb = new("AvoidAdsb", RS.ArdupilotCopterMode_AvoidAdsb_Description, true, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeAvoidAdsb,0);
    /// <summary>
    /// RTL, but traces path to get home
    /// </summary>
    public static OpMode SmartRtl    = new("SmartRtl", RS.ArdupilotCopterMode_SmartRtl_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)CopterMode.CopterModeSmartRtl,0);
    
    #endregion
    
    public ArduCopterModeClient(IHeartbeatClient heartbeat, ICommandClient command) 
        : base(heartbeat, command)
    {
    }

    protected override OpMode Convert(HeartbeatPayload? hb)
    {
        if (hb == null) return OpMode.Unknown;
        return AllModes.FirstOrDefault(x => x.CustomMode == hb.CustomMode) ?? OpMode.Unknown;
    }

    public override IEnumerable<OpMode> AvailableModes => AllModes;
    
}