using System;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface IMissionServer
{
   IRxEditableValue<ushort> Current { get; }
   IObservable<IChangeSet<MissionItemIntPayload,ushort>> Items { get; }
   void SendReached(ushort seq);
}