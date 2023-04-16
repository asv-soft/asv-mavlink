using System;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IGnssStatusClient
{
    IRxValue<double> GroundVelocity { get; }
    IRxValue<GpsInfo> Info { get; }
    IRxValue<GeoPoint> Position { get; }
}

public class GnssStatusClient : DisposableOnceWithCancel, IGnssStatusClient
{
    private readonly RxValue<double> _groundVelocity;
    private readonly RxValue<GpsInfo> _info;
    private readonly RxValue<GeoPoint> _position;

    public GnssStatusClient(IObservable<GpsRawIntPayload> pipe)
    {
        _info = new RxValue<GpsInfo>(null).DisposeItWith(Disposable);
        pipe.Select(_ => new GpsInfo(_)).Subscribe(_info).DisposeItWith(Disposable);
        _groundVelocity = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        pipe.Select(_ => _.Vel / 100D).Subscribe(_groundVelocity).DisposeItWith(Disposable);
        _position = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
        pipe.Select(_ => new GeoPoint(_.Lat / 10000000D, _.Lon / 10000000D, _.Alt / 1000D)).Subscribe(_position).DisposeItWith(Disposable);
    }
    
    public GnssStatusClient(IObservable<Gps2RawPayload> pipe)
    {
        _info = new RxValue<GpsInfo>(null).DisposeItWith(Disposable);
        pipe.Select(_ => new GpsInfo(_)).Subscribe(_info).DisposeItWith(Disposable);
        _groundVelocity = new RxValue<double>(Double.NaN).DisposeItWith(Disposable);
        pipe.Select(_ => _.Vel / 100D).Subscribe(_groundVelocity).DisposeItWith(Disposable);
        _position = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
        pipe.Select(_ => new GeoPoint(_.Lat / 10000000D, _.Lon / 10000000D, _.Alt / 1000D)).Subscribe(_position).DisposeItWith(Disposable);
    }


    public IRxValue<double> GroundVelocity => _groundVelocity;

    public IRxValue<GpsInfo> Info => _info;

    public IRxValue<GeoPoint> Position => _position;
}