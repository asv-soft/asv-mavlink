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
using ZLogger;

namespace Asv.Mavlink;

public class GbsClientDeviceConfig:MavlinkClientDeviceConfig
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

public class GbsClientDevice : MavlinkClientDevice
{
    public const string DeviceClass = "GBS";
    private readonly GbsClientDeviceConfig _config;
    private readonly ILogger _logger;

    public GbsClientDevice(
        MavlinkClientDeviceId identity, 
        GbsClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        IMavlinkContext core) 
        : base(identity,config,extenders,core)
    {
        _config = config;
        _logger = Core.LoggerFactory.CreateLogger<GbsClientDevice>();
    }

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices([EnumeratorCancellation] CancellationToken cancel)
    {
        IHeartbeatClient? hb = null;
        
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            if (microservice is IHeartbeatClient heartbeat)
            {
                hb = heartbeat;
            }
            yield return microservice;
        }
        yield return new StatusTextClient(Identity, Core);
        var command = new CommandClient(Identity, _config.Command, Core);
        yield return command;
        var paramBase = new ParamsClient(Identity, _config.Params, Core);
        yield return paramBase;
        yield return new ParamsClientEx(paramBase, _config.Params, MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        var client = new AsvGbsClient(Identity, Core);
        if (hb == null)
        {
            _logger.ZLogWarning($"{Id} {nameof(HeartbeatClient)} microservice not found");
            yield break;
        }
        yield return new AsvGbsExClient(client, hb, command);
        
    }
}