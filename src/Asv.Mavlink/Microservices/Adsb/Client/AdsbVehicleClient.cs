#nullable enable
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using DynamicData;
using Microsoft.Extensions.Logging;
using R3;

namespace Asv.Mavlink;

public class AdsbVehicleClientConfig
{
    public int TargetTimeoutMs { get; set; } = 10_000;
    public int CheckOldDevicesMs { get; set; } = 3_000;
}

public class AdsbVehicleClient : MavlinkMicroserviceClient, IAdsbVehicleClient
{
    private readonly System.Reactive.Subjects.Subject<AdsbVehiclePayload> _onAdsbTarget;
    private readonly SourceCache<AdsbVehicle, uint> _targetSource;
    private readonly RxValue<TimeSpan> _targetTimeout;
    private readonly IDisposable _disposeIt;


    public AdsbVehicleClient(MavlinkClientIdentity identity, AdsbVehicleClientConfig config,ICoreServices core) 
        : base("ADSB", identity, core)
    {
        _onAdsbTarget = new System.Reactive.Subjects.Subject<AdsbVehiclePayload>();
        var d1 = InternalFilter<AdsbVehiclePacket>()
            .Select(p => p.Payload)
            .Subscribe(_onAdsbTarget);

        _targetTimeout =
            new RxValue<TimeSpan>(TimeSpan.FromMilliseconds(config.TargetTimeoutMs));
        _targetSource = new SourceCache<AdsbVehicle, uint>(v => v.IcaoAddress);
        Targets = _targetSource.Connect().Transform(v => (IAdsbVehicle)v);
        var d2 =_onAdsbTarget.Subscribe(UpdateTarget);
        var d3  = core.TimeProvider.CreateTimer(DeleteOldTargets, null,
            TimeSpan.FromMilliseconds(config.CheckOldDevicesMs), TimeSpan.FromMilliseconds(config.CheckOldDevicesMs));
        _disposeIt = Disposable.Combine(_onAdsbTarget, _targetTimeout, _targetSource, d1, d2, d3);

    }

    private void DeleteOldTargets(object? state)
    {
        if (_targetSource.Count == 0) return;
        _targetSource.Edit(update =>
        {
            var itemsToDelete = update.Items.Where(device => Core.TimeProvider.GetElapsedTime(device.GetLastHit()) > _targetTimeout.Value)
                .ToList();
            foreach (var item in itemsToDelete)
            {
                item.Dispose();
            }

            update.RemoveKeys(itemsToDelete.Select(device => device.IcaoAddress));
        });
    }

    private void UpdateTarget(AdsbVehiclePayload payload)
    {
        _targetSource.Edit(u =>
        {
            var lookup = u.Lookup(payload.IcaoAddress);
            if (lookup.HasValue)
            {
                lookup.Value.InternalUpdate(payload, Core.TimeProvider.GetTimestamp());
            }
            else
            {
                u.AddOrUpdate(new AdsbVehicle(payload,Core.TimeProvider.GetTimestamp()));
            }
        });
    }

    public IObservable<AdsbVehiclePayload> OnTarget => _onAdsbTarget;
    public IObservable<IChangeSet<IAdsbVehicle, uint>> Targets { get; }
    public IRxEditableValue<TimeSpan> TargetTimeout => _targetTimeout;

    public override void Dispose()
    {
        _disposeIt.Dispose();
        base.Dispose();
    }
}

public class AdsbVehicle : DisposableOnceWithCancel, IAdsbVehicle
{
    private long _lastHit;
    private readonly RxValue<string> _callSign;
    private readonly RxValue<GeoPoint> _location;
    private readonly RxValue<double> _heading;
    private readonly RxValue<AdsbEmitterType> _emitterType;
    private readonly RxValue<AdsbAltitudeType> _altitudeType;
    private readonly RxValue<TimeSpan> _tslc;
    private readonly uint _icaoAddress;
    private readonly RxValue<AdsbFlags> _flags;
    private readonly RxValue<double> _horVelocity;
    private readonly RxValue<double> _verVelocity;
    private readonly RxValue<ushort> _squawk;

    public AdsbVehicle(AdsbVehiclePayload payload, long currentTime)
    {
        if (payload == null) throw new ArgumentNullException(nameof(payload));
        
        _icaoAddress = payload.IcaoAddress;
        _heading = new RxValue<double>().DisposeItWith(Disposable);
        _emitterType = new RxValue<AdsbEmitterType>().DisposeItWith(Disposable);
        _altitudeType = new RxValue<AdsbAltitudeType>().DisposeItWith(Disposable);
        _tslc = new RxValue<TimeSpan>().DisposeItWith(Disposable);
        _horVelocity = new RxValue<double>().DisposeItWith(Disposable);
        _flags = new RxValue<AdsbFlags>().DisposeItWith(Disposable);
        _verVelocity = new RxValue<double>().DisposeItWith(Disposable);
        _callSign = new RxValue<string>().DisposeItWith(Disposable);
        _location = new RxValue<GeoPoint>().DisposeItWith(Disposable);
        _squawk = new RxValue<ushort>().DisposeItWith(Disposable);
        InternalUpdate(payload,currentTime);
    }
    
    public uint IcaoAddress => _icaoAddress;
    public IRxValue<GeoPoint> Location => _location;
    public IRxValue<AdsbAltitudeType> AltitudeType => _altitudeType;
    public IRxValue<double> Heading => _heading;
    public IRxValue<double> HorVelocity => _horVelocity;
    public IRxValue<double> VerVelocity => _verVelocity;
    public IRxValue<AdsbFlags> Flags => _flags;
    public IRxValue<ushort> Squawk => _squawk;
    public IRxValue<string> CallSign => _callSign;
    public IRxValue<AdsbEmitterType> EmitterType => _emitterType;
    public IRxValue<TimeSpan> Tslc => _tslc;
    public long GetLastHit()
    {
        return Interlocked.CompareExchange(ref _lastHit, 0, 0);
    }
    public void InternalUpdate(AdsbVehiclePayload payload, long dateTime)
    {
        if (IcaoAddress != payload.IcaoAddress) throw new InvalidOperationException("IcaoAddress not equal");
        _callSign.OnNext(MavlinkTypesHelper.GetString(payload.Callsign));
        _location.OnNext(new GeoPoint(payload.Lat * 1e-7, payload.Lon * 1e-7, payload.Altitude * 1e-3 ));
        _heading.OnNext(payload.Heading * 1e-2);
        _emitterType.OnNext(payload.EmitterType);
        _altitudeType.OnNext(payload.AltitudeType);
        _tslc.OnNext(TimeSpan.FromSeconds(payload.Tslc));
        _horVelocity.OnNext(payload.HorVelocity * 1e-2);
        _verVelocity.OnNext(payload.VerVelocity * 1e-2);
        _flags.OnNext(payload.Flags);
        _squawk.OnNext(payload.Squawk);
        Interlocked.Exchange(ref _lastHit, dateTime);
    }
    
     
}