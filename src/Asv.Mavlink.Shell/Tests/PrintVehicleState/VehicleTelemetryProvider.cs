using System;
using System.Collections.Generic;
using Asv.IO;
using Asv.Mavlink.Minimal;
using DotLiquid.Util;

namespace Asv.Mavlink.Shell;

public class VehicleTelemetryProvider
{
    private readonly ArduCopterClientDevice _device;
    private readonly IHeartbeatClient _heartbeat;
    private readonly HeartbeatPayload _heartbeatPayload;
    private readonly IPositionClient _position;

    private const int DeltaXy = 10;
    public int XyThreshold => DeltaXy;
    
    private const int DeltaZ = 5;
    public int ZThreshold => DeltaZ;
    
    public VehicleTelemetryProvider(ArduCopterClientDevice device)
    {
        _device = device;
        _heartbeat = _device.GetMicroservice<IHeartbeatClient>() ?? throw new InvalidOperationException("No heartbeat");
        _position = _device.GetMicroservice<IPositionClient>() ?? throw new InvalidOperationException("No position");
        _heartbeatPayload = _heartbeat.RawHeartbeat.CurrentValue ?? throw new InvalidOperationException("No heartbeat");
    }
    
    /// <summary>
    /// Gets the current name of the device.
    /// </summary>
    /// <returns>The device name, or an empty string if unavailable.</returns>
    public string GetDeviceName()
    {
        return _device.Name.CurrentValue ?? String.Empty;
    }

    /// <summary>
    /// Retrieves a dictionary of telemetry parameters and their current string representations.
    /// </summary>
    /// <returns>A dictionary of telemetry field names and values.</returns>
    public Dictionary<string, string> GetTelemetry()
    {
        var home = _position.Home.CurrentValue;
        var global = _position.GlobalPosition.CurrentValue;

        var homeStr = home == null ? "Not Accessible" : $"{home.Longitude} {home.Latitude} {home.Altitude}";
        var globalStr = global == null ? "Not Accessible" :
            $"{global.Lat} {global.Lon} (MSL){global.Alt} (AGL){global.RelativeAlt}";

        return new Dictionary<string, string>
        {
            { "Link", _device.Link.State.ToString() ?? "" },
            { "PacketRateHz", _heartbeat.GetPropertyValue("PacketRateHz")?.ToString() ?? "" },
            { "SystemStatus", _heartbeatPayload.SystemStatus.ToString()},
            { "Type", _heartbeatPayload.Type.ToString()},
            { "Autopilot", _heartbeatPayload.Autopilot.ToString()},
            { "BaseMode", _heartbeatPayload.BaseMode.ToString("F") },
            { "CustomMode", _heartbeatPayload.CustomMode.ToString() },
            { "MavlinkVersion", _heartbeatPayload.MavlinkVersion.ToString()},
            { "Home", homeStr },
            { "GlobalPosition", globalStr }
        };
    }
}
