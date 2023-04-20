using System;
using System.Reactive.Concurrency;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public class SdrServerDeviceConfig : ServerDeviceConfig
{
    public AsvSdrServerConfig Sdr { get; set; } = new();
}

public class SdrServerDevice:ServerDevice, ISdrServerDevice
{
    public static Logger Logger = LogManager.GetCurrentClassLogger();


    public SdrServerDevice(IMavlinkV2Connection connection,
        IPacketSequenceCalculator seq, MavlinkServerIdentity identity, SdrServerDeviceConfig config, IScheduler scheduler)
        : base(connection, seq, identity, config, scheduler)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        
        var sdr = new AsvSdrServer(connection, identity, config.Sdr,seq, scheduler).DisposeItWith(Disposable);
        var cmd = new CommandServer(connection, seq, identity, scheduler).DisposeItWith(Disposable);
        CommandLongEx = new CommandLongServerEx(cmd).DisposeItWith(Disposable);
        SdrEx = new AsvSdrServerEx(sdr, Heartbeat, CommandLongEx).DisposeItWith(Disposable);
        var paramsBase = new ParamsServer(connection, seq, identity, scheduler).DisposeItWith(Disposable);
        Params = new ParamsServerEx(paramsBase).DisposeItWith(Disposable);
    }

    public override void Start()
    {
        base.Start();
        SdrEx.Base.Start();
    }
    
    public IAsvSdrServerEx SdrEx { get; }
    public ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    public IParamsServerEx Params { get; }
}