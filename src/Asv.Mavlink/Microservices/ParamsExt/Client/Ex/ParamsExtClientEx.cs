using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

public class ParamsExtClientExConfig : ParamsExtClientConfig
{
    public int ReadListTimeoutMs { get; set; } = 5000;
}

public class ParamsExtClientEx : DisposableOnceWithCancel, IParamsExtClientEx
{
    private readonly ParamsClientExConfig _config;
    private readonly SourceCache<ParamExtItem, string> _paramsSource;
    private readonly RxValue<bool> _isSynced;
    private ImmutableDictionary<string, ParamExtDescription> _descriptions;
    private readonly RxValue<ushort?> _remoteCount;
    private readonly RxValue<ushort> _localCount;
    private readonly Subject<(string, MavParamExtValue)> _onValueChanged;

    public ParamsExtClientEx(IParamsExtClient client, ParamsClientExConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        Base = client;
        _paramsSource = new SourceCache<ParamExtItem, string>(x => x.Name).DisposeItWith(Disposable);
        _isSynced = new RxValue<bool>(false).DisposeItWith(Disposable);
        Items = _paramsSource.Connect().Transform(_ => (IParamExtItem)_).RefCount();
        client.OnParamExtValue.Where(_ => IsInit).Buffer(TimeSpan.FromMilliseconds(100)).Subscribe(OnUpdate)
            .DisposeItWith(Disposable);

        _remoteCount = new RxValue<ushort?>(null).DisposeItWith(Disposable);
        _localCount = new RxValue<ushort>(0).DisposeItWith(Disposable);
        _paramsSource.CountChanged.Select(_ => (ushort)_).Subscribe(_localCount).DisposeItWith(Disposable);
        _onValueChanged = new Subject<(string, MavParamExtValue)>().DisposeItWith(Disposable);
    }

    public IObservable<(string, MavParamExtValue)> OnValueChanged => _onValueChanged;
    public bool IsInit { get; set; }

    public void Init(IEnumerable<ParamExtDescription> existDescription)
    {
        _descriptions = existDescription.ToImmutableDictionary(_ => _.Name);
        IsInit = true;
    }

    public IParamsExtClient Base { get; }

    private void OnUpdate(IList<ParamExtValuePayload> items)
    {
        if (items.Count == 0) return;
        _paramsSource.Edit(_ =>
        {
            foreach (var value in items)
            {
                _remoteCount.Value = value.ParamCount;
                var name = MavlinkTypesHelper.GetString(value.ParamId);
                var exist = _.Lookup(name);
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
                    _.AddOrUpdate(newItem);
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
            var allSynced = _paramsSource.Items.All(_ => _.IsSynced.Value);
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

    public IRxValue<bool> IsSynced => _isSynced;

    public IObservable<IChangeSet<IParamExtItem, string>> Items { get; }

    public async Task ReadAll(IProgress<double> progress = null, CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        var tcs = new TaskCompletionSource<bool>();
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);
        var lastUpdate = DateTime.Now;
        ushort? paramsCount = null;
        using var c2 = Base.OnParamExtValue.Sample(TimeSpan.FromMilliseconds(50)).Subscribe(_ =>
        {
            paramsCount = _.ParamCount;
            if (_paramsSource.Count == _.ParamCount)
            {
                tcs.TrySetResult(true);
            }

            progress.Report((double)_paramsSource.Count / _.ParamCount);
            lastUpdate = DateTime.Now;
        });
        using var c3 = Observable.Timer(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100))
            .Subscribe(_ =>
            {
                if (DateTime.Now - lastUpdate > TimeSpan.FromMilliseconds(_config.ReadListTimeoutMs))
                {
                    tcs.TrySetResult(false);
                }
            });
        _paramsSource.Edit(_ =>
        {
            foreach (var item in _.Items)
            {
                item.Dispose();
            }

            _.Clear();
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
    
    public IRxValue<ushort?> RemoteCount => _remoteCount;

    public IRxValue<ushort> LocalCount => _localCount;

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
}