using Asv.Mavlink.Server;

namespace Asv.Mavlink;

public interface ISdrServerDevice
{
    IMavlinkServer Server { get; }
}