#nullable enable
using System;
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

public class RadioClientDevice : ClientDevice, IRadioClientDevice
{
    


    private readonly RadioClientDeviceConfig _config;
    private readonly ParamsClientEx _params;
    private readonly ILogger _logger;
    private readonly CommandClient _command;
    private readonly AsvRadioClient _client;
    private readonly ParamsClient _param;
    private readonly AsvRadioClientEx _radio;
    private IDisposable? _serialNumSubscription;
    private CancellationTokenSource? _disposeCancel;

    public RadioClientDevice(MavlinkClientIdentity identity, 
        RadioClientDeviceConfig config, 
        ICoreServices core)
        : base(identity, config, core)
    {
        _logger = core.Log.CreateLogger<RadioClientDevice>();
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _command = new CommandClient(identity, config.Command, core);
        _client = new AsvRadioClient(identity,core);
        _radio = new AsvRadioClientEx(_client, Heartbeat, Command);
        _param = new ParamsClient(identity,  config.Params,core);
        _params = new ParamsClientEx(_param, config.Params);
    }

    protected override async Task InternalInit()
    {
        _params.Init(MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        try
        {
            _logger.ZLogTrace($"Try to read serial number from param {_config.SerialNumberParamName}");

            _serialNumSubscription?.Dispose();
            _serialNumSubscription = Params.Filter(_config.SerialNumberParamName)
                .Select(serial => $"RADIO [{(int)serial:D5}]")
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
            _logger.ZLogWarning($"Error to get RADIO serial number:{e.Message}");
        }
    }

    public override DeviceClass Class => DeviceClass.Radio;
    public IAsvRadioClientEx Radio => _radio;
    public ICommandClient Command => _command;
    public IParamsClientEx Params => _params;
    
    #region Dispose and async dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _params.Dispose();
            _command.Dispose();
            _client.Dispose();
            _param.Dispose();
            _radio.Dispose();
            _serialNumSubscription?.Dispose();
            _disposeCancel?.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_params).ConfigureAwait(false);
        await CastAndDispose(_command).ConfigureAwait(false);
        await CastAndDispose(_client).ConfigureAwait(false);
        await CastAndDispose(_param).ConfigureAwait(false);
        await CastAndDispose(_radio).ConfigureAwait(false);
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