using Asv.Mavlink.Server;

namespace Asv.Mavlink;

public interface IGbsServerDevice
{
    void Start();
    IHeartbeatServer Heartbeat { get; }
    ICommandServer Command { get; }
    IAsvGbsServerEx Gbs { get; }
}