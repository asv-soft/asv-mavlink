using System;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IAdsbVehicleServer : IDisposable
{
    Task Send(Action<AdsbVehiclePayload> fillCallback);
}