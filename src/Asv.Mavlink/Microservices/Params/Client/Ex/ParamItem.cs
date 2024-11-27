using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink;

public sealed class ParamItem: IDisposable,IAsyncDisposable
{
    private readonly IParamsClient _client;
    private readonly IMavParamEncoding _converter;
    private MavParamValue _remoteValue;
    private readonly BindableReactiveProperty<MavParamValue> _value;

    public ParamItem(IParamsClient client, IMavParamEncoding converter, ParamDescription paramDescriptions, ParamValuePayload payload)
    {
        _client = client;
        _converter = converter;
        Info = paramDescriptions;
        Name = MavlinkTypesHelper.GetString(payload.ParamId);
        Type = payload.ParamType;
        Index = payload.ParamIndex;
        _value = new BindableReactiveProperty<MavParamValue>(_remoteValue = converter.ConvertFromMavlinkUnion(payload.ParamValue,payload.ParamType));
        IsSynced = _value.Select(x => _remoteValue == x).ToReadOnlyBindableReactiveProperty();
    }
    
    public ParamDescription Info { get; }
    public string Name { get; }
    public MavParamType Type { get; }
    public ushort Index { get; }
    public IReadOnlyBindableReactiveProperty<bool> IsSynced { get; }
    public BindableReactiveProperty<MavParamValue> Value => _value;
    public async Task Read(CancellationToken cancel)
    {
        var res = await _client.Read(Name, cancel).ConfigureAwait(false);
        InternalUpdate(res);
    }
    public async Task Write(CancellationToken cancel)
    {
        await _client.Write(Name, Type, _converter.ConvertToMavlinkUnion(Value.Value), cancel).ConfigureAwait(false);
    }
    internal void InternalUpdate(ParamValuePayload payload)
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

    #region Dispose

    public void Dispose()
    {
        _value.Dispose();
        IsSynced.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_value).ConfigureAwait(false);
        await CastAndDispose(IsSynced).ConfigureAwait(false);

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