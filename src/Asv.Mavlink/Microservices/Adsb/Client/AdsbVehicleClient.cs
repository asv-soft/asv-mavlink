#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public class AdsbVehicleClientConfig
{
    public int TargetTimeoutMs { get; set; } = 10_000;
    public int CheckOldDevicesMs { get; set; } = 3_000;
}

public class AdsbVehicleClient : MavlinkMicroserviceClient, IAdsbVehicleClient
{
    private readonly Subject<AdsbVehiclePayload> _onAdsbTarget;
    private readonly ObservableDictionary<uint,IAdsbVehicle> _targetSource;
    private readonly ReactiveProperty<TimeSpan> _targetTimeout;

    public AdsbVehicleClient(MavlinkClientIdentity identity, AdsbVehicleClientConfig config,IMavlinkContext core) 
        : base(AdsbHelper.MicroserviceName, identity, core)
    {
        _onAdsbTarget = new Subject<AdsbVehiclePayload>();
        _sub1 = InternalFilter<AdsbVehiclePacket>()
            .Select(p => p.Payload)
            .Subscribe(_onAdsbTarget.AsObserver());

        _targetTimeout =
            new ReactiveProperty<TimeSpan>(TimeSpan.FromMilliseconds(config.TargetTimeoutMs));
        _targetSource = new ObservableDictionary<uint,IAdsbVehicle>();
        _sub2 =_onAdsbTarget.Subscribe(UpdateTarget);
        _sub3  = core.TimeProvider.CreateTimer(DeleteOldTargets, null,
            TimeSpan.FromMilliseconds(config.CheckOldDevicesMs), TimeSpan.FromMilliseconds(config.CheckOldDevicesMs));
    }

    private void DeleteOldTargets(object? state)
    {
        if (_targetSource.Count == 0) return;
        var itemsToDelete = _targetSource.Select(x=>(AdsbVehicle)x.Value)
            .Where(device => Core.TimeProvider.GetElapsedTime(device.GetLastHit()) >= _targetTimeout.Value)
            .ToList();
        
        foreach (var item in itemsToDelete)
        {
            item.Dispose();
            _targetSource.Remove(item.IcaoAddress);
        }
    }

    private void UpdateTarget(AdsbVehiclePayload payload)
    {
        if (_targetSource.TryGetValue(payload.IcaoAddress, out var item))
        {
            ((AdsbVehicle)item).InternalUpdate(payload, Core.TimeProvider.GetTimestamp());
        }
        else
        {
            _targetSource[payload.IcaoAddress] = new AdsbVehicle(payload, Core.TimeProvider.GetTimestamp());
        }
    }

    public Observable<AdsbVehiclePayload> OnTarget => _onAdsbTarget;
    public IReadOnlyObservableDictionary<uint,IAdsbVehicle> Targets=> _targetSource;
    public ReactiveProperty<TimeSpan> TargetTimeout => _targetTimeout;

    #region Dispose

    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly ITimer _sub3;
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _onAdsbTarget.Dispose();
            _targetTimeout.Dispose();
            _sub1.Dispose();
            _sub2.Dispose();
            _sub3.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_onAdsbTarget).ConfigureAwait(false);
        await CastAndDispose(_targetTimeout).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await _sub3.DisposeAsync().ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

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