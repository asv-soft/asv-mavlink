using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Minimal;
using ConsoleAppFramework;
using DotLiquid.Util;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Logging;
using R3;
using Spectre.Console;
using ZLogger;


namespace Asv.Mavlink.Shell
{
    public class PrintVehicleState
    {
        private readonly CancellationTokenSource _cancel = new();
        private const int DeltaXy = 10;
        private const int DeltaZ = 5;
        private string _lastCommand = string.Empty;
        private Thread? _actionsThread;
        private ArduCopterClientDevice? _device;
        private IDeviceExplorer? _deviceExplorer;
        private Table? _table;
        private Table? _headerTable;
        private Table? _statusTable;
        private Table? _logTable;
        private readonly Queue<string> _commandsJournal = new();

        private readonly List<KeyValuePair<string, string>> _telemetry =
        [
            new("Link", ""),
            new("PacketRateHz", ""),
            new("SystemStatus", ""),
            new("Type", ""),
            new("Autopilot", ""),
            new("BaseMode", ""),
            new("CustomMode", ""),
            new("MavlinkVersion", ""),
            new("Home", ""),
            new("GlobalPosition", ""),
            new("LastCommand", "")
        ];

        /// <summary>
        /// Vehicle state real time monitoring
        /// </summary>
        /// <param name="connection">-connection, Connection string. Default "tcp://127.0.0.1:7341"</param>
        [Command("print-vehicle-state")]
        public int Run(string connection = "tcp://127.0.0.1:7341")
        {
            ShellCommandsHelper.CreateDeviceExplorer(connection, out _deviceExplorer);
            while (_device is null)
            {
                _device = ShellCommandsHelper.DeviceAwaiter(_deviceExplorer) as ArduCopterClientDevice;
                if (_device is not null) continue;
                AnsiConsole.Clear();
                AnsiConsole.WriteLine("This command available only to MavType = 2 devices");
                AnsiConsole.WriteLine("Press R to repeat or any key to exit");
                var key =  Console.ReadKey();
                if(key.Key == ConsoleKey.R) continue;
                return 0;
            }
            
            var status2 = @"Waiting for init device";
            AnsiConsole.Status().StartAsync(status2, statusContext =>
            {
                statusContext.Spinner(Spinner.Known.Bounce);
                while (_device is { State.CurrentValue: ClientDeviceState.Uninitialized })
                {
                    Task.Delay(TimeSpan.FromMilliseconds(100));
                }

                return Task.CompletedTask;
            });
            CreateTables();
            Task.Factory.StartNew(() => RunAsync(_device), TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(KeyListen, TaskCreationOptions.LongRunning);
            _actionsThread = new Thread(KeyListen);
            _actionsThread.Start();

            if (_table != null)
                AnsiConsole.Live(_table).AutoClear(true).StartAsync(async ctx =>
                {
                    while (_cancel.IsCancellationRequested is false)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(35));
                        if (_table is null) continue;
                        ctx.Refresh();
                    }
                });
            return 0;
        }

        private void CreateTables()
        {
            _logTable = new Table().AddColumn("Log");
            _headerTable = new Table().Expand().AddColumns("[red]U[/]", "[red]D[/]", "[red]LeftArrow[/]",
                    "[red]RightArrow[/]", "[red]T[/]", "[red]Q[/]", "[red]PageUp[/]", "[red]PageDown[/]")
                .Title("[aqua]Controls[/]");
            _headerTable.AddRow("Up", $"Down", "Move Left", "Move Right", "Take Off", "Quit", "Speed Up", "Slow Down");
            _table = new Table().AddColumns("Status", "Log").Expand().Title($"{Markup.Escape(_device.Name.CurrentValue)}");
            _statusTable = new Table().AddColumns("Param", "Value").BorderColor(Color.Green);
            foreach (var item in _telemetry)
            {
                _statusTable.AddRow($"{item.Key}", $"[aqua]{item.Value}[/]");
            }
            _table.AddRow(_statusTable);
            _table.AddRow(_headerTable);
        }

        protected async Task<int> RunAsync(ArduVehicleClientDevice? vehicle)
        {
            while (!_cancel.IsCancellationRequested)
            {
                Print(vehicle);
                await Task.Delay(1000, _cancel.Token).ConfigureAwait(false);
            }
            return 0;
        }

        private void Print(ArduVehicleClientDevice? vehicle)
        {
            var heartbeat = vehicle.Heartbeat.RawHeartbeat.CurrentValue;
            var position = vehicle.Microservices.FirstOrDefault(m => m is IPositionClient) as PositionClient;
            var homePos = position?.Home.CurrentValue;
            var homePosString = "Not Accessible";
            var currentPosString = homePosString;
            if (homePos is not null)
            {
                homePosString = $"{homePos?.Longitude} {homePos?.Longitude} {homePos?.Altitude}";
            }
            var currentPos = position?.GlobalPosition.CurrentValue;
            if (currentPos is not null)
            {
                currentPosString =
                    $"{currentPos?.Lat} {currentPos?.Lon} (MSL){currentPos?.Alt} (AGL){currentPos?.RelativeAlt}" ?? "";
            }

            var dict = new Dictionary<string, string>
            {
                { nameof(ArduCopterClientDevice.Link), vehicle.Link.State.ToString() ?? string.Empty },
                {
                    nameof(HeartbeatClient.PacketRateHz),
                    vehicle.Microservices.FirstOrDefault(r => r is IHeartbeatClient).GetPropertyValue("PacketRateHz")
                        .ToString() ?? string.Empty
                },
                {
                    nameof(HeartbeatPayload.SystemStatus),
                    heartbeat?.SystemStatus.ToString() ?? string.Empty
                },
                {
                    nameof(HeartbeatPayload.Type),
                    heartbeat?.Type.ToString() ?? string.Empty
                },
                {
                    nameof(HeartbeatPayload.Autopilot),
                    heartbeat?.Autopilot.ToString() ?? string.Empty
                },
                {
                    nameof(HeartbeatPayload.BaseMode),
                    heartbeat?.BaseMode.ToString("F") ?? string.Empty
                },
                {
                    nameof(HeartbeatPayload.CustomMode),
                    heartbeat?.Autopilot.ToString() ?? string.Empty
                },
                {
                    nameof(HeartbeatPayload.MavlinkVersion),
                    heartbeat?.MavlinkVersion.ToString() ?? string.Empty
                },
                {
                    nameof(PositionClient.Home),
                    homePosString
                },
                {
                    nameof(PositionClient.GlobalPosition),
                    currentPosString
                },
                { "LastCommand", _lastCommand }
            };
            var count = 0;
            foreach (var item in dict)
            {
                _statusTable.UpdateCell(count, 1, $"{item.Value}");
                count++;
            }

            _table.UpdateCell(0, 0, _statusTable);

            _table.UpdateCell(0, 1, _logTable);
        }

        private static GeoPoint GetCurrentPosition(ArduCopterClientDevice client)
        {
            var pos = client.Microservices.FirstOrDefault(_ => _ is IGnssClient) as GnssClient;
            if (pos?.Main.CurrentValue is null) return GeoPoint.Zero;
            return new GeoPoint(pos.Main.CurrentValue.Lat, pos.Main.CurrentValue.Lon,
                pos.Main.CurrentValue.Alt);
        }

        private void WriteJournal(string log)
        {
            _commandsJournal.Enqueue(log);
            if (_commandsJournal.Count >= 10)
            {
                _commandsJournal.Dequeue();
            }

            _logTable = new Table().AddColumn("Log");
            foreach (var item in _commandsJournal)
            {
                _logTable.AddRow(item);
            }
        }

        private void KeyListen()
        {
            var controlClient =
                _device.Microservices.FirstOrDefault(_ => _ is IControlClient) as ArduCopterControlClient;
            var paramsClient = _device.Microservices.FirstOrDefault(_ => _ is IParamsClient) as ParamsClientEx;
            if (controlClient is null)
            {
                AnsiConsole.Clear();
                AnsiConsole.WriteLine($"Unable to get control of {_device?.Name.CurrentValue}");
                throw new NullReferenceException();
            }

            while (true)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.RightArrow:
                        _lastCommand = $"GOTO: delta={DeltaXy} Azimuth=90";
                        var newPoint =
                            GetCurrentPosition(_device).RadialPoint(DeltaXy, 90);
                        controlClient.GoTo(newPoint, _cancel.Token).Wait();
                        break;
                    case ConsoleKey.LeftArrow:
                        _lastCommand = $"GOTO: delta={DeltaXy} Azimuth=270";
                        var newPoint1 = GetCurrentPosition(_device).RadialPoint(DeltaXy, 270);
                        controlClient.GoTo(newPoint1, _cancel.Token).Wait();
                        break;
                    case ConsoleKey.UpArrow:
                        _lastCommand = $"GOTO: delta={DeltaXy} Azimuth=0";
                        var newPoint2 = GetCurrentPosition(_device).RadialPoint(DeltaXy, 0);
                        controlClient.GoTo(newPoint2, _cancel.Token).Wait();
                        break;
                    case ConsoleKey.DownArrow:
                        _lastCommand = $"GOTO: delta={DeltaXy} Azimuth=180";
                        var newPoint3 = GetCurrentPosition(_device).RadialPoint(DeltaXy, 180);
                        controlClient.GoTo(newPoint3, _cancel.Token).Wait();
                        break;
                    case ConsoleKey.U:
                        var pointDown = GetCurrentPosition(_device);
                        _lastCommand = $"Up: H={pointDown.Altitude:F1} D=+{DeltaZ}";
                        var newPoint4 = pointDown.AddAltitude(DeltaZ);
                        controlClient.GoTo(newPoint4, _cancel.Token).Wait();
                        break;
                    case ConsoleKey.D:
                        var pointUp = GetCurrentPosition(_device);
                        _lastCommand = $"Down: H={pointUp.Altitude:F1} D=+{DeltaZ}";
                        var newPoint5 = pointUp.AddAltitude(-DeltaZ);
                        _lastCommand = $"Down: H={pointUp.Altitude:F1} D=-{DeltaZ}";
                        controlClient.GoTo(newPoint5, _cancel.Token).Wait();
                        break;
                    case ConsoleKey.PageUp:
                        var paramUp = paramsClient.ReadOnce("MPC_XY_VEL_MAX", CancellationToken.None).Result;
                        paramsClient.WriteOnce("MPC_XY_VEL_MAX", paramUp + 1.0f, _cancel.Token).Wait();
                        _lastCommand = $"MPC_XY_VEL_MAX: {paramUp} D=+1";

                        break;
                    case ConsoleKey.PageDown:
                        var paramDown = paramsClient.ReadOnce("MPC_XY_VEL_MAX", CancellationToken.None).Result;
                        paramsClient.WriteOnce("MPC_XY_VEL_MAX", paramDown - 1.0f, _cancel.Token).Wait();
                        _lastCommand = $"MPC_XY_VEL_MAX: {paramDown} D=-1";
                        break;
                    case ConsoleKey.Q:
                        _cancel.Cancel(false);
                        break;
                    case ConsoleKey.T:
                        _lastCommand = "Takeoff";
                        controlClient.SetGuidedMode();
                        controlClient.TakeOff(GetCurrentPosition(_device).Altitude + 50f,
                            CancellationToken.None).Wait();
                        break;
                }

                WriteJournal(_lastCommand);
            }
        }
    }
}