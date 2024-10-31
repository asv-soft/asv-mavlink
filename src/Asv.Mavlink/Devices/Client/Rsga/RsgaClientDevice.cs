using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Diagnostic.Client;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class RsgaClientDeviceConfig:ClientDeviceConfig
{
    public ParamsClientExConfig Params { get; set; } = new();
    public AsvChartClientConfig Charts { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();
    public DiagnosticClientConfig Diagnostics { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
    
}

public class RsgaClientDevice : ClientDevice
{
    

    private readonly RsgaClientDeviceConfig _config;
    private readonly ILogger _logger;

    public RsgaClientDevice(MavlinkClientIdentity identity, RsgaClientDeviceConfig config, ICoreServices core)
        :base(identity,config,core, DeviceClass.Rsga)
    {
        _config = config;
        _logger = core.Log.CreateLogger<RsgaClientDevice>();
       
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
        yield return new AsvChartClient(Identity, _config.Charts, Core);
        yield return new DiagnosticClient(Identity, _config.Diagnostics, Core);
        var command = new CommandClient(Identity, _config.Command, Core);
        yield return command;
        var rsgaBase = new AsvRsgaClient(Identity, Core);
        yield return rsgaBase;
        yield return new AsvRsgaClientEx(rsgaBase,command);
    }

    protected override Task InitAfterMicroservices(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }

  
}