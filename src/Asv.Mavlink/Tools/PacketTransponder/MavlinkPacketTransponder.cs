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
        private readonly ReaderWriterLockSlim _dataLock = new();
        private int _isSending;
        private readonly ReactiveProperty<PacketTransponderState> _state = new();
        private TPacket _packet;
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
            if (_packet == null) throw new Exception($"You need call '{nameof(Set)}' method< before call start");
            lock (_sync)
            {
                _timer?.Dispose();
                _timer = _core.TimeProvider.CreateTimer(OnTick,null,dueTime, period);
                IsStarted = true;
            }
        }

        private void OnTick(object? state)
        {
            if (Interlocked.CompareExchange(ref _isSending, 1, 0) == 1)
            {
                LogSkipped();
                return;
            }

            try
            {
                _dataLock.EnterReadLock();
                _packet.Sequence = _core.Sequence.GetNextSequenceNumber();
                _core.Connection.Send(_packet, DisposeCancel);
                LogSuccess();
            }
            catch (Exception e)
            {
                LogError(e);
            }
            finally
            {
                _dataLock.ExitReadLock();
                Interlocked.Exchange(ref _isSending, 0);
            }
        }

        private void LogError(Exception e)
        {
            if (_state.Value == PacketTransponderState.ErrorToSend) return;
            _state.OnNext(PacketTransponderState.ErrorToSend);
            _logger.ZLogError($"{new TPacket().Name} sending error:{e.Message}");
        }

        private void LogSuccess()
        {
            if (_state.Value == PacketTransponderState.Ok) return;
            _state.OnNext(PacketTransponderState.Ok);
            _logger.ZLogDebug($"{new TPacket().Name} start stream");
        }

        private void LogSkipped()
        {
            if (_state.Value == PacketTransponderState.Skipped) return;
            _state.OnNext(PacketTransponderState.Skipped);
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
            try
            {
                _dataLock.EnterWriteLock();
                changeCallback(_packet);
            }
            catch (Exception e)
            {
                _logger.ZLogError(e,$"Error to set new value for {new TPacket().Name}:{e.Message}");
            }
            finally
            {
                _dataLock.ExitWriteLock();
            }
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataLock.Dispose();
                _state.Dispose();
                _timer?.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            await CastAndDispose(_dataLock).ConfigureAwait(false);
            await CastAndDispose(_state).ConfigureAwait(false);
            if (_timer != null) await _timer.DisposeAsync().ConfigureAwait(false);

            await base.DisposeAsyncCore().ConfigureAwait(false);

            return;

            static async ValueTask CastAndDispose(IDisposable resource)
            {
                if (resource is IAsyncDisposable resourceAsyncDisposable)
                    await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
                else
                    resource.Dispose();
            }
        }

        #endregion

    }
}
