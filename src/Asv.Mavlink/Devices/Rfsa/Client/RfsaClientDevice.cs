using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Diagnostic.Client;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class RfsaClientDeviceConfig:ClientDeviceConfig
{
    public ParamsClientExConfig Params { get; set; } = new();
    public AsvChartClientConfig Charts { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();
    public DiagnosticClientConfig Diagnostics { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
    
}

public class RfsaClientDevice:ClientDevice, IRfsaClientDevice
{
    private readonly RfsaClientDeviceConfig _config;
    private readonly ILogger _logger;
    private readonly ParamsClientEx _params;
    private readonly AsvChartClient _charts;
    private readonly DiagnosticClient _diagnostics;
    private readonly CommandClient _command;
    private readonly ParamsClient _paramBase;
    private IDisposable? _serialNumSubscription;
    private CancellationTokenSource? _disposeCancel;

    public RfsaClientDevice(MavlinkClientIdentity identity, RfsaClientDeviceConfig config, ICoreServices core)
        :base(identity,config,core)
    {
        _config = config;
        _logger = core.Log.CreateLogger<RfsaClientDevice>();
        _command = new CommandClient(identity, config.Command,core);
        _paramBase = new ParamsClient(identity, config.Params,core);
        _params = new ParamsClientEx(_paramBase, config.Params);
        _charts = new AsvChartClient(identity,config.Charts, core);
        _diagnostics = new DiagnosticClient(identity,config.Diagnostics, core);
    }
    
    protected override async Task InternalInit()
    {
        _params.Init(MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        try
        {
            _logger.ZLogTrace($"Try to read serial number from param {_config.SerialNumberParamName}");
            _serialNumSubscription?.Dispose();
            _serialNumSubscription = Params.Filter(_config.SerialNumberParamName)
                .Select(serial => $"RFSA [{(int)serial:D5}]")
                .Subscribe(EditableName);
            
            _disposeCancel?.Cancel(false);
            _disposeCancel?.Dispose();
            _disposeCancel = new CancellationTokenSource();
            await Params.ReadOnce(_config.SerialNumberParamName,_disposeCancel.Token).ConfigureAwait(false);
            _disposeCancel.Dispose();
            _disposeCancel = null;
           
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
    
    public Task<MavResult> Enable(ulong frequencyHz, uint spanHz, CancellationToken cancel = default)
    {
        return Command.CommandLong(item => RfsaHelper.SetArgsForEnableCommand(item, frequencyHz,spanHz),cancel)
            .ContinueWith(x=>x.Result.Result, cancel);
    }

    public Task<MavResult> Disable(CancellationToken cancel = default)
    {
        return Command.CommandLong(RfsaHelper.SetArgsForDisableCommand,cancel)
            .ContinueWith(x=>x.Result.Result, cancel);
        
    }
    
    public override DeviceClass Class => DeviceClass.Rfsa;
}