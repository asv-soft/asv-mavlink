using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public class Px4CopterModeClient(IHeartbeatClient heartbeat, ICommandClient command)
    : ModeClient(heartbeat, command, Px4Mode.Unknown,Px4Mode.AllModes)
{
    
}


