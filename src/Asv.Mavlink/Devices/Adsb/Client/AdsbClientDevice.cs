using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

public class AdsbClientDeviceConfig : ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
}

public class AdsbClientDevice : ClientDevice, IAdsbClientDevice
{
    public AdsbClientDevice(IMavlinkV2Connection connection, 
        MavlinkClientIdentity identity, 
        IPacketSequenceCalculator seq, 
        IScheduler scheduler,
        AdsbClientDeviceConfig config) : base(connection, identity, config, seq, scheduler)
    {
        Command = new CommandClient(connection, identity, seq, config.Command, scheduler).DisposeItWith(Disposable);
        var client = new AdsbVehicleClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        AdsbClient = new AdsbVehicleClientEx(client);
    }

    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }

    protected override Task<string> GetCustomName(CancellationToken cancel)
    {
        return Task.FromResult("ADSB");
    }

    public override DeviceClass Class => DeviceClass.Plane;
    public IAdsbVehicleClientEx AdsbClient { get; }
    public ICommandClient Command { get; }
}