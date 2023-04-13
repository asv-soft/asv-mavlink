using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public class ArduPlaneClient:ArduVehicle
{
    public ArduPlaneClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, VehicleClientConfig config, IPacketSequenceCalculator seq, IScheduler scheduler) : base(connection, identity, config, seq, scheduler)
    {
    }

    protected override Task<string> GetCustomName(CancellationToken cancel)
    {
        return Task.FromResult("Arduplane");
    }

    public override DeviceClass Class => DeviceClass.Plane;
    protected override Task<IReadOnlyCollection<ParamDescription>> GetParamDescription()
    {
        // TODO: Read from device by FTP or load from XML file
        return Task.FromResult((IReadOnlyCollection<ParamDescription>)new List<ParamDescription>());
    }
}