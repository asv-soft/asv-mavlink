using System;
using System.Reactive.Concurrency;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public class SdrServerDeviceConfig : ServerDeviceConfig
{
    public AsvGbsServerConfig Gbs { get; set; } = new();
}

public class SdrServerDevice:ServerDevice, ISdrServerDevice
{
    public static Logger Logger = LogManager.GetCurrentClassLogger();


    public SdrServerDevice(IAsvGbsCommon interfaceImplementation, IMavlinkV2Connection connection,
        IPacketSequenceCalculator seq, MavlinkServerIdentity identity, SdrServerDeviceConfig config, IScheduler scheduler)
        : base(connection, seq, identity, config, scheduler)
    {
        if (interfaceImplementation == null) throw new ArgumentNullException(nameof(interfaceImplementation));
        if (config == null) throw new ArgumentNullException(nameof(config));
        
        var gbs = new AsvGbsServer(connection, seq, identity,config.Gbs, scheduler).DisposeItWith(Disposable);
        var cmd = new CommandServer(connection, seq, identity, scheduler).DisposeItWith(Disposable);
        CommandLongEx = new CommandLongServerEx(cmd).DisposeItWith(Disposable);
        GbsEx = new AsvGbsExServer(gbs, Heartbeat, CommandLongEx, interfaceImplementation).DisposeItWith(Disposable);
        
        interfaceImplementation.CustomMode.Subscribe(mode => Heartbeat.Set(_ =>
        {
            _.CustomMode = (uint)mode;
        })).DisposeItWith(Disposable);
    }

    public override void Start()
    {
        base.Start();
        GbsEx.Base.Start();
    }
    public IAsvGbsServerEx GbsEx { get; }
    public ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
}