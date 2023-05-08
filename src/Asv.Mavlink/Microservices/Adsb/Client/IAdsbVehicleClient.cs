using System;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IAdsbVehicleClient : IDisposable
{
    ushort FullId { get; }
    RxValue<AdsbVehiclePayload> Target { get; }
}