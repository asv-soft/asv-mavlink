using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

/// <summary>
/// Represents the configuration for the ParametersExtendedClient.
/// </summary>
/// <remarks>
/// This class holds the configuration settings for the ParametersExtendedClient class.
/// It specifies the number of read attempts and the timeout duration for reading parameters.
/// </remarks>
public class ParamsExtClientConfig
{
    public int ReadAttemptCount { get; set; } = 3;
    public int ReadTimeouMs { get; set; } = 1000;
}

/// <summary>
/// Represents a client for interacting with the ParametersExtended service.
/// </summary>
/// <remarks>
/// This client provides methods for reading and writing parameters.
/// It communicates with the service using the MAVLink protocol.
/// </remarks>
public class ParamsExtClient : MavlinkMicroserviceClient, IParamsExtClient
{
    

    private readonly ILogger _logger;

    private readonly ParamsExtClientConfig _config;
    private readonly Subject<ParamExtValuePayload> _onParamExtValue;
    private readonly Subject<ParamExtAckPayload> _onParamExtAck;
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;

    public ParamsExtClient(MavlinkClientIdentity identity, ParamsExtClientConfig config,
        IMavlinkContext core)
        : base("PARAMS_EXT", identity, core)
    {
        _logger = core.LoggerFactory.CreateLogger<ParamsExtClient>();
        _config = config ?? throw new ArgumentNullException(nameof(config));

        _onParamExtValue = new Subject<ParamExtValuePayload>();
        _sub1 = InternalFilter<ParamExtValuePacket>().Select(p => p.Payload).Subscribe(_onParamExtValue.AsObserver());
        
        _onParamExtAck = new Subject<ParamExtAckPayload>();
        _sub2 = InternalFilter<ParamExtAckPacket>().Select(p => p.Payload).Subscribe(_onParamExtAck.AsObserver());
    }

    public Observable<ParamExtValuePayload> OnParamExtValue => _onParamExtValue;
    
    public Observable<ParamExtAckPayload> OnParamExtAck => _onParamExtAck;

    public ValueTask SendRequestList(CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{Id} Attempt to read all params");
        return InternalSend<ParamExtRequestListPacket>(packet =>
        {
            packet.Payload.TargetComponent = Identity.Target.ComponentId;
            packet.Payload.TargetSystem = Identity.Target.SystemId;
        }, cancel);
    }

    public async Task<ParamExtValuePayload> Read(string name, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{Id} Attempt to read single param by id: {name}");
        return await InternalCall<ParamExtValuePayload, ParamExtRequestReadPacket, ParamExtValuePacket>(packet =>
            {
                packet.Payload.TargetComponent = Identity.Target.ComponentId;
                packet.Payload.TargetSystem = Identity.Target.SystemId;
                packet.Payload.ParamIndex = -1;
                MavlinkTypesHelper.SetString(packet.Payload.ParamId, name);
            }, packet => name.Equals(MavlinkTypesHelper.GetString(packet.Payload.ParamId)),
            packet => packet.Payload, _config.ReadAttemptCount,
            timeoutMs: _config.ReadTimeouMs, cancel: cancel).ConfigureAwait(false);
    }

    public async Task<ParamExtValuePayload> Read(ushort index, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{Id} Attempt to read single param by index: {index}");
        return await InternalCall<ParamExtValuePayload, ParamExtRequestReadPacket, ParamExtValuePacket>(packet =>
            {
                packet.Payload.TargetComponent = Identity.Target.ComponentId;
                packet.Payload.TargetSystem = Identity.Target.SystemId;
                MavlinkTypesHelper.SetString(packet.Payload.ParamId, string.Empty);
                packet.Payload.ParamIndex = (short)index;
            }, packet => index == packet.Payload.ParamIndex,
            packet => packet.Payload, _config.ReadAttemptCount, 
            timeoutMs: _config.ReadTimeouMs, cancel: cancel).ConfigureAwait(false);
    }
    
    public async Task<ParamExtAckPayload> Write(string name, MavParamExtType type, char[] value,
        CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{Id} Attempt to write single param by id: {name} with value: {string.Join(", ", value)}");
        return await InternalCall<ParamExtAckPayload, ParamExtSetPacket, ParamExtAckPacket>(packet =>
            {
                packet.Payload.TargetComponent = Identity.Target.ComponentId;
                packet.Payload.TargetSystem = Identity.Target.SystemId;
                packet.Payload.ParamType = type;
                MavlinkTypesHelper.SetString(packet.Payload.ParamId, name);
                packet.Payload.ParamValue = value;
            }, packet => name.Equals(MavlinkTypesHelper.GetString(packet.Payload.ParamId)),
            packet => packet.Payload, _config.ReadAttemptCount,
            timeoutMs: _config.ReadTimeouMs, cancel: cancel).ConfigureAwait(false);
    }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _onParamExtValue.Dispose();
            _onParamExtAck.Dispose();
            _sub1.Dispose();
            _sub2.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_onParamExtValue).ConfigureAwait(false);
        await CastAndDispose(_onParamExtAck).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);

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