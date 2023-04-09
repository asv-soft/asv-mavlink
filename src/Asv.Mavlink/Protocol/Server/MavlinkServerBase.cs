using System.Reactive.Concurrency;
using Asv.Common;

namespace Asv.Mavlink.Server
{
    public class MavlinkServerBase:DisposableOnceWithCancel, IMavlinkServer
    {
        private readonly IPacketSequenceCalculator _seq;
        private readonly StatusTextServer _statusText;
        private readonly MavlinkParamsServer _params;
        private readonly HeartbeatServer _heartbeat;
        private readonly DebugServer _debug;
        private readonly CommandServer _command;
        private readonly LoggingServer _logging;
        private readonly V2ExtensionServer _v2Extension;
        private readonly MavlinkServerIdentity _identity;
        private readonly AsvGbsServer _gbs;
        private readonly AsvSdrServer _sdr;

        public MavlinkServerBase(IMavlinkV2Connection connection, MavlinkServerIdentity identity, IScheduler rxScheduler,IPacketSequenceCalculator sequenceCalculator = null, bool disposeConnection = true)
        {
            _seq = sequenceCalculator ?? new PacketSequenceCalculator();
            _heartbeat = new HeartbeatServer(connection, _seq, identity, new MavlinkHeartbeatServerConfig
            {
                HeartbeatRateMs = 1000
            },rxScheduler).DisposeItWith(Disposable);
            _statusText = new StatusTextServer(connection,_seq, identity,new StatusTextLoggerConfig
            {
                MaxQueueSize = 100,
                MaxSendRateHz = 10
            }).DisposeItWith(Disposable);
            _command = new CommandServer(connection,_seq,identity,rxScheduler).DisposeItWith(Disposable);
            _debug = new DebugServer(connection,_seq,identity).DisposeItWith(Disposable);
            _logging = new LoggingServer(connection, _seq, identity).DisposeItWith(Disposable);
            _v2Extension = new V2ExtensionServer(connection,_seq,identity).DisposeItWith(Disposable);
            _params = new MavlinkParamsServer(connection, _seq, identity).DisposeItWith(Disposable);
            _gbs = new AsvGbsServer(connection, _seq, identity).DisposeItWith(Disposable);
            _sdr = new AsvSdrServer(connection, _seq, identity).DisposeItWith(Disposable);
            MavlinkV2Connection = connection;
            _identity = identity;
            if (disposeConnection)
            {
                connection.DisposeItWith(Disposable);
            }
        }
        
        public MavlinkServerIdentity Identity => _identity;
        public IMavlinkParamsServer Params => _params;
        public IMavlinkHeartbeatServer Heartbeat => _heartbeat;
        public IStatusTextServer StatusText => _statusText;
        public IDebugServer Debug => _debug;
        public ICommandServer Command => _command;
        public ILoggingServer Logging => _logging;
        public IV2ExtensionServer V2Extension => _v2Extension;
        public IMavlinkV2Connection MavlinkV2Connection { get; }
        public IAsvGbsServer Gbs => _gbs;
        public IAsvSdrServer Sdr => _sdr;
    }
}
