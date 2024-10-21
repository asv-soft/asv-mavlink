using System;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink
{
    public class MavlinkPacketTransponder<TPacket,TPayload> : IMavlinkPacketTransponder<TPacket,TPayload>, IDisposable
        where TPacket : IPacketV2<TPayload>, new()
        where TPayload : IPayload, new()
    {
        private readonly IMavlinkV2Connection _connection;
        private readonly IPacketSequenceCalculator _seq;
        private readonly object _sync = new();
        private readonly ILogger _logger;
        private readonly ReaderWriterLockSlim _dataLock = new();
        private int _isSending;
        private readonly RxValue<PacketTransponderState> _state = new();
        private TPacket _packet;
        private ITimer? _timer;
        private readonly TimeProvider _timeProvider;
        private readonly CancellationTokenSource _disposeCancel;

        public MavlinkPacketTransponder(MavlinkIdentity identityConfig, ICoreServices core)
        {
            _logger = core.Log.CreateLogger<IMavlinkPacketTransponder<TPacket,TPayload>>();
            _connection = core.Connection;
            _seq = core.Sequence;
            _timeProvider = core.TimeProvider;
            _disposeCancel = new CancellationTokenSource();
            _packet = new TPacket
            {
                CompatFlags = 0,
                IncompatFlags = 0,
                ComponentId = identityConfig.ComponentId,
                SystemId = identityConfig.SystemId,
            };
        }

        public void Start(TimeSpan dueTime, TimeSpan period)
        {
            if (_packet == null) throw new Exception($"You need call '{nameof(Set)}' method< before call start");
            lock (_sync)
            {
                _timer?.Dispose();
                _timer = _timeProvider.CreateTimer(OnTick,null,dueTime, period);
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
                ((IPacketV2<IPayload>) _packet).Sequence = _seq.GetNextSequenceNumber();
                _connection.Send((IPacketV2<IPayload>)_packet, DisposeCancel).Wait(DisposeCancel);
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

        private CancellationToken DisposeCancel => _disposeCancel.Token;

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

        public IRxValue<PacketTransponderState> State => _state;

        public void Stop()
        {
            lock (_sync)
            {
                _timer?.Dispose();
                _timer = null;
                IsStarted = false;
            }
        }

        public void Set(Action<TPayload> changeCallback)
        {
            try
            {
                _dataLock.EnterWriteLock();
                changeCallback(_packet.Payload);
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

        public void Dispose()
        {
            _dataLock.Dispose();
            Stop();
            _state?.Dispose();
            _disposeCancel.Dispose();
        }
    }
}
