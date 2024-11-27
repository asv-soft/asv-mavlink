using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

/// <summary>
/// Represents a telemetry client for communicating with a MAVLink microservice.
/// </summary>
public sealed class TelemetryClient : MavlinkMicroserviceClient, ITelemetryClient
{
    private readonly ILogger _logger;
    private readonly ReactiveProperty<RadioStatusPayload?> _radio;
    private readonly ReactiveProperty<SysStatusPayload?> _systemStatus;
    private readonly ReactiveProperty<ExtendedSysStatePayload?> _extendedSystemState;
    private readonly ReactiveProperty<BatteryStatusPayload?> _battery;
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly IDisposable _sub3;
    private readonly IDisposable _sub4;


    public TelemetryClient(MavlinkClientIdentity identity, ICoreServices core)
        : base("RTT", identity, core)
    {
        _logger = core.Log.CreateLogger<TelemetryClient>();
        _radio = new ReactiveProperty<RadioStatusPayload?>();
        _sub1 = InternalFilter<RadioStatusPacket>().Select(p=>p?.Payload).Subscribe(_radio.AsObserver());
        _systemStatus = new ReactiveProperty<SysStatusPayload?>();
        _sub2 = InternalFilter<SysStatusPacket>().Select(p => p?.Payload).Subscribe(_systemStatus.AsObserver());
        _extendedSystemState = new ReactiveProperty<ExtendedSysStatePayload?>();
        _sub3 = InternalFilter<ExtendedSysStatePacket>().Select(p => p?.Payload).Subscribe(_extendedSystemState.AsObserver());
        _battery = new ReactiveProperty<BatteryStatusPayload?>();
        _sub4 = InternalFilter<BatteryStatusPacket>().Select(p => p?.Payload).Subscribe(_battery.AsObserver());
    }
    public ReadOnlyReactiveProperty<RadioStatusPayload?> Radio => _radio;
    public ReadOnlyReactiveProperty<SysStatusPayload?> SystemStatus => _systemStatus;
    public ReadOnlyReactiveProperty<ExtendedSysStatePayload?> ExtendedSystemState => _extendedSystemState;
    public ReadOnlyReactiveProperty<BatteryStatusPayload?> Battery => _battery;
    public ValueTask RequestDataStream(byte streamId, ushort rateHz, bool startStop, CancellationToken cancel = default)
    {
        _logger.ZLogDebug($"{LogSend} {( startStop ? "Enable stream":"DisableStream")} with ID '{streamId}' and rate {rateHz} Hz");
        return InternalSend<RequestDataStreamPacket>(p =>
        {
            p.Payload.TargetSystem = Identity.Target.SystemId;
            p.Payload.TargetComponent = Identity.Target.ComponentId;
            p.Payload.ReqMessageRate = rateHz;
            p.Payload.StartStop = (byte)(startStop ? 1 : 0);
            p.Payload.ReqStreamId = streamId;
        }, cancel);
    }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _radio.Dispose();
            _systemStatus.Dispose();
            _extendedSystemState.Dispose();
            _battery.Dispose();
            _sub1.Dispose();
            _sub2.Dispose();
            _sub3.Dispose();
            _sub4.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_radio).ConfigureAwait(false);
        await CastAndDispose(_systemStatus).ConfigureAwait(false);
        await CastAndDispose(_extendedSystemState).ConfigureAwait(false);
        await CastAndDispose(_battery).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await CastAndDispose(_sub3).ConfigureAwait(false);
        await CastAndDispose(_sub4).ConfigureAwait(false);

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