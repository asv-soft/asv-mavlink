using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink;

public sealed class ParamExtItem: IDisposable,IAsyncDisposable
{
    private readonly IParamsExtClient _client;
    private readonly ParamExtValuePayload _payload;
    private readonly BindableReactiveProperty<bool> _isSynced;
    private readonly BindableReactiveProperty<MavParamExtValue> _value;
    private MavParamExtValue _remoteValue;
    private readonly IDisposable _sub1;

    public ParamExtItem(IParamsExtClient client, ParamExtDescription paramExtDescription, ParamExtValuePayload payload)
    {
        _client = client;
        _payload = payload;
        Info = paramExtDescription;
        Name = MavlinkTypesHelper.GetString(payload.ParamId);
        Type = payload.ParamType;
        Index = payload.ParamIndex;
        _isSynced = new BindableReactiveProperty<bool>(true);
        _value = new BindableReactiveProperty<MavParamExtValue>(_remoteValue = MavParamExtHelper.CreateFromBuffer(payload.ParamValue, payload.ParamType));
        _sub1 = _value.Subscribe(v => _isSynced.OnNext(_remoteValue == v));
    }
    
    public ParamExtDescription Info { get; }
    public string Name { get; }
    public MavParamExtType Type { get; }
    public ushort Index { get; }
    
    public IReadOnlyBindableReactiveProperty<bool> IsSynced => _isSynced;
    public IBindableReactiveProperty<MavParamExtValue> Value => _value;

    public async Task Read(CancellationToken cancel)
    {
        var res = await _client.Read(Name, cancel).ConfigureAwait(false);
        InternalUpdate(res);
    }
    
    public async Task Write(CancellationToken cancel)
    {
        await _client.Write(Name, Type, Value.Value, cancel).ConfigureAwait(false);
    }

    internal void InternalUpdate(ParamExtValuePayload payload)
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

    public void Dispose()
    {
        _isSynced.Dispose();
        _value.Dispose();
        _sub1.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_isSynced).ConfigureAwait(false);
        await CastAndDispose(_value).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }
}