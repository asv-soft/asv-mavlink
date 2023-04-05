using Asv.Mavlink.Server;

namespace Asv.Mavlink;

public interface IGbsServerDevice
{
    IMavlinkServer Server { get; }
}