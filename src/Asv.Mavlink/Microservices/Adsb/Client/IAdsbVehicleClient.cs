using System;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface IAdsbVehicleClient : IDisposable
{
    IObservable<AdsbVehiclePayload> OnTarget { get; }
    IObservable<IChangeSet<IAdsbVehicle, uint>> Targets { get; }
    IRxEditableValue<TimeSpan> TargetTimeout { get; }
}

public interface IAdsbVehicle
{
    uint IcaoAddress { get; }
    IRxValue<string> CallSign { get; }
    IRxValue<GeoPoint> Location { get; }
    /// <summary>
    /// ADSB altitude type.
    /// </summary>
    IRxValue<AdsbAltitudeType> AltitudeType { get; }
    IRxValue<double> Heading { get; }
    IRxValue<AdsbEmitterType> EmitterType { get; }
    /// <summary>
    /// Time since last communication
    /// </summary>
    IRxValue<TimeSpan> Tslc { get; }
    IRxValue<AdsbFlags> Flags { get; }
    IRxValue<double> HorVelocity { get; }
    IRxValue<double> VerVelocity { get; }
}

