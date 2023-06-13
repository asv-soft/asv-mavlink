using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class ParamItem: DisposableOnceWithCancel,IParamItem
{
    private readonly IParamsClient _client;
    private readonly IMavParamEncoding _converter;
    private readonly ParamValuePayload _payload;
    private readonly RxValue<bool> _isSynced;
    private readonly IRxEditableValue<MavParamValue> _value;
    private MavParamValue _remoteValue;

    public ParamItem(IParamsClient client, IMavParamEncoding converter, ParamDescription paramDescriptions, ParamValuePayload payload)
    {
        _client = client;
        _converter = converter;
        _payload = payload;
        Info = paramDescriptions;
        Name = MavlinkTypesHelper.GetString(payload.ParamId);
        Type = payload.ParamType;
        Index = payload.ParamIndex;
        _isSynced = new RxValue<bool>(true).DisposeItWith(Disposable);
        _value = new RxValue<MavParamValue>(_remoteValue = converter.ConvertFromMavlinkUnion(payload.ParamValue,payload.ParamType)).DisposeItWith(Disposable);
        _value.Subscribe(_ => _isSynced.OnNext(_remoteValue == _)).DisposeItWith(Disposable);
        Disposable.AddAction(() =>
        {
            
        });
    }
    
    public ParamDescription Info { get; }
    
    public string Name { get; }
    public MavParamType Type { get; }
    public ushort Index { get; }
    
    public IRxValue<bool> IsSynced => _isSynced;
    public IRxEditableValue<MavParamValue> Value => _value;

    public async Task Read(CancellationToken cancel)
    {
        var res = await _client.Read(Name, cancel).ConfigureAwait(false);
        Update(res);
    }
    
    public async Task Write(CancellationToken cancel)
    {
        await _client.Write(Name, Type, _converter.ConvertToMavlinkUnion(_value.Value), cancel).ConfigureAwait(false);
    }

    internal void Update(ParamValuePayload payload)
    {
        var name = MavlinkTypesHelper.GetString(payload.ParamId);
        if (name != Name) throw new Exception($"Invalid index: want {Name} but got {name}");
        try
        {
            _remoteValue = _converter.ConvertFromMavlinkUnion(payload.ParamValue, payload.ParamType);
            _value.OnNext(_remoteValue);
        }
        catch (Exception)
        {
            
        }
        finally
        {
        }
        

    }
}