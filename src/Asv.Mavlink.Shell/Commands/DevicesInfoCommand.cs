using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.V2.Minimal;
using ConsoleAppFramework;
using DynamicData;
using DynamicData.Binding;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class DevicesInfoCommand
{
    private uint _refreshRate;
    private MavlinkRouter _router;
    private IObservableList<MavlinkDeviceModel> _list;

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
        _router = new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects, publishScheduler: Scheduler.Default);
        _router.WrapToV2ExtensionEnabled = true;
        var portConfig = new MavlinkPortConfig
        {
            ConnectionString = connectionString,
            IsEnabled = true,
            Name = "DevicesInfoPort"
        };
        _router.AddPort(portConfig);
        
        var svc = new MavlinkDeviceBrowser(_router, TimeSpan.FromSeconds(devicesTimeout), Scheduler.Default);
        var devicesDisp = svc.Devices
            .Do(_ => { })
            .Transform(_ => new MavlinkDeviceModel(_))
            .BindToObservableList(out _list)
            .DisposeMany()
            .Subscribe();
        
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
            devicesDisp.Dispose();
            svc.Dispose();
            _router.Dispose();
        }
    }

    private void RenderRows(Table table, IObservableList<MavlinkDeviceModel> devices)
    {
        foreach (var device in devices.Items)
        {
            table.AddRow(
                $"[red]{device.DeviceFullId}[/]",
                $"{device.Type}",
                $"{device.SystemId}",
                $"{device.ComponentId}",
                $"{device.MavlinkVersion}",
                $"{device.BaseModeText}",
                $"{device.SystemStatusText}",
                $"[green]{device.RateText}[/]"
            );
        }
    }

    private class MavlinkDeviceModel
    {
        private uint _rate;
        private DateTime _lastUpdate;
        private uint _lastRate;

        public MavlinkDeviceModel(IMavlinkDevice info)
        {
            DeviceFullId = info.FullId;
            Type = ConvertTypeToString(info.Type);
            SystemId = info.SystemId;
            ComponentId = info.ComponentId;
            MavlinkVersion = info.MavlinkVersion;
            RateText = "0.0 Hz";
            Observable.Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), Scheduler.Default).Subscribe(_ =>
            {
                var now = DateTime.Now;
                var rate = (((double)_rate - _lastRate) / (now - _lastUpdate).TotalSeconds);
                RateText = $"{rate:F1} Hz";
                _lastUpdate = now;
                _lastRate = _rate;
            });

            info.Ping.Sample(TimeSpan.FromSeconds(2), Scheduler.Default)
                .Subscribe(_ =>
                {
                    ToggleLinkPing = false;
                    ToggleLinkPing = true;
                });

            info.Ping.Subscribe(_ => Interlocked.Increment(ref _rate));
            info.BaseMode.Subscribe(_ => BaseModeText = $"{_.ToString("F").Replace("MavModeFlag", "")}");
            info.SystemStatus.Subscribe(_ => SystemStatusText = _.ToString("G").Replace("MavState", ""));
        }
    
        public ushort DeviceFullId { get; }
        public string Type { get; }
        public byte SystemId { get; }
        public byte ComponentId { get; }
        public byte MavlinkVersion { get; }
        public bool ToggleLinkPing { get; private set; }
        public string RateText { get; private set; }
        public string BaseModeText { get; private set; }
        public string SystemStatusText { get; private set; }

        private string ConvertTypeToString(MavType type)
        {
            var typeStr = $"{type.ToString("G").Replace("MavType", "")}";
            if (byte.TryParse(typeStr, out _))
            {
                return "Unknown";
            }

            return typeStr;
        } 
    }
}