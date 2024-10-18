using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class AsvSdrClientCalibrationTable:DisposableOnceWithCancel
{
    private readonly IAsvSdrClient _ifc;
    private readonly RxValue<ushort> _remoteSize;
    private readonly ILogger _logger;
    private readonly TimeSpan _deviceUploadTimeout;
    private readonly RxValue<CalibrationTableMetadata> _metadata;

    public AsvSdrClientCalibrationTable(
        AsvSdrCalibTablePayload payload, 
        IAsvSdrClient ifc, 
        TimeSpan deviceUploadTimeout,
        ILoggerFactory? logFactory = null)
    {
        logFactory??=NullLoggerFactory.Instance;
        _logger = logFactory.CreateLogger<AsvSdrClientCalibrationTable>();
        _ifc = ifc;
        _deviceUploadTimeout = deviceUploadTimeout;
        _remoteSize = new RxValue<ushort>(payload.RowCount).DisposeItWith(Disposable);
        _metadata = new RxValue<CalibrationTableMetadata>(new CalibrationTableMetadata(payload)).DisposeItWith(Disposable);
        Name = MavlinkTypesHelper.GetString(payload.TableName);
        Index = payload.TableIndex;
    }
    public string Name { get; }
    public ushort Index { get; }

    public IRxValue<ushort> RemoteSize => _remoteSize;
    public IRxEditableValue<CalibrationTableMetadata> Metadata => _metadata;

    internal void Update(AsvSdrCalibTablePayload payload)
    {
        var name = MavlinkTypesHelper.GetString(payload.TableName);
        if (payload.TableIndex!= Index) throw new ArgumentException($"Invalid table index. Expected: {Index}, actual: {payload.TableIndex}");
        if (name != Name) throw new ArgumentException($"Invalid table name. Expected: {Name}, actual: {name}");
        _metadata.OnNext(new CalibrationTableMetadata(payload));
        _remoteSize.OnNext(payload.RowCount);
    }
    
    public async Task<CalibrationTableRow[]> Download(IProgress<double> progress = null,CancellationToken cancel = default)
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
    
    public async Task Upload(CalibrationTableRow[] data, IProgress<double> progress = null,CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        progress.Report(0);
        _logger.ZLogInformation($"Begin upload rows for '{Name}[{Index}]' table");

        var reqId = _ifc.GenerateRequestIndex();

        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        var tcs = new TaskCompletionSource<Unit>();
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);
        
        var lastUpdateTime = DateTime.Now;
        using var checkTimer = Observable.Timer(_deviceUploadTimeout, _deviceUploadTimeout)
            .Subscribe(_ =>
            {
                if (DateTime.Now - lastUpdateTime > _deviceUploadTimeout)
                {
                    _logger.ZLogWarning($"'{Name}[{Index}]' table upload timeout");
                    tcs.TrySetException(new Exception($"'{Name}[{Index}]' table upload timeout"));
                }
            });

        using var sub1 = _ifc.OnCalibrationTableRowUploadCallback.Subscribe(req =>
        {
            _logger.ZLogDebug($"Payload request '{Name}[{Index}]' row with index={req.RowIndex}");
            lastUpdateTime = DateTime.Now;
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
                arg.TargetComponent = _ifc.Identity.TargetComponentId;
                arg.TargetSystem = _ifc.Identity.TargetSystemId;
                arg.RefFreq = value.FrequencyHz;
                arg.RefPower = value.RefPower;
                arg.RefValue = value.RefValue;
                arg.Adjustment = value.Adjustment;
            }, cancel);
        });

        using var sub2 = _ifc.OnCalibrationAcc.Where(p => p.RequestId == reqId).Subscribe(p =>
        {
            lastUpdateTime = DateTime.Now;
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
    
    
    
}