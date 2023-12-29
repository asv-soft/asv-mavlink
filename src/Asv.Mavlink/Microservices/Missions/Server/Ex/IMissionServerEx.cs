using System;
using System.Collections.Generic;
using Asv.Common;
using DynamicData;

namespace Asv.Mavlink;

/// <summary>
/// Represents an extended mission server interface.
/// </summary>
public interface IMissionServerEx
{
   /// <summary>
   /// Gets the base IMissionServer property.
   /// </summary>
   /// <value>
   /// The base IMissionServer property.
   /// </value>
   IMissionServer Base { get; }

   /// <summary>
   /// Gets the current value of the editable value.
   /// </summary>
   /// <typeparam name="ushort">The type of the value.</typeparam>
   /// <returns>The current value of the editable value.</returns>
   IRxEditableValue<ushort> Current { get; }

   /// <summary>
   /// Gets the editable value that represents whether a certain condition has been reached.
   /// </summary>
   /// <typeparam name="ushort">The type of the value.</typeparam>
   /// <returns>The instance of the <see cref="IRxEditableValue{T}"/> that represents whether a certain condition has been reached.</returns>
   IRxEditableValue<ushort> Reached { get; }

   /// <summary>
   /// Gets an observable collection of server mission items with their respective ushort identifiers.
   /// </summary>
   /// <returns>
   /// An <see cref="IObservable{IChangeSet{ServerMissionItem, ushort}}"/> representing the collection of server mission items.
   /// </returns>
   IObservable<IChangeSet<ServerMissionItem,ushort>> Items { get; }
   void AddItems(IEnumerable<ServerMissionItem> items);
   void RemoveItems(IEnumerable<ServerMissionItem> items);
}