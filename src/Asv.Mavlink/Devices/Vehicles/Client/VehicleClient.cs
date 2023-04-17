using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public class VehicleClientConfig:ClientDeviceConfig
{
    public ushort MavDataStreamAllRateHz { get; set; } = 1;
    public ushort MavDataStreamExtendedStatusRateHz { get; set; } = 1;
    public ushort MavDataStreamPositionRateHz { get; set; } = 1;
    public CommandProtocolConfig Command { get; set; } = new();
    public MissionClientConfig Missions { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
}

public abstract class VehicleClient : ClientDevice, IVehicleClient
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly VehicleClientConfig _config;
    private readonly ParamsClientEx _params;

    protected VehicleClient(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        VehicleClientConfig config,
        IPacketSequenceCalculator seq, 
        IScheduler scheduler):base(connection,identity,config,seq,scheduler)
    {
        _config = config;
        Commands = new CommandClient(connection,identity,seq,config.Command,scheduler).DisposeItWith(Disposable);
        Offboard = new OffboardClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        _params = new ParamsClientEx(new ParamsClient(Connection, Identity, Seq, _config.Params, Scheduler), _config.Params).DisposeItWith(Disposable);
        Logging = new LoggingClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        var missions = new MissionClient(connection, identity, seq, _config.Missions, scheduler).DisposeItWith(Disposable);
        Missions = new MissionClientEx(missions).DisposeItWith(Disposable);
        Ftp = new FtpClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        var gnss = new GnssClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        Gnss = new GnssClientEx(gnss).DisposeItWith(Disposable);
        V2Extension = new V2ExtensionClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        var pos = new PositionClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        Position = new PositionClientEx(pos,Heartbeat,Commands).DisposeItWith(Disposable);
        var rtt = new TelemetryClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        Rtt = new TelemetryClientEx(rtt).DisposeItWith(Disposable);
        Debug = new DebugClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
        Dgps = new DgpsClient(connection, identity, seq, scheduler).DisposeItWith(Disposable);
    }
    protected override async Task InternalInit()
    {
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamAll, _config.MavDataStreamAllRateHz , true, DisposeCancel).ConfigureAwait(false);
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamExtendedStatus, _config.MavDataStreamExtendedStatusRateHz, true, DisposeCancel).ConfigureAwait(false);
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamPosition,_config.MavDataStreamPositionRateHz , true, DisposeCancel).ConfigureAwait(false);
        await Position.GetHomePosition(DisposeCancel).ConfigureAwait(false);
        var version = await Commands.GetAutopilotVersion(DisposeCancel).ConfigureAwait(false);
        var paramDesc = await GetParamDescription().ConfigureAwait(false);
        if (version.Capabilities.HasFlag(MavProtocolCapability.MavProtocolCapabilityParamUnion))
        {
            _params.Init(new MavParamValueConverter(), paramDesc);
        }
        else
        {
            _params.Init(new MavParamArdupilotValueConverter(),paramDesc);
        }
    }
    protected abstract Task<IReadOnlyCollection<ParamDescription>> GetParamDescription();

    public ICommandClient Commands { get; }
    public IDebugClient Debug { get; }
    public IDgpsClient Dgps { get; }
    public IFtpClient Ftp { get; }
    public IGnssClientEx Gnss { get; }
    public ILoggingClient Logging { get; }
    public IMissionClientEx Missions { get; }
    public IOffboardClient Offboard { get; }
    public IParamsClientEx Params => _params;
    public IPositionClientEx Position { get; }
    public ITelemetryClientEx Rtt { get; }
    public IV2ExtensionClient V2Extension { get; }
    public abstract Task EnsureInGuidedMode(CancellationToken cancel);
    public abstract Task<bool> CheckGuidedMode(CancellationToken cancel);
    public abstract Task SetAutoMode(CancellationToken cancel = default);
    public abstract Task GoTo(GeoPoint point, CancellationToken cancel = default);
    public abstract Task DoLand(CancellationToken cancel = default);
    public abstract Task DoRtl(CancellationToken cancel = default);
    
    

    public async Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        Logger.Info($"=> TakeOff(altitude:{altInMeters:F2})");
        await EnsureInGuidedMode(cancel).ConfigureAwait(false);
        await Position.ArmDisarm(true, cancel).ConfigureAwait(false);
        await Position.TakeOff(altInMeters,  cancel).ConfigureAwait(false);
    }
}