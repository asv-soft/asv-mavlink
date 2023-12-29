using System;
using System.Collections.Generic;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a debug client interface.
    /// </summary>
    public interface IDebugClient:IDisposable
    {
        /// <summary>
        /// Gets an observable sequence of key-value pairs where the key represents the name of a property and
        /// the value represents a float value associated with that property.
        /// </summary>
        /// <remarks>
        /// The subscription to this observable sequence will provide updated values for different properties
        /// as key-value pairs whenever there is a change in any property value.
        /// </remarks>
        /// <returns>
        /// An IObservable sequence of key-value pairs with string keys and float values.
        /// </returns>
        IObservable<KeyValuePair<string,float>> NamedFloatValue { get; }

        /// <summary>
        /// Gets an <see cref="IObservable{T}"/> sequence of key-value pairs representing
        /// the named integer values.
        /// </summary>
        /// <remarks>
        /// The key represents the name of the value, while the value represents the
        /// integer value itself.
        /// </remarks>
        /// <value>
        /// An <see cref="IObservable{T}"/> sequence of key-value pairs.
        /// </value>
        IObservable<KeyValuePair<string, int>> NamedIntValue { get; }

        /// <summary>
        /// Gets an observable sequence of DebugFloatArrayPayload.
        /// </summary>
        /// <value>
        /// The observable sequence of DebugFloatArrayPayload.
        /// </value>
        IObservable<DebugFloatArrayPayload> DebugFloatArray { get; }
    }
}
