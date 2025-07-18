namespace Asv.Mavlink;

public class ArduCopterModeClient(IHeartbeatClient heartbeat, ICommandClient command)
    : ModeClient(heartbeat, command, ArduCopterMode.Unknown,ArduCopterMode.AllModes)
{
    
    
}