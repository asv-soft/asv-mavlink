using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.Ardupilotmega;
using Asv.Mavlink.Minimal;



namespace Asv.Mavlink;

public class ArduQuadPlaneModeClient(IHeartbeatClient heartbeat, ICommandClient command)
    : ModeClient(heartbeat, command, ArduPlaneMode.Unknown,ArduPlaneMode.AllModes)
{
    
}