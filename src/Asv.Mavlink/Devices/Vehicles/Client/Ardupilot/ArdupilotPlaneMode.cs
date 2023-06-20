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
            yield return AvoidAdsb;
            yield return Guided;
            yield return Initialising;
            yield return Qstabilize;
            yield return Qhover;
            yield return Qloiter;
            yield return Qland;
            yield return Qrtl;
            yield return Qautotune;
        }
    }

    public static ArdupilotPlaneMode Manual      = new("Manual", "Manual", PlaneMode.PlaneModeManual);
    public static ArdupilotPlaneMode Circle      = new("Circle", "Circle", PlaneMode.PlaneModeCircle);
    public static ArdupilotPlaneMode Stabilize   = new("Stabilize", "Stabilize", PlaneMode.PlaneModeStabilize);
    public static ArdupilotPlaneMode Training    = new("Training", "Training", PlaneMode.PlaneModeTraining);
    public static ArdupilotPlaneMode Acro        = new("Acro", "Acro", PlaneMode.PlaneModeAcro);
    public static ArdupilotPlaneMode FlyByWireA  = new("Fly by wire A", "Fly by wire A", PlaneMode.PlaneModeFlyByWireA);
    public static ArdupilotPlaneMode FlyByWireB  = new("Fly by wire B", "Fly by wire B", PlaneMode.PlaneModeFlyByWireB);
    public static ArdupilotPlaneMode Cruise      = new("Cruise", "Cruise", PlaneMode.PlaneModeCruise);
    public static ArdupilotPlaneMode Autotune    = new("Autotune", "Autotune", PlaneMode.PlaneModeAutotune);
    public static ArdupilotPlaneMode Auto        = new("Auto", "Auto", PlaneMode.PlaneModeAuto);
    public static ArdupilotPlaneMode Rtl         = new("RTL", "RTL", PlaneMode.PlaneModeRtl);
    public static ArdupilotPlaneMode Loiter      = new("Loiter", "Loiter", PlaneMode.PlaneModeLoiter);
    public static ArdupilotPlaneMode AvoidAdsb   = new("Avoid Adsb", "AvoidAdsb", PlaneMode.PlaneModeAvoidAdsb);
    public static ArdupilotPlaneMode Guided      = new("Guided", "Guided", PlaneMode.PlaneModeGuided);
    public static ArdupilotPlaneMode Initialising= new("Initialising", "Initialising", PlaneMode.PlaneModeInitializing);
    public static ArdupilotPlaneMode Qstabilize  = new("Quad Stabilize", "Qstabilize", PlaneMode.PlaneModeQstabilize);
    public static ArdupilotPlaneMode Qhover      = new("Quad Hover", "Qhover", PlaneMode.PlaneModeQhover);
    public static ArdupilotPlaneMode Qloiter     = new("Quad Loiter", "Qloiter", PlaneMode.PlaneModeQloiter);
    public static ArdupilotPlaneMode Qland       = new("Quad Land", "Qland", PlaneMode.PlaneModeQland);
    public static ArdupilotPlaneMode Qrtl        = new("Quad RTL", "Qrtl", PlaneMode.PlaneModeQrtl);
    public static ArdupilotPlaneMode Qautotune   = new("Qautotune", "Qautotune", PlaneMode.PlaneModeQautotune);
   
    
    #endregion
    
    public ArdupilotPlaneMode(string name, string description, PlaneMode customModeValue) : base(name, description)
    {
        CustomMode = customModeValue;
    }
    public PlaneMode CustomMode { get; }
   
}