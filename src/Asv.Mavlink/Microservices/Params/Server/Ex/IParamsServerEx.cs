using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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
    
    private static void CheckType(IMavParamTypeMetadata param, MavParamType type)
    {
        if (param.Type != type)
        {
            throw new ArgumentException($"Parameter must be of type {type:G}", nameof(param));
        }
    }
    
    
}

/// <summary>
/// Helper class for working with the Params Server Extension.
/// </summary>
public static class ParamsServerExHelper
{
    
}

