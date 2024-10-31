#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class SdrClientDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public AsvSdrClientExConfig SdrEx { get; set; } = new();
    public MissionClientConfig Missions { get; set; } = new();
    public MissionClientExConfig MissionsEx { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public ParamsClientExConfig ParamsEx { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
}
public class SdrClientDevice : ClientDevice
{
    

    private readonly SdrClientDeviceConfig _config;
    private readonly ILogger _logger;
  
    public SdrClientDevice(MavlinkClientIdentity identity, SdrClientDeviceConfig config, ICoreServices core) 
        : base(identity, config, core, DeviceClass.SdrPayload)
    {
        _logger = core.Log.CreateLogger<SdrClientDevice>();
        _config = config;
        
    }

    protected override Task InitBeforeMicroservices(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }

    protected override IEnumerable<IMavlinkMicroserviceClient> CreateMicroservices()
    {
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

    protected override Task InitAfterMicroservices(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }
}