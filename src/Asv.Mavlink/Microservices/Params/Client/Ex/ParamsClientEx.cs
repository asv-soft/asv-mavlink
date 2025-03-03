using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;


namespace Asv.Mavlink;

public class ParamsClientExConfig : ParameterClientConfig
{
    public int ChunkUpdateBufferMs { get; set; } = 100;
}

public sealed class ParamsClientEx : MavlinkMicroserviceClient, IParamsClientEx
{
    private readonly ParamsClientExConfig _config;
    private readonly IMavParamEncoding _converter;
    private readonly ObservableDictionary<string, ParamItem> _paramsSource;
    private readonly BindableReactiveProperty<bool> _isSynced;
    private readonly Subject<(string, MavParamValue)> _onValueChanged;
    private readonly ILogger<ParamsClientEx> _logger;
    private readonly CancellationTokenSource _disposeCancel;
    private readonly IDisposable _sub1;
    private readonly ImmutableDictionary<string, ParamDescription> _existDescription;
    private ReaderWriterLockSlim _paramsLock = new();

    public ParamsClientEx(IParamsClient client, ParamsClientExConfig config, IMavParamEncoding converter,
        IEnumerable<ParamDescription> existDescription):base(ParamsHelper.MicroserviceName,client.Identity,client.Core)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = client.Core.LoggerFactory.CreateLogger<ParamsClientEx>();
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        _existDescription = existDescription.ToImmutableDictionary(x => x.Name, x => x);
        Base = client;
        _disposeCancel = new CancellationTokenSource();
        _paramsSource = new ObservableDictionary<string, ParamItem>();
        _isSynced = new BindableReactiveProperty<bool>(false);

        if (config.ChunkUpdateBufferMs > 0)
        {
            _sub1 = client.OnParamValue.Chunk(TimeSpan.FromMilliseconds(config.ChunkUpdateBufferMs),client.Core.TimeProvider).Subscribe(OnUpdate);    
        }
        else
        {
            _sub1 = client.OnParamValue.Subscribe(x=>OnUpdate([x]));
        }
        
        
        RemoteCount = client.OnParamValue.Select(x => (int)x.ParamCount).ToReadOnlyReactiveProperty(-1);
        LocalCount = _paramsSource.ObserveCountChanged().ToReadOnlyReactiveProperty();
        _onValueChanged = new Subject<(string, MavParamValue)>();
    }

    public IParamsClient Base { get; }
    public ReadOnlyReactiveProperty<int> RemoteCount { get; }
    public ReadOnlyReactiveProperty<int> LocalCount { get; }
    public Observable<(string, MavParamValue)> OnValueChanged => _onValueChanged;
    public ReadOnlyReactiveProperty<bool> IsSynced => _isSynced;
    public IReadOnlyObservableDictionary<string, ParamItem> Items => _paramsSource;

    private void OnUpdate(IList<ParamValuePayload> items)
    {
        if (items.Count == 0) return;
        foreach (var value in items)
        {
            var name = MavlinkTypesHelper.GetString(value.ParamId);
            if (_paramsSource.TryGetValue(name, out var item) == false)
            {
                if (_existDescription.TryGetValue(name, out var desc) == false)
                {
                    desc = CreateDefaultDescription(value);
                }

                item = new ParamItem(Base, _converter, desc, value);
                _paramsSource.Add(name, item);
                // we don't need to unsubscribe from this subscription because it will be disposed with item
                item.IsSynced.AsObservable().Subscribe(OnSyncedChanged);
            }

            item.InternalUpdate(value);
            
            _onValueChanged.OnNext((name, item.Value.Value));
        }
    }

    private void OnSyncedChanged(bool value)
    {
        if (value == false) _isSynced.Value = false;
        else
        {
            var allSynced = _paramsSource.All(i => i.Value.IsSynced.Value);
            if (allSynced) _isSynced.Value = true;
        }
    }

    private static ParamDescription CreateDefaultDescription(ParamValuePayload value)
    {
        var desc = new ParamDescription
        {
            Name = MavlinkTypesHelper.GetString(value.ParamId),
            DisplayName = MavlinkTypesHelper.GetString(value.ParamId),
            Description = $"Has no description. Type {value.ParamType:G}. Index: {value.ParamIndex}", //TODO: Localize
            Max = null,
            Min = null,
            Units = null,
            UnitsDisplayName = null,
            GroupName = null,
            User = null,
            Increment = null,
            IsReadOnly = false,
            IsRebootRequired = false,
            Values = null,
            Calibration = 0,
            BoardType = null
        };

        switch (value.ParamType)
        {
            case MavParamType.MavParamTypeUint8:
                desc.Max = byte.MaxValue;
                desc.Min = byte.MinValue;
                desc.Increment = 1;
                break;
            case MavParamType.MavParamTypeInt8:
                desc.Max = sbyte.MaxValue;
                desc.Min = sbyte.MinValue;
                desc.Increment = 1;
                break;
            case MavParamType.MavParamTypeUint16:
                desc.Max = ushort.MaxValue;
                desc.Min = ushort.MinValue;
                desc.Increment = 1;
                break;
            case MavParamType.MavParamTypeInt16:
                desc.Max = short.MaxValue;
                desc.Min = short.MinValue;
                desc.Increment = 1;
                break;
            case MavParamType.MavParamTypeUint32:
                desc.Max = uint.MaxValue;
                desc.Min = uint.MinValue;
                desc.Increment = 1;
                break;
            case MavParamType.MavParamTypeInt32:
                desc.Max = int.MaxValue;
                desc.Min = int.MinValue;
                desc.Increment = 1;
                break;
            case MavParamType.MavParamTypeUint64:
                desc.Max = ulong.MaxValue;
                desc.Min = ulong.MinValue;
                desc.Increment = 1;
                break;
            case MavParamType.MavParamTypeInt64:
                desc.Max = long.MaxValue;
                desc.Min = long.MinValue;
                desc.Increment = 1;
                break;
            case MavParamType.MavParamTypeReal32:
                break;
            case MavParamType.MavParamTypeReal64:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return desc;
    }


    public async Task ReadAll(IProgress<double>? progress = null, bool readMissedOneByOne = true,
        CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        var tcs = new TaskCompletionSource<bool>();
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);
        var lastUpdate = Base.Core.TimeProvider.GetTimestamp();
        using var c2 = Base.OnParamValue.Subscribe(p =>
        {
            _ = p.ParamCount;
            if (_paramsSource.Count == p.ParamCount)
            {
                tcs.TrySetResult(true);
            }

            var progressPercent = (double)_paramsSource.Count / p.ParamCount;
            progress.Report(progressPercent);
            Interlocked.Exchange(ref lastUpdate, Base.Core.TimeProvider.GetTimestamp());
        });
        var timeout = TimeSpan.FromMilliseconds(_config.ReadTimeouMs * _config.ReadAttemptCount);
        await using var c3 = Base.Core.TimeProvider.CreateTimer(_ =>
        {
            var last = Interlocked.Read(ref lastUpdate);
            if (Base.Core.TimeProvider.GetElapsedTime(last) > timeout)
            {
                tcs.TrySetResult(false);
            }
        }, null, TimeSpan.FromMilliseconds(_config.ReadTimeouMs), TimeSpan.FromMilliseconds(_config.ReadTimeouMs));
        
        await Base.SendRequestList(linkedCancel.Token).ConfigureAwait(false);
        var readAllParams = await tcs.Task.ConfigureAwait(false);
        
        _isSynced.Value = true;
        if (readAllParams == true) return;
        
        if (RemoteCount.CurrentValue != LocalCount.CurrentValue)
        {
            if (readMissedOneByOne == false) return;
            // read not all params=>try to read one by one
            for (ushort i = 0; i < RemoteCount.CurrentValue; i++)
            {
                var result = await Base.Read(i, linkedCancel.Token).ConfigureAwait(false);
                if (RemoteCount.CurrentValue != LocalCount.CurrentValue) return;
                progress.Report((double)LocalCount.CurrentValue / RemoteCount.CurrentValue);
            }
        }
    }

    public async Task<MavParamValue> ReadOnce(string name, CancellationToken cancel = default)
    {
        var result = await Base.Read(name, cancel).ConfigureAwait(false);
        return _converter.ConvertFromMavlinkUnion(result.ParamValue, result.ParamType);
    }

    public async Task<MavParamValue> WriteOnce(string name, MavParamValue value, CancellationToken cancel = default)
    {
        var floatValue = _converter.ConvertToMavlinkUnion(value);
        var result = await Base.Write(name, value.Type, floatValue, cancel).ConfigureAwait(false);
        return _converter.ConvertFromMavlinkUnion(result.ParamValue, result.ParamType);
    }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _isSynced.Dispose();
            _onValueChanged.Dispose();
            _disposeCancel.Dispose();
            _sub1.Dispose();
            RemoteCount.Dispose();
            LocalCount.Dispose();
        }
        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_isSynced).ConfigureAwait(false);
        await CastAndDispose(_onValueChanged).ConfigureAwait(false);
        await CastAndDispose(_disposeCancel).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(RemoteCount).ConfigureAwait(false);
        await CastAndDispose(LocalCount).ConfigureAwait(false);
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