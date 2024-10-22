#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public class AdsbClientDeviceConfig : ClientDeviceConfig
{
    public AdsbVehicleClientConfig Adsb { get; set; } = new();
}

public class AdsbClientDevice(MavlinkClientIdentity identity, AdsbClientDeviceConfig config, ICoreServices core)
    : ClientDevice(identity, config, core), IAdsbClientDevice
{
    private readonly AdsbVehicleClient _adsb = new(identity, config.Adsb, core);
    private bool _disposedValue;

    

    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }
    public override DeviceClass Class => DeviceClass.Adsb;
    public IAdsbVehicleClient Adsb => _adsb;
    protected override void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            // dispose managed state (managed objects).
            _adsb.Dispose();
        }
        _disposedValue = true;
        // free unmanaged resources (unmanaged objects) and override a finalizer below.
        // set large fields to null.
    }
}