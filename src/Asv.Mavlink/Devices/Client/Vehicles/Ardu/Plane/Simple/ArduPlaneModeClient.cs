namespace Asv.Mavlink;

public class ArduPlaneModeClient(IHeartbeatClient heartbeat, ICommandClient command) 
    : ModeClient(heartbeat, command, ArduPlaneMode.Unknown, ArduPlaneMode.AllModes)
{
}