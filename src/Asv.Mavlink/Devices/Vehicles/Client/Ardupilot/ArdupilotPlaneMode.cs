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
        allModes.AddRange(WellKnownModes);
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
    public static ArdupilotPlaneMode Manual      = new("Manual", RS.ArdupilotPlaneMode_Manual_Description, PlaneMode.PlaneModeManual);
    /// <summary>
    /// Gently turns aircraft
    /// </summary>
    public static ArdupilotPlaneMode Circle      = new("Circle", RS.ArdupilotPlaneMode_Circle_Description, PlaneMode.PlaneModeCircle);
    /// <summary>
    /// Wing-leveling on stick release
    /// </summary>
    public static ArdupilotPlaneMode Stabilize   = new("Stabilize", RS.ArdupilotPlaneMode_Stabilize_Description, PlaneMode.PlaneModeStabilize);
    /// <summary>
    /// Manual control up to roll and pitch limits
    /// </summary>
    public static ArdupilotPlaneMode Training    = new("Training", RS.ArdupilotPlaneMode_Training_Description, PlaneMode.PlaneModeTraining);
    /// <summary>
    /// Rate controlled mode with no attitude limits
    /// </summary>
    public static ArdupilotPlaneMode Acro        = new("Acro", RS.ArdupilotPlaneMode_Acro_Description, PlaneMode.PlaneModeAcro);
    /// <summary>
    /// Roll and pitch follow stick input, up to set limits
    /// </summary>
    public static ArdupilotPlaneMode FlyByWireA  = new("Fly by wire A", RS.ArdupilotPlaneMode_FlyByWireA_Description, PlaneMode.PlaneModeFlyByWireA);
    /// <summary>
    /// Like FBWA, but with automatic height and speed control
    /// </summary>
    public static ArdupilotPlaneMode FlyByWireB  = new("Fly by wire B", RS.ArdupilotPlaneMode_FlyByWireB_Description, PlaneMode.PlaneModeFlyByWireB);
    /// <summary>
    /// Like FBWB, but with ground course tracking and terrain following
    /// </summary>
    public static ArdupilotPlaneMode Cruise      = new("Cruise", RS.ArdupilotPlaneMode_Cruise_Description, PlaneMode.PlaneModeCruise);
    /// <summary>
    /// Like FBWA, but learns attitude tuning while flying
    /// </summary>
    public static ArdupilotPlaneMode Autotune    = new("Autotune", RS.ArdupilotPlaneMode_Autotune_Description, PlaneMode.PlaneModeAutotune);
    /// <summary>
    /// Follows Mission
    /// </summary>
    public static ArdupilotPlaneMode Auto        = new("Auto", RS.ArdupilotPlaneMode_Auto_Description, PlaneMode.PlaneModeAuto);
    /// <summary>
    /// Returns to and circles home or rally point
    /// </summary>
    public static ArdupilotPlaneMode Rtl         = new("RTL", RS.ArdupilotPlaneMode_Rtl_Description, PlaneMode.PlaneModeRtl);
    /// <summary>
    /// Circles point where mode switched
    /// </summary>
    public static ArdupilotPlaneMode Loiter      = new("Loiter", RS.ArdupilotPlaneMode_Loiter_Description, PlaneMode.PlaneModeLoiter);
    /// <summary>
    /// Circles user defined point from GCS
    /// </summary>
    public static ArdupilotPlaneMode Guided      = new("Guided", RS.ArdupilotPlaneMode_Guided_Description, PlaneMode.PlaneModeGuided);
    /// <summary>
    ///  Allows you to fly your vehicle manually, but self-levels the roll and pitch axis
    /// </summary>
    public static ArdupilotPlaneMode Qstabilize  = new("Quad Stabilize", RS.ArdupilotPlaneMode_Qstabilize_Description, PlaneMode.PlaneModeQstabilize);
    /// <summary>
    /// QuadPlane maintains a consistent altitude while allowing roll, pitch, and yaw to be controlled normally
    /// </summary>
    public static ArdupilotPlaneMode Qhover      = new("Quad Hover", RS.ArdupilotPlaneMode_Qhover_Description, PlaneMode.PlaneModeQhover);
    /// <summary>
    /// Automatically attempts to maintain the current location, heading and altitude
    /// </summary>
    public static ArdupilotPlaneMode Qloiter     = new("Quad Loiter", RS.ArdupilotPlaneMode_Qloiter_Description, PlaneMode.PlaneModeQloiter);
    /// <summary>
    /// Attempts to bring the QuadPlane straight down at the position the vehicle is located when the mode is entered
    /// </summary>
    public static ArdupilotPlaneMode Qland       = new("Quad Land", RS.ArdupilotPlaneMode_Qland_Description, PlaneMode.PlaneModeQland);
    /// <summary>
    /// Navigates QuadPlane from its current position to hover above the home position and then land
    /// </summary>
    public static ArdupilotPlaneMode Qrtl        = new("Quad RTL", RS.ArdupilotPlaneMode_Qrtl_Description, PlaneMode.PlaneModeQrtl);
    /// <summary>
    /// Automated pitch and bank procedure to improve control loops
    /// </summary>
    public static ArdupilotPlaneMode Qautotune   = new("Qautotune", RS.ArdupilotPlaneMode_Qautotune_Description, PlaneMode.PlaneModeQautotune);
   
    
    #endregion
    
    public ArdupilotPlaneMode(string name, string description, PlaneMode customModeValue, bool internalMode = false) : base(name, description, internalMode)
    {
        CustomMode = customModeValue;
    }
    public PlaneMode CustomMode { get; }
   
}