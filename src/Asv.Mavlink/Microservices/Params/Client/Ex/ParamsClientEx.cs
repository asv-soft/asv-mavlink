using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
    private IMavParamEncoding _converter;
    private readonly ParamsClientExConfig _config;
    private readonly SourceCache<ParamItem, string> _paramsSource;
    private readonly RxValue<bool> _isSynced;
    private ImmutableDictionary<string,ParamDescription> _descriptions;
    private readonly RxValue<ushort?> _remoteCount;
    private readonly RxValue<ushort> _localCount;
    private readonly Subject<(string, MavParamValue)> _onValueChanged;

    public ParamsClientEx(IParamsClient client, ParamsClientExConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        Base = client;
        _paramsSource = new SourceCache<ParamItem, string>(x => x.Name).DisposeItWith(Disposable);
        _isSynced = new RxValue<bool>(false).DisposeItWith(Disposable);
        Items = _paramsSource.Connect().Transform(i=>(IParamItem)i).RefCount();
        client.OnParamValue.Where(_=>IsInit).Buffer(TimeSpan.FromMilliseconds(100)).Subscribe(OnUpdate).DisposeItWith(Disposable);
        
        _remoteCount = new RxValue<ushort?>(null).DisposeItWith(Disposable);
        _localCount = new RxValue<ushort>(0).DisposeItWith(Disposable);
        _paramsSource.CountChanged.Select(i=>(ushort)i).Subscribe(_localCount).DisposeItWith(Disposable);
        _onValueChanged = new Subject<(string, MavParamValue)>().DisposeItWith(Disposable);
    }

    public IObservable<(string, MavParamValue)> OnValueChanged => _onValueChanged;
    public bool IsInit { get; set; }

    public void Init(IMavParamEncoding converter, IEnumerable<ParamDescription> existDescription)
    {
        _descriptions = existDescription.ToImmutableDictionary(d => d.Name);
        _converter = converter;
        IsInit = true;
    }

    public IParamsClient Base { get; }
    private void OnUpdate(IList<ParamValuePayload> items)
    {
        if (items.Count == 0) return;
        _paramsSource.Edit(u =>
        {
            foreach (var value in items)
            {
                _remoteCount.Value = value.ParamCount;
                var name = MavlinkTypesHelper.GetString(value.ParamId);
                var exist = u.Lookup(name);
                if (exist.HasValue)
                {
                    exist.Value.Update(value);
                    if (_onValueChanged.HasObservers)
                    {
                        _onValueChanged.OnNext((name, _converter.ConvertFromMavlinkUnion(value.ParamValue, value.ParamType)));
                    }
                }
                else
                {
                    if (_descriptions.TryGetValue(name, out var desc) == false)
                    {
                        desc = CreateDefaultDescription(value);
                    }
                    var newItem = new ParamItem(Base, _converter, desc, value);
                    u.AddOrUpdate(newItem);
                    newItem.IsSynced.Subscribe(OnSyncedChanged);
                    if (_onValueChanged.HasObservers)
                    {
                        _onValueChanged.OnNext((name, _converter.ConvertFromMavlinkUnion(value.ParamValue, value.ParamType)));
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
        await using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled(), false);
        var lastUpdate = DateTime.Now;
        ushort? paramsCount = null;
        using var c2 = Base.OnParamValue.Sample(TimeSpan.FromMilliseconds(50)).Subscribe(p =>
        {
            paramsCount = p.ParamCount;
            if (_paramsSource.Count == p.ParamCount)
            {
                tcs.TrySetResult(true);
            }
            progress.Report((double)_paramsSource.Count / p.ParamCount);
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
    
    public async Task<MavParamValue> ReadOnce(string name, CancellationToken cancel = default)
    {
        var result = await Base.Read(name,cancel).ConfigureAwait(false);
        return _converter.ConvertFromMavlinkUnion(result.ParamValue, result.ParamType);
    }

    public async Task<MavParamValue> WriteOnce(string name, MavParamValue value, CancellationToken cancel = default)
    {
        var floatValue = _converter.ConvertToMavlinkUnion(value);
        var result = await Base.Write(name,value.Type,floatValue, cancel).ConfigureAwait(false);
        return _converter.ConvertFromMavlinkUnion(result.ParamValue, result.ParamType);
    }
}