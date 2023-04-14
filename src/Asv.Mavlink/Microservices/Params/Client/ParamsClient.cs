using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public class ParameterClientConfig
{
    public int ReadAttemptCount { get; set; } = 3;
    public int ReadTimeouMs { get; set; } = 1000;
}

public class ParamsClient : MavlinkMicroserviceClient, IParamsClient
{
    private readonly ParameterClientConfig _config;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly Subject<ParamValuePayload> _onParamValue;

    public ParamsClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq,ParameterClientConfig config, IScheduler scheduler) 
        : base("PARAMS", connection, identity, seq, scheduler)
    {
        _config = config;
        _onParamValue = new Subject<ParamValuePayload>().DisposeItWith(Disposable);
        InternalFilter<ParamValuePacket>().Select(_ => _.Payload).Subscribe(_onParamValue)
            .DisposeItWith(Disposable);
    }

    public IObservable<ParamValuePayload> OnParamValue => _onParamValue;
    
    public Task SendRequestList(CancellationToken cancel = default)
    {
        Logger.Info($"{LogSend} Request all params from vehicle");
        return InternalSend<ParamRequestListPacket>(_ =>
        {
            _.Payload.TargetComponent = Identity.TargetComponentId;
            _.Payload.TargetSystem = Identity.TargetSystemId;

        }, cancel);
    }

    public Task<ParamValuePayload> Read(string name, CancellationToken cancel = default)
    {
        return InternalCall<ParamValuePayload, ParamRequestReadPacket, ParamValuePacket>(_ =>
            {
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
                _.Payload.ParamIndex = -1;
                MavlinkTypesHelper.SetString(_.Payload.ParamId, name);
            }, _ => name.Equals(MavlinkTypesHelper.GetString(_.Payload.ParamId)),
            _ => _.Payload,
            _config.ReadAttemptCount, timeoutMs: _config.ReadTimeouMs, cancel: cancel);
    }
    public Task<ParamValuePayload> Read(ushort index, CancellationToken cancel = default)
    {
        return InternalCall<ParamValuePayload, ParamRequestReadPacket, ParamValuePacket>(_ =>
            {
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
                MavlinkTypesHelper.SetString(_.Payload.ParamId, string.Empty);
                _.Payload.ParamIndex = (short)index;
            }, _ => index == _.Payload.ParamIndex,
            _ => _.Payload,
            _config.ReadAttemptCount, timeoutMs: _config.ReadTimeouMs, cancel: cancel);
    }

    public Task Write(string name, MavParamType type, float value, CancellationToken cancel = default)
    {
        return InternalCall<ParamValuePayload, ParamRequestReadPacket, ParamValuePacket>(_ =>
            {
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.TargetSystem = Identity.TargetSystemId;
                MavlinkTypesHelper.SetString(_.Payload.ParamId, name);
            }, _ => name.Equals(MavlinkTypesHelper.GetString(_.Payload.ParamId)),
            _ => _.Payload,
            _config.ReadAttemptCount, timeoutMs: _config.ReadTimeouMs, cancel: cancel);
    }
}