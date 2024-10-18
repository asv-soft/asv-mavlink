using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class GbsServerDeviceConfig:ServerDeviceConfig
{
    public AsvGbsServerConfig Gbs { get; set; } = new();
    public ParamsServerExConfig Params { get; set; } = new();
}
public class GbsServerDevice:ServerDevice, IGbsServerDevice
{
    public GbsServerDevice(IMavlinkV2Connection connection,
        MavlinkIdentity identity,
        IPacketSequenceCalculator seq,
        GbsServerDeviceConfig config,
        IEnumerable<IMavParamTypeMetadata> paramList,
        IMavParamEncoding encoding,
        IConfiguration paramStore,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? loggerFactory = null) : base(connection, seq, identity, config, timeProvider,scheduler,loggerFactory)
    {
        var command = new CommandServer(connection,seq,identity,timeProvider,scheduler,loggerFactory).DisposeItWith(Disposable);
        CommandLongEx = new CommandLongServerEx(command,timeProvider,scheduler,loggerFactory).DisposeItWith(Disposable);
        var gbs = new AsvGbsServer(connection, seq, identity, config.Gbs, timeProvider,scheduler,loggerFactory).DisposeItWith(Disposable);
        Gbs = new AsvGbsExServer(gbs,Heartbeat,CommandLongEx,timeProvider,scheduler,loggerFactory).DisposeItWith(Disposable);
        var paramsBase = new ParamsServer(connection, seq, identity, timeProvider,scheduler,loggerFactory).DisposeItWith(Disposable);
        Params = new ParamsServerEx(paramsBase,StatusText,paramList,encoding,paramStore,config.Params,timeProvider,scheduler,loggerFactory).DisposeItWith(Disposable);
    }

    public override void Start()
    {
        base.Start();
        Gbs.Base.Start();
    }
    public ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    public IAsvGbsServerEx Gbs { get; }
    public IParamsServerEx Params { get; }
}