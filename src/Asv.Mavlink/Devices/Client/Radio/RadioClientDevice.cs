#nullable enable
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;


public class RadioClientDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
}

public class RadioClientDevice : ClientDevice
{
    private readonly RadioClientDeviceConfig _config;
    private readonly ILogger _logger;

    public RadioClientDevice(MavlinkClientIdentity identity, RadioClientDeviceConfig config, ICoreServices core)
        : base(identity, config, core, DeviceClass.Radio)
    {
        _logger = core.Log.CreateLogger<RadioClientDevice>();
        _config = config ?? throw new ArgumentNullException(nameof(config));
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
        var client = new AsvRadioClient(Identity, Core);
        yield return new AsvRadioClientEx(client, Heartbeat, command);
    }

    protected override Task InitAfterMicroservices(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }
    
}