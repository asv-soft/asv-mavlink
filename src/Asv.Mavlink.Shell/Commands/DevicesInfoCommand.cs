using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;
using BenchmarkDotNet.Toolchains.Roslyn;
using ConsoleAppFramework;
using ObservableCollections;
using R3;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class DevicesInfoCommand
{
    private uint _refreshRate;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private IProtocolRouter _router;
    private IDeviceExplorer _explorer;
    private ISynchronizedView<KeyValuePair<DeviceId,IClientDevice>,MavlinkDeviceModel> _list;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


    /// <summary>
    /// Command that shows info about devices in the mavlink network
    /// </summary>
    /// <param name="connectionString">-cs, The address of the connection to the mavlink device</param>
    /// <param name="iterations">-i, States how many iterations should the program work through</param>
    /// <param name="devicesTimeout">-dt, (in seconds) States the lifetime of a mavlink device that shows no Heartbeat</param>
    /// <param name="refreshRate">-r, (in ms) States how fast should the console be refreshed</param>
    /// <returns></returns>
    [Command("devices-info")]
    public int Run(string connectionString, uint? iterations = null, uint devicesTimeout = 10, uint refreshRate = 3000)
    {
        RunAsync(connectionString, iterations, devicesTimeout, refreshRate).Wait();
        ConsoleAppHelper.WaitCancelPressOrProcessExit();
        return 0;
    }
    
    private async Task RunAsync(string connectionString, uint? iterations, uint devicesTimeout,  uint refreshRate)
    {
        _refreshRate = refreshRate;
        var protocol = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        });
        _router = protocol.CreateRouter("ROUTER");
        _router.AddPort(connectionString);
        var seq = new PacketSequenceCalculator();
        _explorer = DeviceExplorer.Create(_router, builder =>
        {
            builder.SetLog(protocol.LoggerFactory);
            builder.SetMetrics(protocol.MeterFactory);
            builder.SetTimeProvider(protocol.TimeProvider);
            builder.Factories.RegisterDefaultDevices(new MavlinkIdentity(254,254), seq, new InMemoryConfiguration());
        });
        
        
        _list = _explorer.Devices.CreateView(x => new MavlinkDeviceModel(x.Value, TimeProvider.System));
            
        
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
        
        try
        {
            await AnsiConsole.Live(table)
                .AutoClear(false)
                .Overflow(VerticalOverflow.Ellipsis)
                .Cropping(VerticalOverflowCropping.Top)
                .StartAsync(async ctx =>
                {
                    var runForever = iterations == null;
                    Console.CancelKeyPress += (_, _) =>
                    {
                        runForever = false;
                    };
                    
                    while (iterations > 0 || runForever)
                    {
                        iterations--;
                        table.Rows.Clear(); 
                        RenderRows(table, _list);
                        ctx.Refresh(); 
                        await Task.Delay(TimeSpan.FromMilliseconds(_refreshRate));
                    }
                });
        }
        finally
        {
            AnsiConsole.Markup("\nDone");
            _list.Dispose();
            await _explorer.DisposeAsync();
            await _router.DisposeAsync();
        }
    }

    private void RenderRows(Table table, ISynchronizedView<KeyValuePair<DeviceId,IClientDevice>,MavlinkDeviceModel> devices)
    {
        foreach (var device in devices.Unfiltered)
        {
            table.AddRow(
                $"[red]{device.View.DeviceFullId}[/]",
                $"{device.View.Type}",
                $"{device.View.SystemId}",
                $"{device.View.ComponentId}",
                $"{device.View.MavlinkVersion}",
                $"{device.View.BaseModeText}",
                $"{device.View.SystemStatusText}",
                $"[green]{device.View.RateText}[/]"
            );
        }
    }

    private class MavlinkDeviceModel
    {
        private uint _rate;
        private long _lastUpdate;
        private uint _lastRate;

#pragma warning disable CS8618, CS9264
        public MavlinkDeviceModel(IClientDevice info,TimeProvider timeProvider )
#pragma warning restore CS8618, CS9264
        {
            var heartbeat = (IHeartbeatClient)info.Microservices.First(x => x is IHeartbeatClient);
            DeviceFullId = heartbeat.Identity.Target;
            Type = info.Id.DeviceClass;
            SystemId = heartbeat.Identity.Target.SystemId;
            ComponentId = heartbeat.Identity.Target.ComponentId;
            if (heartbeat.RawHeartbeat.CurrentValue != null)
                MavlinkVersion = heartbeat.RawHeartbeat.CurrentValue.MavlinkVersion;
            RateText = "0.0 Hz";
            
            Observable.Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), timeProvider).Subscribe(_ =>
            {
                var time = Interlocked.Exchange(ref _lastUpdate, timeProvider.GetTimestamp());
                var cnt = Interlocked.Exchange(ref _lastRate,0);
                var rate = (cnt / timeProvider.GetElapsedTime(time).TotalSeconds);
                RateText = $"{rate:F1} Hz";
            });

            heartbeat.RawHeartbeat.ThrottleLast(TimeSpan.FromSeconds(2), timeProvider)
                .Subscribe(_ =>
                {
                    Interlocked.Increment(ref _lastRate);
                    ToggleLinkPing = false;
                    ToggleLinkPing = true;
                });

            heartbeat.RawHeartbeat.Subscribe(_ => Interlocked.Increment(ref _rate));
            heartbeat.RawHeartbeat.WhereNotNull().Subscribe(p => BaseModeText = $"{p.BaseMode.ToString("F").Replace("MavModeFlag", "")}");
            heartbeat.RawHeartbeat.WhereNotNull().Subscribe(p => SystemStatusText = p.SystemStatus.ToString("G").Replace("MavState", ""));
        }

        public MavlinkIdentity DeviceFullId { get; set; }
        public string Type { get; }
        public byte SystemId { get; }
        public byte ComponentId { get; }
        public byte MavlinkVersion { get; }
        public bool ToggleLinkPing { get; private set; }
        public string RateText { get; private set; }
        public string BaseModeText { get; private set; }
        public string SystemStatusText { get; private set; }

       
    }
}