using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink
{
    public class StatusTextLoggerConfig
    {
        public int MaxQueueSize { get; set; } = 100;
        public int MaxSendRateHz { get; set; } = 10;
    }

    public class StatusTextServer : MavlinkMicroserviceServer, IStatusTextServer
    {
        private readonly int _maxMessageSize = new StatustextPayload().Text.Length;
        private readonly StatusTextLoggerConfig _config;
        private readonly ConcurrentQueue<KeyValuePair<MavSeverity,string>> _messageQueue = new();
        private readonly ILogger _logger;
        private int _isSending;

        public StatusTextServer(
            IMavlinkV2Connection connection, 
            IPacketSequenceCalculator seq,
            MavlinkIdentity identity, 
            StatusTextLoggerConfig config, 
            IScheduler? scheduler = null,
            ILogger? logger = null):   
            base("STATUS",connection,identity,seq,scheduler,logger)
        {
            _logger = logger ?? NullLogger.Instance;
            if (seq == null) throw new ArgumentNullException(nameof(seq));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            _logger.ZLogDebug($"Create status logger for [sys:{identity.SystemId}, com:{identity.ComponentId}] with send rate:{config.MaxSendRateHz} Hz, buffer size: {config.MaxQueueSize}");

            Observable.Timer(TimeSpan.FromSeconds(1.0 / _config.MaxSendRateHz),
                TimeSpan.FromSeconds(1.0 / _config.MaxSendRateHz)).Subscribe(TrySend)
                .DisposeItWith(Disposable);
        }

        private async void TrySend(long l)
        {
            if (Interlocked.CompareExchange(ref _isSending,1,0) == 1) return;

            try
            {
                KeyValuePair<MavSeverity, string> res;
                if (_messageQueue.TryDequeue(out res))
                {
                    await InternalSend<StatustextPacket>(p =>
                    {
                        p.Payload.Severity = res.Key;
                        MavlinkTypesHelper.SetString(p.Payload.Text, res.Value);
                        
                    },DisposeCancel).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                _logger.ZLogWarning($"Error occured to send packet: {e.Message}");
            }
            finally
            {
                Interlocked.Exchange(ref _isSending, 0);
            }
        }

        public bool Log((MavSeverity severity, string message) msg)
        {
            _logger.ZLogTrace($"{msg.severity:G} => {msg.message}");

            if (msg.message == null)
            {
                _logger.LogWarning("Sending message is null");
                return false;
            }
            if (msg.message.Length > _maxMessageSize)
            {
                var newMessage = msg.message.Substring(0, _maxMessageSize - 3) + "...";
                _logger.ZLogWarning($"Reduced too long message for status text server");
                _messageQueue.Enqueue(new KeyValuePair<MavSeverity, string>(msg.severity, newMessage));
                return true;
            }
            if (_messageQueue.Count > _config.MaxQueueSize)
            {
                _logger.ZLogWarning($"Message queue overflow (current size:{_messageQueue.Count}).");
                return false;
            }
            _messageQueue.Enqueue(new KeyValuePair<MavSeverity, string>(msg.severity, msg.message));
            return true;
        }

        public bool Log(MavSeverity severity, string message)
        {
            return Log((severity, message));
        }

       
    }

    
}
