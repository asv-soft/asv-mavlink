using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Diagnostic.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class RsgaClientDeviceConfig:ClientDeviceConfig
{
    public ParamsClientExConfig Params { get; set; } = new();
    public AsvChartClientConfig Charts { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();
    public DiagnosticClientConfig Diagnostics { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
    
}

public class RsgaClientDevice : ClientDevice, IRsgaClientDevice
{
    private readonly RsgaClientDeviceConfig _config;
    private readonly ILogger _logger;
    private readonly ParamsClientEx _params;
    private readonly AsvChartClient _charts;
    private readonly DiagnosticClient _diagnostics;
    private readonly CommandClient _command;
    private readonly AsvRsgaClientEx _rsga;

    public RsgaClientDevice(
        IMavlinkV2Connection link, 
        MavlinkClientIdentity identity, 
        RsgaClientDeviceConfig config, 
        IPacketSequenceCalculator seq, 
        IScheduler? scheduler = null, 
        ILogger? logger = null)
        :base(link,identity,config,seq,scheduler,logger)
    {
        _config = config;
        _logger = logger ?? NullLogger.Instance;
        scheduler ??= Scheduler.Default;
        _command = new CommandClient(link, identity, seq, config.Command).DisposeItWith(Disposable);
        var paramBase = new ParamsClient(link, identity, seq, config.Params).DisposeItWith(Disposable);
        _params = new ParamsClientEx(paramBase, config.Params).DisposeItWith(Disposable);
        _charts = new AsvChartClient(config.Charts, link, identity, seq).DisposeItWith(Disposable);
        _diagnostics = new DiagnosticClient(config.Diagnostics, link, identity, seq, scheduler).DisposeItWith(Disposable);
        var rsga = new AsvRsgaClient(link,identity, seq).DisposeItWith(Disposable);
        _rsga = new AsvRsgaClientEx(rsga,_command).DisposeItWith((Disposable));
    }
    
    protected override async Task InternalInit()
    {
        _params.Init(MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        try
        {
            _logger.ZLogTrace($"Try to read compatibilities.");
            await Rsga.Base.GetCompatibilities().ConfigureAwait(false);
            _logger.ZLogTrace($"Try to read serial number from param {_config.SerialNumberParamName}");
            Params.Filter(_config.SerialNumberParamName)
                .Select(serial => $"RSGA [{(int)serial:D5}]")
                .Subscribe(EditableName)
                .DisposeItWith(Disposable);
            await Params.ReadOnce(_config.SerialNumberParamName, DisposeCancel).ConfigureAwait(false);
            
        }
        catch (Exception e)
        {
            _logger.ZLogWarning($"Error to get serial number:{e.Message}");
        }
    }
    public IParamsClientEx Params => _params;
    public IAsvChartClient Charts => _charts;
    public ICommandClient Command => _command;
    public IDiagnosticClient Diagnostic => _diagnostics;
    public IAsvRsgaClientEx Rsga => _rsga;

    protected override string DefaultName => $"RSGA [{Identity.TargetSystemId:00},{Identity.TargetComponentId:00}]";
    public override DeviceClass Class => DeviceClass.Rsga;
}