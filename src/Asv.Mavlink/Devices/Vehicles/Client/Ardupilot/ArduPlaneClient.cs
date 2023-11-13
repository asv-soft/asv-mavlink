#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class ArduPlaneClient:ArduVehicle
{
    public ArduPlaneClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, VehicleClientConfig config, IPacketSequenceCalculator seq, IScheduler? scheduler = null) : base(connection, identity, config, seq, scheduler)
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

    public override Task SetAutoMode(CancellationToken cancel = default)
    {
        return Commands.DoSetMode(1, (uint)PlaneMode.PlaneModeAuto, 0,cancel);
    }

    public override IEnumerable<IVehicleMode> AvailableModes => ArdupilotPlaneMode.AllModes;
    protected override IVehicleMode? InternalInterpretMode(HeartbeatPayload heartbeatPayload)
    {
        return AvailableModes.Cast<ArdupilotPlaneMode>()
            .FirstOrDefault(_ => _.CustomMode == (PlaneMode)heartbeatPayload.CustomMode);
    }

    public override Task SetVehicleMode(IVehicleMode mode, CancellationToken cancel = default)
    {
        if (mode is not ArdupilotPlaneMode) throw new Exception($"Invalid mode. Only {nameof(ArdupilotPlaneMode)} supported");
        return Commands.DoSetMode(1, (uint)((ArdupilotPlaneMode)mode).CustomMode, 0,cancel);
    }


    public override Task DoLand(CancellationToken cancel = default)
    {
        throw new System.NotImplementedException();
    }

    public override Task DoRtl(CancellationToken cancel = default)
    {
        return SetVehicleMode(ArdupilotPlaneMode.Rtl, cancel);
    }
    
}