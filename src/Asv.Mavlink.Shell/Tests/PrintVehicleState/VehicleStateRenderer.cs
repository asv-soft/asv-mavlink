using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace Asv.Mavlink.Shell
{
    public class VehicleStateRenderer
    {
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
        /// <param name="direction">The user command or vehicle direction input.</param>
        /// <param name="param">Optional parameter associated with the command (e.g. altitude or velocity).</param>
        public void WriteJournal(VehicleDirection direction, object? param)
        {
            switch (direction)
                {
                    case VehicleDirection.Right:
                        _lastCommand = $"GOTO: delta={_telemetryProvider.XyThreshold} Azimuth=90";
                        break;
                    case VehicleDirection.Left:
                        _lastCommand = $"GOTO: delta={_telemetryProvider.XyThreshold} Azimuth=270";
                        break;
                    case VehicleDirection.Up:
                        _lastCommand = $"GOTO: delta={_telemetryProvider.XyThreshold} Azimuth=0";
                        break;
                    case VehicleDirection.Down:
                        _lastCommand = $"GOTO: delta={_telemetryProvider.XyThreshold} Azimuth=180";
                        break;
                    case VehicleDirection.U:
                        _lastCommand = $"Up: H={param:F1} D=+{_telemetryProvider.ZThreshold}";
                        break;
                    case VehicleDirection.D:
                        _lastCommand = $"Down: H={param:F1} D=-{_telemetryProvider.ZThreshold}";
                        break;
                    case VehicleDirection.PageUp:
                        _lastCommand = $"MPC_XY_VEL_MAX: {param} D=+1";
                        break;
                    case VehicleDirection.PageDown:
                        _lastCommand = $"MPC_XY_VEL_MAX: {param} D=-1";
                        break;
                    case VehicleDirection.Q:
                        break;
                    case VehicleDirection.T:
                        _lastCommand = "Takeoff";
                        break;
                }
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

        /// <summary>
        /// Displays a one-time styled message in the console, such as info, warning, or error.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="type">The message type which determines the color/style. Default is Info.</param>
        public void ShowMessage(string message, MessageType type = MessageType.Info)
        {
            var markup = type switch
            {
                MessageType.Warning => "[bold yellow]",
                MessageType.Error => "[bold red]",
                _ => "[bold aqua]"
            };

            AnsiConsole.MarkupLine($"\n{markup}{message}[/]");
        }
        
        #region  MessageType
        public enum MessageType
        {
            Info,
            Warning,
            Error
        }
        #endregion
    }
}
