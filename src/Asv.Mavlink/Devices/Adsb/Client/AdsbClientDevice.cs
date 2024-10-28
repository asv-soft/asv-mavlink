#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public class AdsbClientDeviceConfig : ClientDeviceConfig
{
    public AdsbVehicleClientConfig Adsb { get; set; } = new();
}

public class AdsbClientDevice : ClientDevice, IAdsbClientDevice
{
    public AdsbClientDevice(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq,
        AdsbClientDeviceConfig config,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null, 
        ILoggerFactory? loggerFactory = null)
        : base(connection, identity, config, seq,timeProvider, scheduler, loggerFactory)
    {
        loggerFactory ??= NullLoggerFactory.Instance;
        Adsb = new AdsbVehicleClient(connection, identity, seq, config.Adsb, timeProvider, scheduler,loggerFactory).DisposeItWith(Disposable);
    }

    protected override string DefaultName => $"ADSB [{Identity.TargetSystemId:00},{Identity.TargetComponentId:00}]";

    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }

    public override DeviceClass Class => DeviceClass.Adsb;
    public IAdsbVehicleClient Adsb { get; }
}