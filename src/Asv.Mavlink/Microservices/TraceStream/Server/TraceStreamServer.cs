using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public class TraceStreamConfig
{
    public int MaxQueueSize { get; set; } = 100;
    public int MaxSendRateHz { get; set; } = 10;
}

public class TraceStreamServer : MavlinkMicroserviceServer, ITraceStreamServer
{
    private const int NameMaxLength = 10;
    private const int MemoryVectorValueMaxLength = 32;

    private int _isSending;

    private readonly TraceStreamConfig _config;
    private readonly ConcurrentQueue<ITraceMessage> _messageQueue = new();
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public TraceStreamServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
                             MavlinkIdentity identity, TraceStreamConfig config, IScheduler scheduler) :
        base("TRACESTREAM", connection, identity, seq, scheduler)
    {
        if (seq == null) throw new ArgumentNullException(nameof(seq));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        if (connection == null) throw new ArgumentNullException(nameof(connection));

        _logger.Debug(
            $"Create status logger for [sys:{identity.SystemId}, com:{identity.ComponentId}] with send rate:{config.MaxSendRateHz} Hz, buffer size: {config.MaxQueueSize}");

        Observable.Timer(TimeSpan.FromSeconds(1.0 / _config.MaxSendRateHz),
                TimeSpan.FromSeconds(1.0 / _config.MaxSendRateHz))
            .Subscribe(TrySend)
            .DisposeItWith(Disposable);
    }

    private async void TrySend(long l)
    {
        if (Interlocked.CompareExchange(ref _isSending, 1, 0) == 1) return;
        try
        {
            if (!_messageQueue.TryDequeue(out var res)) return;
            switch (res)
            {
                case DebugVectorMessage debugVectorMessage:
                    await InternalSend<DebugVectPacket>(packet =>
                    {
                        MavlinkTypesHelper.SetString(packet.Payload.Name, debugVectorMessage.Name);
                        packet.Payload.TimeUsec = debugVectorMessage.TimeUsec;
                        packet.Payload.X = debugVectorMessage.X;
                        packet.Payload.Y = debugVectorMessage.Y;
                        packet.Payload.Z = debugVectorMessage.Z;
                    }, DisposeCancel).ConfigureAwait(false);
                    break;
                case MemoryVectorMessage memoryVectorMessage:
                    await InternalSend<MemoryVectPacket>(packet =>
                    {
                        packet.Payload.Address = memoryVectorMessage.Address;
                        packet.Payload.Ver = memoryVectorMessage.Version;
                        packet.Payload.Type = memoryVectorMessage.Type;
                        packet.Payload.Value = memoryVectorMessage.Value;
                    }, DisposeCancel).ConfigureAwait(false);
                    break;
                case NamedValueIntMessage namedValueIntMessage:
                    await InternalSend<NamedValueIntPacket>(packet =>
                    {
                        packet.Payload.TimeBootMs = namedValueIntMessage.TimeBoot;
                        MavlinkTypesHelper.SetString(packet.Payload.Name, namedValueIntMessage.Name);
                        packet.Payload.Value = namedValueIntMessage.Value;
                    }, DisposeCancel).ConfigureAwait(false);
                    break;
                case NamedValueFloatMessage namedValueFloatMessage:
                    await InternalSend<NamedValueFloatPacket>(packet =>
                    {
                        packet.Payload.TimeBootMs = namedValueFloatMessage.TimeBoot;
                        MavlinkTypesHelper.SetString(packet.Payload.Name, namedValueFloatMessage.Name);
                        packet.Payload.Value = namedValueFloatMessage.Value;
                    }, DisposeCancel).ConfigureAwait(false);
                    break;
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Error occured to send packet: {e.Message}");
        }
        finally
        {
            Interlocked.Exchange(ref _isSending, 0);
        }
    }

    public bool AddMessage(DebugVectorMessage debugVectorMessage)
    {
        _logger.Trace(debugVectorMessage);
        if (debugVectorMessage == null)
        {
            _logger.Warn("Sending message is null");
            return false;
        }
        if (debugVectorMessage.Name.Length > NameMaxLength)
        {
            var newName = debugVectorMessage.Name.Substring(0, NameMaxLength);
            _logger.Warn("Reduced too long DebugVectorMessage Name for trace stream server");
            debugVectorMessage.Name = newName;
            _messageQueue.Enqueue(debugVectorMessage);
            return true;
        }
        if (_messageQueue.Count > _config.MaxQueueSize)
        {
            _logger.Warn($"Message queue overflow (current size:{_messageQueue.Count}).");
            return false;
        }
        _messageQueue.Enqueue(debugVectorMessage);
        return true;
    }

    public bool AddMessage(MemoryVectorMessage memoryVectorMessage)
    {
        _logger.Trace(memoryVectorMessage);
        if (memoryVectorMessage == null)
        {
            _logger.Warn("Sending message is null");
            return false;
        }
        if (memoryVectorMessage.Value.Length > MemoryVectorValueMaxLength)
        {
            var newValue = memoryVectorMessage.Value.Take(MemoryVectorValueMaxLength).ToArray();
            _logger.Warn("Reduced too long MemoryVectorValueMaxLength Value for trace stream server");
            memoryVectorMessage.Value = newValue;
            _messageQueue.Enqueue(memoryVectorMessage);
            return true;
        }
        if (_messageQueue.Count > _config.MaxQueueSize)
        {
            _logger.Warn($"Message queue overflow (current size:{_messageQueue.Count}).");
            return false;
        }
        _messageQueue.Enqueue(memoryVectorMessage);
        return true;
    }

    public bool AddMessage(NamedValueIntMessage namedValueIntMessage)
    {
        _logger.Trace(namedValueIntMessage);
        if (namedValueIntMessage == null)
        {
            _logger.Warn("Sending message is null");
            return false;
        }
        if (namedValueIntMessage.Name.Length > NameMaxLength)
        {
            var newName = namedValueIntMessage.Name.Substring(0, NameMaxLength);
            _logger.Warn("Reduced too long NamedValueIntMessage Name for trace stream server");
            namedValueIntMessage.Name = newName;
            _messageQueue.Enqueue(namedValueIntMessage);
            return true;
        }
        if (_messageQueue.Count > _config.MaxQueueSize)
        {
            _logger.Warn($"Message queue overflow (current size:{_messageQueue.Count}).");
            return false;
        }
        _messageQueue.Enqueue(namedValueIntMessage);
        return true;
    }

    public bool AddMessage(NamedValueFloatMessage namedValueFloatMessage)
    {
        _logger.Trace(namedValueFloatMessage);
        if (namedValueFloatMessage == null)
        {
            _logger.Warn("Sending message is null");
            return false;
        }
        if (namedValueFloatMessage.Name.Length > NameMaxLength)
        {
            var newName = namedValueFloatMessage.Name.Substring(0, NameMaxLength);
            _logger.Warn("Reduced too long NamedValueFloatMessage Name for trace stream server");
            namedValueFloatMessage.Name = newName;
            _messageQueue.Enqueue(namedValueFloatMessage);
            return true;
        }
        if (_messageQueue.Count > _config.MaxQueueSize)
        {
            _logger.Warn($"Message queue overflow (current size:{_messageQueue.Count}).");
            return false;
        }
        _messageQueue.Enqueue(namedValueFloatMessage);
        return true;
    }
}