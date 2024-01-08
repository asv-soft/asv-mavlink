using System;

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
}

/// <summary>
/// Helper class for working with the Params Server Extension.
/// </summary>
public static class ParamsServerExHelper
{
    
}

