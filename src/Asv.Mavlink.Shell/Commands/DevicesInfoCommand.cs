using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using ObservableCollections;
using R3;
using Spectre.Console;

namespace Asv.Mavlink.Shell
{
    public class DevicesInfoCommand
    {
        private uint _refreshRate;
        private IProtocolRouter _router;
        private ISynchronizedView<KeyValuePair<DeviceId, IClientDevice>, MavlinkDeviceModel> _list;
        private bool _isDisposed;

        /// <summary>
        /// Command to display information about devices in the MAVLink network.
        /// </summary>
        /// <param name="connectionString">-cs, Connection address to the MAVLink device.</param>
        /// <param name="iterations">-i, Number of iterations for the program to run.</param>
        /// <param name="devicesTimeout">-dt, Lifetime of a MAVLink device if it does not send a Heartbeat.</param>
        /// <param name="refreshRate">-r, Console refresh rate (in ms).</param>
        [Command("devices-info")]
        public int Run(string connectionString, uint? iterations = null, uint devicesTimeout = 10, uint refreshRate = 3000)
        {
            RunAsync(connectionString, iterations, devicesTimeout, refreshRate).Wait();
            ConsoleAppHelper.WaitCancelPressOrProcessExit();
            return 0;
        }

        private async Task RunAsync(string connectionString, uint? iterations, uint devicesTimeout, uint refreshRate)
        {
            _refreshRate = refreshRate;
            var runForever = iterations == null;
            Console.CancelKeyPress += (_, _) => { runForever = false; };
            
            ShellCommandsHelper.CreateDeviceExplorer(connectionString, out var deviceExplorer);
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            
            _list = deviceExplorer.Devices.CreateView(x => new MavlinkDeviceModel(x.Value, TimeProvider.System));
            var table = CreateDeviceInfoTable();
            
            // while (!deviceExplorer.Devices.Any())
            // {
            //     if (devices.Count == 0)
            //     {
            //         AnsiConsole.Clear();
            //         AnsiConsole.MarkupLine("Waiting for connections...");
            //         await DelayWithCancellation(refreshRate, token);
            //     }
            //     else
            //     {
            //         AnsiConsole.Clear();
            //         break;
            //     }
            // }

            try
            {
                await AnsiConsole.Live(table)
                    .AutoClear(false)
                    .Overflow(VerticalOverflow.Ellipsis)
                    .Cropping(VerticalOverflowCropping.Top)
                    .StartAsync(async ctx =>
                    {
                        var localIterations = (int?) iterations ?? 0;

                        while (localIterations > 0 || runForever)
                        {
                            if (_list.Count == 0)
                            {
                                AnsiConsole.Clear();
                                AnsiConsole.MarkupLine("Waiting for connections...");
                                await Task.Delay(TimeSpan.FromMilliseconds(refreshRate), token);
                                ctx.Refresh();
                                return;
                            }
                            
                            if (iterations.HasValue) localIterations--;
                            AnsiConsole.Clear();
                            table.Rows.Clear();
                            RenderRows(table, _list);

                            ctx.Refresh();
                            await Task.Delay(TimeSpan.FromMilliseconds(_refreshRate), token);
                        }
                    });
            }
            catch (ObjectDisposedException ex)
            {
                AnsiConsole.MarkupLine($"[red]Error: object has been disposed.[/] {ex.Message}");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupInterpolated($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (!_isDisposed)
                {
                    _list.Dispose();
                    await deviceExplorer.DisposeAsync();
                    await _router.DisposeAsync();
                    _isDisposed = true;
                }
            }
        }

        private Table CreateDeviceInfoTable()
        {
            var table = new Table();
            table.Title("[[ [yellow]Devices Info[/] ]]");
            table.AddColumn("Id");
            table.AddColumn("Type");
            table.AddColumn("System Id");
            table.AddColumn("Component Id");
            table.AddColumn("Mavlink Version");
            table.AddColumn("Modes");
            table.AddColumn("System Status");
            table.AddColumn("Heartbeat Rate");

            return table;
        }

        private void RenderRows(
            Table table, 
            ISynchronizedView<KeyValuePair<DeviceId, IClientDevice>, MavlinkDeviceModel> devices
        )
        {
            foreach (var device in devices)
            {
                if (!device.IsInitialized.CurrentValue)
                {
                    return;
                }
                
                table.AddRow(
                    Markup.Escape($"{device.DeviceFullId}"),
                    Markup.Escape($"{device.Type}"),
                    Markup.Escape($"{device.SystemId}"),
                    Markup.Escape($"{device.ComponentId}"),
                    Markup.Escape($"{device.MavlinkVersion}"),
                    Markup.Escape($"{device.BaseModeText}"),
                    Markup.Escape($"{device.SystemStatusText}"),
                    Markup.Escape($"{device.RateText}")
                );
            }
        }

        private class MavlinkDeviceModel
        {
            public ReactiveProperty<bool> IsInitialized { get; }
            private uint _rate;
            private long _lastUpdate;
            private uint _lastRate;

            public MavlinkDeviceModel(IClientDevice device, TimeProvider timeProvider)
            {
                IsInitialized = new ReactiveProperty<bool>(false);

                device.State.Where(x => x == ClientDeviceState.Complete)
                    .Take(1)
                    .Subscribe(x => AfterDeviceInitialized(device, timeProvider));
            }
            
            protected virtual void AfterDeviceInitialized(IClientDevice device, TimeProvider timeProvider)
            {
                var heartbeatClient = device.GetMicroservice<IHeartbeatClient>();

                    if (heartbeatClient is null)
                    {
                        return;
                    }
                    
                    IsInitialized.OnNext(true);

                    DeviceFullId = heartbeatClient.Identity.Target;
                    Type = device.Id.DeviceClass;
                    SystemId = heartbeatClient.Identity.Target.SystemId;
                    ComponentId = heartbeatClient.Identity.Target.ComponentId;
                    MavlinkVersion = heartbeatClient.RawHeartbeat.CurrentValue?.MavlinkVersion ?? 0;

                    RateText = "0.0 Hz";

                    Observable.Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), timeProvider).Subscribe(_ =>
                    {
                        var time = Interlocked.Exchange(ref _lastUpdate, timeProvider.GetTimestamp());
                        var cnt = Interlocked.Exchange(ref _lastRate, 0);
                        var rate = (cnt / timeProvider.GetElapsedTime(time).TotalSeconds);
                        RateText = $"{rate:F1} Hz";
                    });

                    heartbeatClient?.RawHeartbeat?.ThrottleLast(TimeSpan.FromSeconds(2), timeProvider)
                        .Subscribe(_ =>
                        {
                            Interlocked.Increment(ref _lastRate);
                            ToggleLinkPing = false;
                            ToggleLinkPing = true;
                        });

                    heartbeatClient?.RawHeartbeat?.Subscribe(_ => Interlocked.Increment(ref _rate));
                    heartbeatClient?.RawHeartbeat?.WhereNotNull().Subscribe(p =>
                        BaseModeText = $"{p.BaseMode.ToString("F").Replace("MavModeFlag", "")}");
                    heartbeatClient?.RawHeartbeat?.WhereNotNull().Subscribe(p =>
                        SystemStatusText = p.SystemStatus.ToString("G").Replace("MavState", ""));
            }

            public MavlinkIdentity DeviceFullId { get; private set; }
            public string Type { get; private set; }
            public byte SystemId { get; private set; }
            public byte ComponentId { get; private set; }
            public byte MavlinkVersion { get; private set; }
            public bool ToggleLinkPing { get; private set; }
            public string RateText { get; private set; }
            public string BaseModeText { get; private set; }
            public string SystemStatusText { get; private set; }
        }
    }
}
