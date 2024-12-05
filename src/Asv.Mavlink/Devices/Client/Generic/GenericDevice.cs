using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class GenericDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Commands { get; set; } = new();
    public MavlinkFtpClientConfig Ftp { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new ();
}

public class GenericDevice: ClientDevice
{
    private readonly MavlinkClientIdentity _identity;
    private readonly GenericDeviceConfig _config;
    private readonly ICoreServices _core;
    private readonly ILogger<GenericDevice> _logger;
    private AutopilotVersionPacket? _autopilotVersion;

    public GenericDevice(MavlinkClientIdentity identity, GenericDeviceConfig config, ICoreServices core) : base(identity,config,core,DeviceClass.Unknown)
    {
        _identity = identity;
        _config = config;
        _core = core;
        _logger = core.Log.CreateLogger<GenericDevice>();
    }
    
    protected override async Task InitBeforeMicroservices(CancellationToken cancel)
    {
        using var client = new CommandClient(_identity, _config.Commands, _core);
        try
        {
            _logger.LogTrace("Try to read AutopilotVersion for checking capabilities");
            _autopilotVersion = await client.RequestMessageOnce<AutopilotVersionPacket>(cancel: cancel).ConfigureAwait(false);
            _logger.ZLogInformation($"Autopilot version capabilities:{_autopilotVersion.Payload.Capabilities.ToString("F")}");
        }
        catch (Exception e)
        {
            _logger.ZLogError($"Error to read AutopilotVersion: {e.Message}");
        }
    }

    protected override IEnumerable<IMavlinkMicroserviceClient> CreateMicroservices()
    {
        yield return new StatusTextClient(_identity, _core);
        
        if (_autopilotVersion != null)
        {
            var cap = _autopilotVersion.Payload.Capabilities;
            if (cap.HasFlag(MavProtocolCapability.MavProtocolCapabilityFtp))
            {
                _logger.ZLogDebug($"Create FTP microservice {_config.Ftp}");
                var ftp = new FtpClient(_identity, _config.Ftp, _core);
                yield return ftp;
                yield return new FtpClientEx(ftp);
            }

            if (cap.HasFlag(MavProtocolCapability.MavProtocolCapabilityParamEncodeCCast))
            {
                _logger.ZLogDebug($"Create CCast params microservice {_config.Params}");
                var paramBase = new ParamsClient(_identity, _config.Params, _core);
                yield return paramBase;
                yield return new ParamsClientEx(paramBase, _config.Params, MavParamHelper.CStyleEncoding, []);
            }
            if (cap.HasFlag(MavProtocolCapability.MavProtocolCapabilityParamEncodeBytewise))
            {
                _logger.ZLogDebug($"Create ByteWise params microservice {_config.Params}");
                var paramBase = new ParamsClient(_identity, _config.Params, _core);
                yield return paramBase;
                yield return new ParamsClientEx(paramBase, _config.Params, MavParamHelper.ByteWiseEncoding, []);
            }
            
            //TODO: add other Capability
        }
    }

    

    protected override Task InitAfterMicroservices(CancellationToken cancel)
    {
        return Task.CompletedTask;
    }

    
}

