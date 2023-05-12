using System.Reactive.Concurrency;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class AdsbServerDeviceConfig : ServerDeviceConfig
{
    public AdsbVehicleServerConfig Config { get; set; } = new();
}

public class AdsbServerDevice : ServerDevice, IAdsbServerDevice
{
    public AdsbServerDevice(IMavlinkV2Connection connection, 
        IPacketSequenceCalculator seq, 
        MavlinkServerIdentity identity, 
        AdsbServerDeviceConfig config, 
        IScheduler scheduler) : base(connection, seq, identity, config, scheduler)
    {
        var command = new CommandServer(connection,seq,identity,scheduler).DisposeItWith(Disposable);
        CommandLongEx = new CommandLongServerEx(command).DisposeItWith(Disposable);
        Adsb = new AdsbVehicleServer(connection, identity, seq, scheduler, config.Config);
    }

    public override void Start()
    {
        base.Start();
        Adsb.Start();
    }

    public ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    public IAdsbVehicleServer Adsb { get; }
}