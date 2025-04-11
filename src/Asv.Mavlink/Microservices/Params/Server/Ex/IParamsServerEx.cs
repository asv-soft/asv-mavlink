using System;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

/// <summary>
/// Represents a server that handles parameter operations.
/// </summary>
public interface IParamsServerEx: IMavlinkMicroserviceServer
{
    /// <summary>
    /// Gets the observable sequence of exceptions that occur in the source sequence.
    /// </summary>
    /// <value>
    /// The observable sequence of exceptions.
    /// </value>
    Observable<Exception> OnError { get; }

    /// <summary>
    /// Gets an observable sequence of `ParamChangedEvent` events that represents updates to the property.
    /// </summary>
    /// <remarks>
    /// This property returns an `Observable<ParamChangedEvent>` object, which allows you to subscribe to
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
    Observable<ParamChangedEvent> OnUpdated { get; }

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
    public Observable<ParamChangedEvent> OnChange(string name)
    {
        return OnUpdated.Where(x => x.Metadata.Name.Equals(name));
    }
    public Observable<ParamChangedEvent> OnChange(IMavParamTypeMetadata param)
    {
        return OnUpdated.Where(x => x.Metadata.Name.Equals(param.Name));
    }
    public Observable<ParamChangedEvent> OnRemoteChange(string name)
    {
        return OnUpdated.Where(x => x.IsRemoteChange && x.Metadata.Name.Equals(name));
    }
    public Observable<ParamChangedEvent> OnRemoteChange(IMavParamTypeMetadata param)
    {
        return OnUpdated.Where(x => x.IsRemoteChange && x.Metadata.Name.Equals(param.Name));
    }
    public Observable<ParamChangedEvent> OnLocalChange(string name)
    {
        return OnUpdated.Where(x => x.IsRemoteChange == false && x.Metadata.Name.Equals(name));
    }
    public Observable<ParamChangedEvent> OnLocalChange(IMavParamTypeMetadata param)
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

    public async Task OnS8(IMavParamTypeMetadata param,CancellationToken disposeCancel, ILogger logger, ParamValueCallback<sbyte> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeInt8);
        await setCallback(this[param], true, disposeCancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext).RegisterTo(disposeCancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            try
            {
                await setCallback(x.NewValue, false, disposeCancel).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.ZLogError(e, $"Error on set {param.Name}={x.NewValue}:{e.Message}");
            }
        }
    }
    
    public void OnU8Command(IMavParamTypeMetadata param,CancellationToken disposeCancel, ILogger logger, Func<CancellationToken, Task> onEvent)
    {
        CheckType(param, MavParamType.MavParamTypeUint8);
        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .SubscribeAwait(OnNext, AwaitOperation.Drop).RegisterTo(disposeCancel);
        this[param] = (byte)0;
        return;

        async ValueTask OnNext(ParamChangedEvent x, CancellationToken cancel)
        {
            try
            {
                if ((byte)x.OldValue == 0 && (byte)x.NewValue != 0)
                {
                    this[param] = (byte)0;
                    await onEvent(cancel).ConfigureAwait(false);    
                }
            }
            catch (Exception e)
            {
                logger.ZLogError(e, $"Error on set {param.Name}={x.NewValue}:{e.Message}");
            }
        }
    }
    
    public async Task OnU8Bool(IMavParamTypeMetadata param,CancellationToken disposeCancel, ILogger logger, ParamValueCallback<bool> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeUint8);
        await setCallback((byte)this[param] != 0, true, disposeCancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext).RegisterTo(disposeCancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            try
            {
                await setCallback((byte)x.NewValue!= 0, false, disposeCancel).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.ZLogError(e, $"Error on set {param.Name}={x.NewValue}:{e.Message}");
            }
        }
    }
    
    public async Task OnU8Enum<TEnum>(IMavParamTypeMetadata param,CancellationToken disposeCancel, ILogger logger, ParamValueCallback<TEnum> setCallback)
        where TEnum : struct, Enum
    {
        CheckType(param, MavParamType.MavParamTypeUint8);

        await setCallback((TEnum)Enum.ToObject(typeof(TEnum), (byte)this[param]), true, disposeCancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext).RegisterTo(disposeCancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            try
            {
                await setCallback((TEnum)Enum.ToObject(typeof(TEnum), (byte)this[param]), false, disposeCancel).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.ZLogError(e, $"Error on set {param.Name}={x.NewValue}:{e.Message}");
            }
        }
    }
    
    public async Task OnU8(IMavParamTypeMetadata param,CancellationToken disposeCancel, ILogger logger, ParamValueCallback<byte> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeUint8);
        await setCallback(this[param], true, disposeCancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext).RegisterTo(disposeCancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            try
            {
                await setCallback(x.NewValue, false, disposeCancel).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.ZLogError(e, $"Error on set {param.Name}={x.NewValue}:{e.Message}");
            }
        }
    }
    
    
    
    public async Task OnS16(IMavParamTypeMetadata param,CancellationToken disposeCancel, ILogger logger, ParamValueCallback<short> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeInt16);
        await setCallback(this[param], true, disposeCancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext).RegisterTo(disposeCancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            try
            {
                await setCallback(x.NewValue, false, disposeCancel).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.ZLogError(e, $"Error on set {param.Name}={x.NewValue}:{e.Message}");
            }
        }
    }
    
    public async Task OnU16(IMavParamTypeMetadata param,CancellationToken disposeCancel, ILogger logger, ParamValueCallback<ushort> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeUint16);
        await setCallback(this[param], true, disposeCancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext).RegisterTo(disposeCancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            try
            {
                await setCallback(x.NewValue, false, disposeCancel).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.ZLogError(e, $"Error on set {param.Name}={x.NewValue}:{e.Message}");
            }
        }
    }
    
    public async Task OnS32(IMavParamTypeMetadata param,CancellationToken disposeCancel, ILogger logger, ParamValueCallback<int> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeInt32);
        await setCallback(this[param], true, disposeCancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext).RegisterTo(disposeCancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            try
            {
                await setCallback(x.NewValue, false, disposeCancel).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.ZLogError(e, $"Error on set {param.Name}={x.NewValue}:{e.Message}");
            }
        }
    }
    
    public async Task OnU32(IMavParamTypeMetadata param,CancellationToken disposeCancel, ILogger logger, ParamValueCallback<uint> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeUint32);
        await setCallback(this[param], true, disposeCancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext).RegisterTo(disposeCancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            try
            {
                await setCallback(x.NewValue, false, disposeCancel).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.ZLogError(e, $"Error on set {param.Name}={x.NewValue}:{e.Message}");
            }
        }
    }
    
    public async Task OnR32(IMavParamTypeMetadata param, CancellationToken disposeCancel, ILogger logger, ParamValueCallback<float> setCallback)
    {
        CheckType(param, MavParamType.MavParamTypeReal32);
        // first call without catch exception
        await setCallback(this[param], true, disposeCancel).ConfigureAwait(false);

        OnUpdated
            .Where(x => x.Metadata.Name.Equals(param.Name))
            .Subscribe(OnNext).RegisterTo(disposeCancel);
        return;

        async void OnNext(ParamChangedEvent x)
        {
            try
            {
                await setCallback(x.NewValue, false, disposeCancel).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.ZLogError(e, $"Error on set {param.Name}={x.NewValue}:{e.Message}");
            }
        }
    }

    #endregion
   
    
}

public delegate Task ParamValueCallback<in T>(T value, bool firstChange, CancellationToken cancel);