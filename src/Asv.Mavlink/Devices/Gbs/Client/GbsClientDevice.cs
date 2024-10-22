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

public class GbsClientDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
}
public class GbsClientDevice : ClientDevice, IGbsClientDevice
{
    

    private readonly GbsClientDeviceConfig _config;
    private readonly ParamsClientEx _params;
    private readonly ILogger _logger;
    private readonly CommandClient _command;
    private readonly AsvGbsClient _gbsBase;
    private readonly AsvGbsExClient _gbsEx;
    private readonly ParamsClient _paramBase;
    private IDisposable? _serialNumSubscribe;
    private CancellationTokenSource? _readOnceCancel;

    public GbsClientDevice(MavlinkClientIdentity identity, GbsClientDeviceConfig config, ICoreServices core) 
        : base(identity,config,core)
    {
        _config = config;
        _logger = Core.Log.CreateLogger<GbsClientDevice>();
        _command = new CommandClient(identity, config.Command, core);
        _gbsBase = new AsvGbsClient(identity,core);
        _gbsEx = new AsvGbsExClient(_gbsBase,Heartbeat,Command);
        _paramBase = new ParamsClient(identity, config.Params,core);
        _params = new ParamsClientEx(_paramBase, config.Params);
        
    }

    public IParamsClientEx Params => _params;

    public ICommandClient Command => _command;

    public IAsvGbsExClient Gbs => _gbsEx;

    protected override async Task InternalInit()
    {
        _params.Init(MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        try
        {
            _logger.ZLogInformation($"Try to read serial number from param {_config.SerialNumberParamName}");
            _serialNumSubscribe?.Dispose();
            _serialNumSubscribe = Params.Filter(_config.SerialNumberParamName)
                .Select(serial => $"RTK GBS [{(int)serial:D5}]")
                .Subscribe(EditableName);
            
            _readOnceCancel?.Cancel(false);
            _readOnceCancel?.Dispose();
            _readOnceCancel = new CancellationTokenSource();
            await Params.ReadOnce(_config.SerialNumberParamName, _readOnceCancel.Token).ConfigureAwait(false);
            _readOnceCancel.Dispose();
            _readOnceCancel = null;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"Error to get serial number:{e.Message}");
        }
    }

    public override DeviceClass Class => DeviceClass.GbsRtk;

    #region Dispose and dispose async

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _params.Dispose();
            _command.Dispose();
            _gbsBase.Dispose();
            _gbsEx.Dispose();
            _paramBase.Dispose();
            _serialNumSubscribe?.Dispose();
            _readOnceCancel?.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_params).ConfigureAwait(false);
        await CastAndDispose(_command).ConfigureAwait(false);
        await CastAndDispose(_gbsBase).ConfigureAwait(false);
        await CastAndDispose(_gbsEx).ConfigureAwait(false);
        await CastAndDispose(_paramBase).ConfigureAwait(false);
        if (_serialNumSubscribe != null) await CastAndDispose(_serialNumSubscribe).ConfigureAwait(false);
        if (_readOnceCancel != null) await CastAndDispose(_readOnceCancel).ConfigureAwait(false);

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