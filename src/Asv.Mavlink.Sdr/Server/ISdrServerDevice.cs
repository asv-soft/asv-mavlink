using Asv.Mavlink.Server;

namespace Asv.Mavlink.Sdr;

public interface ISdrServerDevice
{
    IMavlinkServer Server { get; }
}