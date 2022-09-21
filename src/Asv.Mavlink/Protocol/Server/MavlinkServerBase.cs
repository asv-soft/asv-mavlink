using System;

namespace Asv.Mavlink.Server
{
    public class MavlinkServerBase:IMavlinkServer,IDisposable
    {
        private readonly IPacketSequenceCalculator _seq;
        private readonly StatusTextServer _statusText;
        private readonly MavlinkParamsServer _params;
        private readonly MavlinkHeartbeatServer _heartbeat;
        private readonly DebugServer _debug;
        private readonly CommandLongServer _commandLong;
        private readonly LoggingServer _logging;
        private readonly V2ExtensionServer _v2Extension;
        private readonly MavlinkServerIdentity _identity;
        private readonly bool _disposeConnection;

        public MavlinkServerBase(IMavlinkV2Connection connection, MavlinkServerIdentity identity,IPacketSequenceCalculator sequenceCalculator = null, bool disposeConnection = true)
        {
            _seq = sequenceCalculator ?? new PacketSequenceCalculator();
            _heartbeat = new MavlinkHeartbeatServer(connection, _seq, identity, new MavlinkHeartbeatServerConfig
            {
                HeartbeatRateMs = 1000
            });
            _statusText = new StatusTextServer(connection,_seq, identity,new StatusTextLoggerConfig
            {
                MaxQueueSize = 100,
                MaxSendRateHz = 10
            });
            _commandLong = new CommandLongServer(connection,_seq,identity);
            _debug = new DebugServer(connection,_seq,identity);
            _logging = new LoggingServer(connection, _seq, identity);
            _v2Extension = new V2ExtensionServer(connection,_seq,identity);
            _params = new MavlinkParamsServer(connection, _seq, identity);
            MavlinkV2Connection = connection;
            _identity = identity;
            _disposeConnection = disposeConnection;
        }

        public MavlinkServerIdentity Identity => _identity;
        public IMavlinkParamsServer Params => _params;
        public IMavlinkHeartbeatServer Heartbeat => _heartbeat;
        public IStatusTextServer StatusText => _statusText;
        public IDebugServer Debug => _debug;
        public ICommandLongServer CommandLong => _commandLong;
        public ILoggingServer Logging => _logging;
        public IV2ExtensionServer V2Extension => _v2Extension;
        public IMavlinkV2Connection MavlinkV2Connection { get; }

        public virtual void Dispose()
        {
            if (_disposeConnection)
                MavlinkV2Connection.Dispose();
            _params.Dispose();
            _heartbeat.Dispose();
            _statusText.Dispose();
            _debug.Dispose();
            _commandLong.Dispose();
            _logging.Dispose();
            _v2Extension.Dispose();


        }
    }
}
