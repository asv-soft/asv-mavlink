using System;

using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvSdr;
using Asv.Mavlink.V2.AsvSdr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class AsvSdrClientCalibrationTable: IDisposable,IAsyncDisposable
{
    private readonly IAsvSdrClient _ifc;
    private readonly ILogger _logger;
    private readonly TimeSpan _deviceUploadTimeout;
    private readonly ReactiveProperty<CalibrationTableMetadata> _metadata;
    private readonly ReactiveProperty<ushort> _remoteSize;
    private readonly CancellationTokenSource _disposeCancel;

    public AsvSdrClientCalibrationTable(
        AsvSdrCalibTablePayload payload, 
        IAsvSdrClient ifc, 
        TimeSpan deviceUploadTimeout,
        ILoggerFactory? logFactory = null)
    {
        logFactory ??= NullLoggerFactory.Instance;
        _logger = logFactory.CreateLogger<AsvSdrClientCalibrationTable>();
        _disposeCancel = new CancellationTokenSource();
        _ifc = ifc;
        _deviceUploadTimeout = deviceUploadTimeout;
        _remoteSize = new ReactiveProperty<ushort>(payload.RowCount);
        _metadata = new ReactiveProperty<CalibrationTableMetadata>(new CalibrationTableMetadata(payload));
        Name = MavlinkTypesHelper.GetString(payload.TableName);
        Index = payload.TableIndex;
    }
    public string Name { get; }
    public ushort Index { get; }
    private CancellationToken DisposeCancel => _disposeCancel.Token;
    public ReadOnlyReactiveProperty<ushort> RemoteSize => _remoteSize;
    public ReactiveProperty<CalibrationTableMetadata> Metadata => _metadata;

    internal void Update(AsvSdrCalibTablePayload payload)
    {
        var name = MavlinkTypesHelper.GetString(payload.TableName);
        if (payload.TableIndex!= Index) throw new ArgumentException($"Invalid table index. Expected: {Index}, actual: {payload.TableIndex}");
        if (name != Name) throw new ArgumentException($"Invalid table name. Expected: {Name}, actual: {name}");
        _metadata.OnNext(new CalibrationTableMetadata(payload));
        _remoteSize.OnNext(payload.RowCount);
    }
    
    public async Task<CalibrationTableRow[]> Download(IProgress<double>? progress = null,CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        progress.Report(0);
        var info = await _ifc.ReadCalibrationTable(Index, cancel).ConfigureAwait(false);
        var count = info.RowCount;
        _remoteSize.OnNext(count);
        var array = new CalibrationTableRow[count];
        for (ushort i = 0; i < count; i++)
        {
            progress.Report((double)(i+1)/count);
            var result = await _ifc.ReadCalibrationTableRow(Index, i, cancel).ConfigureAwait(false);
            array[i] = new CalibrationTableRow(result);
        }
        return array;
    }
    
    public async Task Upload(CalibrationTableRow[] data, IProgress<double>? progress = null,CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        progress.Report(0);
        _logger.ZLogInformation($"Begin upload rows for '{Name}[{Index}]' table");

        var reqId = _ifc.GenerateRequestIndex();

        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        var tcs = new TaskCompletionSource<Unit>();
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);
        
        var lastUpdateTime = _ifc.Core.TimeProvider.GetTimestamp();

        using var checkTimer = _ifc.Core.TimeProvider.CreateTimer(x =>
        {
            if (_ifc.Core.TimeProvider.GetElapsedTime( lastUpdateTime) > _deviceUploadTimeout)
            {
                _logger.ZLogWarning($"'{Name}[{Index}]' table upload timeout");
                tcs.TrySetException(new Exception($"'{Name}[{Index}]' table upload timeout"));
            }
        }, null, _deviceUploadTimeout, _deviceUploadTimeout);

        using var sub1 = _ifc.OnCalibrationTableRowUploadCallback.Subscribe(req =>
        {
            _logger.ZLogDebug($"Payload request '{Name}[{Index}]' row with index={req.RowIndex}");
            lastUpdateTime = _ifc.Core.TimeProvider.GetTimestamp();
            if (data.Length <= req.RowIndex)
            {
                tcs.TrySetException(
                    new AsvSdrException($"Requested mission item with index '{req.RowIndex}' not found in local store"));
                return;
            }
            var value = data[req.RowIndex];
            progress?.Report((double)(req.RowIndex) / data.Length);
            _ifc.SendCalibrationTableRowUploadItem(arg =>
            {
                arg.RowIndex = req.RowIndex;
                arg.TableIndex = Index;
                arg.TargetComponent = _ifc.Identity.Target.ComponentId;
                arg.TargetSystem = _ifc.Identity.Target.SystemId;
                arg.RefFreq = value.FrequencyHz;
                arg.RefPower = value.RefPower;
                arg.RefValue = value.RefValue;
                arg.Adjustment = value.Adjustment;
            }, cancel);
        });

        using var sub2 = _ifc.OnCalibrationAcc.Where(p => p.RequestId == reqId).Subscribe(p =>
        {
            lastUpdateTime = _ifc.Core.TimeProvider.GetTimestamp();
            if (p.Result == AsvSdrRequestAck.AsvSdrRequestAckOk)
            {
                tcs.TrySetResult(Unit.Default);
            }
            else
            {
                tcs.TrySetException(new AsvSdrException($"Error to upload '{Name}[{Index}]' table to payload:{p.Result:G}"));
            }

        });

        await _ifc.SendCalibrationTableRowUploadStart(args =>
        {
            args.TableIndex = Index;
            args.RowCount = (ushort)data.Length;
            args.RequestId = reqId;
            args.CreatedUnixUs = MavlinkTypesHelper.ToUnixTimeUs(DateTime.Now);
            
        }, linkedCancel.Token).ConfigureAwait(false);
        await tcs.Task.ConfigureAwait(false);
        
    }


    #region Dispose

    public void Dispose()
    {
        _metadata.Dispose();
        _remoteSize.Dispose();
        _disposeCancel.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_metadata).ConfigureAwait(false);
        await CastAndDispose(_remoteSize).ConfigureAwait(false);
        await CastAndDispose(_disposeCancel).ConfigureAwait(false);

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