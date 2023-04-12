using Asv.Common;

namespace Asv.Mavlink;

public interface IClientDevice
{
    MavlinkClientIdentity Identity { get; }
    DeviceClass Class { get; }
    IRxValue<string> Name { get; }
    IHeartbeatClient Heartbeat { get; }
    IRxValue<InitState> OnInit { get; }
}

public enum InitState
{
    WaitConnection,
    Failed,
    InProgress,
    Complete
}

public enum DeviceClass
{
    Unknown,
    Plane,
    Copter,
    GbsRtk,
    SdrPayload
}