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

namespace Asv.Mavlink;

public class ParamsClientExConfig:ParameterClientConfig
{
    public int ReadListTimeoutMs { get; set; } = 5000;
}

public class ParamsClientEx : DisposableOnceWithCancel, IParamsClientEx
{
    private IMavParamValueConverter _converter;
    private readonly ParamsClientExConfig _config;
    private readonly SourceCache<ParamItem, string> _missionSource;
    private readonly RxValue<bool> _isSynced;
    private ImmutableDictionary<string,ParamDescription> _descriptions;
    private readonly RxValue<ushort?> _remoteCount;
    private readonly RxValue<ushort> _localCount;

    public ParamsClientEx(IParamsClient client, ParamsClientExConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        Base = client;
        _missionSource = new SourceCache<ParamItem, string>(x => x.Name).DisposeItWith(Disposable);
        _isSynced = new RxValue<bool>(false).DisposeItWith(Disposable);
        Items = _missionSource.Connect().DisposeMany().Transform(_=>(IParamItem)_).Publish().RefCount();
        client.OnParamValue.Where(_=>IsInit && _.ParamIndex !=65535).Buffer(TimeSpan.FromMilliseconds(100)).Subscribe(OnUpdate).DisposeItWith(Disposable);
        
        _remoteCount = new RxValue<ushort?>(null).DisposeItWith(Disposable);
        _localCount = new RxValue<ushort>(0).DisposeItWith(Disposable);
        _missionSource.CountChanged.Select(_=>(ushort)_).Subscribe(_localCount).DisposeItWith(Disposable);
    }

    public bool IsInit { get; set; }

    public void Init(IMavParamValueConverter converter, IEnumerable<ParamDescription> existDescription)
    {
        _descriptions = existDescription.ToImmutableDictionary(_ => _.Name);
        _converter = converter;
        IsInit = true;
    }

    public IParamsClient Base { get; }
    private void OnUpdate(IList<ParamValuePayload> items)
    {
        _missionSource.Edit(_ =>
        {
            foreach (var value in items)
            {
                _remoteCount.Value = value.ParamCount;
                var name = MavlinkTypesHelper.GetString(value.ParamId);
                var exist = _.Lookup(name);
                if (exist.HasValue)
                {
                    exist.Value.Update(value);
                }
                else
                {
                    if (_descriptions.TryGetValue(name, out var desc) == false)
                    {
                        desc = CreateDefaultDescription(value);
                    }
                    var newItem = new ParamItem(Base, _converter, desc, value);
                    _.AddOrUpdate(newItem);
                    newItem.IsSynced.Subscribe(OnSyncedChanged);
                }
            }
            
        });
    }

    private void OnSyncedChanged(bool value)
    {
        if (value == false) _isSynced.OnNext(false);
        else
        {
            var allSynced = _missionSource.Items.All(_ => _.IsSynced.Value);
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

    public IRxValue<bool> IsSynced => _isSynced;

    public IObservable<IChangeSet<IParamItem, string>> Items { get; }
    
    public async Task ReadAll(IProgress<double> progress = null, CancellationToken cancel = default)
    {
        progress ??= new Progress<double>();
        using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
        var tcs = new TaskCompletionSource<bool>();
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled());
        var lastUpdate = DateTime.Now;
        ushort? paramsCount = null;
        using var c2 = Base.OnParamValue.Sample(TimeSpan.FromMilliseconds(50)).Subscribe(_ =>
        {
            paramsCount = _.ParamCount;
            if (_missionSource.Count == _.ParamCount)
            {
                tcs.TrySetResult(true);
            }
            progress.Report((double)_missionSource.Count / _.ParamCount);
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
        await Base.SendRequestList(linkedCancel.Token).ConfigureAwait(false);
        var readAllParams = await tcs.Task.ConfigureAwait(false);
        // if (readAllParams == true) return;
        // if (paramsCount.HasValue == false) throw new Exception("Can't read params: collection is empty or connection error");
        // // read no all params=>try to read one by one
        // c3.Dispose();
        // for (ushort i = 0; i < paramsCount; i++)
        // {
        //     if (_missionSource.Lookup(i).HasValue) continue;
        //     var result = await Base.Read(i,linkedCancel.Token).ConfigureAwait(false);
        //     if (_missionSource.Count == result.ParamCount) return;
        //     progress.Report((double)_missionSource.Count / result.ParamCount);
        // }
        progress.Report(1);
        _isSynced.OnNext(true);
    }

    public IRxValue<ushort?> RemoteCount => _remoteCount;

    public IRxValue<ushort> LocalCount => _localCount;
    
    public async Task<decimal> ReadOnce(string name, CancellationToken cancel = default)
    {
        var result = await Base.Read(name,cancel).ConfigureAwait(false);
        return _converter.ConvertFromMavlinkUnionToParamValue(result.ParamValue, result.ParamType);
    }
}