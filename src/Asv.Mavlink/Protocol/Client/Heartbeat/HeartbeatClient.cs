using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public class HeartbeatClient : MavlinkMicroserviceClient, IHeartbeatClient
    {
        private readonly int _heartBeatTimeoutMs;
        private readonly RxValue<HeartbeatPayload> _heartBeat = new RxValue<HeartbeatPayload>();
        private readonly RxValue<int> _packetRate = new();
        private readonly RxValue<double> _linkQuality = new RxValue<double>();
        private readonly LinkIndicator _link = new LinkIndicator(3);
        private DateTime _lastHeartbeat;
        private int _lastPacketId;
        private int _packetCounter;
        private int _prev;

        public HeartbeatClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq, int heartBeatTimeoutMs = 2000):base(connection,identity, seq,"HEARTBEAT")
        {
            _heartBeatTimeoutMs = heartBeatTimeoutMs;
            FilterVehiclePackets
                .Select(_ => _.Sequence)
                .Subscribe(_ =>
                {
                    Interlocked.Exchange(ref _lastPacketId, _);
                    Interlocked.Increment(ref _packetCounter);
                }).DisposeItWith(Disposable);


            Filter<HeartbeatPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_heartBeat).DisposeItWith(Disposable);
            Disposable.Add(_heartBeat);

            FilterVehiclePackets
                .Select(_ => 1)
                .Buffer(TimeSpan.FromSeconds(3))
                .Select(_ => _.Sum()/3).Subscribe(_packetRate).DisposeItWith(Disposable);
            Disposable.Add(_packetRate);

            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Subscribe(CheckConnection).DisposeItWith(Disposable);
            RawHeartbeat.Subscribe(_ =>
            {
                if (DisposeCancel.IsCancellationRequested) return;
                _lastHeartbeat = DateTime.Now;
                _link.Upgrade();
                CalculateLinqQuality();
            }).DisposeItWith(Disposable);
            Disposable.Add(_link);
        }

        private void CalculateLinqQuality()
        {
            if (_packetCounter <= 3)
            {
                return;
            }
            var last = _lastPacketId;
            var count = Interlocked.Exchange(ref _packetCounter, 0);
            var first = Interlocked.Exchange(ref _prev, last);

            var seq = last - first;
            if (seq < 0) seq = last + byte.MaxValue - first + 1;
            _linkQuality.OnNext(Math.Round(((double)count) / seq,2));
        }

        public IRxValue<HeartbeatPayload> RawHeartbeat => _heartBeat;
        public IRxValue<int> PacketRateHz => _packetRate;
        public IRxValue<double> LinkQuality => _linkQuality;
        public IRxValue<LinkState> Link => _link;

        private void CheckConnection(long value)
        {
            if (DateTime.Now - _lastHeartbeat > TimeSpan.FromMilliseconds(_heartBeatTimeoutMs))
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
