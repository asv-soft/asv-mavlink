using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Diagnostic.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class RsgaClientDeviceConfig:ClientDeviceBaseConfig
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
    private readonly ParamsClientEx _params;
    private readonly AsvChartClient _charts;
    private readonly DiagnosticClient _diagnostics;
    private readonly CommandClient _command;
    private readonly AsvRsgaClientEx _rsga;
    private readonly ParamsClient _paramBase;
    private readonly AsvRsgaClient _rsgaBase;
    private IDisposable? _serialNumSubscription;
    private CancellationTokenSource? _disposeCancel;

    public RsgaClientDevice(MavlinkClientIdentity identity, RsgaClientDeviceConfig config, ICoreServices core)
        :base(identity,config,core)
    {
        _config = config;
        _logger = core.Log.CreateLogger<RsgaClientDevice>();
        _command = new CommandClient(identity, config.Command,core);
        _paramBase = new ParamsClient(identity, config.Params, core);
        _params = new ParamsClientEx(_paramBase, config.Params);
        _charts = new AsvChartClient(identity,config.Charts,  core);
        _diagnostics = new DiagnosticClient(identity,config.Diagnostics, core);
        _rsgaBase = new AsvRsgaClient(identity,core);
        _rsga = new AsvRsgaClientEx(_rsgaBase, _command);
    }
    
    protected override async Task InternalInit()
    {
        _params.Init(MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        try
        {
            _logger.ZLogTrace($"Try to read compatibilities.");
            await Rsga.Base.GetCompatibilities().ConfigureAwait(false);
            
            _serialNumSubscription?.Dispose();
            _serialNumSubscription = Params.Filter(_config.SerialNumberParamName)
                .Select(serial => $"RSGA [{(int)serial:D5}]")
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
    public IAsvRsgaClientEx Rsga => _rsga;
    public override DeviceClass Class => DeviceClass.Rsga;

    #region Dispose and dispose async
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _params.Dispose();
            _charts.Dispose();
            _diagnostics.Dispose();
            _command.Dispose();
            _rsga.Dispose();
            _paramBase.Dispose();
            _rsgaBase.Dispose();
            _serialNumSubscription?.Dispose();
            _disposeCancel?.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_params).ConfigureAwait(false);
        await CastAndDispose(_charts).ConfigureAwait(false);
        await CastAndDispose(_diagnostics).ConfigureAwait(false);
        await CastAndDispose(_command).ConfigureAwait(false);
        await CastAndDispose(_rsga).ConfigureAwait(false);
        await CastAndDispose(_paramBase).ConfigureAwait(false);
        await CastAndDispose(_rsgaBase).ConfigureAwait(false);
        if (_serialNumSubscription != null) await CastAndDispose(_serialNumSubscription).ConfigureAwait(false);
        if (_disposeCancel != null) await CastAndDispose(_disposeCancel).ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}