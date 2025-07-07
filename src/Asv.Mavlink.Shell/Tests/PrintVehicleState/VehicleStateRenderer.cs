using System;
using System.Collections.Generic;
using Asv.IO;
using Asv.Mavlink.Minimal;
using DotLiquid.Util;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class VehicleStateRenderer : ITableRenderer
{
    private readonly IClientDevice _device;
    private readonly IHeartbeatClient _heartbeat;
    private readonly HeartbeatPayload _heartbeatPayload;
    private readonly IPositionClient _position;

    private readonly List<KeyValuePair<string, string>> _telemetryRows = new()
    {
        new(TelemetryKeys.Link, string.Empty),
        new(TelemetryKeys.PacketRateHz, string.Empty),
        new(TelemetryKeys.Type, string.Empty),
        new(TelemetryKeys.SystemStatus, string.Empty),
        new(TelemetryKeys.Autopilot, string.Empty),
        new(TelemetryKeys.BaseMode, string.Empty),
        new(TelemetryKeys.CustomMode, string.Empty),
        new(TelemetryKeys.MavlinkVersion, string.Empty),
        new(TelemetryKeys.Home, string.Empty),
        new(TelemetryKeys.GlobalPosition, string.Empty),
        new(TelemetryKeys.LastCommand, string.Empty)
    };

    private Table _mainTable;
    private Table _statusTable;
    private Table _headerTable;
    private string _lastCommand = string.Empty;

    public VehicleStateRenderer(IClientDevice device)
    {
        _device = device;
        _heartbeat = _device.GetMicroservice<IHeartbeatClient>() ?? throw new InvalidOperationException("No heartbeat");
        _position = _device.GetMicroservice<IPositionClient>() ?? throw new InvalidOperationException("No position");
        _heartbeatPayload = _heartbeat.RawHeartbeat.CurrentValue ?? throw new InvalidOperationException("No heartbeat");

        CreateTable();
    }

    public void Update()
    {
        var data = GetTelemetry();
        data[TelemetryKeys.LastCommand] = _lastCommand;

        for (int i = 0; i < _telemetryRows.Count; i++)
        {
            var key = _telemetryRows[i].Key;
            if (data.TryGetValue(key, out var value))
            {
                _statusTable.UpdateCell(i, 1, $"[aqua]{value}[/]");
            }
        }
    }

    public Table Render() => _mainTable;

    public void WriteJournal(string text)
    {
        _lastCommand = text;
    }

    public string GetDeviceName()
    {
        return _device.Name.CurrentValue ?? string.Empty;
    }

    private void CreateTable()
    {
        _headerTable = new Table().Expand()
            .AddColumns("[red]U[/]", "[red]D[/]", "[red]LeftArrow[/]", "[red]RightArrow[/]", "[red]T[/]", "[red]Q[/]", "[red]PageUp[/]", "[red]PageDown[/]")
            .Title("[aqua]Controls[/]");
        _headerTable.AddRow("Up", "Down", "Move Left", "Move Right", "Take Off", "Quit", "Speed Up", "Slow Down");

        _statusTable = new Table().AddColumns("Param", "Value").BorderColor(Color.Green);
        foreach (var row in _telemetryRows)
        {
            _statusTable.AddRow(row.Key, "[aqua]" + row.Value + "[/]");
        }

        _mainTable = new Table().Expand().AddColumn("Vehicle Info").Title($"{Markup.Escape(GetDeviceName())}");
        _mainTable.AddRow(_statusTable);
        _mainTable.AddRow(_headerTable);
    }

    private Dictionary<string, string> GetTelemetry()
    {
        var home = _position.Home.CurrentValue;
        var global = _position.GlobalPosition.CurrentValue;

        var homeStr = home == null
            ? "Not Accessible"
            : $"{home.Longitude} {home.Latitude} {home.Altitude}";

        var globalStr = global == null
            ? "Not Accessible"
            : $"{global.Lat} {global.Lon} (MSL){global.Alt} (AGL){global.RelativeAlt}";

        return new Dictionary<string, string>
        {
            { TelemetryKeys.Link, _device.Link.State.ToString() ?? string.Empty },
            { TelemetryKeys.PacketRateHz, _heartbeat.GetPropertyValue("PacketRateHz")?.ToString() ?? string.Empty },
            { TelemetryKeys.SystemStatus, _heartbeatPayload.SystemStatus.ToString() },
            { TelemetryKeys.Type, _heartbeatPayload.Type.ToString() },
            { TelemetryKeys.Autopilot, _heartbeatPayload.Autopilot.ToString() },
            { TelemetryKeys.BaseMode, _heartbeatPayload.BaseMode.ToString("F") },
            { TelemetryKeys.CustomMode, _heartbeatPayload.CustomMode.ToString() },
            { TelemetryKeys.MavlinkVersion, _heartbeatPayload.MavlinkVersion.ToString() },
            { TelemetryKeys.Home, homeStr },
            { TelemetryKeys.GlobalPosition, globalStr }
        };
    }
}

public class CommandInfo
{
    public VehicleAction Action { get; init; }
    public double? AltitudeBeforeChange { get; set; }
    public double? VelocityParam { get; set; }

    public string ToJournalString()
    {
        return Action switch
        {
            VehicleAction.GoRight => $"GOTO: delta={TelemetryParams.DeltaXy} Azimuth={TelemetryParams.RedialDegRight}",
            VehicleAction.GoLeft => $"GOTO: delta={TelemetryParams.DeltaXy} Azimuth={TelemetryParams.RedialDegLeft}",
            VehicleAction.GoForward => $"GOTO: delta={TelemetryParams.DeltaXy} Azimuth={TelemetryParams.RedialDegForward}",
            VehicleAction.GoBackwards => $"GOTO: delta={TelemetryParams.DeltaXy} Azimuth={TelemetryParams.RedialDegBackwards}",
            VehicleAction.Upward => $"Up: H={AltitudeBeforeChange:F1} D=+{TelemetryParams.DeltaZ}",
            VehicleAction.Downward => $"Down: H={AltitudeBeforeChange:F1} D=-{TelemetryParams.DeltaZ}",
            VehicleAction.PageUp => $"{TelemetryParams.VelocityMaxParamName}: {VelocityParam} D=+1",
            VehicleAction.PageDown => $"{TelemetryParams.VelocityMaxParamName}: {VelocityParam} D=-1",
            VehicleAction.TakeOff => "Takeoff",
            _ => string.Empty
        };
    }
}