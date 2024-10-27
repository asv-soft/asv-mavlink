#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;

namespace Asv.Mavlink;

public class AdsbClientDeviceConfig : ClientDeviceBaseConfig
{
    public AdsbVehicleClientConfig Adsb { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();
}

public class AdsbClientDevice(MavlinkClientIdentity identity, AdsbClientDeviceConfig config, ICoreServices core, IEnumerable<ParamDescription> paramDescriptions)
    : ClientDevice(identity, config, core,DeviceClass.Adsb)
{

    protected override IEnumerable<IMavlinkMicroserviceClient> CreateMicroservices()
    {
        yield return new CommandClient(Identity, config.Command, Core);
        var paramBase = new ParamsClient(Identity, config.Params, Core);
        yield return paramBase;
        yield return new ParamsClientEx(paramBase, config.Params, MavParamHelper.CStyleEncoding, paramDescriptions);
        yield return new AdsbVehicleClient(Identity, config.Adsb, Core);
    }

    protected override Task InitBeforeMicroservices(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }

    protected override Task InitAfterMicroservices(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }
}