#nullable enable
namespace Asv.Mavlink;

public interface IRadioClientDevice
{
    IHeartbeatClient Heartbeat { get; }
    ICommandClient Command { get; }
    IParamsClientEx Params { get; }
    IAsvRadioClientEx Radio { get; }
}
