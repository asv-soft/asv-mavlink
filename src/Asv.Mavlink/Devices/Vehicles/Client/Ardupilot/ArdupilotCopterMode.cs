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
            yield return SmartRtl;
        }
    }

    /// <summary>
    /// Wing-leveling on stick release
    /// </summary>
    public static ArdupilotCopterMode Stabilize   = new("Stabilize", "Stabilize", CopterMode.CopterModeStabilize);
    /// <summary>
    /// Holds attitude, no self-level
    /// </summary>
    public static ArdupilotCopterMode Acro        = new("Acro", "Acro", CopterMode.CopterModeAcro);
    /// <summary>
    /// Holds altitude and self-levels the roll & pitch
    /// </summary>
    public static ArdupilotCopterMode AltHold     = new("AltHold", "AltHold", CopterMode.CopterModeAltHold);
    /// <summary>
    /// Executes pre-defined mission
    /// </summary>
    public static ArdupilotCopterMode Auto        = new("Auto", "Auto", CopterMode.CopterModeAuto);
    /// <summary>
    /// Navigates to single points commanded by GCS
    /// </summary>
    public static ArdupilotCopterMode Guided      = new("Guided", "Guided", CopterMode.CopterModeGuided);
    /// <summary>
    /// Holds altitude and position, uses GPS for movements
    /// </summary>
    public static ArdupilotCopterMode Loiter      = new("Loiter", "Loiter", CopterMode.CopterModeLoiter);
    /// <summary>
    /// Returns above takeoff location, may also include landing
    /// </summary>
    public static ArdupilotCopterMode Rtl         = new("RTL", "RTL", CopterMode.CopterModeRtl);
    /// <summary>
    /// Automatically circles a point in front of the vehicle
    /// </summary>
    public static ArdupilotCopterMode Circle      = new("Circle", "Circle", CopterMode.CopterModeCircle);
    /// <summary>
    /// Reduces altitude to ground level, attempts to go straight down
    /// </summary>
    public static ArdupilotCopterMode Land        = new("Land", "Land", CopterMode.CopterModeLand);
    /// <summary>
    /// Like stabilize, but coordinates yaw with roll like a plane
    /// </summary>
    public static ArdupilotCopterMode Drift       = new("Drift", "Drift", CopterMode.CopterModeDrift);
    /// <summary>
    /// Alt-hold, but holds pitch & roll when sticks centered
    /// </summary>
    public static ArdupilotCopterMode Sport       = new("Sport", "Sport", CopterMode.CopterModeSport);
    /// <summary>
    /// Rises and completes an automated flip
    /// </summary>
    public static ArdupilotCopterMode Flip        = new("Flip", "Flip", CopterMode.CopterModeFlip);
    /// <summary>
    /// Automated pitch and bank procedure to improve control loops
    /// </summary>
    public static ArdupilotCopterMode AutoTune    = new("AutoTune", "AutoTune", CopterMode.CopterModeAutotune);
    /// <summary>
    /// Like loiter, but manual roll and pitch when sticks not centered
    /// </summary>
    public static ArdupilotCopterMode PosHold     = new("PosHold", "PosHold", CopterMode.CopterModePoshold);
    /// <summary>
    /// Brings copter to an immediate stop
    /// </summary>
    public static ArdupilotCopterMode Brake       = new("Brake", "Brake", CopterMode.CopterModeBrake);
    /// <summary>
    /// Holds position after a throwing takeoff
    /// </summary>
    public static ArdupilotCopterMode Throw       = new("Throw", "Throw", CopterMode.CopterModeThrow);
    /// <summary>
    /// RTL, but traces path to get home
    /// </summary>
    public static ArdupilotCopterMode SmartRtl    = new("SmartRtl", "SmartRtl", CopterMode.CopterModeSmartRtl);
    
    #endregion
    
    public ArdupilotCopterMode(string name, string description, CopterMode customModeValue) : base(name, description)
    {
        CustomMode = customModeValue;
    }
    public CopterMode CustomMode { get; }
   
}

