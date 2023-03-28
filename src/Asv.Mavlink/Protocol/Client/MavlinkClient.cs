using System;
using System.Reactive.Concurrency;
using Asv.Common;
using NLog;

namespace Asv.Mavlink.Client
{
    public class MavlinkClientIdentity
    {
        public byte SystemId { get; set; } = 254;
        public byte ComponentId { get; set; } = 254;
        public byte TargetSystemId { get; set; } = 1;
        public byte TargetComponentId { get; set; } = 1;

        public override string ToString()
        {
            return $"[Client:{SystemId}.{ComponentId}]=>[Server:{TargetSystemId}.{TargetComponentId}]";
        }
    }

    public class MavlinkClientConfig
    {
        public int CommandTimeoutMs { get; set; } = 5000;
        public int TimeoutToReadAllParamsMs { get; set; } = 60*60*1000;
        public int ReadParamTimeoutMs { get; set; } = 10000;
    }

    public class MavlinkClient : DisposableOnceWithCancel, IMavlinkClient
    {
        private readonly IMavlinkV2Connection _mavlinkConnection;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly MavlinkTelemetry _rtt;
        private readonly MavlinkParameterClient _params;
        private readonly MavlinkCommandClient _mavlinkCommands;
        private readonly MissionClient _mission;
        private readonly MavlinkOffboardMode _mavlinkOffboard;
        private readonly MavlinkCommon _mode;
        private readonly DebugClient _debugs;
        private readonly HeartbeatClient _heartbeat;
        private readonly LoggingClient _logging;
        private readonly DgpsClient _rtk;
        private IV2ExtensionClient _v2Ext;
        private readonly IPacketSequenceCalculator _seq;
        private readonly AsvGbsClient _gbs;

        public MavlinkClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, MavlinkClientConfig config, IPacketSequenceCalculator sequence = null, bool disposeConnection = true, IScheduler scheduler = null)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _seq = sequence ?? new PacketSequenceCalculator();
            Identity = identity;
            _mavlinkConnection = connection ?? throw new ArgumentNullException(nameof(connection));

            scheduler ??= Scheduler.Default;

            _rtt = new MavlinkTelemetry(_mavlinkConnection, identity, _seq, scheduler)
                .DisposeItWith(Disposable);;

            _params = new MavlinkParameterClient(_mavlinkConnection, identity, _seq, new VehicleParameterProtocolConfig {ReadWriteTimeoutMs = config.ReadParamTimeoutMs,TimeoutToReadAllParamsMs = config.TimeoutToReadAllParamsMs}, scheduler)
                .DisposeItWith(Disposable);;

            _mavlinkCommands = new MavlinkCommandClient(_mavlinkConnection, identity, _seq,new CommandProtocolConfig { CommandTimeoutMs = config.CommandTimeoutMs}, scheduler)
                .DisposeItWith(Disposable);;

            _mission = new MissionClient(_mavlinkConnection,identity, _seq, new MissionClientConfig{ CommandTimeoutMs = config.CommandTimeoutMs}, scheduler)
                .DisposeItWith(Disposable);;

            _mavlinkOffboard = new MavlinkOffboardMode(_mavlinkConnection,identity, _seq, scheduler)
                .DisposeItWith(Disposable);;

            _mode = new MavlinkCommon(_mavlinkConnection,identity,_seq, scheduler)
                .DisposeItWith(Disposable);;

            _debugs = new DebugClient(_mavlinkConnection, identity,_seq, scheduler)
                .DisposeItWith(Disposable);;

            _heartbeat = new HeartbeatClient(_mavlinkConnection,identity,_seq, scheduler)
                .DisposeItWith(Disposable);;

            _logging = new LoggingClient(_mavlinkConnection,identity, _seq, scheduler)
                .DisposeItWith(Disposable);;

            _v2Ext = new V2ExtensionClient(_mavlinkConnection, _seq, identity, scheduler)
                .DisposeItWith(Disposable);;

            _rtk = new DgpsClient(_mavlinkConnection, identity,_seq, scheduler)
                .DisposeItWith(Disposable);
            
            _gbs = new AsvGbsClient(_mavlinkConnection, identity, _seq, scheduler)
                .DisposeItWith(Disposable);

            if (disposeConnection)
                _mavlinkConnection.DisposeItWith(Disposable);
        }

        protected IMavlinkV2Connection Connection => _mavlinkConnection;
        public MavlinkClientIdentity Identity { get; }
        public IHeartbeatClient Heartbeat => _heartbeat;
        public IMavlinkTelemetry Rtt => _rtt;
        public IMavlinkParameterClient Params => _params;
        public IMavlinkCommandClient Commands => _mavlinkCommands;
        public IMissionClient Mission => _mission;
        public IMavlinkOffboardMode Offboard => _mavlinkOffboard;
        public IMavlinkCommon Common => _mode;
        public IDebugClient Debug => _debugs;
        public ILoggingClient Logging => _logging;
        public IV2ExtensionClient V2Extension => _v2Ext;
        public IDgpsClient Rtk => _rtk;
        public IAsvGbsClient Gbs => _gbs;
        public IMavlinkV2Connection MavlinkV2Connection => _mavlinkConnection;
    }
}
