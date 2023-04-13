using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IGbsServerDevice:IServerDevice
{
    ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    IAsvGbsServerEx Gbs { get; }
}