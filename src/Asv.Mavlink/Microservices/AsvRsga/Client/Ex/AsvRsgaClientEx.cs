using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class AsvRsgaClientEx : MavlinkMicroserviceClient, IAsvRsgaClientEx
{
    private readonly ILogger _logger;
    private readonly ICommandClient _commandClient;
    private readonly ObservableList<AsvRsgaCustomMode> _supportedModes;
    private readonly IDisposable _sub2;
    private readonly ReactiveProperty<AsvRsgaCustomMode> _currentMode;
    private readonly ReactiveProperty<AsvRsgaCustomSubMode> _currentSubMode;

    public AsvRsgaClientEx(IAsvRsgaClient client, ICommandClient commandClient, IHeartbeatClient heartbeatClient)
        :base(RsgaHelper.MicroserviceExName, client.Identity, client.Core)
    {
        ArgumentNullException.ThrowIfNull(heartbeatClient);
        _logger = client.Core.LoggerFactory.CreateLogger<AsvRsgaClientEx>();
        _currentMode = new ReactiveProperty<AsvRsgaCustomMode>(AsvRsgaCustomMode.AsvRsgaCustomModeIdle);
        _currentSubMode = new ReactiveProperty<AsvRsgaCustomSubMode>(0);
        heartbeatClient.RawHeartbeat
            .DistinctUntilChangedBy(x => HashCode.Combine(x?.BaseMode, x?.CustomMode))
            .Subscribe(UpdateMode)
            .RegisterTo(DisposeCancel);
        _commandClient = commandClient ?? throw new ArgumentNullException(nameof(commandClient));
        Base = client;
        _supportedModes = new ObservableList<AsvRsgaCustomMode>();
        _sub2 = client.OnCompatibilityResponse.Subscribe(OnCapabilityResponse);
    }

    private void UpdateMode(HeartbeatPayload? heartbeatPayload)
    {
        _currentMode.Value = RsgaHelper.GetCustomMode(heartbeatPayload);
        _currentSubMode.Value = RsgaHelper.GetCustomSubMode(heartbeatPayload);
    }

    private void OnCapabilityResponse(AsvRsgaCompatibilityResponsePayload? asv)
    {
        if (asv == null) return;
        if (asv.Result != AsvRsgaRequestAck.AsvRsgaRequestAckOk)
        {
            _logger.ZLogWarning($"Error to get compatibility:{asv.Result:G}");
            return;
        }
        
        _supportedModes.Clear();
        _supportedModes.AddRange(RsgaHelper.GetSupportedModes(asv));
    }

    protected override async Task InternalInit(CancellationToken cancel)
    {
        await base.InternalInit(cancel).ConfigureAwait(false);
        await RefreshInfo(cancel).ConfigureAwait(false);
    }

    public IAsvRsgaClient Base { get; }

    public ReadOnlyReactiveProperty<AsvRsgaCustomMode> CurrentMode => _currentMode;

    public ReadOnlyReactiveProperty<AsvRsgaCustomSubMode> CurrentSubMode => _currentSubMode;

    public IReadOnlyObservableList<AsvRsgaCustomMode> AvailableModes => _supportedModes;

    public Task RefreshInfo(CancellationToken cancel = default)
    {
        return Base.GetCompatibilities(cancel);
    }

    public async Task<MavResult> SetMode(AsvRsgaCustomMode mode, CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item => RsgaHelper.SetArgsForSetMode(item, mode),cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StartRecord(string name, CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item => RsgaHelper.SetArgsForStartRecord(item, name),cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public Task<MavResult> StopRecord(CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _currentMode.Dispose();
            _currentSubMode.Dispose();
            _sub2.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        _sub2.Dispose();
        await base.DisposeAsyncCore().ConfigureAwait(false);
    }
}