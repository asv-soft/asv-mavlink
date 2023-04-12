using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.Custom;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class VehicleClientConfig:ClientDeviceConfig
{
    public ushort MavDataStreamAllRateHz { get; set; } = 1;
    public ushort MavDataStreamExtendedStatusRateHz { get; set; } = 1;
    public ushort MavDataStreamPositionRateHz { get; set; } = 1;
    public CommandProtocolConfig Command { get; set; } = new();
    public ParamsClientConfig Params { get; set; } = new();
    public MissionClientConfig Missions { get; set; } = new();
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
        Params = new ParamsClient(connection,identity,seq,config.Params,scheduler).DisposeItWith(Disposable);
        Missions = new MissionClientEx(new MissionClient(connection, identity, seq, _config.Missions, scheduler)).DisposeItWith(Disposable);
        Gnss = new GnssClientEx(new GnssClient(connection, identity, seq, scheduler)).DisposeItWith(Disposable);
        Rtt = new TelemetryClientEx(new TelemetryClient(connection, identity, seq, scheduler),Commands).DisposeItWith(Disposable);
        Position = new PositionClientEx(new PositionClient(connection, identity, seq, scheduler),Heartbeat,Commands).DisposeItWith(Disposable);
    }

    

    protected override async Task InternalInit()
    {
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamAll, _config.MavDataStreamAllRateHz , true, DisposeCancel).ConfigureAwait(false);
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamExtendedStatus, _config.MavDataStreamExtendedStatusRateHz, true, DisposeCancel).ConfigureAwait(false);
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamPosition,_config.MavDataStreamPositionRateHz , true, DisposeCancel).ConfigureAwait(false);
        await Position.GetHomePosition(DisposeCancel).ConfigureAwait(false);
    }
    public ICommandClient Commands { get; }
    public IParamsClient Params { get; }
    public IMissionClientEx Missions { get; }
    public IGnssClientEx Gnss { get; }
    public ITelemetryClientEx Rtt { get; }
    public IPositionClientEx Position { get; }
}