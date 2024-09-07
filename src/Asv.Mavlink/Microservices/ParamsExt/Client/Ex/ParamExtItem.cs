using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class ParamExtItem: DisposableOnceWithCancel,IParamExtItem
{
    private readonly IParamsExtClient _client;
    private readonly ParamExtValuePayload _payload;
    private readonly RxValue<bool> _isSynced;
    private readonly IRxEditableValue<MavParamExtValue> _value;
    private MavParamExtValue _remoteValue;

    public ParamExtItem(IParamsExtClient client, ParamExtDescription ParamExtDescription, ParamExtValuePayload payload)
    {
        _client = client;
        _payload = payload;
        Info = ParamExtDescription;
        Name = MavlinkTypesHelper.GetString(payload.ParamId);
        Type = payload.ParamType;
        Index = payload.ParamIndex;
        _isSynced = new RxValue<bool>(true).DisposeItWith(Disposable);
        _value = new RxValue<MavParamExtValue>(_remoteValue = MavParamExtHelper.CreateFromBuffer(payload.ParamValue, payload.ParamType)).DisposeItWith(Disposable);
        _value.Subscribe(v => _isSynced.OnNext(_remoteValue == v)).DisposeItWith(Disposable);
    }
    
    public ParamExtDescription Info { get; }
    
    public string Name { get; }
    public MavParamExtType Type { get; }
    public ushort Index { get; }
    
    public IRxValue<bool> IsSynced => _isSynced;
    public IRxEditableValue<MavParamExtValue> Value => _value;

    public async Task Read(CancellationToken cancel)
    {
        var res = await _client.Read(Name, cancel).ConfigureAwait(false);
        Update(res);
    }
    
    public async Task Write(CancellationToken cancel)
    {
        await _client.Write(Name, Type, Value.Value, cancel).ConfigureAwait(false);
    }

    internal void Update(ParamExtValuePayload payload)
    {
        var name = MavlinkTypesHelper.GetString(payload.ParamId);
        if (name != Name) throw new Exception($"Invalid index: want {Name} but got {name}");
        try
        {
            _remoteValue = MavParamExtHelper.CreateFromBuffer(payload.ParamValue, payload.ParamType);
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