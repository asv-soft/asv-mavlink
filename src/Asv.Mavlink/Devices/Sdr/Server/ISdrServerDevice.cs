using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface ISdrServerDevice:IServerDevice
{
    ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    IParamsServerEx Params { get; }
    IAsvSdrServerEx SdrEx { get; }
}