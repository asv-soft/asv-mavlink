using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Reactive.Subjects;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public struct ParamChangedEvent
{
    public ParamChangedEvent((ushort, IMavParamTypeMetadata) param, MavParamValue oldValue, MavParamValue newValue, bool isRemoteChange)
    {
        ParamIndex = param.Item1;
        Metadata = param.Item2;
        OldValue = oldValue;
        NewValue = newValue;
        IsRemoteChange = isRemoteChange;
    }
    public bool IsRemoteChange { get; }
    public IMavParamTypeMetadata Metadata { get; }
    public ushort ParamIndex { get; }
    public MavParamValue OldValue { get; }
    public MavParamValue NewValue { get; }
}

public class ParamsServerExConfig
{
    public int SendingParamItemDelayMs { get; set; } = 100;
    public string CfgPrefix { get; set; } = "MAV_CFG_";
}

public class ParamsServerEx: DisposableOnceWithCancel, IParamsServerEx
{
    private readonly IParamsServer _server;
    private readonly IStatusTextServer _statusTextServer;
    private readonly IMavParamEncoding _encoding;
    private readonly IConfiguration _cfg;
    private readonly ParamsServerExConfig _serverCfg;

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly Subject<Exception> _onErrorSubject;
    private int _sendingInProgressFlag;
    private readonly ImmutableDictionary<string,(ushort,IMavParamTypeMetadata)> _paramDict;
    private readonly ImmutableList<IMavParamTypeMetadata> _paramList;
    private readonly Subject<ParamChangedEvent> _onParamChangedSubject;
    public ParamsServerEx(IParamsServer server, IStatusTextServer statusTextServer, IEnumerable<IMavParamTypeMetadata> paramDescriptions, IMavParamEncoding encoding, IConfiguration cfg, ParamsServerExConfig serverCfg)
    {
        _server = server;
        _statusTextServer = statusTextServer ?? throw new ArgumentNullException(nameof(statusTextServer));
        _encoding = encoding;
        _cfg = cfg;
        _serverCfg = serverCfg;
        _onErrorSubject = new Subject<Exception>().DisposeItWith(Disposable);
        _paramList = paramDescriptions.OrderBy(_=>_.Name).ToImmutableList();
        var dict = new Dictionary<string, (ushort,IMavParamTypeMetadata)>();
        for (var i = 0; i < _paramList.Count; i++)
        {
            dict.Add(_paramList[i].Name,((ushort)i,_paramList[i]));
        }
        _paramDict = dict.ToImmutableDictionary();
        _onParamChangedSubject = new Subject<ParamChangedEvent>().DisposeItWith(Disposable);
        
        server.OnParamSet.Subscribe(OnParamSet).DisposeItWith(Disposable);
        server.OnParamRequestList.Subscribe(OnParamRequestList).DisposeItWith(Disposable);
        server.OnParamRequestRead.Subscribe(OnParamRequestRead).DisposeItWith(Disposable);
    }

    private async void OnParamRequestRead(ParamRequestReadPacket _)
    {
        (ushort,IMavParamTypeMetadata) param;
        if (_.Payload.ParamIndex < 0)
        {
            var name =  MavlinkTypesHelper.GetString(_.Payload.ParamId);
            if (_paramDict.TryGetValue(name, out param) == false)
            {
                var msg = $"Error to get mavlink param: param '{name}' not found";
                Logger.Error(msg);
                _statusTextServer.Error($"Param '{name}' not found");
                _onErrorSubject.OnNext(new ArgumentException(msg,name));
                return;
            }
        }
        else
        {
            if (_.Payload.ParamIndex >= _paramDict.Count)
            {
                var msg = $"Error to get mavlink param: param with index '{_.Payload.ParamIndex}' not found";
                Logger.Error(msg);
                _statusTextServer.Error($"Param '{_.Payload.ParamIndex}' not found");
                _onErrorSubject.OnNext(new ArgumentException(msg));
                return;
            }
            param = ((ushort)_.Payload.ParamIndex, _paramList[_.Payload.ParamIndex]);
        }
            
        var cfgKey = GetCfgKey(param.Item2.Name);
        var currentValue = MavParamHelper.ReadFromConfig(_cfg, cfgKey, param.Item2);
        await SendParam(param, currentValue, DisposeCancel).ConfigureAwait(false);
    }

    private async void OnParamRequestList(ParamRequestListPacket paramRequestListPacket)
    {
        if (Interlocked.CompareExchange(ref _sendingInProgressFlag, 1, 0) == 1)
        {
            Logger.Warn("Skip duplicate request for params list");
            _statusTextServer.Error($"Skip duplicate request");
            return;
        }
        try
        {
            for (var index = 0; index < _paramList.Count; index++)
            {
                var param = _paramList[index];
                var cfgKey = GetCfgKey(param.Name);
                var currentValue =  MavParamHelper.ReadFromConfig(_cfg, cfgKey,param);
                await SendParam(((ushort)index,param), currentValue, DisposeCancel).ConfigureAwait(false);
                await Task.Delay(_serverCfg.SendingParamItemDelayMs, DisposeCancel).ConfigureAwait(false);
            }
        }
        finally
        {
            Interlocked.Exchange(ref _sendingInProgressFlag, 0);
        }
    }

    private async void OnParamSet(ParamSetPacket _)
    {
        var name = MavlinkTypesHelper.GetString(_.Payload.ParamId);
        if (_paramDict.TryGetValue(name, out var param) == false)
        {
            var msg = $"Error to set mavlink param: param '{name}' not found";
            Logger.Error(msg);
            _statusTextServer.Error($"Param '{name}' not found");
            _onErrorSubject.OnNext(new ArgumentException(msg, name));
            return;
        }

        var cfgKey = GetCfgKey(param.Item2.Name);
        var currentValue = MavParamHelper.ReadFromConfig(_cfg, cfgKey, param.Item2);
        if (param.Item2.Type != _.Payload.ParamType)
        {
            var msg = $"Error to set mavlink param: param '{name}' type didn't equal. Want {param.Item2.Type} but found {_.Payload.ParamType}";
            Logger.Error(msg);
            _statusTextServer.Error($"param type '{name}' not equal");
            _onErrorSubject.OnNext(new ArgumentException(msg, name));
            await SendParam(param, currentValue, DisposeCancel).ConfigureAwait(false);
            return;
        }
        
        var newValue = _encoding.ConvertFromMavlinkUnion(_.Payload.ParamValue, param.Item2.Type);
        if (param.Item2.IsValid(newValue) == false)
        {
            var errorMsg = param.Item2.GetValidationError(newValue);
            var msg = $"Error to set mavlink param '{name}' [{param.Item2.Type}]: {errorMsg}";
            Logger.Error(msg);
            _statusTextServer.Error($"param '{name}':{errorMsg}");
            _onErrorSubject.OnNext(new ArgumentException(msg, name));
            await SendParam(param, currentValue, DisposeCancel).ConfigureAwait(false);
            return;
        }

        if (ValidateParam(param.Item2, newValue, out var errorText) == false)
        {
            Logger.Warn("Set param {0} validation fail:{1}",param.Item2.Name,errorText);
            _statusTextServer.Error($"param '{name}':{errorText}");
            return;
        }
        Logger.Info("Set param {0} from {1} => {2}", param.Item2.Name, currentValue, newValue);
        MavParamHelper.WriteToConfig(_cfg, cfgKey, newValue);
        _onParamChangedSubject.OnNext(new ParamChangedEvent(param, currentValue, newValue, true));
        await SendParam(param, newValue, DisposeCancel).ConfigureAwait(false);
    }

    private bool ValidateParam(IMavParamTypeMetadata desc, MavParamValue value, out string errorMsg)
    {
        errorMsg = null;
        switch (desc.Type)
        {
            case MavParamType.MavParamTypeUint8:
            case MavParamType.MavParamTypeInt8:
            case MavParamType.MavParamTypeUint16:
            case MavParamType.MavParamTypeInt16:
            case MavParamType.MavParamTypeUint32:
            case MavParamType.MavParamTypeInt32:
            case MavParamType.MavParamTypeReal32:
                if (value > desc.MaxValue)
                {
                    errorMsg = $"must be <'{desc.MaxValue}'";
                    return false;
                }
                if (value < desc.MinValue)
                {
                    errorMsg = $"must be >'{desc.MinValue}'";
                    return false;
                }
                break;
            case MavParamType.MavParamTypeUint64:
            case MavParamType.MavParamTypeInt64:
            case MavParamType.MavParamTypeReal64:
            default:
                errorMsg = $"wrong type {desc.Type}";
                return false;
        }
        return true;
    }

    private async Task SendParam((ushort, IMavParamTypeMetadata) param, MavParamValue value, CancellationToken cancel)
    {
        await _server.SendParamValue(_ =>
        {
            _.ParamIndex = param.Item1;
            MavlinkTypesHelper.SetString(_.ParamId, param.Item2.Name);
            _.ParamType = param.Item2.Type;
            _.ParamCount = (ushort)_paramList.Count;
            _.ParamValue = _encoding.ConvertToMavlinkUnion(value);
        }, cancel).ConfigureAwait(false);
    }

    private string GetCfgKey(string name)
    {
        return $"{_serverCfg.CfgPrefix}{name}";
    }

    public IObservable<Exception> OnError => _onErrorSubject;
    public IObservable<ParamChangedEvent> OnUpdated => _onParamChangedSubject;

    public MavParamValue this[string name]
    {
        get
        {
            if (_paramDict.TryGetValue(name, out var param) == false)
            {
                throw new ArgumentException($"Param '{name}' not found", nameof(name));
            }
            var cfgKey = GetCfgKey(param.Item2.Name);
            return MavParamHelper.ReadFromConfig(_cfg, cfgKey, param.Item2);
        }
        set
        {
            if (_paramDict.TryGetValue(name, out var param) == false)
            {
                throw new ArgumentException($"Param '{name}' not found", nameof(name));
            }
            var cfgKey = GetCfgKey(param.Item2.Name);
            if (param.Item2.IsValid(value) == false)
            {
                var errorMsg = param.Item2.GetValidationError(value);
                throw new ArgumentException($"Error to set mavlink param '{name}' [{param.Item2.Type}]: {errorMsg}", nameof(value));
            }
            var oldValue = MavParamHelper.ReadFromConfig(_cfg, cfgKey, param.Item2);
            MavParamHelper.WriteToConfig(_cfg, cfgKey, value);
            _onParamChangedSubject.OnNext(new ParamChangedEvent(param, oldValue, value, false));
            // TODO: put to queue and send in background using delay (throttle)
            SendParam(param, value, DisposeCancel).Wait();
        }
    }

    public MavParamValue this[IMavParamTypeMetadata param]
    {
        get => this[param.Name];
        set => this[param.Name] = value;
    }
}