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

public class SdrClientDeviceConfig:ClientDeviceBaseConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public AsvSdrClientExConfig SdrEx { get; set; } = new();
    public MissionClientConfig Missions { get; set; } = new();
    public MissionClientExConfig MissionsEx { get; set; } = new();
    public ParameterClientConfig Params { get; set; } = new();
    public ParamsClientExConfig ParamsEx { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
}
public class SdrClientDevice : ClientDevice, ISdrClientDevice
{
    

    private readonly SdrClientDeviceConfig _config;
    private readonly ILogger _logger;
    private readonly ParamsClientEx _params;
    private readonly AsvSdrClient _adrBase;
    private readonly MissionClient _missionBase;
    private readonly ParamsClient _paramsBase;
    private readonly AsvSdrClientEx _sdr;
    private readonly CommandClient _command;
    private readonly MissionClientEx _missions;
    private IDisposable? _serialNumSubscription;
    private CancellationTokenSource? _disposeCancel;

    public SdrClientDevice(MavlinkClientIdentity identity, SdrClientDeviceConfig config, ICoreServices core) 
        : base(identity, config, core)
    {
        _logger = core.Log.CreateLogger<SdrClientDevice>();
        _config = config;
        _command = new CommandClient(identity, config.Command, core);
        _adrBase = new AsvSdrClient(identity, core);
        _sdr = new AsvSdrClientEx(_adrBase,Heartbeat, Command,config.SdrEx);
        _missionBase = new MissionClient(identity, config.Missions, core);
        _missions = new MissionClientEx(_missionBase, config.MissionsEx);
        _paramsBase = new ParamsClient(identity, config.Params, core);
        _params = new ParamsClientEx(_paramsBase, config.ParamsEx);
    }

    protected override async Task InternalInit()
    {
        _params.Init(MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        try
        {
            _serialNumSubscription?.Dispose();
            _serialNumSubscription = Params.Filter(_config.SerialNumberParamName)
                .Select(serial => $"SDR [{(int)serial:D5}]")
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
            _logger.ZLogWarning($"Error to get SDR serial number:{e.Message}");
        }
    }

    public override DeviceClass Class => DeviceClass.SdrPayload;

    public IAsvSdrClientEx Sdr => _sdr;

    public ICommandClient Command => _command;

    public IMissionClientEx Missions => _missions;

    public IParamsClientEx Params => _params;

    #region Dispose and dispose async

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _params.Dispose();
            _adrBase.Dispose();
            _missionBase.Dispose();
            _paramsBase.Dispose();
            _sdr.Dispose();
            _command.Dispose();
            _missions.Dispose();
            _serialNumSubscription?.Dispose();
            _disposeCancel?.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_params).ConfigureAwait(false);
        await CastAndDispose(_adrBase).ConfigureAwait(false);
        await CastAndDispose(_missionBase).ConfigureAwait(false);
        await CastAndDispose(_paramsBase).ConfigureAwait(false);
        await CastAndDispose(_sdr).ConfigureAwait(false);
        await CastAndDispose(_command).ConfigureAwait(false);
        await CastAndDispose(_missions).ConfigureAwait(false);
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