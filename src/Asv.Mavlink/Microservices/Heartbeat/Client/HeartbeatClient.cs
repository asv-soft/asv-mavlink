#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
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
        private readonly RxValueBehaviour<HeartbeatPayload?> _heartBeat;
        private readonly RxValueBehaviour<double> _packetRate;
        private readonly RxValueBehaviour<double> _linkQuality;
        private readonly LinkIndicator _link;
        private long _lastHeartbeat;
        private long _totalRateCounter;
        private readonly TimeSpan _heartBeatTimeoutMs;
        private readonly List<byte> _lastPacketList = new();
        private readonly TimeProvider _timeProvider;
        private readonly object _sync = new();
        private readonly IDisposable _disposeIt;
        private readonly ILogger<HeartbeatClient> _logger;


        public HeartbeatClient(MavlinkClientIdentity identity, HeartbeatClientConfig config, ICoreServices core)
            :base("HEARTBEAT", identity, core)
        {
            _logger = core.Log.CreateLogger<HeartbeatClient>();
            _logger.ZLogTrace($"{Name} ID={identity},timeout:{config.HeartbeatTimeoutMs} ms, rate:{config.RateMovingAverageFilter}, warn after {config.LinkQualityWarningSkipCount} skip");
            ArgumentNullException.ThrowIfNull(config);
            var builder = Disposable.CreateBuilder();
            _timeProvider = core.TimeProvider;
            FullId = MavlinkHelper.ConvertToFullId(identity.Target.ComponentId, identity.Target.SystemId);
            _rxRate = new IncrementalRateCounter(config.RateMovingAverageFilter,core.TimeProvider);
            _heartBeatTimeoutMs = TimeSpan.FromMilliseconds(config.HeartbeatTimeoutMs);
            InternalFilteredVehiclePackets
                .Select(x => x.Sequence)
                .Subscribe(x =>
                {
                    lock (_sync)
                    {
                        _lastPacketList.Add(x);    
                    }
                    Interlocked.Increment(ref _totalRateCounter);
                }).AddTo(ref builder);
            _heartBeat = new RxValueBehaviour<HeartbeatPayload?>(null).AddTo(ref builder);
            InternalFilter<HeartbeatPacket>()
                .Select(p => p.Payload)
                .Subscribe(_heartBeat)
                .AddTo(ref builder);

            _packetRate = new RxValueBehaviour<double>(0)
                .AddTo(ref builder);
            _link = new LinkIndicator(config.LinkQualityWarningSkipCount)
                .AddTo(ref builder);
            _linkQuality = new RxValueBehaviour<double>(0)
                .AddTo(ref builder);
            _timeProvider
                .CreateTimer(CheckConnection, null, TimeSpan.Zero, CheckConnectionDelay)
                .AddTo(ref builder);
            
            // we need skip first packet because it's not a real packet
            RawHeartbeat.Skip(1).Subscribe(_ =>
            {
                Interlocked.Exchange(ref _lastHeartbeat,_timeProvider.GetTimestamp());
                _link.Upgrade();
            }).AddTo(ref builder);

            if (config.PrintLinkStateToLog)
            {
                _link.DistinctUntilChanged().Skip(1).Subscribe(PrintLinkToLog).AddTo(ref builder);
            }
            if (config.PrintStatisticsToLogDelayMs > 0)
            {
                var delay = TimeSpan.FromMilliseconds(config.PrintStatisticsToLogDelayMs);
                _timeProvider.CreateTimer(PrintRateAndQualityToLog, null,delay,delay).AddTo(ref builder);
            }
            
            _disposeIt = builder.Build();
            
        }

        private void PrintRateAndQualityToLog(object? state)
        {
            _logger.ZLogInformation($"Link {Identity} rate={PacketRateHz.Value:F2} Hz, quality={LinkQuality.Value:P0}");
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
        public IRxValue<HeartbeatPayload?> RawHeartbeat => _heartBeat;
        public IRxValue<double> PacketRateHz => _packetRate;
        public IRxValue<double> LinkQuality => _linkQuality;
        public IRxValue<LinkState> Link => _link;

        private void CheckConnection(object? state)
        {
            CalculateLinqQuality();
            var rate = _rxRate.Calculate(Interlocked.Read(ref _totalRateCounter));
            _packetRate.OnNext(rate);
            if (_timeProvider.GetElapsedTime(_lastHeartbeat) <= _heartBeatTimeoutMs) return;
            _link.Downgrade();
            if (_link.Value == LinkState.Disconnected)
            {
                _packetRate.OnNext(0);
                _linkQuality.OnNext(0);
            }
        }

        public override void Dispose()
        {
            _disposeIt.Dispose();
            base.Dispose();
        }
    }
}
