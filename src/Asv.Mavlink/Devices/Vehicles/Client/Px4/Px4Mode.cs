using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

/*
public class Px4Mode:VehicleMode,IEquatable<Px4VehicleMode>, IEquatable<Px4Mode>
{
    #region Static
    
    public static ImmutableArray<IVehicleMode> AllModes { get; }
    
    static Px4Mode()
    {
        var wellKnownModes = new HashSet<CopterMode>(WellKnownModes.Select(_=>_.CustomMode));
        var allModes = new List<IVehicleMode>();
        allModes.AddRange(WellKnownModes);
        foreach (var copterMode in Enum.GetValues<CopterMode>())
        {
            if (wellKnownModes.Contains(copterMode)) continue;
            // this is not "well known" mode, try to create description from enum
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
            yield return GuidedNoGps;
            yield return AvoidAdsb;
            yield return SmartRtl;
        }
    }
    
    #endregion
    
    
    public Px4Mode(string name, string description, MavModeFlag modeFlag, CustomMainMode customMainMode, CustomSubMode customSubMode, bool internalMode = false) : base(name, description, internalMode)
    {
        ModeFlag = modeFlag;
        CustomMainMode = customMainMode;
        CustomSubMode = customSubMode;
    }
    
    public MavModeFlag ModeFlag { get; }
    public CustomMainMode CustomMainMode { get;  }
    public CustomSubMode CustomSubMode { get; }

    public bool Equals(Px4Mode other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ModeFlag == other.ModeFlag && CustomMainMode == other.CustomMainMode && CustomSubMode == other.CustomSubMode;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Px4Mode)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)ModeFlag, (int)CustomMainMode, (int)CustomSubMode);
    }

    public static bool operator ==(Px4Mode left, Px4Mode right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Px4Mode left, Px4Mode right)
    {
        return !Equals(left, right);
    }
}*/