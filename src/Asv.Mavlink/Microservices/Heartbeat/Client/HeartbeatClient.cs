#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Minimal;

using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink
{
    public class HeartbeatClientConfig
    {
        public int HeartbeatTimeoutMs { get; set; } = 2000;
        public int LinkQualityWarningSkipCount { get; set; } = 3;
        public int RateMovingAverageFilter { get; set; } = 3;
        public int PrintStatisticsToLogDelayMs { get; set; } = 10_000;
        public bool PrintLinkStateToLog { get; set; } = true;
    }
    
    public class HeartbeatClient : MavlinkMicroserviceClient, IHeartbeatClient
    {
        private static readonly TimeSpan CheckConnectionDelay = TimeSpan.FromSeconds(1);
        private readonly CircularBuffer2<double> _valueBuffer = new(5);
        private readonly IncrementalRateCounter _rxRate;
        private readonly ReactiveProperty<double> _packetRate;
        private readonly ReactiveProperty<double> _linkQuality;
        private readonly ManualLinkIndicator _link;
        private long _lastHeartbeat;
        private long _totalRateCounter;
        private readonly TimeSpan _heartBeatTimeoutMs;
        private readonly List<byte> _lastPacketList = new();
        private readonly TimeProvider _timeProvider;
        private readonly object _sync = new();
        private readonly ILogger<HeartbeatClient> _logger;
        private readonly ReadOnlyReactiveProperty<HeartbeatPayload?> _heartBeat;
        private readonly IDisposable _obs1;
        private readonly IDisposable _obs2;
        private readonly IDisposable? _obs3;
        private readonly ITimer? _obs4;


        public HeartbeatClient(MavlinkClientIdentity identity, HeartbeatClientConfig config, ICoreServices core)
            :base(Heartbeat.MicroserviceName, identity, core)
        {
            _logger = core.LoggerFactory.CreateLogger<HeartbeatClient>();
            _logger.ZLogTrace($"{TypeName} ID={identity},timeout:{config.HeartbeatTimeoutMs} ms, rate:{config.RateMovingAverageFilter}, warn after {config.LinkQualityWarningSkipCount} skip");
            ArgumentNullException.ThrowIfNull(config);
            _timeProvider = core.TimeProvider;
            FullId = MavlinkHelper.ConvertToFullId(identity.Target.ComponentId, identity.Target.SystemId);
            _rxRate = new IncrementalRateCounter(config.RateMovingAverageFilter,core.TimeProvider);
            _heartBeatTimeoutMs = TimeSpan.FromMilliseconds(config.HeartbeatTimeoutMs);
            _obs1 = InternalFilteredDeviceMessages
                .Select(x => x.Sequence)
                .Subscribe(x =>
                {
                    lock (_sync)
                    {
                        _lastPacketList.Add(x);    
                    }
                    Interlocked.Increment(ref _totalRateCounter);
                });
            _heartBeat = InternalFilter<HeartbeatPacket>()
                .Select(p => p?.Payload)
                .ToReadOnlyReactiveProperty();

            _packetRate = new ReactiveProperty<double>(0);
            _link = new ManualLinkIndicator(config.LinkQualityWarningSkipCount);
            // TODO: error at LinkIndicator. After first downgrade status will be Downgrade. Remove after fix at Asv.Common
            for (var i = 0; i < config.LinkQualityWarningSkipCount + 1; i++)
            {
                _link.Downgrade();
            }
            
            
            _linkQuality = new ReactiveProperty<double>();
            _timeProvider
                .CreateTimer(CheckConnection, null, CheckConnectionDelay, CheckConnectionDelay);
            
            // we need skip first packet because it's not a real packet
            _obs2 = RawHeartbeat.Skip(1).Subscribe(_ =>
            {
                Interlocked.Exchange(ref _lastHeartbeat, _timeProvider.GetTimestamp());
                _link.Upgrade();
            });

            if (config.PrintLinkStateToLog)
            {
                _obs3 = _link.State.Skip(1).Subscribe(PrintLinkToLog);
            }
            if (config.PrintStatisticsToLogDelayMs > 0)
            {
                var delay = TimeSpan.FromMilliseconds(config.PrintStatisticsToLogDelayMs);
                _obs4 = _timeProvider.CreateTimer(PrintRateAndQualityToLog, null,delay,delay);
            }
        }

        private void PrintRateAndQualityToLog(object? state)
        {
            _logger.ZLogInformation($"Link {Identity} rate={PacketRateHz.CurrentValue:F2} Hz, quality={LinkQuality.CurrentValue:P0}");
        }

        private void PrintLinkToLog(LinkState x)
        {
            switch (x)
            {
                case LinkState.Disconnected:
                    _logger.ZLogError($"Link {Identity} changed to {x:G}");
                    break;
                case LinkState.Downgrade:
                    _logger.ZLogWarning($"Link {Identity} changed to {x:G}");
                    break;
                case LinkState.Connected:
                    _logger.ZLogInformation($"Link {Identity} changed to {x:G}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(x), x, null);
            }
        }

        private void CalculateLinqQuality()
        {
            var count = 0;
            byte first;
            byte last;
            lock (_sync)
            {
                first = _lastPacketList.FirstOrDefault();
                last = _lastPacketList.LastOrDefault();
                count = _lastPacketList.Count;
                _lastPacketList.Clear();    
            }
            
            if (count == 0) return;
            
            var seq = last - first + 1;
            if (seq <= 0)
            {
                seq = last + byte.MaxValue - first + 2;
            }
            Debug.Assert(seq != 0);
            _valueBuffer.PushFront(Math.Min(1, Math.Round(((double)count) / seq,2)));
            _linkQuality.OnNext(_valueBuffer.Average());
        }

        public ushort FullId { get; }
        public ReadOnlyReactiveProperty<HeartbeatPayload?> RawHeartbeat => _heartBeat;
        public ReadOnlyReactiveProperty<double> PacketRateHz => _packetRate;
        public ReadOnlyReactiveProperty<double> LinkQuality => _linkQuality;
        public ILinkIndicator Link => _link;

        private void CheckConnection(object? state)
        {
            CalculateLinqQuality();
            var rate = _rxRate.Calculate(Interlocked.Read(ref _totalRateCounter));
            _packetRate.OnNext(rate);
            if (_timeProvider.GetElapsedTime(_lastHeartbeat) <= _heartBeatTimeoutMs) return;
            _link.Downgrade();
            if (_link.State.CurrentValue == LinkState.Disconnected)
            {
                _packetRate.OnNext(0);
                _linkQuality.OnNext(0);
            }
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _packetRate.Dispose();
                _linkQuality.Dispose();
                _link.Dispose();
                _heartBeat.Dispose();
                _obs1.Dispose();
                _obs2.Dispose();
                _obs3?.Dispose();
                _obs4?.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsyncCore()
        {
            await CastAndDispose(_packetRate).ConfigureAwait(false);
            await CastAndDispose(_linkQuality).ConfigureAwait(false);
            await CastAndDispose(_link).ConfigureAwait(false);
            await CastAndDispose(_heartBeat).ConfigureAwait(false);
            await CastAndDispose(_obs1).ConfigureAwait(false);
            await CastAndDispose(_obs2).ConfigureAwait(false);
            if (_obs3 != null) await CastAndDispose(_obs3).ConfigureAwait(false);
            if (_obs4 != null) await _obs4.DisposeAsync().ConfigureAwait(false);
            await CastAndDispose(Link).ConfigureAwait(false);

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
