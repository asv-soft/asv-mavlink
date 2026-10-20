using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink
{
    public class MavlinkPacketTransponder<TPacket> : AsyncDisposableWithCancel, IMavlinkPacketTransponder<TPacket>
        where TPacket : MavlinkMessage, new()
    {
        private readonly IMavlinkContext _core;
        private readonly object _sync = new();
        private readonly ILogger _logger;
        private bool _isSending;
        private TaskCompletionSource? _sendCompletion;
        private readonly ReactiveProperty<PacketTransponderState> _state = new();
        private readonly TPacket _packet;
        private ITimer? _timer;

        public MavlinkPacketTransponder(MavlinkIdentity identity, IMavlinkContext core)
        {
            ArgumentNullException.ThrowIfNull(identity);
            _core = core ?? throw new ArgumentNullException(nameof(core));
            _logger = core.LoggerFactory.CreateLogger<IMavlinkPacketTransponder<TPacket>>();
            
            _packet = new TPacket
            {
                ComponentId = identity.ComponentId,
                SystemId = identity.SystemId,
            };
        }

        public void Start(TimeSpan dueTime, TimeSpan period)
        {
            lock (_sync)
            {
                ThrowIfDisposed();
                _timer?.Dispose();
                _timer = _core.TimeProvider.CreateTimer(OnTick,null,dueTime, period);
                IsStarted = true;
            }
        }

        private void OnTick(object? state)
        {
            CancellationToken cancel;
            TPacket sendPacket;
            lock (_sync)
            {
                if (IsDisposed || !IsStarted) return;
                if (_isSending)
                {
                    LogSkipped();
                    return;
                }

                // Send may complete after enqueueing, before serialization. Each queued packet
                // therefore needs its own snapshot, independent of Set and subsequent ticks.
                try
                {
                    sendPacket = new TPacket();
                    var payload = _packet.GetPayload();
                    Span<byte> buffer = stackalloc byte[payload.GetMaxByteSize()];
                    var remaining = buffer;
                    payload.Serialize(ref remaining);
                    ReadOnlySpan<byte> serialized = buffer[..(buffer.Length - remaining.Length)];
                    sendPacket.GetPayload().Deserialize(ref serialized);
                    sendPacket.SystemId = _packet.SystemId;
                    sendPacket.ComponentId = _packet.ComponentId;
                    if (_packet is MavlinkV2Message source && sendPacket is MavlinkV2Message target)
                    {
                        target.CompatFlags = source.CompatFlags;
                        target.IncompatFlags = source.IncompatFlags;
                        target.Signature.IsPresent = source.Signature.IsPresent;
                        target.Signature.LinkId = source.Signature.LinkId;
                        target.Signature.Timestamp = source.Signature.Timestamp;
                        target.Signature.Sign = source.Signature.Sign;
                    }
                    sendPacket.Tags.Clear();
                    for (var i = 0; i < _packet.Tags.Count; i++)
                    {
                        sendPacket.Tags.Add(_packet.Tags[i]);
                    }
                    sendPacket.Sequence = _core.Sequence.GetNextSequenceNumber();
                }
                catch (Exception e)
                {
                    LogError(e);
                    return;
                }
                cancel = DisposeCancel;
                _isSending = true;
            }

            _ = SendAsync(sendPacket, cancel);
        }

        private async Task SendAsync(TPacket packet, CancellationToken cancel)
        {
            Exception? error = null;
            try
            {
                await _core.Connection.Send(packet, cancel).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                error = e;
            }
            finally
            {
                lock (_sync)
                {
                    _isSending = false;
                    _sendCompletion?.TrySetResult();
                    _sendCompletion = null;
                    if (!IsDisposed)
                    {
                        if (error == null) LogSuccess();
                        else LogError(error);
                    }
                }
            }
        }

        private void LogError(Exception e)
        {
            if (_state.Value == PacketTransponderState.ErrorToSend) return;
            _state.Value = PacketTransponderState.ErrorToSend;
            _logger.ZLogError($"{new TPacket().Name} sending error:{e.Message}");
        }

        private void LogSuccess()
        {
            if (_state.Value == PacketTransponderState.Ok) return;
            _state.Value = PacketTransponderState.Ok;
            _logger.ZLogDebug($"{new TPacket().Name} start stream");
        }

        private void LogSkipped()
        {
            if (_state.Value == PacketTransponderState.Skipped) return;
            _state.Value = PacketTransponderState.Skipped;
            _logger.ZLogWarning($"{new TPacket().Name} skipped sending: previous command has not yet been executed");
        }
        public bool IsStarted { get; private set; }

        public ReadOnlyReactiveProperty<PacketTransponderState> State => _state;

        public void Stop()
        {
            lock (_sync)
            {
                _timer?.Dispose();
                _timer = null;
                IsStarted = false;
            }
        }

        public void Set(Action<TPacket> changeCallback)
        {
            lock (_sync)
            {
                if (IsDisposed) return;
                try
                {
                    changeCallback(_packet);
                }
                catch (Exception e)
                {
                    _logger.ZLogError(e,$"Error to set new value for {new TPacket().Name}:{e.Message}");
                }
            }
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
                lock (_sync) _state.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            Stop();
            Task completion;
            lock (_sync)
            {
                completion = _isSending
                    ? (_sendCompletion ??= new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)).Task
                    : Task.CompletedTask;
            }

            await base.DisposeAsyncCore().ConfigureAwait(false);
            await completion.ConfigureAwait(false);
            lock (_sync) _state.Dispose();
        }

        #endregion

    }
}
