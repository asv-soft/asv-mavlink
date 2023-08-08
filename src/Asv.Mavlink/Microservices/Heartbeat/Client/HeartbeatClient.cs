#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly CircularBuffer2<double> _valueBuffer = new(5);
        private readonly IncrementalRateCounter _rxRate;
        private readonly RxValue<HeartbeatPayload> _heartBeat;
        private readonly RxValue<double> _packetRate;
        private readonly RxValue<double> _linkQuality;
        private readonly LinkIndicator _link;
        private DateTime _lastHeartbeat;
        private int _prev;
        private long _totalRateCounter;
        private readonly TimeSpan _heartBeatTimeoutMs;
        private SortedSet<byte> _lastPacketList = new();
        private readonly object _lastPacketListLock = new();

        public HeartbeatClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, HeartbeatClientConfig config, IScheduler? scheduler = null):base("HEARTBEAT", connection, identity, seq)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            FullId = (ushort)(identity.TargetComponentId | identity.TargetSystemId << 8);
            _rxRate = new IncrementalRateCounter(config.RateMovingAverageFilter);
            _heartBeatTimeoutMs = TimeSpan.FromMilliseconds(config.HeartbeatTimeoutMs);
            InternalFilteredVehiclePackets
                .Select(x => x.Sequence)
                .Subscribe(x =>
                {
                    lock (_lastPacketListLock)
                    {
                        _lastPacketList.Add(x);    
                    }
                    Interlocked.Increment(ref _totalRateCounter);
                }).DisposeItWith(Disposable);

            _heartBeat = new RxValue<HeartbeatPayload>().DisposeItWith(Disposable);
            InternalFilter<HeartbeatPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_heartBeat).DisposeItWith(Disposable);

            _packetRate = new RxValue<double>().DisposeItWith(Disposable);
            _link = new LinkIndicator(config.LinkQualityWarningSkipCount).DisposeItWith(Disposable);
            _linkQuality = new RxValue<double>(1).DisposeItWith(Disposable);
            if (scheduler != null)
            {
                Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), scheduler)
                    .Subscribe(CheckConnection)
                    .DisposeItWith(Disposable);    
            }
            else
            {
                Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
                    .Subscribe(CheckConnection)
                    .DisposeItWith(Disposable);
            }
            
            RawHeartbeat.Subscribe(_ =>
            {
                if (IsDisposed) return;
                _lastHeartbeat = DateTime.Now;
                _link.Upgrade();
            }).DisposeItWith(Disposable);
            Disposable.Add(_link);
        }

        private void CalculateLinqQuality()
        {
            byte first;
            byte last;
            var count = 0;
            lock (_lastPacketListLock)
            {
                first = _lastPacketList.FirstOrDefault();
                last = _lastPacketList.LastOrDefault();
                count = _lastPacketList.Count;
                _lastPacketList.Clear();
            }
            if (count == 0) return;
            
            var seq = last - first;
            if (seq < 0) seq = last + byte.MaxValue - first + 1;
            _linkQuality.OnNext(Math.Min(1, Math.Round(((double)count) / seq,2)));
        }

        public ushort FullId { get; }
        public IRxValue<HeartbeatPayload> RawHeartbeat => _heartBeat;
        public IRxValue<double> PacketRateHz => _packetRate;
        public IRxValue<double> LinkQuality => _linkQuality;
        public IRxValue<LinkState> Link => _link;

        private void CheckConnection(long value)
        {
            CalculateLinqQuality();
            _packetRate.OnNext(_rxRate.Calculate(Interlocked.Read(ref _totalRateCounter)));
            if (DateTime.Now - _lastHeartbeat > _heartBeatTimeoutMs)
            {
                _link.Downgrade();
                if (_link.Value == LinkState.Disconnected)
                {
                    _packetRate.OnNext(0);
                    _linkQuality.OnNext(0);
                }
            }
        }
    }
}
