#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink
{
    public class HeartbeatClientConfig
    {
        public int HeartbeatTimeoutMs { get; set; } = 2000;
        public int LinkQualityWarningSkipCount { get; set; } = 3;
        public int RateMovingAverageFilter { get; set; } = 3;
    }
    
    public class HeartbeatClient : MavlinkMicroserviceClient, IHeartbeatClient
    {
        
        private static readonly TimeSpan CheckConnectionDelay = TimeSpan.FromSeconds(1);
        
        private readonly IScheduler _scheduler;
        private readonly CircularBuffer2<double> _valueBuffer = new(5);
        private readonly IncrementalRateCounter _rxRate;
        private readonly RxValueBehaviour<HeartbeatPayload> _heartBeat;
        private readonly RxValueBehaviour<double> _packetRate;
        private readonly RxValueBehaviour<double> _linkQuality;
        private readonly LinkIndicator _link;
        private long _lastHeartbeat;
        private long _totalRateCounter;
        private readonly TimeSpan _heartBeatTimeoutMs;
        private readonly List<byte> _lastPacketList = new();
        private readonly TimeProvider _timeProvider;
        private readonly object _sync = new();


        public HeartbeatClient(
            IMavlinkV2Connection connection, 
            MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, 
            HeartbeatClientConfig config, 
            TimeProvider? timeProvider = null,
            IScheduler? scheduler = null,
            ILoggerFactory? logFactory = null)
            :base("HEARTBEAT", connection, identity, seq,timeProvider,scheduler,logFactory)
        {
            _scheduler = scheduler ?? System.Reactive.Concurrency.Scheduler.Immediate;
            logFactory??=NullLoggerFactory.Instance;
            ILogger logger = logFactory.CreateLogger<HeartbeatClient>();
            logger.ZLogTrace(
                $"ctor: ID={identity},timeout:{config.HeartbeatTimeoutMs} ms, rate:{config.RateMovingAverageFilter}, warn after {config.LinkQualityWarningSkipCount} skip");
            ArgumentNullException.ThrowIfNull(config);
            _timeProvider = timeProvider ?? TimeProvider.System;
            FullId = MavlinkHelper.ConvertToFullId(identity.TargetComponentId, identity.TargetSystemId);
            _rxRate = new IncrementalRateCounter(config.RateMovingAverageFilter,timeProvider);
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
                }).DisposeItWith(Disposable);

            _heartBeat = new RxValueBehaviour<HeartbeatPayload>(new HeartbeatPayload()).DisposeItWith(Disposable);
            InternalFilter<HeartbeatPacket>()
                .Select(p => p.Payload)
                .Subscribe(_heartBeat).DisposeItWith(Disposable);

            _packetRate = new RxValueBehaviour<double>(0)
                .DisposeItWith(Disposable);
            _link = new LinkIndicator(config.LinkQualityWarningSkipCount)
                .DisposeItWith(Disposable);
            _linkQuality = new RxValueBehaviour<double>(0)
                .DisposeItWith(Disposable);
            _timeProvider
                .CreateTimer(CheckConnection, null, CheckConnectionDelay, CheckConnectionDelay)
                .DisposeItWith(Disposable);
            
            RawHeartbeat.Subscribe(_ =>
            {
                if (IsDisposed) return;
                Interlocked.Exchange(ref _lastHeartbeat,_timeProvider.GetTimestamp());
                _link.Upgrade();
            }).DisposeItWith(Disposable);
            
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
        public IRxValue<HeartbeatPayload> RawHeartbeat => _heartBeat;
        public IRxValue<double> PacketRateHz => _packetRate;
        public IRxValue<double> LinkQuality => _linkQuality;
        public IRxValue<LinkState> Link => _link;

        private void CheckConnection(object? state)
        {
            CalculateLinqQuality();
            var rate = _rxRate.Calculate(Interlocked.Read(ref _totalRateCounter));
            _scheduler.Schedule(() => _packetRate.OnNext(rate));
            if (_timeProvider.GetElapsedTime(_lastHeartbeat) <= _heartBeatTimeoutMs) return;
            _link.Downgrade();
            if (_link.Value == LinkState.Disconnected)
            {
                _scheduler.Schedule(() =>
                {
                    _packetRate.OnNext(0);
                    _linkQuality.OnNext(0);
                });
            }
        }
    }
}
