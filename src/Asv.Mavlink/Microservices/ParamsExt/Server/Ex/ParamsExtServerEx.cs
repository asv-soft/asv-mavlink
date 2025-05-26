using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class ParamsExtServerExConfig
{
    public int SendingParamItemDelayMs { get; set; } = 10;
    public string CfgPrefix { get; set; } = "MAV_CFG_";
}

public sealed class ParamsExtServerEx : MavlinkMicroserviceServer, IParamsExtServerEx
{
    private readonly ILogger _logger;

    private readonly Subject<Exception> _onErrorSubject;
    private int _sendingInProgressFlag;
    private readonly ImmutableDictionary<string, (ushort, IMavParamExtTypeMetadata)> _paramDict;
    private readonly ImmutableList<IMavParamExtTypeMetadata> _paramList;
    private readonly Subject<ParamExtChangedEvent> _onParamChangedSubject;

    private readonly IParamsExtServer _server;
    private readonly IStatusTextServer _statusTextServer;
    private readonly IConfiguration _cfg;
    private readonly ParamsExtServerExConfig _serverCfg;
    
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly IDisposable _sub3;

    public ParamsExtServerEx(
        IParamsExtServer server, 
        IStatusTextServer statusTextServer,
        IEnumerable<IMavParamExtTypeMetadata> paramDescriptions, 
        IConfiguration cfg,
        ParamsExtServerExConfig serverCfg) : base(ParamsExtHelper.MicroserviceExName, server.Identity, server.Core)
    {
        _logger = server.Core.LoggerFactory.CreateLogger<ParamsExtServer>();
        _server = server ?? throw new ArgumentNullException(nameof(server));
        _statusTextServer = statusTextServer ?? throw new ArgumentNullException(nameof(statusTextServer));
        _cfg = cfg ?? throw new ArgumentNullException(nameof(cfg));
        _serverCfg = serverCfg ?? throw new ArgumentNullException(nameof(serverCfg));

        _onErrorSubject = new Subject<Exception>();
        _paramList = paramDescriptions.OrderBy(metadata => metadata.Name).ToImmutableList();
        var builder = ImmutableDictionary.CreateBuilder<string, (ushort, IMavParamExtTypeMetadata)>();
        for (var i = 0; i < _paramList.Count; i++)
        {
            builder.Add(_paramList[i].Name, ((ushort)i, _paramList[i]));
        }

        _paramDict = builder.ToImmutable();
        
        _logger.ZLogDebug($"Create params server for [sys:{server.Identity.SystemId}, com:{server.Identity.ComponentId}] with {_paramDict.Count} params");
        
        _onParamChangedSubject = new Subject<ParamExtChangedEvent>();

        _sub1 = server.OnParamExtSet.Subscribe(OnParamSet);
        _sub2 = server.OnParamExtRequestList.Subscribe(OnParamRequestList);
        _sub3 = server.OnParamExtRequestRead.Subscribe(OnParamRequestRead);
    }


    private async void OnParamRequestRead(ParamExtRequestReadPacket packet)
    {
        (ushort, IMavParamExtTypeMetadata) param;
        if (packet.Payload.ParamIndex < 0)
        {
            var name = MavlinkTypesHelper.GetString(packet.Payload.ParamId);
            if (_paramDict.TryGetValue(name, out param) == false)
            {
                _logger.ZLogError($"Error to get mavlink param: param '{name}' not found");
                _statusTextServer.Error($"Param '{name}' not found");
                _onErrorSubject.OnNext(new ArgumentException($"Error to get mavlink param: param '{name}' not found", name));
                return;
            }
        }
        else
        {
            if (packet.Payload.ParamIndex >= _paramDict.Count)
            {
                _logger.ZLogError($"Error to get mavlink param: param with index '{packet.Payload.ParamIndex}' not found");
                _statusTextServer.Error($"Param '{packet.Payload.ParamIndex}' not found");
                _onErrorSubject.OnNext(new ArgumentException($"Error to get mavlink param: param with index '{packet.Payload.ParamIndex}' not found"));
                return;
            }

            param = ((ushort)packet.Payload.ParamIndex, _paramList[packet.Payload.ParamIndex]);
        }

        var currentValue = param.Item2.ReadFromConfig(_cfg, _serverCfg.CfgPrefix);
        await SendParam(param, currentValue, DisposeCancel).ConfigureAwait(false);
    }

    private async void OnParamRequestList(ParamExtRequestListPacket paramRequestListPacket)
    {
        if (Interlocked.CompareExchange(ref _sendingInProgressFlag, 1, 0) == 1)
        {
            _logger.LogWarning("Skip duplicate request for params list");
            _statusTextServer.Error($"Skip duplicate request");
            return;
        }
        _logger.LogDebug($"Send all params to client");
        try
        {
            for (var index = 0; index < _paramList.Count; index++)
            {
                var param = _paramList[index];
                var currentValue = param.ReadFromConfig(_cfg, _serverCfg.CfgPrefix);
                await SendParam(((ushort)index, param), currentValue, DisposeCancel).ConfigureAwait(false);
                if (_serverCfg.SendingParamItemDelayMs > 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(_serverCfg.SendingParamItemDelayMs), Core.TimeProvider, DisposeCancel).ConfigureAwait(false);    
                }
                
            }
        }
        finally
        {
            Interlocked.Exchange(ref _sendingInProgressFlag, 0);
        }
    }

    private async void OnParamSet(ParamExtSetPacket packet)
    {
        var name = MavlinkTypesHelper.GetString(packet.Payload.ParamId);
        if (_paramDict.TryGetValue(name, out var param) == false)
        {
            _logger.ZLogError($"Error to set mavlink param: param '{name}' not found");
            _statusTextServer.Error($"Param '{name}' not found");
            _onErrorSubject.OnNext(new ArgumentException($"Error to set mavlink param: param '{name}' not found", name));
            return;
        }

        var currentValue = param.Item2.ReadFromConfig(_cfg, _serverCfg.CfgPrefix);
        if (param.Item2.Type != packet.Payload.ParamType)
        {
            _logger.ZLogError($"Error to set mavlink param: param '{name}' type didn't equal. Want {param.Item2.Type} but found {packet.Payload.ParamType}");
            _statusTextServer.Error($"param type '{name}' not equal");
            _onErrorSubject.OnNext(new ArgumentException($"Error to set mavlink param: param '{name}' type didn't equal. Want {param.Item2.Type} but found {packet.Payload.ParamType}", name));
            await SendAck(param, currentValue, DisposeCancel).ConfigureAwait(false);
            return;
        }

        var newValue = new MavParamExtValue(packet.Payload.ParamValue);
        if (param.Item2.IsValid(newValue) == false)
        {
            var errorMsg = param.Item2.GetValidationError(newValue);
            _logger.ZLogError($"Error to set mavlink param '{name}' [{param.Item2.Type}]: {errorMsg}");
            _statusTextServer.Error($"param '{name}':{errorMsg}");
            _onErrorSubject.OnNext(new ArgumentException($"Error to set mavlink param '{name}' [{param.Item2.Type}]: {errorMsg}", name));
            await SendAck(param, currentValue, DisposeCancel).ConfigureAwait(false);
            return;
        }
        _logger.ZLogError($"Set param {param.Item2.Name} from {currentValue} => {newValue}");
        param.Item2.WriteToConfig(_cfg, newValue, _serverCfg.CfgPrefix);
        _onParamChangedSubject.OnNext(new ParamExtChangedEvent(param, currentValue, newValue, true));
        await SendAck(param, newValue, DisposeCancel).ConfigureAwait(false);
    }

    private async Task SendAck((ushort, IMavParamExtTypeMetadata) param, MavParamExtValue value,
        CancellationToken cancel)
    {
        await _server.SendParamExtAck(payload =>
        {
            MavlinkTypesHelper.SetString(payload.ParamId, param.Item2.Name);
            payload.ParamType = param.Item2.Type;
            value.CopyTo(payload.ParamValue);
            payload.ParamResult = ParamAck.ParamAckAccepted;
        }, cancel).ConfigureAwait(false);
    }
    
    private async Task SendParam((ushort, IMavParamExtTypeMetadata) param, MavParamExtValue value,
        CancellationToken cancel)
    {
        await _server.SendParamExtValue(payload =>
        {
            payload.ParamIndex = param.Item1;
            MavlinkTypesHelper.SetString(payload.ParamId, param.Item2.Name);
            payload.ParamType = param.Item2.Type;
            payload.ParamCount = (ushort)_paramList.Count;
            value.CopyTo(payload.ParamValue);
        }, cancel).ConfigureAwait(false);
    }

    public Observable<Exception> OnError => _onErrorSubject;
    public Observable<ParamExtChangedEvent> OnUpdated => _onParamChangedSubject;

    public MavParamExtValue this[string name]
    {
        get
        {
            if (_paramDict.TryGetValue(name, out var param) == false)
            {
                throw new ArgumentException($"Param '{name}' not found", nameof(name));
            }

            return param.Item2.ReadFromConfig(_cfg, _serverCfg.CfgPrefix);
        }
        set
        {
            if (_paramDict.TryGetValue(name, out var param) == false)
            {
                throw new ArgumentException($"Param '{name}' not found", nameof(name));
            }

            if (param.Item2.IsValid(value) == false)
            {
                var errorMsg = param.Item2.GetValidationError(value);
                throw new ArgumentException($"Error to set mavlink param '{name}' [{param.Item2.Type}]: {errorMsg}",
                    nameof(value));
            }

            var oldValue = param.Item2.ReadFromConfig(_cfg, _serverCfg.CfgPrefix);
            param.Item2.WriteToConfig(_cfg, value, _serverCfg.CfgPrefix);
            _onParamChangedSubject.OnNext(new ParamExtChangedEvent(param, oldValue, value, false));
            // TODO: put to queue and send in background using delay (throttle)
            SendParam(param, value, DisposeCancel).Wait();
        }
    }

    public MavParamExtValue this[IMavParamExtTypeMetadata param]
    {
        get => this[param.Name];
        set => this[param.Name] = value;
    }

    #region Dispose

    protected sealed override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _onErrorSubject.Dispose();
            _onParamChangedSubject.Dispose();
            _sub1.Dispose();
            _sub2.Dispose();
            _sub3.Dispose();
        }

        base.Dispose(disposing);
    }

    protected sealed override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_onErrorSubject).ConfigureAwait(false);
        await CastAndDispose(_onParamChangedSubject).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await CastAndDispose(_sub3).ConfigureAwait(false);

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