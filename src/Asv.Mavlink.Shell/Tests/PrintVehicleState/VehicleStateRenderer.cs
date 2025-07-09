using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Minimal;
using DotLiquid.Util;
using Spectre.Console;

namespace Asv.Mavlink.Shell
{
    public class VehicleStateRenderer
    {
        private readonly CancellationToken _cancel;
        private readonly Queue<string> _journal = new();
        private readonly IClientDevice _device;
        private readonly IHeartbeatClient _heartbeat;
        private readonly HeartbeatPayload _heartbeatPayload;
        private readonly IPositionClient _position;
        private readonly List<KeyValuePair<string, string>> _telemetryRows =
        [
            new(TelemetryKeys.Link, ""),
            new(TelemetryKeys.PacketRateHz, ""),
            new(TelemetryKeys.Type, ""),
            new(TelemetryKeys.SystemStatus, ""),
            new(TelemetryKeys.Autopilot, ""),
            new(TelemetryKeys.BaseMode, ""),
            new(TelemetryKeys.CustomMode, ""),
            new(TelemetryKeys.MavlinkVersion, ""),
            new(TelemetryKeys.Home, ""),
            new(TelemetryKeys.GlobalPosition, ""),
            new(TelemetryKeys.LastCommand, "")
        ];
        private string _lastCommand = "";
        private Table? _mainTable;
        private Table? _statusTable;
        private Table? _logTable;
        private Table? _headerTable;
        
        public VehicleStateRenderer(IClientDevice device,
            CancellationToken cancel)
        {
            _device = device;     
            _heartbeat = _device.GetMicroservice<IHeartbeatClient>() ?? throw new InvalidOperationException("No heartbeat");
            _position = _device.GetMicroservice<IPositionClient>() ?? throw new InvalidOperationException("No position");
            _heartbeatPayload = _heartbeat.RawHeartbeat.CurrentValue ?? throw new InvalidOperationException("No heartbeat");
            _cancel = cancel;
        }

        /// <summary>
        /// Creates and initializes the internal tables for telemetry, logs, and control hints.
        /// </summary>
        private void CreateTables()
        {
            _logTable = new Table().AddColumn("Log");
            _headerTable = new Table().Expand().AddColumns("[red]U[/]", "[red]D[/]", "[red]LeftArrow[/]",
                    "[red]RightArrow[/]", "[red]T[/]", "[red]Q[/]", "[red]PageUp[/]", "[red]PageDown[/]")
                .Title("[aqua]Controls[/]");
            _headerTable.AddRow("Up", $"Down", "Move Left", "Move Right", "Take Off", "Quit", "Speed Up", "Slow Down");
            _mainTable = new Table().AddColumns("Status", "Log").Expand().Title($"{Markup.Escape(GetDeviceName())}");
            _statusTable = new Table().AddColumns("Param", "Value").BorderColor(Color.Green);
            foreach (var item in _telemetryRows)
            {
                _statusTable.AddRow($"{item.Key}", $"[aqua]{item.Value}[/]");
            }
            _mainTable.AddRow(_statusTable);
            _mainTable.AddRow(_headerTable);
        }
        
        /// <summary>
        /// Updates the telemetry and log tables with the latest data from the telemetry provider.
        /// Re-renders status values and log entries.
        /// </summary>
        public void UpdateTable()
        {
            if (_statusTable is null || _mainTable is null || _logTable is null)
            {
                CreateTables();
            }

            var dict = GetTelemetry();
            dict[TelemetryKeys.LastCommand] = _lastCommand;

            int count = 0;
            foreach (var item in dict)
            {
                _statusTable?.UpdateCell(count, 1, item.Value); 
                count++;
            }

            if (_statusTable is not null && _logTable is not null)
            {
                _mainTable?.UpdateCell(0, 0, _statusTable);
                _mainTable?.UpdateCell(0, 1, _logTable);
            }
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
            var globalStr = global == null
                ? "Not Accessible"
                : $"{global.Lat} {global.Lon} (MSL){global.Alt} (AGL){global.RelativeAlt}";

            return new Dictionary<string, string>
            {
                { TelemetryKeys.Link, _device.Link.State.ToString() ?? "" },
                { TelemetryKeys.PacketRateHz, _heartbeat.GetPropertyValue("PacketRateHz")?.ToString() ?? "" },
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

        /// <summary>
        /// Starts a live console UI session that continuously refreshes the telemetry display
        /// until cancellation is requested.
        /// </summary>
        /// <returns>A task that represents the asynchronous rendering loop.</returns>
        public async Task DisplayTableAsync(CancellationToken ct)
        {
            if (_mainTable is null) return;

            await AnsiConsole.Live(_mainTable).AutoClear(true).StartAsync(async ctx =>
            {
                while (!ct.IsCancellationRequested)
                {
                    if (ct.IsCancellationRequested)
                    {
                        AnsiConsole.Write("All done");
                        break;   
                        
                    }
                    await Task.Delay(35, ct);
                    
                    UpdateTable();
                    
                    ctx.Refresh();
                }
            });
        }
        
        /// <summary>
        /// Logs the last executed vehicle command and updates the displayed journal.
        /// </summary>
        public void WriteJournal(CommandInfo info)
        {
            _lastCommand = info.ToJournalString();
            Write(_lastCommand);
        }
        
        /// <summary>
        /// Adds a log entry to the journal and rebuilds the log table.
        /// Keeps the most recent 10 entries.
        /// </summary>
        /// <param name="log">The log message to write.</param>
        private void Write(string log)
        {
            _journal.Enqueue(log); 
            if (_journal.Count >= 10)
            {
                _journal.Dequeue();
            }

            _logTable = new Table().AddColumn("Log");
            foreach (var item in _journal)  
            {
                _logTable.AddRow(item);
            }
        }
        
        #region CommandInfo
        public class CommandInfo
        {
            public VehicleAction Direction { get; set; }
            public double? AltitudeBeforeChange { get; set; }
            public double? VelocityParam { get; set; }

            public string ToJournalString()
            {
                return Direction switch
                {
                    VehicleAction.GoRight => $"GOTO: delta={TelemetryParams.DeltaXy} Azimuth={TelemetryParams.RedialDegRight}",
                    VehicleAction.GoLeft => $"GOTO: delta={TelemetryParams.DeltaXy} Azimuth={TelemetryParams.RedialDegLeft}",
                    VehicleAction.GoUp => $"GOTO: delta={TelemetryParams.DeltaXy} Azimuth={TelemetryParams.RedialDegUp}",
                    VehicleAction.GoDown => $"GOTO: delta={TelemetryParams.DeltaXy} Azimuth={TelemetryParams.RedialDegLeft}",
                    VehicleAction.Upward => $"Up: H={AltitudeBeforeChange:F1} D=+{TelemetryParams.DeltaZ}",
                    VehicleAction.Downward => $"Down: H={AltitudeBeforeChange:F1} D=-{TelemetryParams.DeltaZ}",
                    VehicleAction.PageUp => $"{TelemetryParams.VelocityMaxParamName}: {VelocityParam} D=+1",
                    VehicleAction.PageDown => $"{TelemetryParams.VelocityMaxParamName}: {VelocityParam} D=-1",
                    VehicleAction.TakeOff => "Takeoff",
                    _ => ""
                };
            }
        }
        #endregion
    }
}
