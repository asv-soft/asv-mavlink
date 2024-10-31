#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class GbsClientDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
}
public class GbsClientDevice : ClientDevice
{
    private readonly GbsClientDeviceConfig _config;
    private readonly ILogger _logger;

    public GbsClientDevice(MavlinkClientIdentity identity, GbsClientDeviceConfig config, ICoreServices core) 
        : base(identity,config,core, DeviceClass.GbsRtk)
    {
        _config = config;
        _logger = Core.Log.CreateLogger<GbsClientDevice>();
    }

    protected override Task InitBeforeMicroservices(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }
    protected override IEnumerable<IMavlinkMicroserviceClient> CreateMicroservices()
    {
        yield return new StatusTextClient(Identity, Core);
        var command = new CommandClient(Identity, _config.Command, Core);
        yield return command;
        var paramBase = new ParamsClient(Identity, _config.Params, Core);
        yield return paramBase;
        yield return new ParamsClientEx(paramBase, _config.Params, MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        var client = new AsvGbsClient(Identity, Core);
        yield return new AsvGbsExClient(client, Heartbeat, command);
    }

    protected override Task InitAfterMicroservices(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }
}