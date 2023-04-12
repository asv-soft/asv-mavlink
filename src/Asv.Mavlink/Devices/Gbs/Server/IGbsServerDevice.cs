using Asv.Mavlink.Server;

namespace Asv.Mavlink;

public interface IGbsServerDevice
{
    void Start();
    IHeartbeatServer Heartbeat { get; }
    ICommandServer Command { get; }
    IAsvGbsServer Gbs { get; }
    IAsvGbsServerEx GbsEx { get; }
}