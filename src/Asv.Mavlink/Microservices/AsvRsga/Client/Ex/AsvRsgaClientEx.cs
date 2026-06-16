using System;
using System.Linq;
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
    private readonly ObservableList<GnssSource> _sources = new();
    private readonly ObservableList<RsgaChartSource> _chartSources = new();
    private readonly Subject<RsgaChartFrame> _chartFrames = new();
    private readonly IDisposable _sub3;
    private readonly IDisposable _sub4;


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
        
        _sub3 = Base.GnssRawStream.Subscribe(CheckGnss);
        _sub4 = Base.ChartRawStream.Subscribe(CheckChart);
    }
    
    private void CheckGnss(AsvRsgaRttGnssPayload data)
    {
        if (_sources.Any(x => x.Stream.CurrentValue.RefId == data.RefId))
        {
            // already exists
            return;
        }
        var source = new GnssSource(data.RefId, Base.GnssRawStream, data);
        _sources.Add(source);
    }

    private void CheckChart(AsvRsgaRttChartPayload data)
    {
        try
        {
            var frame = RsgaChartHelper.ReadChartData(data);
            if (_chartSources.Any(x => x.ChartType == frame.ChartType) == false)
            {
                _chartSources.Add(new RsgaChartSource(frame.ChartType, ChartFrames, frame));
            }
            _chartFrames.OnNext(frame);
        }
        catch (Exception e)
        {
            _logger.ZLogWarning(e, $"Error to decode RSGA chart:{e.Message}");
        }
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
        RsgaHelper.CheckRecordName(name);
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item => RsgaHelper.SetArgsForStartRecord(item, name),cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> StopRecord(CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(RsgaHelper.SetArgsForStopRecord,cs.Token).ConfigureAwait(false);
        return result.Result;
    }
    
    public IReadOnlyObservableList<GnssSource> GnssSources => _sources;

    public Observable<RsgaChartFrame> ChartFrames => _chartFrames;

    public IReadOnlyObservableList<RsgaChartSource> ChartSources => _chartSources;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _currentMode.Dispose();
            _currentSubMode.Dispose();
            _sub2.Dispose();
            _sub3.Dispose();
            _sub4.Dispose();
            _chartFrames.Dispose();
            foreach (var gnssSource in _sources)
            {
                gnssSource.Dispose();
            }
            _sources.Clear();
            foreach (var chartSource in _chartSources)
            {
                chartSource.Dispose();
            }
            _chartSources.Clear();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        _sub2.Dispose();
        
        if (_sub3 is IAsyncDisposable sub2AsyncDisposable)
            await sub2AsyncDisposable.DisposeAsync().ConfigureAwait(false);
        else
            _sub3.Dispose();

        if (_sub4 is IAsyncDisposable sub4AsyncDisposable)
            await sub4AsyncDisposable.DisposeAsync().ConfigureAwait(false);
        else
            _sub4.Dispose();

        if (_chartFrames is IAsyncDisposable chartFramesAsyncDisposable)
            await chartFramesAsyncDisposable.DisposeAsync().ConfigureAwait(false);
        else
            _chartFrames.Dispose();
        
        foreach (var gnssSource in _sources)
        {
            gnssSource.Dispose();
        }
        _sources.Clear();
        
        foreach (var chartSource in _chartSources)
        {
            chartSource.Dispose();
        }
        _chartSources.Clear();

        await base.DisposeAsyncCore().ConfigureAwait(false);
    }
}
