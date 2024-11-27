using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class ParameterClientConfig
{
    public int ReadAttemptCount { get; set; } = 6;
    public int ReadTimeouMs { get; set; } = 1000;
}

public class ParamsClient : MavlinkMicroserviceClient, IParamsClient
{
    private readonly ParameterClientConfig _config;
    private readonly ILogger _logger;
    private readonly Subject<ParamValuePayload> _onParamValue;
    private readonly IDisposable _sub1;

    public ParamsClient(MavlinkClientIdentity identity, ParameterClientConfig config, ICoreServices core) 
        : base("PARAMS",  identity, core)
    {
        _logger = core.Log.CreateLogger<ParamsClient>();
        _config = config ?? throw new ArgumentNullException(nameof(config));;
        _onParamValue = new Subject<ParamValuePayload>();
        _sub1 = InternalFilter<ParamValuePacket>().Select(p => p.Payload).Subscribe(_onParamValue.AsObserver());
    }

    public Observable<ParamValuePayload> OnParamValue => _onParamValue;
    
    public Task SendRequestList(CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{LogSend} Request all params from vehicle");
        return InternalSend<ParamRequestListPacket>(p =>
        {
            p.Payload.TargetComponent = Identity.Target.ComponentId;
            p.Payload.TargetSystem = Identity.Target.SystemId;
        }, cancel);
    }

    public Task<ParamValuePayload> Read(string name, CancellationToken cancel = default)
    {
        return InternalCall<ParamValuePayload, ParamRequestReadPacket, ParamValuePacket>(p =>
            {
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
                p.Payload.ParamIndex = -1;
                MavlinkTypesHelper.SetString(p.Payload.ParamId, name);
            }, p => name.Equals(MavlinkTypesHelper.GetString(p.Payload.ParamId)),
            p => p.Payload,
            _config.ReadAttemptCount, timeoutMs: _config.ReadTimeouMs, cancel: cancel);
    }
    
    public Task<ParamValuePayload> Read(ushort index, CancellationToken cancel = default)
    {
        return InternalCall<ParamValuePayload, ParamRequestReadPacket, ParamValuePacket>(p =>
            {
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
                MavlinkTypesHelper.SetString(p.Payload.ParamId, string.Empty);
                p.Payload.ParamIndex = (short)index;
            }, p => index == p.Payload.ParamIndex,
            p => p.Payload,
            _config.ReadAttemptCount, timeoutMs: _config.ReadTimeouMs, cancel: cancel);
    }

    public Task<ParamValuePayload> Write(string name, MavParamType type, float value, CancellationToken cancel = default)
    {
        return InternalCall<ParamValuePayload, ParamSetPacket, ParamValuePacket>(p =>
            {
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
                p.Payload.ParamType = type;
                p.Payload.ParamValue = value;
                MavlinkTypesHelper.SetString(p.Payload.ParamId, name);
            }, p => name.Equals(MavlinkTypesHelper.GetString(p.Payload.ParamId)),
            p => p.Payload,
            _config.ReadAttemptCount, timeoutMs: _config.ReadTimeouMs, cancel: cancel);
    }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _onParamValue.Dispose();
            _sub1.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_onParamValue).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);

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