using System;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IAdsbVehicleServer : IDisposable
{
    void Start();
    void Set(Action<AdsbVehiclePayload> changeCallback);
}