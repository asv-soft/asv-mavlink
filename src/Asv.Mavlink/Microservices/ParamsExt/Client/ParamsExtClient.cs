using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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

    /// <summary>
    /// Represents a client for interacting with the ParametersExtended service.
    /// </summary>
    /// <remarks>
    /// This client provides methods for reading and writing parameters.
    /// It communicates with the service using the MAVLink protocol.
    /// </remarks>
    public ParamsExtClient(
        IMavlinkV2Connection connection, 
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq, 
        ParamsExtClientConfig config,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? logFactory = null)
        : base("PARAMS_EXT", connection, identity, seq, timeProvider,scheduler,logFactory)
    {
        ArgumentNullException.ThrowIfNull(seq);
        ArgumentNullException.ThrowIfNull(identity);
        ArgumentNullException.ThrowIfNull(connection);
        logFactory??=NullLoggerFactory.Instance;
        _logger = logFactory.CreateLogger<ParamsExtClient>();
        _config = config ?? throw new ArgumentNullException(nameof(config));
        
        _onParamExtValue = new Subject<ParamExtValuePayload>().DisposeItWith(Disposable);
        InternalFilter<ParamExtValuePacket>().Select(p => p.Payload).Subscribe(_onParamExtValue)
            .DisposeItWith(Disposable);
        
        _onParamExtAck = new Subject<ParamExtAckPayload>().DisposeItWith(Disposable);
        InternalFilter<ParamExtAckPacket>().Select(p => p.Payload).Subscribe(_onParamExtAck)
            .DisposeItWith(Disposable);
    }

    public IObservable<ParamExtValuePayload> OnParamExtValue => _onParamExtValue;
    
    public IObservable<ParamExtAckPayload> OnParamExtAck => _onParamExtAck;

    public Task SendRequestList(CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{LogSend} Attempt to read all params");
        return InternalSend<ParamExtRequestListPacket>(packet =>
        {
            packet.Payload.TargetComponent = Identity.TargetComponentId;
            packet.Payload.TargetSystem = Identity.TargetSystemId;
        }, cancel);
    }

    public Task<ParamExtValuePayload> Read(string name, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{LogSend} Attempt to read single param by id: {name}");
        return InternalCall<ParamExtValuePayload, ParamRequestReadPacket, ParamExtValuePacket>(packet =>
            {
                packet.Payload.TargetComponent = Identity.TargetComponentId;
                packet.Payload.TargetSystem = Identity.TargetSystemId;
                packet.Payload.ParamIndex = -1;
                MavlinkTypesHelper.SetString(packet.Payload.ParamId, name);
            }, packet => name.Equals(MavlinkTypesHelper.GetString(packet.Payload.ParamId)),
            packet => packet.Payload, _config.ReadAttemptCount,
            timeoutMs: _config.ReadTimeouMs, cancel: cancel);
    }

    public Task<ParamExtValuePayload> Read(ushort index, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{LogSend} Attempt to read single param by index: {index}");
        return InternalCall<ParamExtValuePayload, ParamExtRequestReadPacket, ParamExtValuePacket>(packet =>
            {
                packet.Payload.TargetComponent = Identity.TargetComponentId;
                packet.Payload.TargetSystem = Identity.TargetSystemId;
                MavlinkTypesHelper.SetString(packet.Payload.ParamId, string.Empty);
                packet.Payload.ParamIndex = (short)index;
            }, packet => index == packet.Payload.ParamIndex,
            packet => packet.Payload, _config.ReadAttemptCount, 
            timeoutMs: _config.ReadTimeouMs, cancel: cancel);
    }
    
    public Task<ParamExtAckPayload> Write(string name, MavParamExtType type, char[] value,
        CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{LogSend} Attempt to write single param by id: {name} with value: {string.Join(", ", value)}");
        return InternalCall<ParamExtAckPayload, ParamExtSetPacket, ParamExtAckPacket>(packet =>
            {
                packet.Payload.TargetComponent = Identity.TargetComponentId;
                packet.Payload.TargetSystem = Identity.TargetSystemId;
                packet.Payload.ParamType = type;
                MavlinkTypesHelper.SetString(packet.Payload.ParamId, name);
                packet.Payload.ParamValue = value;
            }, packet => name.Equals(MavlinkTypesHelper.GetString(packet.Payload.ParamId)),
            packet => packet.Payload, _config.ReadAttemptCount,
            timeoutMs: _config.ReadTimeouMs, cancel: cancel);
    }
}