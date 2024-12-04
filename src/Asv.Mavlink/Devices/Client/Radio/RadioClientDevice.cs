#nullable enable
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


public class RadioClientDeviceConfig:MavlinkClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public override void Load(string key, IConfiguration configuration)
    {
        base.Load(key, configuration);
        Command = configuration.Get<CommandProtocolConfig>();
        Params = configuration.Get<ParamsClientExConfig>();
    }
    
    public override void Save(string key, IConfiguration configuration)
    {
        base.Save(key, configuration);
        configuration.Set(Command);
        configuration.Set(Params);
    }
}

public class RadioClientDevice : MavlinkClientDevice
{
    public const string DeviceClass = "RADIO";
    private readonly RadioClientDeviceConfig _config;
    private readonly ILogger _logger;

    public RadioClientDevice(
        MavlinkClientDeviceId identity, 
        RadioClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        ICoreServices core) 
        : base(identity,config,extenders,core)
    {
        _logger = core.LoggerFactory.CreateLogger<RadioClientDevice>();
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

  

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices(
        [EnumeratorCancellation] CancellationToken cancel)
    {
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            yield return microservice;
        }
        
        yield return new StatusTextClient(Identity, Core);
        var command = new CommandClient(Identity, _config.Command, Core);
        yield return command;
        var paramBase = new ParamsClient(Identity, _config.Params, Core);
        yield return paramBase;
        yield return new ParamsClientEx(paramBase, _config.Params, MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        var client = new AsvRadioClient(Identity, Core);
        yield return new AsvRadioClientEx(client, Heartbeat, command);
    }

    
}