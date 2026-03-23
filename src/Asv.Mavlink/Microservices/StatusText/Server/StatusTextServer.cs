using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

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
    private readonly ITimer _timer;

    public StatusTextServer(MavlinkIdentity identity, StatusTextLoggerConfig config, IMavlinkContext core):   
        base(StatusTextHelper.MicroserviceName,identity,core)
    {
        _logger = core.LoggerFactory.CreateLogger<StatusTextServer>();
        _config = config ?? throw new ArgumentNullException(nameof(config));

        _logger.ZLogDebug($"Create status logger for [sys:{identity.SystemId}, com:{identity.ComponentId}] with send rate:{config.MaxSendRateHz} Hz, buffer size: {config.MaxQueueSize}");

        var time = TimeSpan.FromSeconds(1.0 / _config.MaxSendRateHz);
        _timer = core.TimeProvider.CreateTimer(state => TrySend(state, DisposeCancel).SafeFireAndForget(),null,time, time);
    }

    private async Task TrySend(object? state, CancellationToken cancel)
    {
        if (Interlocked.CompareExchange(ref _isSending,1,0) == 1) return;

        try
        {
            if (_messageQueue.TryDequeue(out var res))
            {
                await InternalSend<StatustextPacket>(p =>
                {
                    p.Payload.Severity = res.Key;
                    MavlinkTypesHelper.SetString(p.Payload.Text, res.Value);
                        
                }, cancel).ConfigureAwait(false);
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

        if (string.IsNullOrWhiteSpace(msg.message))
        {
            _logger.LogWarning("Message is null => skip it...");
            return false;
        }
        if (msg.message.Length > _maxMessageSize)
        {
            var newMessage = $"{msg.message[..(_maxMessageSize - 3)]}...";
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

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _messageQueue.Clear();
            _timer.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        _messageQueue.Clear();
        await _timer.DisposeAsync().ConfigureAwait(false);
        await base.DisposeAsyncCore().ConfigureAwait(false);
    }

    #endregion

}
