using System;
using System.Collections.ObjectModel;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface IAdsbVehicleClientEx : IDisposable
{
    ReadOnlyObservableCollection<AdsbVehiclePayload> Targets { get; }
}