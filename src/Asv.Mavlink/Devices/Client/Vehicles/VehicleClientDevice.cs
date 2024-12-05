using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;
using Asv.Mavlink.Common;
using Asv.Mavlink.Diagnostic.Client;

using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class MavDataStreamConfig
{
    public ushort AllRateHz { get; set; } = 1;
    public ushort ExtendedStatusRateHz { get; set; } = 1;
    public ushort PositionRateHz { get; set; } = 1;
}

public class VehicleClientDeviceConfig: MavlinkClientDeviceConfig
{
    public ParamsClientExConfig Params { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();
    public MissionClientExConfig Missions { get; set; } = new();
    public MavlinkFtpClientConfig Ftp { get; set; } = new();
    public DiagnosticClientConfig Diagnostic { get; set; } = new();
    public MavDataStreamConfig MavDataStream { get; set; } = new();
    
    
    public override void Load(string key, IConfiguration configuration)
    {
        base.Load(key, configuration);
        Params = configuration.Get<ParamsClientExConfig>();
        Command = configuration.Get<CommandProtocolConfig>();
        Missions = configuration.Get<MissionClientExConfig>();
        Ftp = configuration.Get<MavlinkFtpClientConfig>();
        Diagnostic = configuration.Get<DiagnosticClientConfig>();
        MavDataStream = configuration.Get<MavDataStreamConfig>();
    }
    
    public override void Save(string key, IConfiguration configuration)
    {
        base.Save(key, configuration);
        configuration.Set(Params);
        configuration.Set(Command);
        configuration.Set(Missions);
        configuration.Set(Ftp);
        configuration.Set(Diagnostic);
        configuration.Set(MavDataStream);
    }
    
   
    
}
public class VehicleClientDevice: MavlinkClientDevice
{
    private readonly VehicleClientDeviceConfig _deviceConfig;
    private readonly ILogger<VehicleClientDevice> _logger;

    
    
    protected VehicleClientDevice(
        MavlinkClientDeviceId identity, 
        VehicleClientDeviceConfig config,
        ImmutableArray<IClientDeviceExtender> extenders, 
        IMavlinkContext core) 
        : base(identity,config,extenders,core)
    {
        _deviceConfig = config;
        _logger = core.LoggerFactory.CreateLogger<VehicleClientDevice>();
    }

    protected override async IAsyncEnumerable<IMicroserviceClient> InternalCreateMicroservices(
        [EnumeratorCancellation] CancellationToken cancel)
    {
        await foreach (var microservice in base.InternalCreateMicroservices(cancel).ConfigureAwait(false))
        {
            yield return microservice;
        }
        yield return new StatusTextClient(Identity, Core);
        var cmd = new CommandClient(Identity,_deviceConfig.Command,Core);
        yield return cmd;
        AutopilotVersionPacket autopilotVersion;
        try
        {
            _logger.LogTrace("Try to read AutopilotVersion for checking capabilities");
            autopilotVersion = await cmd.RequestMessageOnce<AutopilotVersionPacket>(cancel: cancel).ConfigureAwait(false);
            _logger.ZLogInformation($"Autopilot version capabilities:{autopilotVersion.Payload.Capabilities.ToString("F")}");
        }
        catch (Exception e)
        {
            _logger.ZLogError($"Error to read AutopilotVersion: {e.Message}");
            throw;
        }
        
        var paramBase = new ParamsClient(Identity, _deviceConfig.Params, Core);
        yield return paramBase;
        if (autopilotVersion.Payload.Capabilities.HasFlag(MavProtocolCapability
                .MavProtocolCapabilityParamEncodeBytewise))
        {
            yield return new ParamsClientEx(paramBase, _deviceConfig.Params, MavParamHelper.ByteWiseEncoding,
                GetParamDescriptions());
        }
        else
        {
            yield return new ParamsClientEx(paramBase, _deviceConfig.Params, MavParamHelper.CStyleEncoding,
                GetParamDescriptions());
        }
        
        yield return new LoggingClient(Identity, Core);
        var missions = new MissionClient(Identity, _deviceConfig.Missions,Core);
        yield return missions;
        yield return new MissionClientEx(missions, _deviceConfig.Missions);
        yield return new FtpClient(Identity,_deviceConfig.Ftp,Core);
        var gnssBase = new GnssClient(Identity,Core);
        yield return gnssBase;
        yield return new GnssClientEx(gnssBase);
        yield return new V2ExtensionClient(Identity,Core);
        var pos = new PositionClient(Identity,Core);
        yield return pos;
        yield return new PositionClientEx(pos,Heartbeat,cmd);
        var rtt = new TelemetryClient(Identity,Core);
        yield return rtt;
        yield return new TelemetryClientEx(rtt);
        yield return new DgpsClient(Identity,Core);
        yield return new DiagnosticClient(Identity, _deviceConfig.Diagnostic, Core);
    }

    protected virtual IEnumerable<ParamDescription> GetParamDescriptions()
    {
        yield break;
    }

    protected override async Task InitAfterMicroservices(CancellationToken cancel)
    {
        if (Microservices.FirstOrDefault(x => x is ITelemetryClient) is ITelemetryClient rtt)
        {
            await rtt.RequestDataStream((int)MavDataStream.MavDataStreamAll, _deviceConfig.MavDataStream.AllRateHz , true, cancel).ConfigureAwait(false);
            await rtt.RequestDataStream((int)MavDataStream.MavDataStreamExtendedStatus, _deviceConfig.MavDataStream.ExtendedStatusRateHz, true, cancel).ConfigureAwait(false);
            await rtt.RequestDataStream((int)MavDataStream.MavDataStreamPosition,_deviceConfig.MavDataStream.PositionRateHz , true, cancel).ConfigureAwait(false);    
        }
    }
}