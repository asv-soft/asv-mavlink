using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class SdrServerDeviceConfig : ServerDeviceConfig
{
    public AsvSdrServerConfig Sdr { get; set; } = new();
    public ParamsServerExConfig Params { get; set; } = new();
}

public class SdrServerDevice:ServerDevice, ISdrServerDevice
{
    public SdrServerDevice(IMavlinkV2Connection connection,
        IPacketSequenceCalculator seq, 
        MavlinkIdentity identity, 
        SdrServerDeviceConfig config, 
        IEnumerable<IMavParamTypeMetadata> paramList,
        IMavParamEncoding encoding,
        IConfiguration paramStore,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? loggerFactory = null)
        : base(connection, seq, identity, config, timeProvider, scheduler,loggerFactory)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        
        var sdr = new AsvSdrServer(connection, identity, config.Sdr,seq,timeProvider, scheduler, loggerFactory).DisposeItWith(Disposable);
        var cmd = new CommandServer(connection, seq, identity, timeProvider, scheduler, loggerFactory).DisposeItWith(Disposable);
        CommandLongEx = new CommandLongServerEx(cmd).DisposeItWith(Disposable);
        
        SdrEx = new AsvSdrServerEx(sdr, StatusText, Heartbeat, CommandLongEx).DisposeItWith(Disposable);
        var paramsBase = new ParamsServer(connection, seq, identity, timeProvider, scheduler, loggerFactory).DisposeItWith(Disposable);
        Params = new ParamsServerEx(paramsBase,StatusText,paramList,encoding,paramStore,config.Params,timeProvider, scheduler, loggerFactory).DisposeItWith(Disposable);
        var mission = new MissionServer(connection, identity, seq, timeProvider, scheduler, loggerFactory).DisposeItWith(Disposable);
        Missions = new MissionServerEx(mission, StatusText, connection, identity, seq, timeProvider, scheduler, loggerFactory).DisposeItWith(Disposable);
        
    }

    public override void Start()
    {
        base.Start();
        SdrEx.Base.Start();
    }
    
    public IAsvSdrServerEx SdrEx { get; }
    public IMissionServerEx Missions { get; }
    public ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    public IParamsServerEx Params { get; }
}