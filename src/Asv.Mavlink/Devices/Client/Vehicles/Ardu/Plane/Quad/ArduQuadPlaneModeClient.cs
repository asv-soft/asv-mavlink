namespace Asv.Mavlink;

public class ArduQuadPlaneModeClient(IHeartbeatClient heartbeat, ICommandClient command)
    : ModeClient(heartbeat, command, ArduPlaneMode.Unknown,ArduPlaneMode.AllModes)
{
    
}