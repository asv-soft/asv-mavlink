using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class Px4CopterClient(
    IMavlinkV2Connection connection,
    MavlinkClientIdentity identity,
    VehicleClientConfig config,
    IPacketSequenceCalculator seq,
    IScheduler scheduler)
    : Px4VehicleClient(connection, identity, config, seq, scheduler)
{
    protected override string DefaultName => $"PX4 [{Identity.TargetSystemId:00},{Identity.TargetComponentId:00}]";
    public override DeviceClass Class => DeviceClass.Copter;
    protected override Task<IReadOnlyCollection<ParamDescription>> GetParamDescription()
    {
        throw new System.NotImplementedException();
    }

    public override Task EnsureInGuidedMode(CancellationToken cancel)
    {
        throw new System.NotImplementedException();
    }

    public override Task<bool> CheckGuidedMode(CancellationToken cancel)
    {
        throw new System.NotImplementedException();
    }

    public override Task GoTo(GeoPoint point, CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override Task DoLand(CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override Task DoRtl(CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override Task TakeOff(double altInMeters, CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerable<IVehicleMode> AvailableModes { get; }
    protected override IVehicleMode InternalInterpretMode(HeartbeatPayload heartbeatPayload)
    {
        throw new System.NotImplementedException();
    }

    public override Task SetVehicleMode(IVehicleMode mode, CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override Task<bool> CheckAutoMode(CancellationToken cancel)
    {
        throw new System.NotImplementedException();
    }

    public override Task SetAutoMode(CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override Task EnsureInAutoMode(CancellationToken cancel)
    {
        throw new System.NotImplementedException();
    }
}