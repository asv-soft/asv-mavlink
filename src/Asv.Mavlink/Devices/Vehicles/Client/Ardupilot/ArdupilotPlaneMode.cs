#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.V2.Ardupilotmega;

namespace Asv.Mavlink;

public class ArdupilotPlaneMode:VehicleMode
{
    #region Static
    
    public static ImmutableArray<IVehicleMode> AllModes { get; }
 
    static ArdupilotPlaneMode()
    {
        var wellKnownModes = new HashSet<PlaneMode>(WellKnownModes.Select(_=>_.CustomMode));
        var allModes = new List<IVehicleMode>();
        foreach (var copterMode in Enum.GetValues<PlaneMode>())
        {
            if (wellKnownModes.Contains(copterMode)) continue;
            // this is no well known mode, try to create description from enum
            allModes.Add(new ArdupilotPlaneMode(copterMode.ToString("G"), String.Empty, copterMode));
        }
        AllModes = allModes.ToImmutableArray();
    }

    private static IEnumerable<ArdupilotPlaneMode> WellKnownModes 
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
        }
    }

    /// <summary>
    /// Manual control surface movement, passthrough
    /// </summary>
    public static ArdupilotPlaneMode Manual      = new("Manual", "Manual", PlaneMode.PlaneModeManual);
    /// <summary>
    /// Gently turns aircraft
    /// </summary>
    public static ArdupilotPlaneMode Circle      = new("Circle", "Circle", PlaneMode.PlaneModeCircle);
    /// <summary>
    /// Wing-leveling on stick release
    /// </summary>
    public static ArdupilotPlaneMode Stabilize   = new("Stabilize", "Stabilize", PlaneMode.PlaneModeStabilize);
    /// <summary>
    /// Manual control up to roll and pitch limits
    /// </summary>
    public static ArdupilotPlaneMode Training    = new("Training", "Training", PlaneMode.PlaneModeTraining);
    /// <summary>
    /// Rate controlled mode with no attitude limits
    /// </summary>
    public static ArdupilotPlaneMode Acro        = new("Acro", "Acro", PlaneMode.PlaneModeAcro);
    /// <summary>
    /// Roll and pitch follow stick input, up to set limits
    /// </summary>
    public static ArdupilotPlaneMode FlyByWireA  = new("Fly by wire A", "Fly by wire A", PlaneMode.PlaneModeFlyByWireA);
    /// <summary>
    /// Like FBWA, but with automatic height and speed control
    /// </summary>
    public static ArdupilotPlaneMode FlyByWireB  = new("Fly by wire B", "Fly by wire B", PlaneMode.PlaneModeFlyByWireB);
    /// <summary>
    /// Like FBWB, but with ground course tracking and terrain following
    /// </summary>
    public static ArdupilotPlaneMode Cruise      = new("Cruise", "Cruise", PlaneMode.PlaneModeCruise);
    /// <summary>
    /// Like FBWA, but learns attitude tuning while flying
    /// </summary>
    public static ArdupilotPlaneMode Autotune    = new("Autotune", "Autotune", PlaneMode.PlaneModeAutotune);
    /// <summary>
    /// Follows Mission
    /// </summary>
    public static ArdupilotPlaneMode Auto        = new("Auto", "Auto", PlaneMode.PlaneModeAuto);
    /// <summary>
    /// Returns to and circles home or rally point
    /// </summary>
    public static ArdupilotPlaneMode Rtl         = new("RTL", "RTL", PlaneMode.PlaneModeRtl);
    /// <summary>
    /// Circles point where mode switched
    /// </summary>
    public static ArdupilotPlaneMode Loiter      = new("Loiter", "Loiter", PlaneMode.PlaneModeLoiter);
    /// <summary>
    /// Circles user defined point from GCS
    /// </summary>
    public static ArdupilotPlaneMode Guided      = new("Guided", "Guided", PlaneMode.PlaneModeGuided);
    /// <summary>
    ///  Allows you to fly your vehicle manually, but self-levels the roll and pitch axis
    /// </summary>
    public static ArdupilotPlaneMode Qstabilize  = new("Quad Stabilize", "Qstabilize", PlaneMode.PlaneModeQstabilize);
    /// <summary>
    /// QuadPlane maintains a consistent altitude while allowing roll, pitch, and yaw to be controlled normally
    /// </summary>
    public static ArdupilotPlaneMode Qhover      = new("Quad Hover", "Qhover", PlaneMode.PlaneModeQhover);
    /// <summary>
    /// Automatically attempts to maintain the current location, heading and altitude
    /// </summary>
    public static ArdupilotPlaneMode Qloiter     = new("Quad Loiter", "Qloiter", PlaneMode.PlaneModeQloiter);
    /// <summary>
    /// Attempts to bring the QuadPlane straight down at the position the vehicle is located when the mode is entered
    /// </summary>
    public static ArdupilotPlaneMode Qland       = new("Quad Land", "Qland", PlaneMode.PlaneModeQland);
    /// <summary>
    /// Navigates QuadPlane from its current position to hover above the home position and then land
    /// </summary>
    public static ArdupilotPlaneMode Qrtl        = new("Quad RTL", "Qrtl", PlaneMode.PlaneModeQrtl);
    /// <summary>
    /// Automated pitch and bank procedure to improve control loops
    /// </summary>
    public static ArdupilotPlaneMode Qautotune   = new("Qautotune", "Qautotune", PlaneMode.PlaneModeQautotune);
   
    
    #endregion
    
    public ArdupilotPlaneMode(string name, string description, PlaneMode customModeValue) : base(name, description)
    {
        CustomMode = customModeValue;
    }
    public PlaneMode CustomMode { get; }
   
}