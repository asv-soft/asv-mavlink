using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class SdrClientDeviceConfig:MavlinkClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public AsvSdrClientExConfig SdrEx { get; set; } = new();
    public MissionClientConfig Missions { get; set; } = new();
    public MissionClientExConfig MissionsEx { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public ParamsClientExConfig ParamsEx { get; set; } = new();
    
    public override void Load(string key, IConfiguration configuration)
    {
        base.Load(key, configuration);
        Command = configuration.Get<CommandProtocolConfig>();
        SdrEx = configuration.Get<AsvSdrClientExConfig>();
        Missions = configuration.Get<MissionClientConfig>();
        MissionsEx = configuration.Get<MissionClientExConfig>();
        Params = configuration.Get<ParamsClientExConfig>();
        ParamsEx = configuration.Get<ParamsClientExConfig>();
    }
    
    public override void Save(string key, IConfiguration configuration)
    {
        base.Save(key, configuration);
        configuration.Set(Command);
        configuration.Set(SdrEx);
        configuration.Set(Missions);
        configuration.Set(MissionsEx);
        configuration.Set(Params);
        configuration.Set(ParamsEx);
    }
    
}
public class SdrClientDevice : MavlinkClientDevice
{
    public const string DeviceClass = "SDR";

    private readonly SdrClientDeviceConfig _config;

    public SdrClientDevice(
        MavlinkClientDeviceId identity, 
        SdrClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        IMavlinkContext core) 
        : base(identity,config,extenders,core)
    {
        core.LoggerFactory.CreateLogger<SdrClientDevice>();
        _config = config;
        
    }

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices(
        [EnumeratorCancellation] CancellationToken cancel)
    {
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            yield return microservice;
        }
        yield return new StatusTextClient(Identity, Core);
        var paramBase = new ParamsClient(Identity, _config.Params, Core);
        yield return paramBase;
        yield return new ParamsClientEx(paramBase, _config.Params, MavParamHelper.ByteWiseEncoding, Array.Empty<ParamDescription>());
        var command = new CommandClient(Identity, _config.Command, Core);
        yield return command;
        var sdrBase = new AsvSdrClient(Identity, Core);
        yield return sdrBase;
        yield return new AsvSdrClientEx(sdrBase,Heartbeat, command,_config.SdrEx);
        var missionBase = new MissionClient(Identity, _config.Missions, Core);
        yield return missionBase;
        yield return new MissionClientEx(missionBase, _config.MissionsEx);
        
    }

}