#nullable enable
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public class VehicleClientConfig:ClientDeviceConfig
{
    public ushort MavDataStreamAllRateHz { get; set; } = 1;
    public ushort MavDataStreamExtendedStatusRateHz { get; set; } = 1;
    public ushort MavDataStreamPositionRateHz { get; set; } = 1;
    public CommandProtocolConfig Command { get; set; } = new();
    public MissionClientExConfig Missions { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public MavlinkFtpClientConfig Ftp { get; set; } = new();
}

public abstract class VehicleClient : ClientDevice, IVehicleClient
{
    private readonly ILogger _logger;
    private readonly VehicleClientConfig _config;
    private readonly ParamsClientEx _params;

    protected VehicleClient(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        VehicleClientConfig config,
        IPacketSequenceCalculator seq, 
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? logFactory = null):base(connection,identity,config,seq,timeProvider,scheduler,logFactory)
    {
        logFactory ??= NullLoggerFactory.Instance;
        _logger = logFactory.CreateLogger<VehicleClient>();
        _config = config;
        Commands = new CommandClient(connection,identity,seq,config.Command,timeProvider,scheduler,logFactory).DisposeItWith(Disposable);
        Offboard = new OffboardClient(connection, identity, seq,timeProvider,scheduler,logFactory).DisposeItWith(Disposable);
        _params = new ParamsClientEx(new ParamsClient(Connection, Identity, Seq, _config.Params,timeProvider,scheduler,logFactory), _config.Params,timeProvider,scheduler,logFactory).DisposeItWith(Disposable);
        Logging = new LoggingClient(connection, identity, seq,timeProvider,scheduler,logFactory).DisposeItWith(Disposable);
        var missions = new MissionClient(connection, identity, seq, _config.Missions,timeProvider,scheduler,logFactory).DisposeItWith(Disposable);
        Missions = new MissionClientEx(missions, _config.Missions,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        Ftp = new FtpClient(config.Ftp, connection, identity, seq, timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        var gnss = new GnssClient(connection, identity, seq,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        Gnss = new GnssClientEx(gnss,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        V2Extension = new V2ExtensionClient(connection, identity, seq,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        var pos = new PositionClient(connection, identity, seq,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        Position = new PositionClientEx(pos,Heartbeat,Commands,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        var rtt = new TelemetryClient(connection, identity, seq,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        Rtt = new TelemetryClientEx(rtt,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        Debug = new DebugClient(connection, identity, seq,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        Trace = new TraceStreamClient(connection, identity, seq,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        Dgps = new DgpsClient(connection, identity, seq,timeProvider, scheduler,logFactory).DisposeItWith(Disposable);
        
        var customMode = new RxValue<IVehicleMode>().DisposeItWith(Disposable);
        Heartbeat
            .RawHeartbeat
            .DistinctUntilChanged(p=> ((int) p.BaseMode * 397) ^ (int) p.CustomMode)
            .Select(p=>InternalInterpretMode(p) ?? VehicleMode.Unknown)
            .Subscribe(customMode)
            .DisposeItWith(Disposable);
        CurrentMode = customMode;
    }

    protected override async Task InternalInit()
    {
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamAll, _config.MavDataStreamAllRateHz , true, DisposeCancel).ConfigureAwait(false);
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamExtendedStatus, _config.MavDataStreamExtendedStatusRateHz, true, DisposeCancel).ConfigureAwait(false);
        await Rtt.Base.RequestDataStream((int)MavDataStream.MavDataStreamPosition,_config.MavDataStreamPositionRateHz , true, DisposeCancel).ConfigureAwait(false);
        await Position.GetHomePosition(DisposeCancel).ConfigureAwait(false);
        var version = await Commands.GetAutopilotVersion(DisposeCancel).ConfigureAwait(false);
        var paramDesc = await GetParamDescription().ConfigureAwait(false);
        if (version.Capabilities.HasFlag(MavProtocolCapability.MavProtocolCapabilityParamEncodeBytewise))
        {
            _params.Init(new MavParamByteWiseEncoding(), paramDesc);
        }
        else
        {
            _params.Init(new MavParamCStyleEncoding(),paramDesc);
        }
    }
    protected abstract Task<IReadOnlyCollection<ParamDescription>> GetParamDescription();

    public ICommandClient Commands { get; }
    public IDebugClient Debug { get; }
    public ITraceStreamClient Trace { get; }
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
    public abstract Task EnsureInAutoMode(CancellationToken cancel);
    public abstract Task<bool> CheckGuidedMode(CancellationToken cancel);
    public abstract Task<bool> CheckAutoMode(CancellationToken cancel);
    public abstract Task SetAutoMode(CancellationToken cancel = default);
    public abstract Task GoTo(GeoPoint point, CancellationToken cancel = default);
    public abstract Task DoLand(CancellationToken cancel = default);
    public abstract Task DoRtl(CancellationToken cancel = default);
    public abstract Task TakeOff(double altInMeters, CancellationToken cancel = default);
    
    public abstract IEnumerable<IVehicleMode> AvailableModes { get; }
    public IRxValue<IVehicleMode> CurrentMode { get; }
    protected abstract IVehicleMode? InternalInterpretMode(HeartbeatPayload heartbeatPayload);
    public abstract Task SetVehicleMode(IVehicleMode mode, CancellationToken cancel = default);
}