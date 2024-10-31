using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Asv.Common;
using ObservableCollections;
using R3;

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
   ReactiveProperty<ushort> Current { get; }

   /// <summary>
   /// Gets the editable value that represents whether a certain condition has been reached.
   /// </summary>
   /// <typeparam name="ushort">The type of the value.</typeparam>
   /// <returns>The instance of the <see cref="IRxEditableValue{T}"/> that represents whether a certain condition has been reached.</returns>
   ReactiveProperty<ushort> Reached { get; }

   /// <summary>
   /// Gets an observable collection of server mission items with their respective ushort identifiers.
   /// </summary>
   IReadOnlyObservableList<ServerMissionItem> Items { get; }
   void AddItems(IEnumerable<ServerMissionItem> items);
   void RemoveItems(IEnumerable<ServerMissionItem> items);

   /// <summary>
   /// Retrieves a snapshot of the mission items.
   /// </summary>
   /// <returns>An array of ServerMissionItem representing the snapshot of the mission items.</returns>
   ImmutableArray<ServerMissionItem> GetItemsSnapshot();
}