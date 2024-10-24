using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Diagnostic.Client;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class RfsaClientDeviceConfig:ClientDeviceBaseConfig
{
    public ParamsClientExConfig Params { get; set; } = new();
    public AsvChartClientConfig Charts { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();
    public DiagnosticClientConfig Diagnostics { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
    
}

public class RfsaClientDevice:ClientDevice
{
    private readonly RfsaClientDeviceConfig _config;
    private readonly ILogger _logger;

    public RfsaClientDevice(MavlinkClientIdentity identity, RfsaClientDeviceConfig config, ICoreServices core)
        :base(identity,config,core,DeviceClass.Rfsa)
    {
        _config = config;
        _logger = core.Log.CreateLogger<RfsaClientDevice>();
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
        yield return new CommandClient(Identity, _config.Command, Core);
    }

    protected override Task InitAfterMicroservices(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }
}