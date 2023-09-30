namespace Asv.Mavlink;

public interface ISdrClientDevice
{
    IAsvSdrClientEx Sdr { get; }
    IHeartbeatClient Heartbeat { get; }
    ICommandClient Command { get; }
    IMissionClientEx Missions { get; }
    IParamsClientEx Params { get; }
}