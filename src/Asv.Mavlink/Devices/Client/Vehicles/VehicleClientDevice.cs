using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Diagnostic.Client;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Asv.Mavlink;

public class VehicleClientDeviceConfig: ClientDeviceConfig
{
    public ParamsClientExConfig Params { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();
    public MissionClientExConfig Missions { get; set; } = new();
    public MavlinkFtpClientConfig Ftp { get; set; } = new();
    public DiagnosticClientConfig Diagnostic { get; set; } = new();
    
    public ushort MavDataStreamAllRateHz { get; set; } = 1;
    public ushort MavDataStreamExtendedStatusRateHz { get; set; } = 1;
    public ushort MavDataStreamPositionRateHz { get; set; } = 1;
    
}
public class VehicleClientDevice: ClientDevice
{
    private readonly VehicleClientDeviceConfig _deviceConfig;
    private readonly ILogger<VehicleClientDevice> _logger;
    private AutopilotVersionPacket? _autopilotVersion;

    protected VehicleClientDevice(MavlinkClientIdentity identity, VehicleClientDeviceConfig deviceConfig, ICoreServices core, DeviceClass @class) 
        : base(identity, deviceConfig, core,@class )
    {
        _deviceConfig = deviceConfig;
        _logger = core.Log.CreateLogger<VehicleClientDevice>();
    }

    protected override async Task InitBeforeMicroservices(CancellationToken cancel)
    {
        using var client = new CommandClient(Identity, _deviceConfig.Command, Core);
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
        yield return new StatusTextClient(Identity, Core);
        var paramBase = new ParamsClient(Identity, _deviceConfig.Params, Core);
        yield return paramBase;
        if (_autopilotVersion != null)
        {
            
            if (_autopilotVersion.Payload.Capabilities.HasFlag(MavProtocolCapability
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
            
        }
        var cmd = new CommandClient(Identity,_deviceConfig.Command,Core);
        yield return cmd;
        yield return new OffboardClient(Identity, Core);
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
        var rtt = this.GetMicroservice<ITelemetryClient>();
        if (rtt != null)
        {
            await rtt.RequestDataStream((int)MavDataStream.MavDataStreamAll, _deviceConfig.MavDataStreamAllRateHz , true, cancel).ConfigureAwait(false);
            await rtt.RequestDataStream((int)MavDataStream.MavDataStreamExtendedStatus, _deviceConfig.MavDataStreamExtendedStatusRateHz, true, cancel).ConfigureAwait(false);
            await rtt.RequestDataStream((int)MavDataStream.MavDataStreamPosition,_deviceConfig.MavDataStreamPositionRateHz , true, cancel).ConfigureAwait(false);    
        }
    }
}