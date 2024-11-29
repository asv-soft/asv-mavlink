using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
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

    public TelemetryClient(MavlinkClientIdentity identity, ICoreServices core)
        : base("RTT", identity, core)
    {
        _logger = core.Log.CreateLogger<TelemetryClient>(); 
        Radio = InternalFilter<RadioStatusPacket>().Select(p=>p?.Payload)
            .ToReadOnlyReactiveProperty();
        SystemStatus  = InternalFilter<SysStatusPacket>().Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        ExtendedSystemState = InternalFilter<ExtendedSysStatePacket>().Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
        Battery = InternalFilter<BatteryStatusPacket>().Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
    }
    
    public ReadOnlyReactiveProperty<RadioStatusPayload?> Radio { get; }
    public ReadOnlyReactiveProperty<SysStatusPayload?> SystemStatus { get; }
    public ReadOnlyReactiveProperty<ExtendedSysStatePayload?> ExtendedSystemState { get; }
    public ReadOnlyReactiveProperty<BatteryStatusPayload?> Battery { get; }
    
    public Task RequestDataStream(byte streamId, ushort rateHz, bool startStop, CancellationToken cancel = default)
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

    protected override async ValueTask DisposeAsyncCore()
    {
        await base.DisposeAsyncCore().ConfigureAwait(false);
    }

    #endregion
}