using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IParamsServerEx
{
    
}

public class ParamValue
{
    public string Name { get; }
    public string Value { get; set; }
}

public class ParamsServerExConfig
{
    public int ReadListTimeoutMs { get; set; } = 100;
}

public class ParamsServerEx: DisposableOnceWithCancel, IParamsServerEx
{
    private readonly IConfiguration _cfg;
    private readonly List<ParamValue> _paramValues = new();

    public ParamsServerEx(IParamsServer server, IEnumerable<ParamDescription> paramDescriptions, IMavParamValueConverter converter, IConfiguration cfg, ParamsServerExConfig serverCfg, string prefix = "MAV_CFG_")
    {
        _cfg = cfg;

        foreach (var param in paramDescriptions)
        {
            var name = prefix + param.Name;
            param.Name = name;
            AllParams.Add(param);

            var paramValue = _cfg.Get<ParamValue>(name);
            _paramValues.Add(paramValue);
        }

        server.OnParamSet.Subscribe(_ =>
        {
            if (!ValidateParam(_.Payload)) return;
            
            var param = AllParams.First(x => x.Name == MavlinkTypesHelper.GetString(_.Payload.ParamId));
            SaveParamToCfg(param.Name, _.Payload.ParamValue.ToString(CultureInfo.InvariantCulture));
            
            server.SendParamValue(__ =>
            {
                MavlinkTypesHelper.SetString(__.ParamId, param.Name);
                __.ParamType = param.ParamType;
                __.ParamIndex = (ushort)AllParams.IndexOf(param);
                __.ParamValue = _.Payload.ParamValue;
            });
        });

        server.OnParamRequestList.Subscribe(_ =>
        {
            foreach (var param in AllParams)
            {
                var paramValue = _paramValues.First(x => x.Name == param.Name);
                server.SendParamValue(__ =>
                {
                    MavlinkTypesHelper.SetString(__.ParamId, param.Name);
                    __.ParamType = param.ParamType;
                    __.ParamIndex = (ushort)AllParams.IndexOf(param);
                    __.ParamValue = converter.ConvertToMavlinkUnionToParamValue(Convert.ToDecimal(paramValue.Value), param.ParamType);
                });
                
                Thread.Sleep(serverCfg.ReadListTimeoutMs);
            }
        });

        server.OnParamRequestRead.Subscribe(_ =>
        {
            var param = AllParams.First(x => x.Name == MavlinkTypesHelper.GetString(_.Payload.ParamId));
            var paramValue = _paramValues.First(x => x.Name == param.Name);
            
            server.SendParamValue(__ =>
            {
                MavlinkTypesHelper.SetString(__.ParamId, param.Name);
                __.ParamType = param.ParamType;
                __.ParamIndex = (ushort)AllParams.IndexOf(param);
                __.ParamValue = converter.ConvertToMavlinkUnionToParamValue(Convert.ToDecimal(paramValue.Value), param.ParamType);;
            });
        });
    }

    public List<ParamDescription> AllParams { get; } = new ();

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
}