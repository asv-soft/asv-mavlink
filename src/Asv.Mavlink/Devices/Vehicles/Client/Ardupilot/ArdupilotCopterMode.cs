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
            yield return AvoidAdsb;
            yield return GuidedNoGps;
            yield return SmartRtl;
        }
    }

    public static ArdupilotCopterMode Stabilize = new("Stabilize", "Stabilize", CopterMode.CopterModeStabilize);
    public static ArdupilotCopterMode Acro = new("Acro", "Acro", CopterMode.CopterModeAcro);
    public static ArdupilotCopterMode AltHold = new("AltHold", "AltHold", CopterMode.CopterModeAltHold);
    public static ArdupilotCopterMode Auto = new("Auto", "Auto", CopterMode.CopterModeAuto);
    public static ArdupilotCopterMode Guided = new("Guided", "Guided", CopterMode.CopterModeGuided);
    public static ArdupilotCopterMode Loiter = new("Loiter", "Loiter", CopterMode.CopterModeLoiter);
    public static ArdupilotCopterMode Rtl = new("RTL", "RTL", CopterMode.CopterModeRtl);
    public static ArdupilotCopterMode Circle = new("Circle", "Circle", CopterMode.CopterModeCircle);
    public static ArdupilotCopterMode Land = new("Land", "Land", CopterMode.CopterModeLand);
    public static ArdupilotCopterMode Drift = new("Drift", "Drift", CopterMode.CopterModeDrift);
    public static ArdupilotCopterMode Sport = new("Sport", "Sport", CopterMode.CopterModeSport);
    public static ArdupilotCopterMode Flip = new("Flip", "Flip", CopterMode.CopterModeFlip);
    public static ArdupilotCopterMode AutoTune = new("AutoTune", "AutoTune", CopterMode.CopterModeAutotune);
    public static ArdupilotCopterMode PosHold = new("PosHold", "PosHold", CopterMode.CopterModePoshold);
    public static ArdupilotCopterMode Brake = new("Brake", "Brake", CopterMode.CopterModeBrake);
    public static ArdupilotCopterMode Throw = new("Throw", "Throw", CopterMode.CopterModeThrow);
    public static ArdupilotCopterMode AvoidAdsb = new("AvoidAdsb", "AvoidAdsb", CopterMode.CopterModeAvoidAdsb);
    public static ArdupilotCopterMode GuidedNoGps = new("GuidedNoGps", "GuidedNoRc", CopterMode.CopterModeGuidedNogps);
    public static ArdupilotCopterMode SmartRtl = new("SmartRtl", "SmartRtl", CopterMode.CopterModeSmartRtl);
    
    #endregion
    
    public ArdupilotCopterMode(string name, string description, CopterMode customModeValue) : base(name, description)
    {
        CustomMode = customModeValue;
    }
    public CopterMode CustomMode { get; }
   
}

