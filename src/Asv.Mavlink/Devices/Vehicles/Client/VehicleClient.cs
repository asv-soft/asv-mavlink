using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class VehicleClientConfig:ClientDeviceConfig
{
    public ushort MavDataStreamAllRateHz { get; set; } = 1;
    public ushort MavDataStreamExtendedStatusRateHz { get; set; } = 1;
    public ushort MavDataStreamPositionRateHz { get; set; } = 1;
    public CommandProtocolConfig Command { get; set; } = new();
    
    public MissionClientConfig Missions { get; set; } = new();
    public ParamsClientExConfig Params { get; set; }
}

public abstract class VehicleClient : ClientDevice, IVehicleClient
{
    private readonly VehicleClientConfig _config;

    protected VehicleClient(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        VehicleClientConfig config,
        IPacketSequenceCalculator seq, 
        IScheduler scheduler):base(connection,identity,config,seq,scheduler)
    {
        _config = config;
        Commands = new CommandClient(connection,identity,seq,config.Command,scheduler).DisposeItWith(Disposable);
        var missions = new MissionClient(connection, identity, seq, _config.Missions, scheduler).DisposeItWith(Disposable);
        Missions = new MissionClientEx(missions).DisposeItWith(Disposable);
        var gnss = new GnssClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        Gnss = new GnssClientEx(gnss).DisposeItWith(Disposable);
        var rtt = new TelemetryClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        Rtt = new TelemetryClientEx(rtt).DisposeItWith(Disposable);
        var pos = new PositionClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        Position = new PositionClientEx(pos,Heartbeat,Commands).DisposeItWith(Disposable);
    }
    
    protected override async Task InternalInit()
    {
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamAll, _config.MavDataStreamAllRateHz , true, DisposeCancel).ConfigureAwait(false);
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamExtendedStatus, _config.MavDataStreamExtendedStatusRateHz, true, DisposeCancel).ConfigureAwait(false);
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamPosition,_config.MavDataStreamPositionRateHz , true, DisposeCancel).ConfigureAwait(false);
        await Position.GetHomePosition(DisposeCancel).ConfigureAwait(false);
        var version = await Commands.GetAutopilotVersion(DisposeCancel).ConfigureAwait(false);
        var paramDesc = await GetParamDescription().ConfigureAwait(false);
        Params = version.Capabilities.HasFlag(MavProtocolCapability.MavProtocolCapabilityParamUnion) 
            ?  new ParamsClientEx(new ParamsClient(Connection,Identity,Seq,_config.Params,Scheduler),new MavParamValueConverter(),paramDesc,_config.Params).DisposeItWith(Disposable)
            :  new ParamsClientEx(new ParamsClient(Connection,Identity,Seq,_config.Params,Scheduler),new MavParamArdupilotValueConverter(),paramDesc,_config.Params).DisposeItWith(Disposable);
    }

    protected abstract Task<IReadOnlyCollection<ParamDescription>> GetParamDescription();
    public ICommandClient Commands { get; }
    public IParamsClientEx Params { get; set; }
    public IMissionClientEx Missions { get; }
    public IGnssClientEx Gnss { get; }
    public ITelemetryClientEx Rtt { get; }
    public IPositionClientEx Position { get; }
}