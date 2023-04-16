using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class ArduCopterClient:ArduVehicle
{
    public ArduCopterClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, VehicleClientConfig config, IPacketSequenceCalculator seq, IScheduler scheduler) 
        : base(connection, identity, config, seq, scheduler)
    {
    }

    protected override Task<string> GetCustomName(CancellationToken cancel)
    {
        return Task.FromResult("ArduCopter");
    }

    public override DeviceClass Class => DeviceClass.Copter;
    protected override Task<IReadOnlyCollection<ParamDescription>> GetParamDescription()
    {
        // TODO: Read from device by FTP or load from XML file
        return Task.FromResult((IReadOnlyCollection<ParamDescription>)new List<ParamDescription>());
    }

    public override async Task GoTo(GeoPoint point, CancellationToken cancel = default)
    {
        await EnsureInGuidedMode(cancel).ConfigureAwait(false);
        await Position.SetTarget(point, cancel).ConfigureAwait(false);
    }

    public override async Task DoLand(CancellationToken cancel = default)
    {
        await EnsureInGuidedMode(cancel).ConfigureAwait(false);
        await Commands.DoSetMode(1, (uint)CopterMode.CopterModeLand, 0, cancel).ConfigureAwait(false);
    }

    public override async Task DoRtl(CancellationToken cancel = default)
    {
        await EnsureInGuidedMode(cancel).ConfigureAwait(false);
        await Commands.DoSetMode(1, (uint)CopterMode.CopterModeRtl, 0, cancel).ConfigureAwait(false);
    }

    

    public override Task<bool> CheckGuidedMode(CancellationToken cancel)
    {
        return Task.FromResult(
            Heartbeat.RawHeartbeat.Value.BaseMode.HasFlag(MavModeFlag.MavModeFlagCustomModeEnabled) &&
            Heartbeat.RawHeartbeat.Value.CustomMode == (int)CopterMode.CopterModeGuided);
    }
    public override async Task EnsureInGuidedMode(CancellationToken cancel)
    {
        if (!await CheckGuidedMode(cancel).ConfigureAwait(false))
        {
            await Commands.DoSetMode(1, (uint)CopterMode.CopterModeGuided, 0,cancel).ConfigureAwait(false);
        }
    }
}