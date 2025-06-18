using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace Asv.Mavlink.Shell
{
    public class VehicleStateRenderer
    {
        private const int DeltaXy = 10;
        private const int DeltaZ = 5;
        private const int AzimuthR = 90;
        private const int AzimuthL = 270;
        private const int AzimuthU = 0;
        private const int AzimuthD = 180;
        private const string VelocityMaxParamName = "MPC_XY_VEL_MAX";
        
        private readonly CancellationToken _cancel;
        private readonly Queue<string> _journal = new();
        private readonly VehicleTelemetryProvider _telemetryProvider;
        private readonly List<KeyValuePair<string, string>> _telemetryRows =
        [
            new("Link", ""),
            new("PacketRateHz", ""),
            new("Type", ""),
            new("SystemStatus", ""),
            new("Autopilot", ""),
            new("BaseMode", ""),
            new("CustomMode", ""),
            new("MavlinkVersion", ""),
            new("Home", ""),
            new("GlobalPosition", ""),
            new("LastCommand", "")
        ];
        private string _lastCommand = "";
        private Table? _mainTable;
        private Table? _statusTable;
        private Table? _logTable;
        private Table? _headerTable;
        
        public VehicleStateRenderer(VehicleTelemetryProvider telemetryProvider,
            CancellationToken cancel)
        {
            _telemetryProvider = telemetryProvider;
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
            _mainTable = new Table().AddColumns("Status", "Log").Expand().Title($"{Markup.Escape(_telemetryProvider.GetDeviceName())}");
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
        public void Print()
        {
            if (_statusTable is null || _mainTable is null || _logTable is null)
            {
                CreateTables();
            }

            var dict = _telemetryProvider.GetTelemetry();
            dict["LastCommand"] = _lastCommand;

            int count = 0;
            foreach (var item in dict)
            {
                _statusTable?.UpdateCell(count, 1, item.Value); 
                count++;
            }

            _mainTable?.UpdateCell(0, 0, _statusTable);
            _mainTable?.UpdateCell(0, 1, _logTable);
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
                    await Task.Delay(35);
                    if (ct.IsCancellationRequested)
                    {
                        AnsiConsole.Write("All done");
                        break;   
                    }
                    ctx.Refresh();
                }
            });
        }
        
        /// <summary>
        /// Logs the last executed vehicle command and updates the displayed journal.
        /// </summary>
        public void WriteJournal(CommandInfo info)
        {
            _lastCommand = info.ToJournalString(_telemetryProvider);
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
        
        #region  MessageType
        public enum MessageType
        {
            Info,
            Warning,
            Error
        }
        #endregion
        
        #region  CommandInfo
        public class CommandInfo
        {
            public VehicleDirection Direction { get; set; }
            public double? AltitudeBeforeChange { get; set; }
            public double? VelocityParam { get; set; }

            public string ToJournalString(VehicleTelemetryProvider telemetry)
            {
                return Direction switch
                {
                    VehicleDirection.Right => $"GOTO: delta={telemetry.DeltaXyValue} Azimuth={telemetry.RedialRight}",
                    VehicleDirection.Left => $"GOTO: delta={telemetry.DeltaXyValue} Azimuth={telemetry.RedialLeft}",
                    VehicleDirection.Up => $"GOTO: delta={telemetry.DeltaXyValue} Azimuth={telemetry.RedialUp}",
                    VehicleDirection.Down => $"GOTO: delta={telemetry.DeltaXyValue} Azimuth={telemetry.RedialDown}",
                    VehicleDirection.U => $"Up: H={AltitudeBeforeChange:F1} D=+{telemetry.DeltaZValue}",
                    VehicleDirection.D => $"Down: H={AltitudeBeforeChange:F1} D=-{telemetry.DeltaZValue}",
                    VehicleDirection.PageUp => $"{telemetry.VelocityMaxParam}: {VelocityParam} D=+1",
                    VehicleDirection.PageDown => $"{telemetry.VelocityMaxParam}: {VelocityParam} D=-1",
                    VehicleDirection.T => "Takeoff",
                    _ => ""
                };
            }
        }
        #endregion
    }
}
