using System;
using System.Collections.Generic;
using Asv.IO;
using Asv.Mavlink.Minimal;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Asv.Mavlink.Shell;

public class VehicleStateRenderable : ITableRenderable
{
    public const string RenderableName = "Vehicle";
    private const string NotAccessibleTelemetry = "Not accessible";
    private readonly IClientDevice _device;
    private readonly IPositionClient _position;
    private readonly IHeartbeatClient _heartbeat;
    private readonly HeartbeatPayload _heartbeatPayload;

    private readonly List<KeyValuePair<string, string>> _telemetryRows =
    [
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
    ];

    private Table _headerTable = new();
    private Table _statusTable = new();
    private Table _mainTable = new();
    
    private string _lastCommand = string.Empty;

    public VehicleStateRenderable(IClientDevice device)
    {
        _device = device;
        _position = _device.GetMicroservice<IPositionClient>() ?? throw new InvalidOperationException("No position client");
        _heartbeat = _device.GetMicroservice<IHeartbeatClient>() ?? throw new InvalidOperationException("No heartbeat client");
        _heartbeatPayload = _heartbeat.RawHeartbeat.CurrentValue ?? throw new InvalidOperationException("No heartbeat client");

        InitTables();
    }
    
    private void InitTables()
    {
        _headerTable = new Table()
            .Expand()
            .AddColumns(
                "[red]U[/]", 
                "[red]D[/]", 
                "[red]Up Arrow[/]", 
                "[red]Down Arrow[/]", 
                "[red]Left Arrow[/]", 
                "[red]Right Arrow[/]", 
                "[red]T[/]",
                "[red]Q[/]", 
                "[red]PageUp[/]", 
                "[red]PageDown[/]"
            ).Title("[aqua]Controls[/]");
        _headerTable.AddRow("Up", "Down", "Move Forward", "Move Backward", "Move Left", "Move Right", "Take Off", "Quit", "+ Max Speed", "- Max Speed");

        _statusTable = new Table().AddColumns("Param", "Value").BorderColor(Color.Green);
        foreach (var row in _telemetryRows)
        {
            _statusTable.AddRow(row.Key, "[aqua]" + row.Value + "[/]");
        }
        
        _mainTable = new Table()
            .Expand()
            .AddColumn("Vehicle Info")
            .Title($"{Markup.Escape(_device.Name.CurrentValue ?? string.Empty)}");
        _mainTable.AddRow(_statusTable);
        _mainTable.AddRow(_headerTable);
    }

    public void Update()
    {
        var data = GetTelemetry();
        data[TelemetryKeys.LastCommand] = _lastCommand;

        for (var i = 0; i < _telemetryRows.Count; i++)
        {
            var key = _telemetryRows[i].Key;
            if (data.TryGetValue(key, out var value))
            {
                _statusTable.UpdateCell(i, 1, $"[aqua]{value}[/]");
            }
        }
    }

    public string Name => RenderableName;
    public IRenderable Render() => _mainTable;

    public void WriteJournal(string text)
    {
        _lastCommand = text;
    }
    
    private Dictionary<string, string> GetTelemetry()
    {
        var homePosition = _position.Home.CurrentValue;
        var globalPosition = _position.GlobalPosition.CurrentValue;

        var homeStr = homePosition is null
            ? NotAccessibleTelemetry
            : $"{homePosition.Longitude} {homePosition.Latitude} {homePosition.Altitude}";

        var globalStr = globalPosition is null
            ? NotAccessibleTelemetry
            : $"{globalPosition.Lat} {globalPosition.Lon} (MSL){globalPosition.Alt} (AGL){globalPosition.RelativeAlt}";

        return new Dictionary<string, string>
        {
            { TelemetryKeys.Link, _device.Link.State.ToString() ?? string.Empty },
            { TelemetryKeys.PacketRateHz, _heartbeat.PacketRateHz.CurrentValue.ToString("F")},
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
            VehicleAction.PageUp => $"{TelemetryParams.VelocityMaxParamName}: {VelocityParam} D=+{TelemetryParams.VelocityDelta}",
            VehicleAction.PageDown => $"{TelemetryParams.VelocityMaxParamName}: {VelocityParam} D=-{TelemetryParams.VelocityDelta}",
            VehicleAction.TakeOff => "Takeoff",
            _ => string.Empty
        };
    }
}