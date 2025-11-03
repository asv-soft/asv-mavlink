using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.Ardupilotmega;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;

namespace Asv.Mavlink;

public class ArduCopterMode:ICustomMode
{
    #region Static

    public static ImmutableArray<ArduCopterMode> AllModes { get; }
 
    static ArduCopterMode()
    {
        var wellKnownModes = new HashSet<CopterMode>(WellKnownModes.Select(m=>m.CustomMode));
        var allModes = new List<ArduCopterMode>();
        allModes.AddRange(WellKnownModes);
        foreach (var copterMode in Enum.GetValues<CopterMode>())
        {
            if (wellKnownModes.Contains(copterMode)) continue;
            // this is not "well known" mode, try to create description from enum
            allModes.Add(new ArduCopterMode(copterMode.ToString("G"), string.Empty, true, MavModeFlag.MavModeFlagCustomModeEnabled, copterMode));
        }
        AllModes = [..allModes];
    }

    private static IEnumerable<ArduCopterMode> WellKnownModes 
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

    public static ArduCopterMode Unknown   = new("Unknown", "Unknown copter mode", false, 0, (CopterMode)uint.MaxValue);
    /// <summary>
    /// Wing-leveling on stick release
    /// </summary>
    public static ArduCopterMode Stabilize   = new("Stabilize", RS.ArdupilotCopterMode_Stabilize_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeStabilize);
    /// <summary>
    /// Holds attitude, no self-level
    /// </summary>
    public static ArduCopterMode Acro        = new("Acro", RS.ArdupilotCopterMode_Acro_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeAcro);
    /// <summary>
    /// Holds altitude and self-levels the roll & pitch
    /// </summary>
    public static ArduCopterMode AltHold     = new("AltHold", RS.ArdupilotCopterMode_AltHold_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeAltHold);
    /// <summary>
    /// Executes pre-defined mission
    /// </summary>
    public static ArduCopterMode Auto        = new("Auto", RS.ArdupilotCopterMode_Auto_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeAuto);
    /// <summary>
    /// Navigates to single points commanded by GCS
    /// </summary>
    public static ArduCopterMode Guided      = new("Guided", RS.ArdupilotCopterMode_Guided_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeGuided);
    /// <summary>
    /// Holds altitude and position, uses GPS for movements
    /// </summary>
    public static ArduCopterMode Loiter      = new("Loiter", RS.ArdupilotCopterMode_Loiter_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeLoiter);
    /// <summary>
    /// Returns above takeoff location, may also include landing
    /// </summary>
    public static ArduCopterMode Rtl         = new("RTL", RS.ArdupilotCopterMode_Rtl_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeRtl);
    /// <summary>
    /// Automatically circles a point in front of the vehicle
    /// </summary>
    public static ArduCopterMode Circle      = new("Circle", RS.ArdupilotCopterMode_Circle_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeCircle);
    /// <summary>
    /// Reduces altitude to ground level, attempts to go straight down
    /// </summary>
    public static ArduCopterMode Land        = new("Land", RS.ArdupilotCopterMode_Land_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeLand);
    /// <summary>
    /// Like stabilize, but coordinates yaw with roll like a plane
    /// </summary>
    public static ArduCopterMode Drift       = new("Drift", RS.ArdupilotCopterMode_Drift_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeDrift);
    /// <summary>
    /// Alt-hold, but holds pitch & roll when sticks centered
    /// </summary>
    public static ArduCopterMode Sport       = new("Sport", RS.ArdupilotCopterMode_Sport_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeSport);
    /// <summary>
    /// Rises and completes an automated flip
    /// </summary>
    public static ArduCopterMode Flip        = new("Flip", RS.ArdupilotCopterMode_Flip_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeFlip);
    /// <summary>
    /// Automated pitch and bank procedure to improve control loops
    /// </summary>
    public static ArduCopterMode AutoTune    = new("AutoTune", RS.ArdupilotCopterMode_AutoTune_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeAutotune);
    /// <summary>
    /// Like loiter, but manual roll and pitch when sticks not centered
    /// </summary>
    public static ArduCopterMode PosHold     = new("PosHold", RS.ArdupilotCopterMode_PosHold_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModePoshold);
    /// <summary>
    /// Brings copter to an immediate stop
    /// </summary>
    public static ArduCopterMode Brake       = new("Brake", RS.ArdupilotCopterMode_Brake_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeBrake);
    /// <summary>
    /// Holds position after a throwing takeoff
    /// </summary>
    public static ArduCopterMode Throw       = new("Throw", RS.ArdupilotCopterMode_Throw_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeThrow);
    /// <summary>
    /// Same as Guided, but uses only attitude targets for navigation
    /// </summary>
    public static ArduCopterMode GuidedNoGps = new("GuidedNoGps", RS.ArdupilotCopterMode_GuidedNoGps_Description, true, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeGuidedNogps);
    /// <summary>
    /// ADS-B based avoidance of manned aircraft. Should not be set-up as a pilot selectable flight mode.
    /// </summary>
    public static ArduCopterMode AvoidAdsb = new("AvoidAdsb", RS.ArdupilotCopterMode_AvoidAdsb_Description, true, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeAvoidAdsb);
    /// <summary>
    /// RTL, but traces path to get home
    /// </summary>
    public static ArduCopterMode SmartRtl    = new("SmartRtl", RS.ArdupilotCopterMode_SmartRtl_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, CopterMode.CopterModeSmartRtl);
    
    #endregion
    
    public ArduCopterMode(string name, string description, bool internalMode, MavModeFlag mode, CopterMode customMode)
    {
        Mode = mode;
        CustomMode = customMode;
        Name = name;
        Description = description;
        InternalMode = internalMode;
    }
    public MavModeFlag Mode { get; }
    public CopterMode CustomMode { get; }
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