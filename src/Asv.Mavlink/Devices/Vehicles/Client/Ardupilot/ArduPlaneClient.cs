using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Common;

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

    public override async Task EnsureInGuidedMode(CancellationToken cancel)
    {
        if (!await CheckGuidedMode(cancel).ConfigureAwait(false))
        {
            await Commands.DoSetMode(1, (uint)PlaneMode.PlaneModeGuided, 0,cancel).ConfigureAwait(false);
        }
    }

    public override Task<bool> CheckGuidedMode(CancellationToken cancel)
    {
        return Task.FromResult(
            Heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
            Heartbeat.RawHeartbeat.Value.CustomMode == (int)PlaneMode.PlaneModeGuided);
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
        return Commands.DoSetMode(1, (int)PlaneMode.PlaneModeRtl, 0, cancel: cancel);
    }
    
}