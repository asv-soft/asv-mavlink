using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;

namespace Asv.Mavlink;

public interface IClientDevice
{
    ushort FullId { get; }
    IMavlinkV2Connection Connection { get; }
    IPacketSequenceCalculator Seq { get; }
    IScheduler Scheduler { get; }
    MavlinkClientIdentity Identity { get; }
    DeviceClass Class { get; }
    IRxValue<string> Name { get; }
    IHeartbeatClient Heartbeat { get; }
    IStatusTextClient StatusText { get; }
    IRxValue<InitState> OnInit { get; }
}

public static class ClientDeviceHelper
{
    public static void WaitUntilConnect(this IClientDevice client)
    {
        client.Heartbeat.Link.Where(_ => _ == LinkState.Connected).FirstAsync().Wait();
    }
    
    public static void WaitUntilConnectAndInit(this IVehicleClient client)
    {
        client.WaitUntilConnect();
        client.OnInit.Where(_ => _ == InitState.Complete).FirstAsync().Wait();
    }
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