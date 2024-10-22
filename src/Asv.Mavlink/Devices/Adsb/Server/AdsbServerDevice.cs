using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class AdsbServerDeviceConfig : ServerDeviceConfig
{
    
}

public class AdsbServerDevice : ServerDevice, IAdsbServerDevice
{
    private readonly CommandServer _command;
    private readonly AdsbVehicleServer _adsb;

    public AdsbServerDevice(MavlinkIdentity identity, AdsbServerDeviceConfig config,ICoreServices core)
        : base(identity, config, core)
    {
        _command = new CommandServer(identity,core);
        _adsb = new AdsbVehicleServer( identity, core);
        Heartbeat.Set(p => p.Type = MavType.MavTypeAdsb);
    }

    public IAdsbVehicleServer Adsb => _adsb;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _command.Dispose();
            _adsb.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_command).ConfigureAwait(false);
        await CastAndDispose(_adsb).ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

}