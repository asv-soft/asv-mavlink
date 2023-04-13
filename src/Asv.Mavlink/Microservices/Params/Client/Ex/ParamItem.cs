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
    private readonly IMavParamValueConverter _converter;
    private readonly ParamValuePayload _payload;
    private readonly RxValue<bool> _isSynced;
    private readonly IRxEditableValue<decimal> _value;
    private bool _isInternalChange = false;

    public ParamItem(IParamsClient client, IMavParamValueConverter converter, ParamDescription paramDescriptions, ParamValuePayload payload)
    {
        _client = client;
        _converter = converter;
        _payload = payload;
        Name = MavlinkTypesHelper.GetString(payload.ParamId);
        Type = payload.ParamType;
        Index = payload.ParamIndex;
        _isSynced = new RxValue<bool>(true).DisposeItWith(Disposable);
        _value = new RxValue<decimal>(converter.ConvertFromMavlinkUnionToParamValue(payload.ParamValue,payload.ParamType)).DisposeItWith(Disposable);
        _value.Where(_=>_isInternalChange).Subscribe(_ => _isSynced.OnNext(false)).DisposeItWith(Disposable);
    }
    public string Name { get; }
    public MavParamType Type { get; }
    public ushort Index { get; }
    
    public IRxValue<bool> IsSynced => _isSynced;
    public IRxEditableValue<decimal> Value => _value;

    public Task Read(CancellationToken cancel)
    {
        return _client.Read(Name, cancel);
    }
    
    public Task Write(CancellationToken cancel)
    {
        return _client.Write(Name, Type, _converter.ConvertToMavlinkUnionToParamValue(_value.Value, Type), cancel);
    }

    internal void Update(ParamValuePayload payload)
    {
        if (payload.ParamIndex != Index) throw new Exception($"Invalid index: want {Index} but got {payload.ParamIndex}");
        var name = MavlinkTypesHelper.GetString(payload.ParamId);
        if (name != Name) throw new Exception($"Invalid index: want {Name} but got {name}");
        try
        {
            _isInternalChange = true;
            _value.OnNext(_converter.ConvertFromMavlinkUnionToParamValue(payload.ParamValue, payload.ParamType));
        }
        finally
        {
            _isInternalChange = false;
        }
        

    }
}