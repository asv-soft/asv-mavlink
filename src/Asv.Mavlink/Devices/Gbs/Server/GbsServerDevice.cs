using System.Collections.Generic;
using System.Reactive.Concurrency;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;

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
        IScheduler scheduler,
        GbsServerDeviceConfig config,
        IEnumerable<IMavParamTypeMetadata> paramList,
        IMavParamEncoding encoding,
        IConfiguration paramStore) : base(connection, seq, identity, config, scheduler)
    {
        var command = new CommandServer(connection,seq,identity,scheduler).DisposeItWith(Disposable);
        CommandLongEx = new CommandLongServerEx(command).DisposeItWith(Disposable);
        var gbs = new AsvGbsServer(connection, seq, identity, config.Gbs, scheduler).DisposeItWith(Disposable);
        Gbs = new AsvGbsExServer(gbs,Heartbeat,CommandLongEx).DisposeItWith(Disposable);
        var paramsBase = new ParamsServer(connection, seq, identity, scheduler).DisposeItWith(Disposable);
        Params = new ParamsServerEx(paramsBase,StatusText,paramList,encoding,paramStore,config.Params).DisposeItWith(Disposable);
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