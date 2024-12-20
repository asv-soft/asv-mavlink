using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;
using Asv.Mavlink.Diagnostic.Client;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class RfsaClientDeviceConfig:MavlinkClientDeviceConfig
{
    public ParamsClientExConfig Params { get; set; } = new();
    public AsvChartClientConfig Charts { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();
    public DiagnosticClientConfig Diagnostics { get; set; } = new();
    
    public override void Load(string key, IConfiguration configuration)
    {
        base.Load(key, configuration);
        Params = configuration.Get<ParamsClientExConfig>();
        Charts = configuration.Get<AsvChartClientConfig>();
        Command = configuration.Get<CommandProtocolConfig>();
        Diagnostics = configuration.Get<DiagnosticClientConfig>();
    }
    
    public override void Save(string key, IConfiguration configuration)
    {
        base.Save(key, configuration);
        configuration.Set(Params);
        configuration.Set(Charts);
        configuration.Set(Command);
        configuration.Set(Diagnostics);
    }
    
}

public class RfsaClientDevice:MavlinkClientDevice
{
    public const string DeviceClass = "Rfsa";
    private readonly RfsaClientDeviceConfig _config;
    private readonly ILogger _logger;

    public RfsaClientDevice(
        MavlinkClientDeviceId identity, 
        RfsaClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        IMavlinkContext core) 
        : base(identity,config,extenders,core)
    {
        _config = config;
        _logger = core.LoggerFactory.CreateLogger<RfsaClientDevice>();
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
        yield return new AsvChartClient(Identity, _config.Charts, Core);
        yield return new DiagnosticClient(Identity, _config.Diagnostics, Core);
        yield return new CommandClient(Identity, _config.Command, Core);
    }

}