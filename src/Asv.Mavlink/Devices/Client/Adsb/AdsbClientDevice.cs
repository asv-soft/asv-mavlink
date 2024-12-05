#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;

namespace Asv.Mavlink;

public class AdsbClientDeviceConfig : MavlinkClientDeviceConfig
{
    public AdsbVehicleClientConfig Adsb { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();

    public override void Load(string key, IConfiguration configuration)
    {
        base.Load(key, configuration);
        Adsb = configuration.Get<AdsbVehicleClientConfig>();
        Params = configuration.Get<ParamsClientExConfig>();
        Command = configuration.Get<CommandProtocolConfig>();
    }
    
    public override void Save(string key, IConfiguration configuration)
    {
        base.Save(key, configuration);
        configuration.Set(Adsb);
        configuration.Set(Params);
        configuration.Set(Command);
    }
}

public class AdsbClientDevice(
    MavlinkClientDeviceId identity,
    AdsbClientDeviceConfig config,
    ImmutableArray<IClientDeviceExtender> extenders,
    IMavlinkContext core)
    : MavlinkClientDevice(identity, config, extenders, core)
{
    public const string DeviceClass = "Adsb";
    private readonly MavlinkClientDeviceId _identity = identity;
    private readonly IMavlinkContext _core = core;

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices([EnumeratorCancellation] CancellationToken cancel)
    {
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            yield return microservice;
        }
        yield return new CommandClient(_identity.Id, config.Command, _core);
        var paramBase = new ParamsClient(_identity.Id, config.Params, _core);
        yield return paramBase;
        yield return new ParamsClientEx(paramBase, config.Params, MavParamHelper.CStyleEncoding, Array.Empty<ParamDescription>());
        yield return new AdsbVehicleClient(_identity.Id, config.Adsb, _core);
        
    }

    
}