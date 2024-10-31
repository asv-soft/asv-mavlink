using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

public class AdsbServerDeviceConfig : ServerDeviceConfig
{
    public ParamsServerExConfig Params { get; set; } = new();
}

public class AdsbServerDevice : ServerDevice, IAdsbServerDevice
{
    private readonly CommandServer _command;
    private readonly AdsbVehicleServer _adsb;
    private readonly ParamsServerEx _paramsEx;
    private readonly ParamsServer _paramsBase;

    public AdsbServerDevice(
        MavlinkIdentity identity, 
        AdsbServerDeviceConfig config,
        IEnumerable<IMavParamTypeMetadata> paramList,
        IMavParamEncoding encoding,
        IConfiguration paramStore,
        ICoreServices core)
        : base(identity, config, core)
    {
        _command = new CommandServer(identity,core);
        _adsb = new AdsbVehicleServer( identity, core);
        _paramsBase = new ParamsServer(identity, core);
        _paramsEx = new ParamsServerEx(_paramsBase, StatusText, paramList, encoding, paramStore,config.Params);
        Heartbeat.Set(p => p.Type = MavType.MavTypeAdsb);
        
    }

    
    public IAdsbVehicleServer Adsb => _adsb;

    public IParamsServerEx Params => _paramsEx;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _command.Dispose();
            _adsb.Dispose();
            _paramsEx.Dispose();
            _paramsBase.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_command).ConfigureAwait(false);
        await CastAndDispose(_adsb).ConfigureAwait(false);
        await CastAndDispose(_paramsEx).ConfigureAwait(false);
        await CastAndDispose(_paramsBase).ConfigureAwait(false);
        

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