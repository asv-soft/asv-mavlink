using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using R3;

namespace Asv.Mavlink;

public class AdsbVehicle : IAdsbVehicle, IDisposable, IAsyncDisposable
{
    private long _lastHit;
    private readonly ReactiveProperty<string> _callSign;
    private readonly ReactiveProperty<GeoPoint> _location;
    private readonly ReactiveProperty<double> _heading;
    private readonly ReactiveProperty<AdsbEmitterType> _emitterType;
    private readonly ReactiveProperty<AdsbAltitudeType> _altitudeType;
    private readonly ReactiveProperty<TimeSpan> _tslc;
    private readonly ReactiveProperty<AdsbFlags> _flags;
    private readonly ReactiveProperty<double> _horVelocity;
    private readonly ReactiveProperty<double> _verVelocity;
    private readonly ReactiveProperty<ushort> _squawk;

    public AdsbVehicle(AdsbVehiclePayload payload, long currentTime)
    {
        ArgumentNullException.ThrowIfNull(payload);

        IcaoAddress = payload.IcaoAddress;
        _heading = new ReactiveProperty<double>();
        _emitterType = new ReactiveProperty<AdsbEmitterType>();
        _altitudeType = new ReactiveProperty<AdsbAltitudeType>();
        _tslc = new ReactiveProperty<TimeSpan>();
        _horVelocity = new ReactiveProperty<double>();
        _flags = new ReactiveProperty<AdsbFlags>();
        _verVelocity = new ReactiveProperty<double>();
        _callSign = new ReactiveProperty<string>();
        _location = new ReactiveProperty<GeoPoint>();
        _squawk = new ReactiveProperty<ushort>();
        InternalUpdate(payload,currentTime);
    }
    
    public uint IcaoAddress { get; }

    public ReadOnlyReactiveProperty<GeoPoint> Location => _location;
    public ReadOnlyReactiveProperty<AdsbAltitudeType> AltitudeType => _altitudeType;
    public ReadOnlyReactiveProperty<double> Heading => _heading;
    public ReadOnlyReactiveProperty<double> HorVelocity => _horVelocity;
    public ReadOnlyReactiveProperty<double> VerVelocity => _verVelocity;
    public ReadOnlyReactiveProperty<AdsbFlags> Flags => _flags;
    public ReadOnlyReactiveProperty<ushort> Squawk => _squawk;
    public ReadOnlyReactiveProperty<string> CallSign => _callSign;
    public ReadOnlyReactiveProperty<AdsbEmitterType> EmitterType => _emitterType;
    public ReadOnlyReactiveProperty<TimeSpan> Tslc => _tslc;
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


    #region Dispose

    public void Dispose()
    {
        _callSign.Dispose();
        _location.Dispose();
        _heading.Dispose();
        _emitterType.Dispose();
        _altitudeType.Dispose();
        _tslc.Dispose();
        _flags.Dispose();
        _horVelocity.Dispose();
        _verVelocity.Dispose();
        _squawk.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_callSign).ConfigureAwait(false);
        await CastAndDispose(_location).ConfigureAwait(false);
        await CastAndDispose(_heading).ConfigureAwait(false);
        await CastAndDispose(_emitterType).ConfigureAwait(false);
        await CastAndDispose(_altitudeType).ConfigureAwait(false);
        await CastAndDispose(_tslc).ConfigureAwait(false);
        await CastAndDispose(_flags).ConfigureAwait(false);
        await CastAndDispose(_horVelocity).ConfigureAwait(false);
        await CastAndDispose(_verVelocity).ConfigureAwait(false);
        await CastAndDispose(_squawk).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}