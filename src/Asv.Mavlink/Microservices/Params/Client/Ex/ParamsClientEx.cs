using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;
using ObservableExtensions = System.ObservableExtensions;

namespace Asv.Mavlink;

public class ParamsClientExConfig : ParameterClientConfig
{
    public int ReadListTimeoutMs { get; set; } = 5000;
    public int ChunkUpdateBufferMs { get; set; } = 100;
}

public sealed class ParamsClientEx : IParamsClientEx, IDisposable, IAsyncDisposable
{
    private static readonly TimeSpan CheckTimeout = TimeSpan.FromMilliseconds(100);
    private readonly ParamsClientExConfig _config;
    private readonly IMavParamEncoding _converter;
    private readonly ObservableDictionary<string, ParamItem> _paramsSource;
    private readonly BindableReactiveProperty<bool> _isSynced;
    private readonly Subject<(string, MavParamValue)> _onValueChanged;
    private readonly ILogger<ParamsClientEx> _logger;
    private readonly CancellationTokenSource _disposeCancel;
    private readonly IDisposable _sub1;
    private readonly ImmutableDictionary<string, ParamDescription> _existDescription;

    public ParamsClientEx(IParamsClient client, ParamsClientExConfig config, IMavParamEncoding converter,
        IEnumerable<ParamDescription> existDescription)
    {
        _logger = client.Core.Log.CreateLogger<ParamsClientEx>();
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        _existDescription = existDescription.ToImmutableDictionary(x => x.Name, x => x);
        Base = client;
        _disposeCancel = new CancellationTokenSource();
        _paramsSource = new ObservableDictionary<string, ParamItem>();
        _isSynced = new BindableReactiveProperty<bool>(false);
        
        //TODO: chunk не срабатывает
        //_sub1 = client.OnParamValue.Chunk(TimeSpan.FromMilliseconds(config.ChunkUpdateBufferMs),client.Core.TimeProvider).Subscribe(OnUpdate);
        
        var listParam = new List<ParamValuePayload>();
        _sub1 = client.OnParamValue.Subscribe(p =>
        {
            listParam.Add(p);
            OnUpdate(listParam);
        });
        RemoteCount = client.OnParamValue.Select(x => (int)x.ParamCount).ToReadOnlyReactiveProperty(-1);
        LocalCount = _paramsSource.ObserveCountChanged().ToReadOnlyReactiveProperty();
        _onValueChanged = new Subject<(string, MavParamValue)>();
    }

    public string Name => $"{Base.Name}Ex";
    public IParamsClient Base { get; }
    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default) => Task.CompletedTask;
    public ReadOnlyReactiveProperty<int> RemoteCount { get; }
    public ReadOnlyReactiveProperty<int> LocalCount { get; }
    public Observable<(string, MavParamValue)> OnValueChanged => _onValueChanged;
    private CancellationToken DisposeCancel => _disposeCancel.Token;
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
            }

            item.InternalUpdate(value);
            item.IsSynced.AsObservable().Subscribe(OnSyncedChanged);
            _onValueChanged.OnNext((name, item.Value.Value));
        }
    }

    private void OnSyncedChanged(bool value)
    {
        if (value == false) _isSynced.OnNext(false);
        else
        {
            var allSynced = _paramsSource.All(i => i.Value.IsSynced.Value);
            if (allSynced) _isSynced.OnNext(true);
        }
    }

    private static ParamDescription CreateDefaultDescription(ParamValuePayload value)
    {
        var desc = new ParamDescription();
        desc.Name = desc.DisplayName = MavlinkTypesHelper.GetString(value.ParamId);
        desc.Description = $"Has no description. Type {value.ParamType:G}. Index: {value.ParamIndex}"; //TODO: Localize
        desc.Max = null;
        desc.Min = null;
        desc.Units = null;
        desc.UnitsDisplayName = null;
        desc.GroupName = null;
        desc.User = null;
        desc.Increment = null;
        desc.IsReadOnly = false;
        desc.IsRebootRequired = false;
        desc.Values = null;
        desc.Calibration = 0;
        desc.BoardType = null;

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

            progress.Report((double)_paramsSource.Count / p.ParamCount);
            Interlocked.Exchange(ref lastUpdate, Base.Core.TimeProvider.GetTimestamp());
        });
        var timeout = TimeSpan.FromMilliseconds(_config.ReadListTimeoutMs);
        await using var c3 = Base.Core.TimeProvider.CreateTimer(_ =>
        {
            var last = Interlocked.Read(ref lastUpdate);
            if (Base.Core.TimeProvider.GetElapsedTime(last) > timeout)
            {
                tcs.TrySetResult(false);
            }
            //TODO: Interlocked.Exchange(ref lastUpdate, Base.Core.TimeProvider.GetTimestamp());
        }, null, CheckTimeout, CheckTimeout);
        var cached = _paramsSource.ToImmutableArray();
        _paramsSource.Clear();
        foreach (var item in cached)
        {
            await item.Value.DisposeAsync().ConfigureAwait(false);
        }

        await Base.SendRequestList(linkedCancel.Token).ConfigureAwait(false);
        var readAllParams = await tcs.Task.ConfigureAwait(false);
        progress.Report(1);
        _isSynced.OnNext(true);
        if (readAllParams == true) return;
        if (RemoteCount.CurrentValue != LocalCount.CurrentValue)
        {
            if (readMissedOneByOne == false) return;
            // read no all params=>try to read one by one
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

    public void Dispose()
    {
        _isSynced.Dispose();
        _onValueChanged.Dispose();
        _disposeCancel.Dispose();
        _sub1.Dispose();
        RemoteCount.Dispose();
        LocalCount.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_isSynced).ConfigureAwait(false);
        await CastAndDispose(_onValueChanged).ConfigureAwait(false);
        await CastAndDispose(_disposeCancel).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(RemoteCount).ConfigureAwait(false);
        await CastAndDispose(LocalCount).ConfigureAwait(false);

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