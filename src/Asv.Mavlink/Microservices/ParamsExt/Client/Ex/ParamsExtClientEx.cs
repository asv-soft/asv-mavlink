using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public class ParamsExtClientExConfig : ParamsExtClientConfig
{
    public int ReadListTimeoutMs { get; set; } = 5000;
    public int ChunkUpdateBufferMs { get; set; } = 100;
}

public class ParamsExtClientEx : MavlinkMicroserviceClient, IParamsExtClientEx
{
    private static readonly TimeSpan CheckTimeout = TimeSpan.FromMilliseconds(100);
    private readonly ParamsExtClientExConfig _config;
    private readonly ObservableDictionary<string, ParamExtItem> _paramsSource;
    private readonly BindableReactiveProperty<bool> _isSynced;
    private readonly Subject<(string, MavParamExtValue)> _onValueChanged;
    private readonly CancellationTokenSource _disposeCancel;
    private readonly IDisposable _sub1;
    private readonly ImmutableDictionary<string, ParamExtDescription> _existDescription;


    public ParamsExtClientEx(
        IParamsExtClient client, 
        ParamsExtClientExConfig config,
        IEnumerable<ParamExtDescription> existDescription) 
        : base(ParamsExtHelper.MicroserviceExName,client.Identity, client.Core)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _existDescription = existDescription.ToImmutableDictionary(x => x.Name, x => x);
        Base = client;
        _disposeCancel = new CancellationTokenSource();
        _paramsSource = new ObservableDictionary<string, ParamExtItem>();
        _isSynced = new BindableReactiveProperty<bool>(false);
        var list = new List<ParamExtValuePayload>();

        //_sub1 = client.OnParamExtValue.Chunk(TimeSpan.FromMilliseconds(config.ChunkUpdateBufferMs),client.Core.TimeProvider).Subscribe(OnUpdate);
        //TODO: fix chunk
        _sub1 = client.OnParamExtValue.Subscribe(p =>
        {
            list.Add(p);
            if (list.Count > 1)
            {
                OnUpdate(list);
            }
        });
        RemoteCount = client.OnParamExtValue.Select(x => (int)x.ParamCount).ToReadOnlyBindableReactiveProperty(-1);
        LocalCount = _paramsSource.ObserveCountChanged().ToReadOnlyBindableReactiveProperty();
        _onValueChanged = new Subject<(string, MavParamExtValue)>();
    }

    public IParamsExtClient Base { get; }
    public IReadOnlyBindableReactiveProperty<int> RemoteCount { get; }
    public IReadOnlyBindableReactiveProperty<int> LocalCount { get; }
    public Observable<(string, MavParamExtValue)> OnValueChanged => _onValueChanged;
    public IReadOnlyBindableReactiveProperty<bool> IsSynced => _isSynced;
    public IReadOnlyObservableDictionary<string, ParamExtItem> Items => _paramsSource;

    private void OnUpdate(IList<ParamExtValuePayload> items)
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

                item = new ParamExtItem(Base, desc, value);
                _paramsSource.Add(name, item);
            }

            item.InternalUpdate(value);
            item.IsSynced.AsObservable().Subscribe(OnSyncedChanged);
            _onValueChanged.OnNext((name, MavParamExtHelper.CreateFromBuffer(value.ParamValue, value.ParamType)));
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

    private static ParamExtDescription CreateDefaultDescription(ParamExtValuePayload value)
    {
        var desc = new ParamExtDescription();

        switch (value.ParamType)
        {
            case MavParamExtType.MavParamExtTypeUint8:
                desc.Max = byte.MaxValue;
                desc.Min = byte.MinValue;
                desc.Increment = 1;
                break;
            case MavParamExtType.MavParamExtTypeInt8:
                desc.Max = sbyte.MaxValue;
                desc.Min = sbyte.MinValue;
                desc.Increment = 1;
                break;
            case MavParamExtType.MavParamExtTypeUint16:
                desc.Max = ushort.MaxValue;
                desc.Min = ushort.MinValue;
                desc.Increment = 1;
                break;
            case MavParamExtType.MavParamExtTypeInt16:
                desc.Max = short.MaxValue;
                desc.Min = short.MinValue;
                desc.Increment = 1;
                break;
            case MavParamExtType.MavParamExtTypeUint32:
                desc.Max = uint.MaxValue;
                desc.Min = uint.MinValue;
                desc.Increment = 1;
                break;
            case MavParamExtType.MavParamExtTypeInt32:
                desc.Max = int.MaxValue;
                desc.Min = int.MinValue;
                desc.Increment = 1;
                break;
            case MavParamExtType.MavParamExtTypeUint64:
                desc.Max = ulong.MaxValue;
                desc.Min = ulong.MinValue;
                desc.Increment = 1;
                break;
            case MavParamExtType.MavParamExtTypeInt64:
                desc.Max = long.MaxValue;
                desc.Min = long.MinValue;
                desc.Increment = 1;
                break;
            case MavParamExtType.MavParamExtTypeReal32:
                break;
            case MavParamExtType.MavParamExtTypeReal64:
                break;
            case MavParamExtType.MavParamExtTypeCustom:
                desc.Max = MavParamExtValue.RawValueBufferMaxLength;
                desc.Min = MavParamExtValue.RawValueBufferMaxLength;
                desc.Increment = 0;
                break;
        }

        desc.Name = desc.DisplayName = MavlinkTypesHelper.GetString(value.ParamId);
        desc.Description = $"Has no description. Type {value.ParamType:G}. Index: {value.ParamIndex}"; //TODO: Localize
        desc.ParamExtType = value.ParamType;
        return desc;
    }


    public async Task ReadAll(IProgress<double>? progress = null, CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        var tcs = new TaskCompletionSource<bool>();
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);
        var lastUpdate = Base.Core.TimeProvider.GetTimestamp();
        using var c2 = Base.OnParamExtValue.Subscribe(p =>
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
            //TODO: time is not ticking, same as params microservice
            //Interlocked.Exchange(ref lastUpdate, Base.Core.TimeProvider.GetTimestamp());
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
    }

    public Observable<MavParamExtValue> Filter(string name)
    {
        MavParamHelper.CheckParamName(name);
        return OnValueChanged.Where(x => x.Item1.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            .Select(x => x.Item2);
    }


    public async Task<MavParamExtValue> ReadOnce(string name, CancellationToken cancel = default)
    {
        var result = await Base.Read(name, cancel).ConfigureAwait(false);
        return MavParamExtHelper.CreateFromBuffer(result.ParamValue, result.ParamType);
    }

    public async Task<MavParamExtValue> WriteOnce(string name, MavParamExtValue value,
        CancellationToken cancel = default)
    {
        var result = await Base.Write(name, value.Type,  value.GetValue().ToString().ToCharArray(), cancel).ConfigureAwait(false);
        return MavParamExtHelper.CreateFromBuffer(result.ParamValue, result.ParamType);
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
    }

    protected override async ValueTask DisposeAsyncCore()
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