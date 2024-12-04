using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class GenericDeviceConfig:MavlinkClientDeviceConfig
{
    public CommandProtocolConfig Commands { get; set; } = new();
    public MavlinkFtpClientConfig Ftp { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new ();
    
    public override void Load(string key, IConfiguration configuration)
    {
        base.Load(key, configuration);
        Commands = configuration.Get<CommandProtocolConfig>();
        Ftp = configuration.Get<MavlinkFtpClientConfig>();
        Params = configuration.Get<ParamsClientExConfig>();
    }
    
    public override void Save(string key, IConfiguration configuration)
    {
        base.Save(key, configuration);
        configuration.Set(Commands);
        configuration.Set(Ftp);
        configuration.Set(Params);
    }
}

public class GenericDevice: MavlinkClientDevice
{
    public const string DeviceClass = "GENERIC";
    
    private readonly GenericDeviceConfig _config;
    private readonly ILogger<GenericDevice> _logger;


    public GenericDevice(
        MavlinkClientDeviceId identity, 
        GenericDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        ICoreServices core) 
        : base(identity,config,extenders,core)
    {
        _config = config;
        _logger = core.LoggerFactory.CreateLogger<GenericDevice>();
    }

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices(
        [EnumeratorCancellation] CancellationToken cancel)
    {
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            yield return microservice;
        }
        yield return new StatusTextClient(Identity, Core);
        
        var client = new CommandClient(Identity, _config.Commands, Core);
        yield return client;
        
        AutopilotVersionPacket autopilotVersion;    
        try
        {
            _logger.LogTrace("Try to read AutopilotVersion for checking capabilities");
            autopilotVersion = await client.RequestMessageOnce<AutopilotVersionPacket>(cancel: cancel).ConfigureAwait(false);
            _logger.ZLogInformation($"Autopilot version capabilities:{autopilotVersion.Payload.Capabilities.ToString("F")}");
        }
        catch (Exception e)
        {
            _logger.ZLogError($"Error to read AutopilotVersion: {e.Message}");
            yield break;
        }
        //TODO: add other Capability
        var cap = autopilotVersion.Payload.Capabilities;
        if (cap.HasFlag(MavProtocolCapability.MavProtocolCapabilityFtp))
        {
            _logger.ZLogDebug($"Create FTP microservice {_config.Ftp}");
            var ftp = new FtpClient(Identity, _config.Ftp, Core);
            yield return ftp;
            yield return new FtpClientEx(ftp);
        }

        if (cap.HasFlag(MavProtocolCapability.MavProtocolCapabilityParamEncodeCCast))
        {
            _logger.ZLogDebug($"Create CCast params microservice {_config.Params}");
            var paramBase = new ParamsClient(Identity, _config.Params, Core);
            yield return paramBase;
            yield return new ParamsClientEx(paramBase, _config.Params, MavParamHelper.CStyleEncoding, []);
        }

        if (cap.HasFlag(MavProtocolCapability.MavProtocolCapabilityParamEncodeBytewise))
        {
            _logger.ZLogDebug($"Create ByteWise params microservice {_config.Params}");
            var paramBase = new ParamsClient(Identity, _config.Params, Core);
            yield return paramBase;
            yield return new ParamsClientEx(paramBase, _config.Params, MavParamHelper.ByteWiseEncoding, []);
        }
    }


    
}

