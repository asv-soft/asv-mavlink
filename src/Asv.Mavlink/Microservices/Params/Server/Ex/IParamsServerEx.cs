using System;
using System.Reactive.Linq;

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
    public MavParamValue this[IMavParamTypeMetadata param]
    {
        get => this[param.Name];
        set => this[param.Name] = value;
    }

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
}

/// <summary>
/// Helper class for working with the Params Server Extension.
/// </summary>
public static class ParamsServerExHelper
{
    
}

