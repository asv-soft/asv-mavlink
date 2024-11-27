using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.Ardupilotmega;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.V2.Ardupilotmega;


namespace Asv.Mavlink;

public class ArduQuadPlaneModeClient(IHeartbeatClient heartbeat, ICommandClient command)
    : ModeClient(heartbeat, command)
{
    #region Static

    public static ImmutableArray<OpMode> AllModes { get; }
 
    static ArduQuadPlaneModeClient()
    {
        var wellKnownModes = new HashSet<uint>(WellKnownModes.Select(m=>m.CustomMode));
        var allModes = new List<OpMode>();
        allModes.AddRange(WellKnownModes);
        foreach (var copterMode in Enum.GetValues<PlaneMode>())
        {
            if (wellKnownModes.Contains((uint)copterMode)) continue;
            // this is not "well known" mode, try to create description from enum
            allModes.Add(new OpMode(copterMode.ToString("G"), string.Empty, true, MavModeFlag.MavModeFlagCustomModeEnabled, (ushort)copterMode,0));
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
            yield return Qstabilize;
            yield return Qhover;
            yield return Qloiter;
            yield return Qland;
            yield return Qrtl;
            yield return Qautotune;
            yield return LoiterToQuadLand;
            yield return Qacro;
        }
    }

    public static OpMode Manual = ArduPlaneModeClient.Manual;
    public static OpMode Circle = ArduPlaneModeClient.Circle;
    public static OpMode Stabilize = ArduPlaneModeClient.Stabilize; 
    public static OpMode Training = ArduPlaneModeClient.Training;
    public static OpMode Acro = ArduPlaneModeClient.Acro;
    public static OpMode FlyByWireA = ArduPlaneModeClient.FlyByWireA;
    public static OpMode FlyByWireB = ArduPlaneModeClient.FlyByWireB;
    public static OpMode Cruise = ArduPlaneModeClient.Cruise;
    public static OpMode Autotune= ArduPlaneModeClient.Autotune;
    public static OpMode Auto = ArduPlaneModeClient.Auto;
    public static OpMode Rtl = ArduPlaneModeClient.Rtl;
    public static OpMode Loiter = ArduPlaneModeClient.Loiter;
    public static OpMode Guided = ArduPlaneModeClient.Guided;
    /// <summary>
    ///  Allows you to fly your vehicle manually, but self-levels the roll and pitch axis
    /// </summary>
    public static OpMode Qstabilize  = new("Quad Stabilize", RS.ArdupilotPlaneMode_Qstabilize_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeQstabilize,0);
    /// <summary>
    /// QuadPlane maintains a consistent altitude while allowing roll, pitch, and yaw to be controlled normally
    /// </summary>
    public static OpMode Qhover      = new("Quad Hover", RS.ArdupilotPlaneMode_Qhover_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeQhover,0);
    /// <summary>
    /// Automatically attempts to maintain the current location, heading and altitude
    /// </summary>
    public static OpMode Qloiter     = new("Quad Loiter", RS.ArdupilotPlaneMode_Qloiter_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeQloiter,0);
    /// <summary>
    /// Attempts to bring the QuadPlane straight down at the position the vehicle is located when the mode is entered
    /// </summary>
    public static OpMode Qland       = new("Quad Land", RS.ArdupilotPlaneMode_Qland_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeQland,0);
    /// <summary>
    /// Navigates QuadPlane from its current position to hover above the home position and then land
    /// </summary>
    public static OpMode Qrtl        = new("Quad RTL", RS.ArdupilotPlaneMode_Qrtl_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeQrtl,0);
    /// <summary>
    /// Automated pitch and bank procedure to improve control loops
    /// </summary>
    public static OpMode Qautotune   = new("Quad Autotune", RS.ArdupilotPlaneMode_Qautotune_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeQautotune,0);
    /// <summary>
    /// This mode will perform a descending fixed wing LOITER down to Q_RTL_ALT and then switch to QLAND mode.
    /// </summary>
    public static OpMode LoiterToQuadLand = new("Loiter To QLand", RS.ArdupilotPlaneMode_Loiter_To_QLand_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.LoiterToQLand, 0);
    /// <summary>
    /// A quadplane mode for advanced users that provides rate based stabilization like Copter ACRO
    /// </summary>
    public static OpMode Qacro = new("Quad Acro", RS.ArdupilotPlaneMode_QAcro_Description, false, MavModeFlag.MavModeFlagCustomModeEnabled, (uint) PlaneMode.PlaneModeQacro,0);
    
    #endregion

    protected override OpMode Convert(HeartbeatPayload? hb)
    {
        if (hb == null) return OpMode.Unknown;
        return AllModes.FirstOrDefault(x => x.CustomMode == hb.CustomMode) ?? OpMode.Unknown;
    }

    public override IEnumerable<OpMode> AvailableModes => AllModes;
    
}