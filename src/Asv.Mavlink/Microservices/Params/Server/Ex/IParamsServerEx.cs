using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents a server that handles parameter operations.
/// </summary>
public interface IParamsServerEx
{
    /// <summary>
    /// Gets the observable sequence of exceptions that occur in the source sequence.
    /// </summary>
    /// <value>
    /// The observable sequence of exceptions.
    /// </value>
    IObservable<Exception> OnError { get; }

    /// <summary>
    /// Gets an observable sequence of `ParamChangedEvent` events that represents updates to the property.
    /// </summary>
    /// <remarks>
    /// This property returns an `IObservable<ParamChangedEvent>` object, which allows you to subscribe to
    /// receive notifications whenever the `OnUpdated` event occurs. The `ParamChangedEvent` class provides
    /// details about the update, such as the parameter that has been changed.
    /// Example usage:
    /// <code>
    /// OnUpdated.Subscribe(event =>
    /// {
    /// Console.WriteLine($"Parameter {event.ParameterName} has changed.");
    /// });
    /// </code>
    /// This will subscribe to the `OnUpdated` event and write a message to the console whenever the property is updated.
    /// </remarks>
    IObservable<ParamChangedEvent> OnUpdated { get; }

    /// <summary>
    /// Gets or sets the MavParamValue with the specified name.
    /// </summary>
    /// <param name="name">The name of the MavParamValue.</param>
    /// <returns>The MavParamValue with the specified name.</returns>
    MavParamValue this[string name] { get; set; }

    /// <summary>
    /// Gets or sets the MavParamValue associated with the specified IMavParamTypeMetadata.
    /// </summary>
    /// <param name="param">The IMavParamTypeMetadata to retrieve or set the MavParamValue for.</param>
    /// <returns>The MavParamValue associated with the specified IMavParamTypeMetadata.</returns>
    MavParamValue this[IMavParamTypeMetadata param] { get; set; }
    IReadOnlyList<IMavParamTypeMetadata> AllParamsList { get; }
    IReadOnlyDictionary<string,(ushort,IMavParamTypeMetadata)> AllParamsDict { get; }
    public IObservable<ParamChangedEvent> OnChange(string name)
    {
        return OnUpdated.Where(x => x.Metadata.Name.Equals(name));
    }
    public IObservable<ParamChangedEvent> OnChange(IMavParamTypeMetadata param)
    {
        return OnUpdated.Where(x => x.Metadata.Name.Equals(param.Name));
    }
    public IObservable<ParamChangedEvent> OnRemoteChange(string name)
    {
        return OnUpdated.Where(x => x.IsRemoteChange && x.Metadata.Name.Equals(name));
    }
    public IObservable<ParamChangedEvent> OnRemoteChange(IMavParamTypeMetadata param)
    {
        return OnUpdated.Where(x => x.IsRemoteChange && x.Metadata.Name.Equals(param.Name));
    }
    public IObservable<ParamChangedEvent> OnLocalChange(string name)
    {
        return OnUpdated.Where(x => x.IsRemoteChange == false && x.Metadata.Name.Equals(name));
    }
    public IObservable<ParamChangedEvent> OnLocalChange(IMavParamTypeMetadata param)
    {
        return OnUpdated.Where(x => x.IsRemoteChange == false && x.Metadata.Name.Equals(param.Name));
    }

    private static void CheckType(IMavParamTypeMetadata param, MavParamType type)
    {
        if (param.Type != type)
        {
            throw new ArgumentException($"Parameter must be of type {type:G}", nameof(param));
        }
    }
    
    #region Disposable on change

    public IDisposable SetOnChangeReal32(IMavParamTypeMetadata param, Action<float> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeReal32);
        setCallback(this[param]);
        return OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(x => setCallback(x.NewValue));
    }
    public IDisposable SetOnChangeInt32(IMavParamTypeMetadata param, Action<int> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeInt32);
        setCallback(this[param]);
        return OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(x => setCallback(x.NewValue));
    }
    public IDisposable SetOnChangeUint32(IMavParamTypeMetadata param, Action<uint> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeUint32);
        setCallback(this[param]);
        return OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(x => setCallback(x.NewValue));
    }
    public IDisposable SetOnChangeUint16(IMavParamTypeMetadata param, Action<ushort> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeUint16);
        setCallback(this[param]);
        return OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(x => setCallback(x.NewValue));
    }
    public IDisposable SetOnChangeInt16(IMavParamTypeMetadata param, Action<short> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeInt16);
        setCallback(this[param]);
        return OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(x => setCallback(x.NewValue));
    }
    public IDisposable SetOnChangeUint8(IMavParamTypeMetadata param, Action<byte> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeUint8);
        setCallback(this[param]);
        return OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(x => setCallback(x.NewValue));
    }
    public IDisposable SetOnChangeInt8(IMavParamTypeMetadata param, Action<sbyte> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeInt8);
        setCallback(this[param]);
        return OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(x => setCallback(x.NewValue));
    }

    #endregion

    #region On change

    public async Task OnS8(IMavParamTypeMetadata param, ParamValueCallback<sbyte> setCallback, CancellationToken cancel)
    {
        CheckType(param, MavParamType.MavParamTypeInt8);
        await setCallback(this[param], true, cancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext,cancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            await setCallback(x.NewValue, false, cancel).ConfigureAwait(false);
        }
    }
    
    public async Task OnU8(IMavParamTypeMetadata param, ParamValueCallback<byte> setCallback, CancellationToken cancel)
    {
        CheckType(param, MavParamType.MavParamTypeUint8);
        await setCallback(this[param], true, cancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext,cancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            await setCallback(x.NewValue, false, cancel).ConfigureAwait(false);
        }
    }
    
    public async Task OnS16(IMavParamTypeMetadata param, ParamValueCallback<short> setCallback, CancellationToken cancel)
    {
        CheckType(param, MavParamType.MavParamTypeInt16);
        await setCallback(this[param], true, cancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext,cancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            await setCallback(x.NewValue, false, cancel).ConfigureAwait(false);
        }
    }
    
    public async Task OnU16(IMavParamTypeMetadata param, ParamValueCallback<ushort> setCallback, CancellationToken cancel)
    {
        CheckType(param, MavParamType.MavParamTypeUint16);
        await setCallback(this[param], true, cancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext,cancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            await setCallback(x.NewValue, false, cancel).ConfigureAwait(false);
        }
    }
    
    public async Task OnS32(IMavParamTypeMetadata param, ParamValueCallback<int> setCallback, CancellationToken cancel)
    {
        CheckType(param, MavParamType.MavParamTypeInt32);
        await setCallback(this[param], true, cancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext,cancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            await setCallback(x.NewValue, false, cancel).ConfigureAwait(false);
        }
    }
    
    public async Task OnU32(IMavParamTypeMetadata param, ParamValueCallback<uint> setCallback, CancellationToken cancel)
    {
        CheckType(param, MavParamType.MavParamTypeUint32);
        await setCallback(this[param], true, cancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext,cancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            await setCallback(x.NewValue, false, cancel).ConfigureAwait(false);
        }
    }
    
    public async Task OnReal32(IMavParamTypeMetadata param, ParamValueCallback<float> setCallback, CancellationToken cancel)
    {
        CheckType(param, MavParamType.MavParamTypeReal32);
        await setCallback(this[param], true, cancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext,cancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            await setCallback(x.NewValue, false, cancel).ConfigureAwait(false);
        }
    }

    #endregion
   
    
}

public delegate Task ParamValueCallback<in T>(T value, bool firstChange, CancellationToken cancel);