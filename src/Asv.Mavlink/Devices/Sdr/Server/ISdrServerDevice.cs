using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface ISdrServerDevice
{
    ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
}