using System;
using Asv.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface IMissionServerEx
{
   IMissionServer Base { get; }
   IRxEditableValue<ushort> Current { get; }
   IRxEditableValue<ushort> Reached { get; }
   IObservable<IChangeSet<ServerMissionItem,ushort>> Items { get; }
}