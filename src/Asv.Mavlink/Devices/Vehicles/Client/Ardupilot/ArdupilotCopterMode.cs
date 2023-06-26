#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.V2.Ardupilotmega;

namespace Asv.Mavlink;

public class ArdupilotCopterMode:VehicleMode
{
    #region Static
    
    public static ImmutableArray<IVehicleMode> AllModes { get; }
 
    static ArdupilotCopterMode()
    {
        var wellKnownModes = new HashSet<CopterMode>(WellKnownModes.Select(_=>_.CustomMode));
        var allModes = new List<IVehicleMode>();
        foreach (var copterMode in Enum.GetValues<CopterMode>())
        {
            if (wellKnownModes.Contains(copterMode)) continue;
            // this is no well known mode, try to create description from enum
            allModes.Add(new ArdupilotCopterMode(copterMode.ToString("G"), String.Empty, copterMode));
        }
        allModes.AddRange(WellKnownModes);
        AllModes = allModes.ToImmutableArray();
    }

    private static IEnumerable<ArdupilotCopterMode> WellKnownModes 
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
            yield return SmartRtl;
        }
    }

    /// <summary>
    /// Wing-leveling on stick release
    /// </summary>
    public static ArdupilotCopterMode Stabilize   = new("Stabilize", RS.ArdupilotCopterMode_Stabilize_Description, CopterMode.CopterModeStabilize);
    /// <summary>
    /// Holds attitude, no self-level
    /// </summary>
    public static ArdupilotCopterMode Acro        = new("Acro", RS.ArdupilotCopterMode_Acro_Description, CopterMode.CopterModeAcro);
    /// <summary>
    /// Holds altitude and self-levels the roll & pitch
    /// </summary>
    public static ArdupilotCopterMode AltHold     = new("AltHold", RS.ArdupilotCopterMode_AltHold_Description, CopterMode.CopterModeAltHold);
    /// <summary>
    /// Executes pre-defined mission
    /// </summary>
    public static ArdupilotCopterMode Auto        = new("Auto", RS.ArdupilotCopterMode_Auto_Description, CopterMode.CopterModeAuto);
    /// <summary>
    /// Navigates to single points commanded by GCS
    /// </summary>
    public static ArdupilotCopterMode Guided      = new("Guided", RS.ArdupilotCopterMode_Guided_Description, CopterMode.CopterModeGuided);
    /// <summary>
    /// Holds altitude and position, uses GPS for movements
    /// </summary>
    public static ArdupilotCopterMode Loiter      = new("Loiter", RS.ArdupilotCopterMode_Loiter_Description, CopterMode.CopterModeLoiter);
    /// <summary>
    /// Returns above takeoff location, may also include landing
    /// </summary>
    public static ArdupilotCopterMode Rtl         = new("RTL", RS.ArdupilotCopterMode_Rtl_Description, CopterMode.CopterModeRtl);
    /// <summary>
    /// Automatically circles a point in front of the vehicle
    /// </summary>
    public static ArdupilotCopterMode Circle      = new("Circle", RS.ArdupilotCopterMode_Circle_Description, CopterMode.CopterModeCircle);
    /// <summary>
    /// Reduces altitude to ground level, attempts to go straight down
    /// </summary>
    public static ArdupilotCopterMode Land        = new("Land", RS.ArdupilotCopterMode_Land_Description, CopterMode.CopterModeLand);
    /// <summary>
    /// Like stabilize, but coordinates yaw with roll like a plane
    /// </summary>
    public static ArdupilotCopterMode Drift       = new("Drift", RS.ArdupilotCopterMode_Drift_Description, CopterMode.CopterModeDrift);
    /// <summary>
    /// Alt-hold, but holds pitch & roll when sticks centered
    /// </summary>
    public static ArdupilotCopterMode Sport       = new("Sport", RS.ArdupilotCopterMode_Sport_Description, CopterMode.CopterModeSport);
    /// <summary>
    /// Rises and completes an automated flip
    /// </summary>
    public static ArdupilotCopterMode Flip        = new("Flip", RS.ArdupilotCopterMode_Flip_Description, CopterMode.CopterModeFlip);
    /// <summary>
    /// Automated pitch and bank procedure to improve control loops
    /// </summary>
    public static ArdupilotCopterMode AutoTune    = new("AutoTune", RS.ArdupilotCopterMode_AutoTune_Description, CopterMode.CopterModeAutotune);
    /// <summary>
    /// Like loiter, but manual roll and pitch when sticks not centered
    /// </summary>
    public static ArdupilotCopterMode PosHold     = new("PosHold", RS.ArdupilotCopterMode_PosHold_Description, CopterMode.CopterModePoshold);
    /// <summary>
    /// Brings copter to an immediate stop
    /// </summary>
    public static ArdupilotCopterMode Brake       = new("Brake", RS.ArdupilotCopterMode_Brake_Description, CopterMode.CopterModeBrake);
    /// <summary>
    /// Holds position after a throwing takeoff
    /// </summary>
    public static ArdupilotCopterMode Throw       = new("Throw", RS.ArdupilotCopterMode_Throw_Description, CopterMode.CopterModeThrow);
    /// <summary>
    /// Same as Guided, but uses only attitude targets for navigation
    /// </summary>
    public static ArdupilotCopterMode GuidedNoGps = new("GuidedNoGps", RS.ArdupilotCopterMode_GuidedNoGps_Description, CopterMode.CopterModeGuidedNogps);
    /// <summary>
    /// RTL, but traces path to get home
    /// </summary>
    public static ArdupilotCopterMode SmartRtl    = new("SmartRtl", RS.ArdupilotCopterMode_SmartRtl_Description, CopterMode.CopterModeSmartRtl);
    
    #endregion
    
    public ArdupilotCopterMode(string name, string description, CopterMode customModeValue) : base(name, description)
    {
        CustomMode = customModeValue;
    }
    public CopterMode CustomMode { get; }
   
}

