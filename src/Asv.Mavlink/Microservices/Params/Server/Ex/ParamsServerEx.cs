using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public interface IParamsServerExItem
{
    
}

public interface IParamsServerEx
{
    IObservable<Exception> OnError { get; }
    IObservable<ParamChangedEvent> OnParamChanged { get; }

}

public struct ParamChangedEvent
{
    public ParamChangedEvent(MavParamDesc param, decimal oldValue, decimal newValue, bool isRemoteChange)
    {
        Param = param;
        OldValue = oldValue;
        NewValue = newValue;
        IsRemoteChange = isRemoteChange;
    }
    public bool IsRemoteChange { get; set; }
    public MavParamDesc Param { get; set; }
    public decimal OldValue { get; set; }
    public decimal NewValue { get; set; }
}

public class ParamsServerExConfig
{
    public int ReadListTimeoutMs { get; set; } = 100;
    public string CfgPrefix { get; set; } = "MAV_CFG_";
}

public class MavParamDesc
{
    public MavParamDesc(string name, MavParamType type, decimal defauultValue)
    {
        
    }
    public string Name { get; set; }
    public MavParamType Type { get; set; }
    public decimal DefaultValue { get; set; }
}



public class ParamsServerEx: DisposableOnceWithCancel, IParamsServerEx
{
    private readonly IStatusTextServer _statusTextServer;
    private readonly IConfiguration _cfg;
    private readonly ParamsServerExConfig _serverCfg;

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly Subject<Exception> _onErrorSubject;
    private int _sendingInProgressFlag;
    private readonly ImmutableSortedDictionary<string,MavParamDesc> _paramValues;
    private readonly Subject<ParamChangedEvent> _onParamChangedSubject;

    public ParamsServerEx(IParamsServer server, IStatusTextServer statusTextServer, IEnumerable<MavParamDesc> paramDescriptions, IMavParamValueConverter converter, IConfiguration cfg, ParamsServerExConfig serverCfg)
    {
        _statusTextServer = statusTextServer ?? throw new ArgumentNullException(nameof(statusTextServer));
        _cfg = cfg;
        _serverCfg = serverCfg;
        _onErrorSubject = new Subject<Exception>().DisposeItWith(Disposable);
        _paramValues = paramDescriptions.ToImmutableSortedDictionary(_=>_.Name, _=>_, StringComparer.InvariantCultureIgnoreCase);
        _onParamChangedSubject = new Subject<ParamChangedEvent>().DisposeItWith(Disposable);
        
        server.OnParamSet.Subscribe( async _ =>
        {
            var name = MavlinkTypesHelper.GetString(_.Payload.ParamId);
            if (_paramValues.TryGetValue(name,out var param) == false)
            {
                var msg = $"Error to set mavlink param: param '{name}' not found";
                Logger.Error(msg);
                _statusTextServer.Error($"param '{name}' not found");
                _onErrorSubject.OnNext(new ArgumentException(msg,name));
                return;
            }

            if (param.Type != _.Payload.ParamType)
            {
                var msg = $"Error to set mavlink param: param '{name}' type didn't equal. Want {param.Type} but found {_.Payload.ParamType}";
                Logger.Error(msg);
                _statusTextServer.Error($"param type '{name}' not equal");
                _onErrorSubject.OnNext(new ArgumentException(msg,name));
                return;
            }

            var cfgKey = GetCfgKey(param.Name);
            var currentValue = _cfg.Get(cfgKey, param.DefaultValue);
            var newValue = converter.ConvertFromMavlinkUnion(_.Payload.ParamValue, _.Payload.ParamType);
            Logger.Info("Set param {0} from {1} => {2}",param.Name,currentValue,newValue);
            _cfg.Set($"{serverCfg.CfgPrefix}{name}",newValue);
            _onParamChangedSubject.OnNext(new ParamChangedEvent(param,currentValue,newValue,true));
            await SendParam(param);
        });

        server.OnParamRequestList.Subscribe( async _ =>
        {
            if (Interlocked.CompareExchange(ref _sendingInProgressFlag,1,0) == 1) return;
            foreach (var param in _paramValues)
            {
                SendParam(param);
                await Task.Delay(serverCfg.ReadListTimeoutMs).ConfigureAwait(false);
            }
        });

        server.OnParamRequestRead.Subscribe(_ =>
        {
            if (_.Payload.ParamIndex < 0)
            {
                if (TryFindParam(_.Payload.ParamIndex, out param) == false) return;
            }
            else
            {
                if (TryFindParam(_.Payload.ParamId, out param) == false) return;
            }
            
            
            var param = AllParams.First(x => x.Name == MavlinkTypesHelper.GetString(_.Payload.ParamId));
            var paramValue = _paramValues.First(x => x.Name == param.Name);
            
            server.SendParamValue(__ =>
            {
                MavlinkTypesHelper.SetString(__.ParamId, param.Name);
                __.ParamType = param.ParamType;
                __.ParamIndex = (ushort)AllParams.IndexOf(param);
                __.ParamValue = converter.ConvertToMavlinkUnion(Convert.ToDecimal(paramValue.Value), param.ParamType);;
            });
        });
    }

    private string GetCfgKey(string name)
    {
        return $"{_serverCfg.CfgPrefix}{name}";
    }

    private bool TryFindParam(char[] paramName, MavParamType type, out ParamsServerExItem paramValue)
    {
        var name = MavlinkTypesHelper.GetString(paramName);
        if (_paramValues.TryGetValue(name,out paramValue) == false)
        {
            var msg = $"Error to set mavlink param: param '{name}' not found";
            Logger.Error(msg);
            _statusTextServer.Error($"param '{name}' not found");
            _onErrorSubject.OnNext(new ArgumentException(msg,name));
            return false;
        }

        if (paramValue.ParamType != type)
        {
            var msg = $"Error to set mavlink param: param '{name}' type didn't equal. Want {type} but found {paramValue.ParamType}";
            Logger.Error(msg);
            _statusTextServer.Error($"param type '{name}' not equal");
            _onErrorSubject.OnNext(new ArgumentException(msg,name));
            return false;
        }

        return true;
    }


    public void InternalUpdate(ParamDescription parameter)
    {
        
    }

    private bool ValidateParam(ParamSetPayload payload)
    {
        switch (payload.ParamType)
        {
            case MavParamType.MavParamTypeUint8:
                if (payload.ParamValue is > byte.MaxValue or < byte.MinValue)
                {
                    return false;
                }
                break;
            case MavParamType.MavParamTypeInt8:
                if (payload.ParamValue is > sbyte.MaxValue or < sbyte.MinValue)
                {
                    return false;
                }
                break;
            case MavParamType.MavParamTypeUint16:
                if (payload.ParamValue is > ushort.MaxValue or < ushort.MinValue)
                {
                    return false;
                }
                break;
            case MavParamType.MavParamTypeInt16:
                if (payload.ParamValue is > short.MaxValue or < short.MinValue)
                {
                    return false;
                }
                break;
            case MavParamType.MavParamTypeUint32:
                if (payload.ParamValue is > uint.MaxValue or < uint.MinValue)
                {
                    return false;
                }
                break;
            case MavParamType.MavParamTypeInt32:
                if (payload.ParamValue is > int.MaxValue or < int.MinValue)
                {
                    return false;
                }
                break;
            case MavParamType.MavParamTypeUint64:
                if (payload.ParamValue is > ulong.MaxValue or < ulong.MinValue)
                {
                    return false;
                }
                break;
            case MavParamType.MavParamTypeInt64:
                if (payload.ParamValue is > long.MaxValue or < long.MinValue)
                {
                    return false;
                }
                break;
            case MavParamType.MavParamTypeReal32:
                break;
            case MavParamType.MavParamTypeReal64:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return true;
    }

    private void SaveParamToCfg(string paramName, string paramValue)
    {
        var param = _paramValues.First(x => x.Name == paramName);

        if (string.IsNullOrEmpty(param.Name)) return;
        
        param.Value = paramValue;
        _cfg.Set(nameof(paramName), param);
    }

    public IObservable<Exception> OnError => _onErrorSubject;
    public IObservable<ParamChangedEvent> OnParamChanged => _onParamChangedSubject;
}