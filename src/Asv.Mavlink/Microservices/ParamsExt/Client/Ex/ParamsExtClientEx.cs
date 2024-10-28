using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;
using R3;
using ObservableExtensions = System.ObservableExtensions;

namespace Asv.Mavlink;

public class ParamsExtClientExConfig : ParamsExtClientConfig
{
    public int ReadListTimeoutMs { get; set; } = 5000;
}

public class ParamsExtClientEx : IParamsExtClientEx,IDisposable
{
    private static readonly TimeSpan CheckTimeout = TimeSpan.FromMilliseconds(100);
    private readonly ParamsExtClientExConfig _config;
    private readonly SourceCache<ParamExtItem, string> _paramsSource;
    private readonly ReactiveProperty<bool> _isSynced;
    private ImmutableDictionary<string, ParamExtDescription> _descriptions;
    private readonly ReactiveProperty<ushort?> _remoteCount;
    private readonly ReactiveProperty<ushort> _localCount;
    private readonly System.Reactive.Subjects.Subject<(string, MavParamExtValue)> _onValueChanged;
    private readonly IDisposable _disposeIt;
    private readonly CancellationTokenSource _disposeCancel;

    public ParamsExtClientEx(IParamsExtClient client, ParamsExtClientExConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        Base = client;
        _disposeCancel = new CancellationTokenSource();
        _paramsSource = new SourceCache<ParamExtItem, string>(x => x.Name);
        _isSynced = new ReactiveProperty<bool>(false);
        Items = _paramsSource.Connect().Transform(i => (IParamExtItem)i).RefCount();
        // TODO: replace buffer to use TimeProvider
        var d1 = client.OnParamExtValue.Where(_ => IsInit).Buffer(TimeSpan.FromMilliseconds(100)).Subscribe(OnUpdate);

        _remoteCount = new ReactiveProperty<ushort?>(null);
        _localCount = new ReactiveProperty<ushort>(0);
        var d2 = _paramsSource.CountChanged.Select(i => (ushort)i).Subscribe(_localCount);
        _onValueChanged = new System.Reactive.Subjects.Subject<(string, MavParamExtValue)>();
        
        _disposeIt = Disposable.Combine(_disposeCancel,_paramsSource, _isSynced,_remoteCount,_localCount,_onValueChanged, d1, d2);
    }

    public IObservable<(string, MavParamExtValue)> OnValueChanged => _onValueChanged;
    public bool IsInit { get; set; }
    
    private CancellationToken DisposeCancel => _disposeCancel.Token;

    public void Init(IEnumerable<ParamExtDescription> existDescription)
    {
        _descriptions = existDescription.ToImmutableDictionary(d => d.Name);
        IsInit = true;
    }

    public IParamsExtClient Base { get; }

    private void OnUpdate(IList<ParamExtValuePayload> items)
    {
        if (items.Count == 0) return;
        _paramsSource.Edit(u =>
        {
            foreach (var value in items)
            {
                _remoteCount.OnNext(value.ParamCount);
                var name = MavlinkTypesHelper.GetString(value.ParamId);
                var exist = u.Lookup(name);
                if (exist.HasValue)
                {
                    exist.Value.Update(value);
                    if (_onValueChanged.HasObservers)
                    {
                        _onValueChanged.OnNext((name, MavParamExtHelper.CreateFromBuffer(value.ParamValue, value.ParamType)));
                    }
                }
                else
                {
                    if (_descriptions.TryGetValue(name, out var desc) == false)
                    {
                        desc = CreateDefaultDescription(value);
                    }

                    var newItem = new ParamExtItem(Base, desc, value);
                    u.AddOrUpdate(newItem);
                    newItem.IsSynced.Subscribe(OnSyncedChanged);
                    if (_onValueChanged.HasObservers)
                    {
                        _onValueChanged.OnNext((name, MavParamExtHelper.CreateFromBuffer(value.ParamValue, value.ParamType)));
                    }
                }
            }
        });
    }

    private void OnSyncedChanged(bool value)
    {
        if (value == false) _isSynced.OnNext(false);
        else
        {
            var allSynced = _paramsSource.Items.All(i => i.IsSynced.Value);
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

    public ReadOnlyReactiveProperty<bool> IsSynced => _isSynced;

    public IObservable<IChangeSet<IParamExtItem, string>> Items { get; }

    public async Task ReadAll(IProgress<double>? progress = null, CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        var tcs = new TaskCompletionSource<bool>();
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);
        var lastUpdate = Base.Core.TimeProvider.GetTimestamp();
        ushort? paramsCount = null;
        using var c2 = Base.OnParamExtValue.Subscribe(p =>
        {
            paramsCount = p.ParamCount;
            if (_paramsSource.Count == p.ParamCount)
            {
                tcs.TrySetResult(true);
            }
            progress.Report((double)_paramsSource.Count / p.ParamCount);
            Interlocked.Exchange(ref lastUpdate, Base.Core.TimeProvider.GetTimestamp());
        });
        var timeout = TimeSpan.FromMilliseconds(_config.ReadListTimeoutMs);
        using var c3 = Base.Core.TimeProvider.CreateTimer(_ =>
        {
            var last = Interlocked.Read(ref lastUpdate);
            if (Base.Core.TimeProvider.GetElapsedTime(last) > timeout)
            {
                tcs.TrySetResult(false);
            }
        },null,CheckTimeout,CheckTimeout);
          
        _paramsSource.Edit(u =>
        {
            foreach (var item in u.Items)
            {
                item.Dispose();
            }

            u.Clear();
        });
        await Base.SendRequestList(linkedCancel.Token).ConfigureAwait(false);
        var readAllParams = await tcs.Task.ConfigureAwait(false);
        progress.Report(1);
        _isSynced.OnNext(true);
    }

    public IObservable<MavParamExtValue> Filter(string name)
    {
        MavParamHelper.CheckParamName(name);
        return OnValueChanged.Where(x => x.Item1.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            .Select(x => x.Item2);
    }
    
    public ReadOnlyReactiveProperty<ushort?> RemoteCount => _remoteCount;

    public ReadOnlyReactiveProperty<ushort> LocalCount => _localCount;

    public async Task<MavParamExtValue> ReadOnce(string name, CancellationToken cancel = default)
    {
        var result = await Base.Read(name, cancel).ConfigureAwait(false);
        return MavParamExtHelper.CreateFromBuffer(result.ParamValue, result.ParamType);
    }

    public async Task<MavParamExtValue> WriteOnce(string name, MavParamExtValue value,
        CancellationToken cancel = default)
    {
        var result = await Base.Write(name, value.Type, value, cancel).ConfigureAwait(false);
        return MavParamExtHelper.CreateFromBuffer(result.ParamValue, result.ParamType);
    }

    public void Dispose()
    {
        _disposeCancel.Cancel(false);
        _disposeIt.Dispose();
    }
}