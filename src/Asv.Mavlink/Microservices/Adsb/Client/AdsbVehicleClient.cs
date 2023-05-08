using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class AdsbVehicleClient : MavlinkMicroserviceClient, IAdsbVehicleClient
{
    private readonly RxValue<AdsbVehiclePayload> _target;
    private int _lastPacketId;
    private int _packetCounter;
    private long _totalRateCounter;
    
    public AdsbVehicleClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq, 
        IScheduler scheduler) : base("ADSB", connection, identity, seq, scheduler)
    {
        FullId = (ushort)(identity.TargetComponentId | identity.TargetSystemId << 8);
        InternalFilteredVehiclePackets
            .Select(_ => _.Sequence)
            .Subscribe(_ =>
            {
                Interlocked.Exchange(ref _lastPacketId, _);
                Interlocked.Increment(ref _packetCounter);
                Interlocked.Increment(ref _totalRateCounter);
            }).DisposeItWith(Disposable);

        _target = new RxValue<AdsbVehiclePayload>().DisposeItWith(Disposable);
        InternalFilter<AdsbVehiclePacket>()
            .Select(_ => _.Payload)
            .Subscribe(_target).DisposeItWith(Disposable);
    }

    public ushort FullId { get; }
    public RxValue<AdsbVehiclePayload> Target => _target;
}